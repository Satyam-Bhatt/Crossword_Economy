using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SimpleAnimation : MonoBehaviour
{
    public Vector3 startPositon;
    public Vector3 endPosition;
    // Start is called before the first frame update
    void Start()
    {
        Sequence sequence = DOTween.Sequence();

        RectTransform rectTransform = GetComponent<RectTransform>();

        endPosition = rectTransform.anchoredPosition;
        rectTransform.anchoredPosition = startPositon;
        transform.localScale = Vector3.zero;

        sequence.AppendInterval(0.5f);
        sequence.Append(transform.DOScale(Vector3.one * 1.5f, 0.5f).SetEase(Ease.OutBack));
        sequence.AppendInterval(0.5f);
        sequence.AppendCallback(() =>
        {
            rectTransform.DOAnchorPos(endPosition, 1f).SetEase(Ease.OutExpo);
            transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutExpo);
        });

        sequence.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
