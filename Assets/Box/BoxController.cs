using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoxController : MonoBehaviour {
    public Sequence seq;
    public Transform root;
	// Use this for initialization
	void Start () {
        seq = DOTween.Sequence();
        seq.Append(root.DORotate(new Vector3(0f, 180f, 0f), 0.75f, RotateMode.FastBeyond360).SetEase(Ease.InOutExpo));
        seq.AppendInterval(2.0f);
        seq.Append(root.DORotate(new Vector3(0f, 360f, 0f), 0.75f, RotateMode.FastBeyond360).SetEase(Ease.InOutExpo));
        seq.AppendInterval(2.0f);
        seq.SetLoops(-1);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
