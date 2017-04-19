using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UndoCell {

    //Cell data;
    public List<int> u_cell_id = new List<int>();
    public List<int> u_cell_bet_value = new List<int>();

    //Basic chips label data;
    public List<int> u_cell_chip_value = new List<int>();

    //Extension chips label data;
    public List<int> u_cell_ext_value = new List<int>();

    //Neighbors;
    public List<int> u_neighbors_knot_value = new List<int>();
    public List<int> u_neighbors_knot_pos = new List<int>();
    public List<int> u_neighbors_value = new List<int>();
    public List<int> u_neighbors_close = new List<int>();
    public List<NeighborsCell> u_neigbors_data = new List<NeighborsCell>();


    //Total bet value;
    public int total_bet_val;

}
