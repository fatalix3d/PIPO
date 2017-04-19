using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PathologicalGames;
using DG.Tweening;

public class StripController : MonoBehaviour {
    public int maxNumbers;
    public Vector3 shift;
    public Transform numberInstance;
    public Transform root, mainRoot;
    public Transform row_a, row_b, row_c;
    public List<Vector3> row_a_pos = new List<Vector3>();
    public List<Vector3> row_b_pos = new List<Vector3>();
    public List<Vector3> row_c_pos = new List<Vector3>();

    public List<Transform> numbersTrans = new List<Transform>();
    public List<int> numbersColor = new List<int>();
    public List<int> numbersRow = new List<int>();

    public List<Texture2D> textures = new List<Texture2D>();
    public Transform LogoTrans;
    public Sequence logoSeq;
    private Sequence mainNumSeq;

    public List<int> red_map = new List<int>();
    public List<int> black_map = new List<int>();

    public bool gen = false;

    void Start () {
        GenRowPos(row_a);
        AnimateLogo();
    }

    public void AnimateLogo()
    {
        logoSeq = DOTween.Sequence();
        logoSeq.Append(LogoTrans.DORotate(new Vector3(0f, 90f, 0f), 0.4f, RotateMode.Fast));
        logoSeq.AppendCallback(()=> { LogoTrans.DORotate(new Vector3(0f, -90f, 0f), 0f, RotateMode.Fast); });
        logoSeq.Append(LogoTrans.DORotate(new Vector3(0f, 0f, 0f), 0.4f, RotateMode.Fast));
        logoSeq.AppendInterval(2.0f);
        logoSeq.SetLoops(-1);
    }

    public void GenRowPos(Transform root)
    {
        int childs = root.childCount;
        for (int i = 0; i < childs; i++)
        {
            row_a_pos.Add(root.GetChild(i).localPosition + shift);
            row_b_pos.Add(root.GetChild(i).localPosition);
            row_c_pos.Add(root.GetChild(i).localPosition - shift);
        }
    }


    public void PushNewNumber(int id, int row)
    {

        gen = true;

        var tNumber = PoolManager.Pools["numbers_box"].Spawn(numberInstance);
        tNumber.SetParent(mainRoot, false);
        tNumber.localPosition = root.localPosition;

        var tScript = tNumber.GetComponent<NumberScript>();
        tScript.ChangeMaterial(textures[id]);
        tScript.ShadowEnable(true);

        numbersTrans.Insert(0, tNumber);
        numbersColor.Insert(0, id);
        numbersRow.Insert(0, row);

        //Main num seq;
        tNumber.DOScale(0.01f, 0.0f);

        mainNumSeq = DOTween.Sequence();
        mainNumSeq.Insert(0f,tNumber.DOScale(1.85f, 0.45f));
        mainNumSeq.Insert(0f,tNumber.DORotate(new Vector3(0f, 180f, 0f), 0.45f, RotateMode.FastBeyond360)).OnComplete(()=> { gen = false; });
        mainNumSeq.Insert(0.45f, tNumber.DOScale(1.7f, 0.25f));


        //Move old numbers;
        if (numbersTrans.Count > 0)
        {
            for (int i = 1; i < numbersTrans.Count; i++)
            {
                if (i < maxNumbers)
                {
                    if (i == 1)
                    {
                        numbersTrans[i].DORotate(new Vector3(0f, 360f, 0f), 0.45f, RotateMode.FastBeyond360);
                    }

                    numbersTrans[i].DOScale(0.85f, 0.45f);
                    numbersTrans[i].GetComponent<NumberScript>().ShadowEnable(false);

                    switch (numbersRow[i])
                    {
                        case 0:
                            numbersTrans[i].SetParent(row_a, true);
                            numbersTrans[i].DOLocalMove(row_a_pos[i - 1], 0.45f).OnComplete(()=> { gen = false; });
                            break;
                        case 1:
                            numbersTrans[i].SetParent(row_b, true);
                            numbersTrans[i].DOLocalMove(row_b_pos[i - 1], 0.45f).OnComplete(() => { gen = false; }); 
                            break;
                        case 2:
                            numbersTrans[i].SetParent(row_c, true);
                            numbersTrans[i].DOLocalMove(row_c_pos[i - 1], 0.45f).OnComplete(() => { gen = false; }); 
                            break;
                    }
                }
                else
                {
                    numbersTrans[i].DOScale(0.01f, 0.3f).OnComplete(()=>
                    {
                        PoolManager.Pools["numbers_box"].Despawn(numbersTrans[i]);
                        numbersTrans.RemoveAt(i);
                        gen = false;
                    });
                    break;

                }
            }
        }
    }

	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (gen == false)
            {
                int num_t = Random.RandomRange(0, 37);
                int row_t = 0;
                bool found = false;
                if (!found)
                {
                    if (num_t == 0)
                    {
                        row_t = 1;
                        found = true;
                    }
                }

                if (!found)
                {
                    for (int i = 0; i < red_map.Count; i++)
                    {
                        if(num_t == red_map[i])
                        {
                            row_t = 0;
                            found = true;
                            break;
                        }
                    }
                }

                if (!found)
                {
                    for (int i = 0; i < black_map.Count; i++)
                    {
                        if (num_t == black_map[i])
                        {
                            row_t = 2;
                            found = true;
                            break;
                        }
                    }
                }

                PushNewNumber(num_t, row_t);
            }
        }
	}
}
