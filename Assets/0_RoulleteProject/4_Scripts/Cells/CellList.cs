using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class CellList : ScriptableObject{
    public List<Cell> cells_list = new List<Cell>();
}
