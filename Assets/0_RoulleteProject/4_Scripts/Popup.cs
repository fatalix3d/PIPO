using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;


public class Popup : MonoBehaviour {
    public CanvasGroup group;
    public RectTransform root;
    public Text txt;
    public Sequence growSeq;
    public bool autoClose = false;

    // Use this for initialization
    void Start() {
        group.alpha = 0;
        root.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        txt.text = "";
    }

    public void ShowPopup(string msg)
    {
        if (growSeq != null && growSeq.IsPlaying())
        {
            growSeq.Complete(false);
        }
        
        growSeq = DOTween.Sequence();

        //Hide message;
        growSeq.Insert(0f, (group.DOFade(0.0f, 0.25f)));
        growSeq.Insert(0f, (root.DOScale(0.01f, 0.25f)));

        //Update message;
        growSeq.InsertCallback(0.25f, () => { txt.text = msg; });

        //Grow message;
        growSeq.Insert(0.25f, (group.DOFade(1.0f, 0.3f)));
        growSeq.Insert(0.25f, (root.DOScale(1.2f, 0.3f)));

        //Shrink;
        if (autoClose)
        {
            growSeq.Append(root.DOScale(1.0f, 0.15f));
            growSeq.AppendInterval(1.5f);
            growSeq.Append(root.DOScale(0.01f, 0.15f));
        }
    }
}
