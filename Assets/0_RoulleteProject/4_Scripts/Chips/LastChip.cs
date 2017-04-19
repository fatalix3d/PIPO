using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LastChip : MonoBehaviour {
    public Transform glow_t, center_t, base_t;
    public Image glow_i, center_i, base_i;

    public Sequence glowSeq, centerSeq, centerSeqTrans, b;
    public Color color_a, color_b;

    void Start()
    {
        StartPulse();
    }

    public void StartPulse()
    {
        //GLOW;
        glowSeq = DOTween.Sequence();
        glowSeq.Append(glow_i.DOFade(0.15f, 0.75f));
        glowSeq.Append(glow_i.DOFade(1.0f, 0.3f));
        glowSeq.SetLoops(-1);

        //CENTER;
        centerSeq = DOTween.Sequence();
        centerSeq.Append(center_i.DOColor(color_a, 0.75f));
        centerSeq.Append(center_i.DOColor(color_b, 0.3f));
        centerSeq.SetLoops(-1);

        //Center Seq;
        centerSeqTrans = DOTween.Sequence();
        centerSeqTrans.Append(center_t.DOScale(0.75f, 0.75f));
        centerSeqTrans.Append(center_t.DOScale(1.0f, 0.3f));
        centerSeqTrans.SetLoops(-1);
    }
}
