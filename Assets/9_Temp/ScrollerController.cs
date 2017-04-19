using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnhancedUI.EnhancedScroller;
using Newtonsoft.Json;

public class Data
{
    public List<string> text_data;
}

public class ScrollerController : MonoBehaviour, IEnhancedScrollerDelegate {

    private List<ScrollerData> _data;
    public EnhancedScroller myScroller;
    public CellDataView animalCellViewPrefab;

    void Start()
    {
        _data = new List<ScrollerData>();
        _data.Add(new ScrollerData() { animalname = "Lion" });
        _data.Add(new ScrollerData() { animalname = "Bear" });
        _data.Add(new ScrollerData() { animalname = "Eagle" });
        _data.Add(new ScrollerData() { animalname = "Ant" });
        _data.Add(new ScrollerData() { animalname = "Cat" });
        _data.Add(new ScrollerData() { animalname = "Sparrow" });
        _data.Add(new ScrollerData() { animalname = "Dog" });
        _data.Add(new ScrollerData() { animalname = "Pig" });

        myScroller.Delegate = this;
        myScroller.ReloadData();
        ReadJsonFile();
    }

    public void ReadJsonFile()
    {
        string dataPath = Application.persistentDataPath;
        Debug.Log(dataPath);
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return _data.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return 100f;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        CellDataView cellView = scroller.GetCellView(animalCellViewPrefab) as CellDataView;
        cellView.SetData(_data[dataIndex]);
        return cellView;
    }
}
