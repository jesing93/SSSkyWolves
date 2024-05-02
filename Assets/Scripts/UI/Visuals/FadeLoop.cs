using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class FadeLoop : MonoBehaviour
{
    enum Type { Image, Text};
    [SerializeField] private Type type;

    private Image panelFade;
    private TextMeshProUGUI title;
    public float fadeDuration = 7.5f;
    public float finalFade = 0f;
    public Ease ease = Ease.InOutQuad;

    // Start is called before the first frame update
    void Awake()
    {
        if (type == Type.Image)  panelFade = GetComponent<Image>();
        if (type == Type.Text)  title = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Start()
    {
        
        if(type == Type.Image) panelFade.DOFade(finalFade, fadeDuration).SetEase(ease).SetLoops(-1, LoopType.Yoyo);
        if (type == Type.Text) title.DOFade(finalFade, fadeDuration).SetEase(ease).SetLoops(-1, LoopType.Yoyo);
    }
}
