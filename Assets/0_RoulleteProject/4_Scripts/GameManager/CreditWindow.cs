using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CreditWindow : MonoBehaviour {
    private Button myButton;
    private bool grow = false;
    public RectTransform root;
    public Text txt;

    void Start () {
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(() => MyClick());
    }
	
	void MyClick() {
        if (!grow)
        {
            root.DOSizeDelta(new Vector2(0.0f, 78.0f), 0.5f).SetEase(Ease.OutExpo);
            txt.DOFade(1.0f, 0.6f).SetEase(Ease.OutExpo);
            grow = true;
        }
        else
        {
            root.DOSizeDelta(new Vector2(0.0f, 0.0f), 0.5f).SetEase(Ease.OutExpo);
            txt.DOFade(1.0f, 0.0f).SetEase(Ease.OutExpo);
            grow = false;
        }
    }
}
