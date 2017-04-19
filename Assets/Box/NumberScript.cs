using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NumberScript : MonoBehaviour {
    public Renderer faceMat;
    public MeshRenderer shadow;

    public void ChangeMaterial(Texture2D tex)
    {
        faceMat.material.mainTexture = tex;
    }

    public void ShadowEnable(bool flag)
    {
        if (flag)
        {
            shadow.enabled = flag;
            shadow.material.DOFade(0.5f, 0.3f);
        }
        else
        {
            shadow.material.DOFade(0.0f, 0.25f).OnComplete(()=> {
                shadow.enabled = flag;
            });
        }
    }
}
