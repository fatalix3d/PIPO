using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class NeighborsCell {
    public int n_field_pos;
    public int n_index;
    public Vector2 n_pos;
    public int n_value;

    public int n_root_number;
    public int n_bet_value;
    public List<int> n_close_id = new List<int>();
    public List<int> n_close_numbers = new List<int>();
    public List<int> n_pos_id = new List<int>();
}
