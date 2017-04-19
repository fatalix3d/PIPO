using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class GrowButton : MonoBehaviour {
    public RectTransform _root;
    public bool interact = false;
    public bool isSelector = false;
    private bool toggled = false;
    private Sequence growSeq;
    public Image img;
    public Sprite normal, pressed;
    public int sibIndex;
    private bool isEnabled = true;
    public Color lockColor;

    public bool selector_active = false;
    public Sprite s_active, s_idle;

    public void Grow()
    {
        if (isEnabled)
        {
            _root.SetSiblingIndex(30);
            growSeq = DOTween.Sequence();
            growSeq.Append(_root.DOScale(1.25f, 0.2f));
            growSeq.Append(_root.DOScale(1.0f, 0.2f)).OnComplete(ReturnSiblingIndex);

            if (interact)
            {
                if (!toggled)
                {
                    ButtonManager.instance.DeselectAllGrowButtons();
                    toggled = true;
                    img.sprite = pressed;
                }
                else
                {
                    toggled = false;
                    img.sprite = normal;
                }
            }

            if (isSelector)
            {
                if (!selector_active)
                {
                    selector_active = true;
                    img.sprite = s_active;
                }
                else
                {
                    selector_active = false;
                    img.sprite = s_idle;
                }
            }
        }
    }

    public void ReturnSiblingIndex()
    {
        _root.SetSiblingIndex(sibIndex);
    }

    public void SetNeighborsCount(int count)
    {
        if (GameManager.instance.game_lock == false)
        {
            GameManager.instance.n_count = count;
            GameManager.instance.SelectNeighbors();
        }
    }

    public void Deselect()
    {
        if (interact)
        {
            toggled = false;
            img.sprite = normal;
        }
    }

    public void LockButton()
    {
        isEnabled = false;
        img.DOColor(lockColor, 0.25f);
    }

    public void UnlockButton()
    {
        isEnabled = true;
        img.DOColor(Color.white, 0.25f);
    }

}
