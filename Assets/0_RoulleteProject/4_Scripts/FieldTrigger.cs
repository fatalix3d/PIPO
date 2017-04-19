using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FieldTrigger : MonoBehaviour {
    private Button myButton;
    public bool isRollButton = false;
    public int cell_id, n_id;

    void Start () {
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(() => MyClick());
    }

    public void MyClick()
    {
        GameManager.instance.cur_cell_isRoll = isRollButton;
        GameManager.instance.SelectCell(cell_id, isRollButton, n_id);
    }

    public void HoverClick()
    {
        Debug.Log(cell_id);
    }
}
