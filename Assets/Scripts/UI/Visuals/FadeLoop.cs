using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeLoop : MonoBehaviour
{
    private Image panelFade;
    public float fadeDuration = 7.5f;
    public float finalFade = 0f;
    public Ease ease = Ease.InOutQuad;

    // Start is called before the first frame update
    void Awake()
    {
        panelFade = GetComponent<Image>();
    }

    // Update is called once per frame
    void Start()
    {
        panelFade.DOFade(finalFade, fadeDuration).SetEase(ease).SetLoops(-1, LoopType.Yoyo);
    }
}
