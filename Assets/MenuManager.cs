using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuManager : MonoBehaviour {
    #region vars
    public List<Transform> but_trans = new List<Transform>();
    public List<Image> selection_glow = new List<Image>();
    public List<Text> selection_text = new List<Text>();
    public List<Vector3> but_rot = new List<Vector3>();
    public List<Color> but_colors = new List<Color>();
    public List<Image> but_img = new List<Image>();
    public List<CanvasGroup> info_cloud = new List<CanvasGroup>();
    public List<bool> selected = new List<bool>();
    public bool rot_enb = false;
    public Sequence selSeq;

    public CanvasGroup menuCanvas;
    public Transform menuRoot;

    public CanvasGroup buyCanvas;
    public Transform buyRoot;

    public Sequence transfer_seq;
    public CanvasGroup transferGroup;
    public List<Transform> load_img = new List<Transform>();
    public uint p_balance, p_credits, t_summ;
    public Text p_credits_text, t_add_summ_text;
    public string cur_summ;

    public List<Text> p_balance_text = new List<Text>();
    public CanvasGroup transactionMenu, addCreditsMenu;
    public Button getCreditBtn, accepTransaction;

    #endregion

    void Start()
    {
        addCreditsMenu.transform.DOLocalMoveX(1000f, 0.45f);
        load_img[0].DOLocalRotate(new Vector3(0f, 0f, 359f),3.5f, RotateMode.LocalAxisAdd).SetLoops(-1).SetEase(Ease.Linear);
        load_img[1].DOLocalRotate(new Vector3(0f, 0f, -359f),3.5f, RotateMode.LocalAxisAdd).SetLoops(-1).SetEase(Ease.Linear);
        UpdateLabels();
    }


    public void EnterToRoom(int index)
    {
        ShowPayMenu();
    }

    //Update labels;
    public void UpdateLabels()
    {
        //Текущий кредит клиента;
        t_summ = 0;
        cur_summ = t_summ.ToString();

        p_credits_text.text = string.Format("{0:###,##0 KZT}", p_credits); 
        t_add_summ_text.text = string.Format("{0:###,##0 KZT}", 0);

        foreach (Text tx in p_balance_text)
        {
            tx.text = string.Format("{0:###,##0 KZT}", p_balance);
        }

        if (p_credits > 0)
        {
            getCreditBtn.interactable = true;
        }
        else
        {
            getCreditBtn.interactable = false;
        }
    }

    //Транзацкция;
    public void TransferMoney(int mode)
    {
        //Kill prev seq;
        if (transfer_seq != null)
        {
            transfer_seq.Kill(false);
        }

        switch (mode)
        {
            case 0:
                //ADD TO CREDIT;
                //====================================;
                if (t_summ > 0 && t_summ <= p_balance)
                {
                    transfer_seq = DOTween.Sequence();
                    transfer_seq.Append(transferGroup.DOFade(1.0f, 0.5f));
                    transfer_seq.AppendCallback(() =>
                    {
                        p_balance -= t_summ;
                        p_credits += t_summ;
                        t_summ = 0;
                    });
                    transfer_seq.AppendInterval(2.0f);
                    transfer_seq.Append(transferGroup.DOFade(0.0f, 0.5f)).OnComplete(() =>
                    {
                        UpdateLabels();
                        ShowAddMenu(false);
                    });
                }
                break;

            case 1:
                //ADD TO BALANCE;
                //====================================;
                if (p_credits > 0)
                {
                    transfer_seq = DOTween.Sequence();
                    transfer_seq.Append(transferGroup.DOFade(1.0f, 0.5f));
                    transfer_seq.AppendCallback(() =>
                    {
                        p_balance += p_credits;
                        p_credits = 0;
                    });
                    transfer_seq.AppendInterval(2.0f);
                    transfer_seq.Append(transferGroup.DOFade(0.0f, 0.5f)).OnComplete(() =>
                    {
                        UpdateLabels();
                    });
                }
                break;
        }
    }

    //Очистить поле ввода;
    public void ClearNum()
    {
        t_add_summ_text.text = string.Format("{0:###,##0 KZT}", 0);
    }

    //Сумма = макс. балансу;
    public void SetAllBalance()
    {
        if (p_balance > 0)
        {
            t_summ = 0;
            t_summ = p_balance;
            t_add_summ_text.text = string.Format("{0:###,##0 KZT}", t_summ); //Текущий баланс операции;
        }
    }

    //Ввод суммы для операции;
    public void EnterNum(int num)
    {
        if (cur_summ[0].ToString() == "0")
        {
            cur_summ = "";
        }

        if (cur_summ.Length < 9)
        {
            cur_summ += num.ToString();
        }

        t_summ = 0;
        t_summ = uint.Parse(cur_summ);

        if (t_summ <= p_balance)
        {
            cur_summ = t_summ.ToString();
            t_add_summ_text.text = string.Format("{0:###,##0 KZT}", t_summ); //Текущий баланс операции;
        }
        else
        {
            t_summ = p_balance;
            cur_summ = t_summ.ToString();
            t_add_summ_text.text = string.Format("{0:###,##0 KZT}", (t_summ)); //Текущий баланс операции;
        }
    }

    //Show add credit window;
    //===============================;
    public void ShowAddMenu(bool show)
    {
        if (show)
        {
            //Fade transaction menu;
            transactionMenu.blocksRaycasts = false;
            transactionMenu.transform.DOLocalMoveX(-1000f, 0.45f);
            transactionMenu.DOFade(0.0f, 0.45f);

            //Show add credits menu;
            addCreditsMenu.blocksRaycasts = true;
            addCreditsMenu.transform.DOLocalMoveX(0f, 0.45f);
            addCreditsMenu.DOFade(1.0f, 0.45f);

            if (show)
            {

            }
        }
        else
        {
            //Show transaction menu;
            transactionMenu.blocksRaycasts = true;
            transactionMenu.transform.DOLocalMoveX(0f, 0.45f);
            transactionMenu.DOFade(1.0f, 0.45f);

            //Fade add credit menu;
            addCreditsMenu.blocksRaycasts = false;
            addCreditsMenu.transform.DOLocalMoveX(1000f, 0.45f);
            addCreditsMenu.DOFade(0.0f, 0.45f);

            //Clear all data;
            t_summ = 0;
            cur_summ = t_summ.ToString();
            t_add_summ_text.text = string.Format("{0:###,##0 KZT}", t_summ);
        }
    }

    public void ShowPayMenu()
    {
        menuCanvas.DOFade(0.10f, 0.45f).SetEase(Ease.InOutQuad);
        menuRoot.DOLocalMoveZ(1000f, 0.45f).SetEase(Ease.InOutQuad);

        buyCanvas.DOFade(1.0f, 0.45f).SetEase(Ease.InOutQuad);
        buyRoot.DOLocalMoveZ(0f, 0.45f).SetEase(Ease.InOutQuad);

        buyCanvas.blocksRaycasts = true;
    }

    public void HidePayMenu()
    {
        Debug.Log("XDA");
        menuCanvas.DOFade(1.0f, 0.45f).SetEase(Ease.InOutQuad);
        menuRoot.DOLocalMoveZ(0f, 0.45f).SetEase(Ease.InOutQuad);

        buyCanvas.DOFade(0.0f, 0.45f).SetEase(Ease.InOutQuad);
        buyRoot.DOLocalMoveZ(-400f, 0.45f).SetEase(Ease.InOutQuad);
        buyCanvas.blocksRaycasts = false;
    }

    public void LoadLevel()
    {
        //Debug.Log("Load level #1");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ShowPayMenu();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            UpdateLabels();
        }
    }

    public void SelectRoullete(int index)
    {
        for(int i = 0; i < but_trans.Count; i++)
        {
            if (index == i)
            {
                but_trans[i].DOScale(1.1f, 0.35f).SetEase(Ease.InOutSine);
                but_trans[i].DOLocalMoveZ(25f, 0.35f).SetEase(Ease.InOutSine);
                selection_glow[i].DOFade(1.0f, 0.35f).SetEase(Ease.InOutSine);
                selection_text[i].DOFade(1.0f, 0.45f).SetEase(Ease.InOutSine);
                info_cloud[i].DOFade(1.0f, 0.35f).SetEase(Ease.InOutSine);
                but_img[i].DOColor(but_colors[0], 0.35f).SetEase(Ease.InOutSine);

                if (rot_enb) { but_trans[i].DORotate(but_rot[1], 0.35f).SetEase(Ease.InOutSine); }

                if (selSeq != null)
                {
                    selSeq.Kill(false);
                }

                if (!selected[i]) {
                    selected[i] = true;
                }
                else
                {
                    LoadLevel();
                }
            }
            else
            {
                but_trans[i].DOScale(1.0f, 0.35f).SetEase(Ease.InOutSine);
                but_trans[i].DOLocalMoveZ(100f, 0.35f).SetEase(Ease.InOutSine);
                selection_glow[i].DOFade(0.0f, 0.35f).SetEase(Ease.InOutSine);
                selection_text[i].DOFade(0.20f, 0.35f).SetEase(Ease.InOutSine);
                but_img[i].DOColor(but_colors[1], 0.35f).SetEase(Ease.InOutSine);
                info_cloud[i].DOFade(0.0f, 0.35f).SetEase(Ease.InOutSine);
                selected[i] = false;

                if (rot_enb)
                {
                    but_trans[i].DORotate(but_rot[i], 0.35f).SetEase(Ease.InOutSine);
                }
            }
        }
    }
}
