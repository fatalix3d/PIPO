using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PathologicalGames;
using DG.Tweening;


public class WinStrip : MonoBehaviour {
    public static WinStrip instance;

    public List<Transform> numbers = new List<Transform>();
    public List<string> colors = new List<string>();
    public RectTransform root;
    public Transform winNumber;
    private Vector2 minSize = new Vector2(0.0f,0.0f);
    private float timer = 0;

    public CloudWin cloud;

    void Awake()
    {
        instance = this;
    }

    //Add win number to strip;
    //====================================================;
    public void AddWinNumber(int num, int c_win, bool win_flag, bool sib)
    {
        string _color = "";
        switch (colors[num])
        {
            case "r":
                _color = "red";
                break;
            case "g":
                _color = "green";
                break;
            case "b":
                _color = "black";
                break;
        }

        if (win_flag)
        {
            SoundManager.instance.PlaySound(2);
            cloud.ShowWinNumber(_color, c_win);
        }
        else
        {
            DOTween.To(() => timer, x => timer = x, 1, 0.4f).OnComplete(() =>
            {
                Transform t_win = PoolManager.Pools["chips_pool"].Spawn(winNumber);
                t_win.SetParent(root);
                t_win.localScale = new Vector2(1.0f, 1.0f);
                t_win.localPosition = root.localPosition;

                if (sib)
                {
                    numbers.Insert(0,t_win);
                    t_win.SetSiblingIndex(0);
                }
                else
                {
                    numbers.Add(t_win);
                }

                t_win.GetComponent<WinNum>().ShowWinNumber(_color, num);
            });
        }
    }

    //Remove first <end> cell;
    //====================================================;
    public void RemoveFirstNum()
    {
        if(numbers.Count > 20)
        {
            int last = numbers.Count - 1;
            numbers[last].GetComponent<WinNum>().l_elem.DOMinSize(minSize, 0f);
            PoolManager.Pools["chips_pool"].Despawn(numbers[last]);
            numbers.RemoveAt(last);
        }
    }
}
