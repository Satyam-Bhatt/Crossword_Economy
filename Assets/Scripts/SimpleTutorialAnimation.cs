using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SimpleTutorialAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Sequence sequence = DOTween.Sequence();

        RectTransform rectTransform = GetComponent<RectTransform>();

        transform.localScale = Vector3.zero;

        sequence.AppendInterval(0.2f);
        sequence.Append(transform.DOScale(Vector3.one * 1.5f, 0.5f).SetEase(Ease.OutBack));

        sequence.Play();
    }
}
