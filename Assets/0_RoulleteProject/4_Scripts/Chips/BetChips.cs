using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class BetChips : MonoBehaviour {
    #region vars
    private Button myButton;
    public Image glow;
    public int value;
    public bool isSelected;
    public int chip_id;
    public RectTransform root;

    public bool _chip_active = true;
    public Image chip_img;
    public Sprite _norm, _lock;
    public Text chip_text;
    public Sequence lockSeq;
    public Color _fadeColor;
    #endregion
    // Use this for initialization
    //======================================;
    void Start () {
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(() => MyClick());
    }

    //CLICK EVENT;
    //======================================;
    public void MyClick()
    {
        if (_chip_active)
        {
            if (!isSelected)
            {
                glow.DOFade(1.0f, 0.3f);
                isSelected = true;
                ButtonManager.instance.DeselectBetChips(chip_id);
                GameManager.instance.cur_bet_value = value;
                GameManager.instance.cur_chip_selection = chip_id;
            }
            else
            {
                Sequence grow_seq = DOTween.Sequence();
                grow_seq.Append(root.DOScale(1.3f, 0.15f));
                grow_seq.Append(root.DOScale(1.0f, 0.15f));
                GameManager.instance.cur_bet_value = value;
                GameManager.instance.ThrowBet();
            }
        }

        SoundManager.instance.PlaySound(1);
    }

    public void SelectChip()
    {
        if (_chip_active)
        {
            if (!isSelected)
            {
                glow.DOFade(1.0f, 0.3f);
                isSelected = true;
                ButtonManager.instance.DeselectBetChips(chip_id);
                GameManager.instance.cur_bet_value = value;
                GameManager.instance.cur_chip_selection = chip_id;
            }
            else
            {
                Sequence grow_seq = DOTween.Sequence();
                grow_seq.Append(root.DOScale(1.3f, 0.15f));
                grow_seq.Append(root.DOScale(1.0f, 0.15f));
                GameManager.instance.cur_bet_value = value;
            }
        }
    }

    //DESELECT EVENT;
    //======================================;
    public void Deselect()
    {
        isSelected = false;
        glow.DOFade(0.0f, 0.3f);
    }

    //LOCK BUTTON;
    //======================================;
    public void LockButton()
    {
        //Deselect all bet chips;
        ButtonManager.instance.DeselectBetChips(99);

        _chip_active = false;
        lockSeq = DOTween.Sequence();
        lockSeq.Append(root.DORotate(new Vector3(0.0f, 90.0f, 0.0f), 0.15f, RotateMode.Fast));
        lockSeq.AppendCallback(SwitchImage);
        lockSeq.Append(root.DORotate(new Vector3(0.0f, 180.0f, 0.0f), 0.15f, RotateMode.Fast));
        lockSeq.Insert(0.25f, chip_img.DOColor(_fadeColor, 0.25f));
    }

    //UNLOCK BUTTON;
    //======================================;
    public void UnlockButton()
    {
        _chip_active = true;
        lockSeq = DOTween.Sequence();
        lockSeq.Append(root.DORotate(new Vector3(0.0f, 270.0f, 0.0f), 0.15f, RotateMode.Fast));
        lockSeq.AppendCallback(SwitchImage);
        lockSeq.Append(root.DORotate(new Vector3(0.0f, 0.0f, 0.0f), 0.15f, RotateMode.Fast));
        lockSeq.Insert(0.25f, chip_img.DOColor(Color.white, 0.25f));
    }

    //SWITCH BUTTON;
    //======================================;
    public void SwitchImage()
    {
        if (!_chip_active)
        {
            chip_img.sprite = _lock;
            chip_text.enabled = false;
        }
        else
        {
            chip_img.sprite = _norm;
            chip_text.enabled = true;
        }
    }
}
