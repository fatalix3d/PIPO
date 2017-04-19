using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCell : MonoBehaviour {
    private Button myButton;
    public RawImage img;
    public int index;

    void Start()
    {
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(() => MyClick());
    }

    public void MyClick()
    {
        FileBrowser.instance.ShowFileInfo(index);
    }

    public void SetImage(Texture2D tex)
    {
        img.texture = tex;
    }
}
