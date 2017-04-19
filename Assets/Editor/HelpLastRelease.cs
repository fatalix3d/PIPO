using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEditor;
using Debug = UnityEngine.Debug;

public class HelpLastRelease : EditorWindow
{
    private const string statsUrl = @"http://hwstats.unity3d.com/index.html";
    private const string releaseUrl = @"http://beta.unity3d.com/download/{0}/download.html";
    private const string serverUrl = @"http://symbolserver.unity3d.com/";
    private const string historyUrl = serverUrl + @"000Admin/history.txt";
    private const string baseName = "UnityYAMLMerge.ex";
    private const string mergerName = baseName + "_";
    private const string extractedName = baseName + "e";
#if UNITY_EDITOR_WIN
    private const string zipName = "7z";
#else
    private const string zipName = "7za";
#endif
    private static WWW wwwHistory, wwwList, wwwMerger;
    private static SortedList<string, string> menu;
    private static int selected;
    private static HelpLastRelease window;
    private static GUIStyle style;

    [MenuItem("Help/Last Release...")]
    static void Init()
    {
        menu = null;
        window = (HelpLastRelease)EditorWindow.GetWindow(typeof(HelpLastRelease));
        window.titleContent.text = "Unity Builds";
        window.Show();
        wwwHistory = new WWW(historyUrl);
        EditorApplication.update += WaitHistory;
    }

    void OnGUI()
    {
        if (menu != null)
        {
            style = new GUIStyle(GUI.skin.button);
            style.alignment = TextAnchor.MiddleLeft;
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            for (int i = menu.Count - 1; i >= 0; i--)
            {
                GUI.contentColor = Color.white;
                if (menu.Values[i].Contains("5.3.")) GUI.contentColor = Color.green;
                if (menu.Values[i].Contains("5.4.")) GUI.contentColor = Color.yellow;
                if (menu.Values[i].Contains("5.5.")) GUI.contentColor = Color.red;
                if (GUILayout.Button(menu.Values[i], style))
                {
                    DownloadList(i);
                }
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndVertical();
        }
        else
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Wait...");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }

    static void DownloadList(int historyNum)
    {
        if (wwwList == null)
        {
            selected = historyNum;
            string listUrl = string.Format("{0}000Admin/{1}", serverUrl, menu.Keys[historyNum]);
            wwwList = new WWW(listUrl);
            EditorApplication.update += WaitList;
        }
    }

    static void WaitList()
    {
        if (wwwList != null)
        {
            if (!string.IsNullOrEmpty(wwwList.error))
            {
                Debug.LogWarning(wwwList.error);
                wwwList.Dispose();
                wwwList = null;
            }
            else
            {
                if (wwwList.isDone)
                {
                    if (!string.IsNullOrEmpty(wwwList.text))
                    {
                        ParseList(wwwList.text);
                    }
                    wwwList.Dispose();
                    wwwList = null;
                }
            }
        }
        else
        {
            EditorApplication.update -= WaitList;
        }
    }

    static void ParseList(string list)
    {
        string[] files = list.Split('\n');
        string[] parts;
        for (int i = 0; i < files.Length; i++)
        {
            parts = files[i].Split(',');
            if (parts[0].Contains("UnityYAMLMerge.exe"))
            {
                string mergerUrl = string.Format("{0}{1}/{2}", serverUrl, parts[0].Trim('\"').Replace('\\', '/'), mergerName);
                DownloadMerger(mergerUrl);
                break;
            }
        }
    }

    static void DownloadMerger(string mergerUrl)
    {
        if (wwwMerger == null)
        {
            wwwMerger = new WWW(mergerUrl);
            EditorApplication.update += WaitMerger;
        }
    }

    static void WaitMerger()
    {
        if (wwwMerger != null)
        {
            if (!string.IsNullOrEmpty(wwwMerger.error))
            {
                Debug.LogWarning(wwwMerger.error);
                wwwMerger.Dispose();
                wwwMerger = null;
            }
            else
            {
                if (wwwMerger.isDone)
                {
                    if (wwwMerger.bytesDownloaded > 0)
                    {
                        SaveMerger(wwwMerger.bytes);
                    }
                    wwwMerger.Dispose();
                    wwwMerger = null;
                }
            }
        }
        else
        {
            EditorApplication.update -= WaitMerger;
        }
    }

    static void SaveMerger(byte[] bytes)
    {
        string path = string.Format("{0}/../Temp/LastRelease", Application.dataPath);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        path = string.Format("{0}/{1}", path, mergerName);
        File.WriteAllBytes(path, bytes);
        ExtractMerger(path);
    }

    static void ExtractMerger(string path)
    {
        string zipPath = string.Format("{0}/Tools/{1}", EditorApplication.applicationContentsPath, zipName);
        string arg = string.Format("e -y \"{0}\"", path);
        Process unzip = new Process();
        try
        {
            unzip.StartInfo.FileName = zipPath;
            unzip.StartInfo.Arguments = arg;
            unzip.StartInfo.WorkingDirectory = Path.GetDirectoryName(path);
            unzip.StartInfo.CreateNoWindow = true;
            unzip.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            unzip.Start();
            unzip.WaitForExit();
            SearchVersion();
        }
        catch (Exception e)
        {
            Debug.LogErrorFormat("{0} exe: {1} param: {2}", e.Message, zipPath, arg);
        }
    }

    static void SearchVersion()
    {
        string path = string.Format("{0}/../Temp/LastRelease/{1}", Application.dataPath, extractedName);
        if (File.Exists(path))
        {
            string[] lines;
            lines = File.ReadAllLines(path, Encoding.Unicode);
            string version = menu.Values[selected].Split(' ')[0] + "_";
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains(version))
                {
                    int pos = lines[i].IndexOf(version);
                    string magic = lines[i].Substring(pos + version.Length, 12);
                    Application.OpenURL(string.Format(releaseUrl, magic));
                    break;
                }
            }
        }
    }

    static void WaitHistory()
    {
        if (wwwHistory != null)
        {
            if (!string.IsNullOrEmpty(wwwHistory.error))
            {
                Debug.LogWarning(wwwHistory.error);
                wwwHistory.Dispose();
                wwwHistory = null;
            }
            else
            {
                if (wwwHistory.isDone)
                {
                    if (!string.IsNullOrEmpty(wwwHistory.text))
                    {
                        FillMenu(wwwHistory.text);
                    }
                    wwwHistory.Dispose();
                    wwwHistory = null;
                }
            }
        }
        else
        {
            EditorApplication.update -= WaitHistory;
        }
    }

    static void FillMenu(string history)
    {
        menu = new SortedList<string, string>();
        string[] releases = history.Split('\n');
        string[] parts;
        DateTime dt;
        string build;
        for (int i = 0; i < releases.Length; i++)
        {
            parts = releases[i].Split(',');
            dt = DateTime.Parse(parts[3] + " " + parts[4]);
            build = string.Format("{0} ({1})", parts[6].Trim('\"'), dt.ToString("dd-MM-yyyy"));
            menu.Add(parts[0], build);
        }
        window.Repaint();
    }

    [MenuItem("Help/Statistics")]
    static void OpenStatistics()
    {
        Application.OpenURL(statsUrl);
    }
}