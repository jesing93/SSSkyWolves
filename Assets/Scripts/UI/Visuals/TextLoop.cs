using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class TextLoop : MonoBehaviour
{
    private TMP_Text textFade;
    

    // Start is called before the first frame update
    void Awake()
    {
        textFade = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Start()
    {
         textFade.DOFade(20, 3f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
        //textFade.DOColor(new Color(220f, 220f, 220f), 2f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
    }
}
