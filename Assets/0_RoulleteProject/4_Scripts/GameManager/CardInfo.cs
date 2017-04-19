using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class CardInfo : MonoBehaviour {
    public static CardInfo instance;
    public Image glow_img;
    public Transform root;
    public CanvasGroup cardCanvas;
    public Text card_num;
    public Int64 card_id;
    public int card_type;
    public Sequence stateSeq;
    public List<Color> colors_ = new List<Color>();

    void Awake()
    {
        instance = this;
    }

    //Show Card Info;
    public void ShowCardInfo(int cardType, Int64 cardId, bool show)
    {
        if (stateSeq != null)
        {
            stateSeq.Kill();
        }

        stateSeq = DOTween.Sequence();
        if (show)
        {
            stateSeq.Append(cardCanvas.DOFade(0.0f, 0.25f).OnComplete(() =>
            {
                switch (cardType)
                {
                    case 0:
                        glow_img.color = colors_[0];
                        break;
                    case 1:
                        glow_img.color = colors_[1];
                        break;
                    case 2:
                        glow_img.color = colors_[2];
                        break;
                    case 3:
                        glow_img.color = colors_[3];
                        break;
                    case 4:
                        glow_img.color = colors_[4];
                        break;
                    case 5:
                        glow_img.color = colors_[5];
                        break;
                }
                card_num.text = String.Format("{0:0000 0000 0000}", cardId);
            }));
            stateSeq.AppendInterval(0.25f);
            stateSeq.Append(cardCanvas.DOFade(1.0f, 0.5f));
        }
        else
        {
            stateSeq.AppendInterval(0.25f);
            stateSeq.Append(cardCanvas.DOFade(0.0f, 0.5f));
        }
    }
}
