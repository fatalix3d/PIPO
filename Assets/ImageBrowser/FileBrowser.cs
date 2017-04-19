using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class FileBrowser : MonoBehaviour {
    public static FileBrowser instance;
    public string path;
    private List<string> img_files_path = new List<string>();
    private List<Texture2D> img_files_prev = new List<Texture2D>();

    public Transform catRoot;
    public GameObject imageCellPrefab;
    public Text file_info;
    public List<ImageCell> imageCellData = new List<ImageCell>(); 

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateDirectory();
    }

    public void UpdateDirectory()
    {
        DirectoryInfo dir = new DirectoryInfo(path);
        img_files_path.Clear();

        try
        {
            if (dir.Exists)
            {
                string[] filesInDir = Directory.GetFiles(path);
                foreach(string fileName in filesInDir)
                {
                    img_files_path.Add(fileName);
                }

                StartCoroutine(LoadImageFromFile());
            }
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

    IEnumerator LoadImageFromFile()
    {
        img_files_prev.Clear();
        string pathPreFix = @"file://";

        if (img_files_path.Count > 0)
        {
            for (int i = 0; i < img_files_path.Count; i++)
            {
                Texture2D tex;
                tex = new Texture2D(32, 32, TextureFormat.DXT1, false);

                WWW www = new WWW(pathPreFix + img_files_path[i]);
                yield return www;

                www.LoadImageIntoTexture(tex);
                www.Dispose();
                www = null;

                img_files_prev.Add(tex);
                var tCell = Instantiate(imageCellPrefab, catRoot);
                tCell.GetComponent<ImageCell>().SetImage(tex);
                var sCell = tCell.GetComponent<ImageCell>();
                sCell.index = i;
                imageCellData.Add(sCell);
            }
        }
        Resources.UnloadUnusedAssets();
        Debug.Log("Total files is : " + img_files_prev.Count.ToString());
    }

    public void ShowFileInfo(int index)
    {
        file_info.text = img_files_path[index];
        Debug.Log(img_files_path[index]);
    }
}
