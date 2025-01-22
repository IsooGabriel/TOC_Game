using DG.Tweening;
using System;
using UnityEngine;

public class ButtonOutlineAnimation_Gabu : ImageAnimation_Gabu
{
    #region 変数
    protected Ease ease = Ease.OutBack;
    protected readonly float _highAlpha = 0.7f;
    #endregion

    #region 関数
    protected override void NormalAnimation()
    {
        if (_i_currentAnimation == _i_lastAnimation)
        {
            return;
        }

        ease = (Ease)UnityEngine.Random.Range(0, Enum.GetNames(typeof(Ease)).Length);
        ease = ease == Ease.INTERNAL_Custom ? Ease.OutBack : ease;

        image.DOColor(new Color(_color.r, _color.g, _color.b, 0), duration: 0.4f).SetEase(ease);
    }


    protected override void HighlightedAnimation()
    {
        if (_i_currentAnimation == _i_lastAnimation)
        {
            return;
        }

        ease = (Ease)UnityEngine.Random.Range(0, Enum.GetNames(typeof(Ease)).Length);
        ease = ease == Ease.INTERNAL_Custom ? Ease.OutBack : ease;

        image.DOColor(new Color(_color.r, _color.g, _color.b, _highAlpha), duration: 0.4f).SetEase(ease);
    }

    protected override void PressedAnimation()
    {
        if (_i_currentAnimation == _i_lastAnimation)
        {
            return;
        }

        ease = (Ease)UnityEngine.Random.Range(0, Enum.GetNames(typeof(Ease)).Length);
        ease = ease == Ease.INTERNAL_Custom ? Ease.OutBack : ease;

        image.DOColor(new Color(_color.r, _color.g, _color.b, 1), duration: 0.4f).SetEase(ease);
    }

    protected override void SelectedAnimation()
    {
        if (_i_currentAnimation == _i_lastAnimation)
        {
            return;
        }
        HighlightedAnimation();
    }

    protected override void DisabledAnimation()
    {
        if (_i_currentAnimation == _i_lastAnimation)
        {
            return;
        }

        ease = (Ease)UnityEngine.Random.Range(0, Enum.GetNames(typeof(Ease)).Length);
        ease = ease == Ease.INTERNAL_Custom ? Ease.OutBack : ease;

        image.DOColor(new Color(Color.gray.a, Color.gray.g, Color.gray.b, _highAlpha), duration: 0.4f).SetEase(ease);
    }
    #endregion 
}
