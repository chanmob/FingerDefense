using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIDoTween : Singleton<UIDoTween>
{
    public void UITweenX(RectTransform _rt, float _posX, float _time, Ease _ease)
    {
        _rt.DOAnchorPosX(_posX, _time).SetEase(_ease);
    }

    public void UITweenY(RectTransform _rt, float _posY, float _time, Ease _ease)
    {
        _rt.DOAnchorPosY(_posY, _time).SetEase(_ease);
    }
}
