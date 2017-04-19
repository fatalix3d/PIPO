using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonManager : MonoBehaviour {

    public static ButtonManager instance;
    public List<GrowButton> main_buttons = new List<GrowButton>();
    public List<BetChips> betChips = new List<BetChips>();
    public AudioSource a_src;
    public int curIndex = 0;

    void Awake()
    {
        instance = this;
    }

    //CHANGE CURRENT BET VAL <KEYBOARD>;
    //====================================================;
    public void BetChange()
    {
        if ((curIndex + 1) < betChips.Count)
        {
            curIndex += 1;
            betChips[curIndex].MyClick();
        }
        else
        {
            curIndex = 0;
            betChips[curIndex].MyClick();
        }
    }

    //Проиграть звук клавиши;
    //====================================================;
    public void PlayClick()
    {
        a_src.Play();
    }

    //Убрать выбор со всех кнопок;
    //====================================================;
    public void DeselectAllGrowButtons()
    {
        for (int i = 0; i < main_buttons.Count; i++)
        {
            main_buttons[i].Deselect();
        }
    }

    //Убрать выбор со всех кнопок ставок;
    //====================================================;
    public void DeselectBetChips(int id)
    {
        foreach (BetChips _chip in betChips)
        {
            if (_chip.chip_id != id)
                _chip.Deselect();
        }
    }

    //Блокировка;
    //====================================================;
    public void LockEvent()
    {
        Debug.Log("Game Lock");
        for (int i = 0; i < betChips.Count; i++)
        {
            betChips[i].LockButton();
        }

        for (int j = 0; j < main_buttons.Count; j++)
        {
            main_buttons[j].LockButton();
        }
    }

    //Разблокировка;
    //====================================================;
    public void UnlockEvent()
    {
        for (int i = 0; i < betChips.Count; i++)
        {
            betChips[i].UnlockButton();
        }

        for (int j = 0; j < main_buttons.Count; j++)
        {
            main_buttons[j].UnlockButton();
        }
    }
}
