using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UndoList {
    public List<UndoCell> undo_states = new List<UndoCell>();
}
