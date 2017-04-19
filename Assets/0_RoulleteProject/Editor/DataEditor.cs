using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class DataEditor : EditorWindow {

    public CellList cells;
    public List<Cell> table_t = new List<Cell>();

    public int cur_index;
    public Vector2 scrollPosition;

    public Transform _root;

    [MenuItem("Window/My Editor")]

    static void Init()
    {
        // Get existing open window or if none, make a new one:
        DataEditor window = (DataEditor)EditorWindow.GetWindow(typeof(DataEditor));
        window.Show();
    }

    void OnGUI()
    {
        //RIGHT SIDE;
        //=======================================================;
        GUI.Box(new Rect(0f, 0f, 600, 850), "Parameters");
        GUILayout.BeginArea(new Rect(5, 20, 590, 840));
        if (cells != null)
        {
            if (table_t.Count > 0)
            {
                //Basic parameters;
                GUILayout.Label("Cell ID : ");
                table_t[cur_index].cell_id = EditorGUILayout.IntField(table_t[cur_index].cell_id);

                GUILayout.Label("Cell Key : ");
                table_t[cur_index].cell_key = EditorGUILayout.TextField(table_t[cur_index].cell_key);

                GUILayout.Label("Cell Label : ");
                table_t[cur_index].cell_label = EditorGUILayout.TextField(table_t[cur_index].cell_label);

                //=======================================================;
                GUILayout.Label("Close numbers : " + table_t[cur_index].cell_close_numbers.Count.ToString());
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("+", GUILayout.Width(30))) { table_t[cur_index].cell_close_numbers.Add(0); }
                for (int j = 0; j < table_t[cur_index].cell_close_numbers.Count; j++)
                {
                    table_t[cur_index].cell_close_numbers[j] = EditorGUILayout.IntField(table_t[cur_index].cell_close_numbers[j], GUILayout.Width(25));
                }
                if (GUILayout.Button("CLR", GUILayout.Width(35))) { table_t[cur_index].cell_close_numbers.Clear(); }
                GUILayout.EndHorizontal();

                GUILayout.Label("Close ID : " + table_t[cur_index].cell_close_id.Count.ToString());
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("+", GUILayout.Width(30))) { table_t[cur_index].cell_close_id.Add(0); }
                for (int j = 0; j < table_t[cur_index].cell_close_id.Count; j++)
                {
                    table_t[cur_index].cell_close_id[j] = EditorGUILayout.IntField(table_t[cur_index].cell_close_id[j], GUILayout.Width(25));
                }
                if (GUILayout.Button("CLR", GUILayout.Width(35))) { table_t[cur_index].cell_close_id.Clear(); }
                GUILayout.EndHorizontal();


                GUILayout.Label("Set light index: ");
                table_t[cur_index].cell_set_light_index = EditorGUILayout.IntField(table_t[cur_index].cell_set_light_index, GUILayout.Width(25));

                GUILayout.Label("Local pos: " + table_t[cur_index].cell_roll_chip.Count);
                if (GUILayout.Button("Add")) { table_t[cur_index].cell_chips_pos.Add(new Vector2(0.0f,0.0f)); }
                if (GUILayout.Button("Add roll flag")) { table_t[cur_index].cell_roll_chip.Add(false); }
                if (GUILayout.Button("Add roll flag")) { table_t[cur_index].cell_roll_chip.Add(false); }

                if (table_t[cur_index].cell_chips_pos.Count > 0)
                {
                    for (int f = 0; f < table_t[cur_index].cell_chips_pos.Count; f++)
                    {
                        GUILayout.BeginHorizontal();
                        table_t[cur_index].cell_chips_pos[f] = EditorGUILayout.Vector2Field("Pos:", table_t[cur_index].cell_chips_pos[f], GUILayout.Width(200));
                        table_t[cur_index].cell_roll_chip[f] = EditorGUILayout.Toggle(table_t[cur_index].cell_roll_chip[f]);
                        GUILayout.EndHorizontal();
                    }
                }

                //GUILayout.Label("Chip instance slot: ");
                //if (GUILayout.Button("Add instance")) { table_t[cur_index].cell_chip_instance.Add(null); }
                //if (table_t[cur_index].cell_chip_instance.Count > 0)
                //{
                //    GUILayout.Label("Instances count : " + table_t[cur_index].cell_chip_instance.Count);
                //}
            }
        }
        GUILayout.EndArea();

        //LEFT SIDE;
        //=======================================================;
        GUI.Box(new Rect(605, 0f, 260, 850), "Триггер / Действия");
        GUILayout.BeginArea(new Rect(605, 20, 250, 840));

        GUILayout.BeginVertical("Box");
        cells = (CellList)EditorGUILayout.ObjectField(cells, typeof(CellList), true);

        //if (GUILayout.Button("Create")) { CreateMyAsset(); }
        //if (GUILayout.Button("Add")) { cells.cells_list.Add(new Cell()); }
        _root = (Transform)EditorGUILayout.ObjectField(_root, typeof(Transform),true);
        if (GUILayout.Button("Set")) { SetData(); }
        if (GUILayout.Button("Open")) { ReadTable(); }
        if (GUILayout.Button("Save")) { SaveTable(); }


        GUILayout.Space(5);
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(240), GUILayout.Height(700));


        if (table_t.Count > 0)
        {
            for (int i = 0; i < table_t.Count; i++)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(i.ToString() + ":" + table_t[i].cell_label))
                {
                    cur_index = i;
                }
                GUILayout.Button("X", GUILayout.Width(32));
                GUILayout.EndHorizontal();
            }
        }
        

        GUILayout.EndVertical();
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    public void CreateMyAsset()
    {
        CellList asset = ScriptableObject.CreateInstance<CellList>();
        //CellList.CreateInstance(*)
        AssetDatabase.CreateAsset(asset, "Assets/NewScripableObject.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }

    public void SetData()
    {
        //int t = _root.childCount;

        //for (int i = 0; i < table_t.Count; i++)
        //{
        //    for(int j =0; j < table_t[i].cell_close_numbers.Count; j++)
        //    {
        //        //Debug.Log(table_t[i].cell_close_numbers[j].ToString());
        //        for (int r = 0; r < t; r++)
        //        {
        //            if (table_t[i].cell_close_numbers[j].ToString() == _root.GetChild(r).name)
        //            {
        //                table_t[i].cell_close_id.Add(_root.GetChild(r).GetComponent<FieldTrigger>().cell_id);
        //            }
        //        }
        //    }
        //}

        //if (_root != false)
        //{
        //    int t = _root.childCount;
        //    for (int i = 0; i < t; i++)
        //    {
        //        for (int j = 0; j < table_t.Count; j++)
        //        {
        //            if (table_t[j].cell_label == _root.GetChild(i).name)
        //            {
        //                var r = _root.GetChild(i).GetComponent<FieldTrigger>();
        //                r.cell_id = j;
        //                break;
        //            }
        //        }
        //    }
        //}

        //for (int i = 0; i < table_t.Count; i++)
        //{
        //    //table_t[i].cell_chip_instance.Clear();
        //    table_t[i].cell_roll_chip.Add(false);
        //}

        if (_root != false)
        {
            int t = _root.childCount;
            for (int i = 0; i < t; i++)
            {
                for (int j = 0; j < table_t.Count; j++)
                {
                    if (table_t[j].cell_label == _root.GetChild(i).name)
                    {
                        table_t[j].cell_chips_pos[0] = _root.GetChild(i).localPosition;
                        Debug.Log("Found");
                        //var r = _root.GetChild(i).GetComponent<FieldTrigger>();
                        //r.cell_id = j;
                        break;
                    }
                }
            }
        }
    }

    public void SaveTable()
    {
        cells.cells_list.Clear();
        for (int i = 0; i < table_t.Count; i++)
        {
            cells.cells_list.Add(table_t[i]);
        }

        EditorUtility.SetDirty(cells);
    }

    public void ReadTable()
    {
        table_t.Clear();
        for (int i = 0; i < cells.cells_list.Count; i++)
        {
            table_t.Add(cells.cells_list[i]);
        }

        Debug.Log("Read");
    }
}

