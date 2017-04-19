using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Cell {
    public int cell_id;
    public string cell_key;
    public string cell_label;
    public bool cell_selected = false;

    public int cell_set_light_index;                            //Cell root lighmap id;
    public List<int> cell_close_numbers = new List<int>();      //Cell lightmap id;
    public List<int> cell_close_id = new List<int>();           //id;

    public bool cell_ext_chip = false;
    public Transform cell_chip_instance;                        //Basic chip instance;
    public Transform cell_extra_chip_instance;                  //Set chip instance;
    public Transform cell_neighbors_knot_instance;              //Neighbors knot chip instance;
    public Transform cell_text_instance;                        //Text chip instance;

    public List<Vector2> cell_chips_pos = new List<Vector2>();  
    public Vector2 cell_roll_chips_pos;
    public List<bool> cell_roll_chip = new List<bool>();

    public int cell_bet_value = 0;

    public ChipsTower cell_chip_tower;
    public ChipsTower ext_cell_chip_tower;
    public ChipsTower knot_cell_chip_tower;
}
