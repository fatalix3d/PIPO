using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnhancedUI.EnhancedScroller;

public class CellDataView : EnhancedScrollerCellView {
    public Text dataText;

    public void SetData(ScrollerData data)
    {
        dataText.text = data.animalname;
    }

}
