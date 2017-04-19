using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CloudWin : MonoBehaviour {

    public Transform cloud;
    public Image img;
    public Text val;
    public Sprite red, green, black;
    public Sequence cloudSeq;

    public void ShowWinNumber(string color_, int value)
    {
        if (cloudSeq != null)
        {
            cloudSeq.Complete();
        }

        cloudSeq = DOTween.Sequence();
        cloudSeq.Append(cloud.DOScale(0.01f, 0.15f));

        cloudSeq.AppendCallback(() => {
            switch (color_)
            {
                case "red":
                    img.sprite = red;
                    break;
                case "green":
                    img.sprite = green;
                    break;
                case "black":
                    img.sprite = black;
                    break;
            }

            val.text = value.ToString();

        });
        cloudSeq.Append(cloud.DOScale(1.25f,0.4f));
        cloudSeq.Append(cloud.DOScale(1.0f,0.2f));
    }
}
