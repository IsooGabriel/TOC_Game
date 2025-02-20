﻿using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UISystem_Gabu : ColorSystem
{
    #region 変数
    protected bool _isButton = false;
    protected bool _isEexecute = true;
    protected int _i_currentAnimation = 0;
    protected int _i_lastAnimation = 0;

    [SerializeField]
    protected Animator _animator;

    [SerializeField]
    protected Transform _transform;
    [SerializeField]
    protected Vector3 _unitPosition;
    [SerializeField]
    protected Vector3 _unitRotation;
    [SerializeField]
    protected Vector3 _unitScale;

    [SerializeField, Header("基本色")]
    protected Color _color = new Color(0.75f, 0.75f, 0.75f);

    [SerializeField, Header("自動で色の彩度、明度を変更")]
    protected bool _isAutoColor = false;
    [SerializeField, Header("S(彩度)gaが変更されなくなる")]
    protected bool _isMonochrome = true;

    [SerializeField, Header("Disabledの時に使われる画像")]
    protected Image _disabledImage;

    [SerializeField, Header("実行時リセット")]
    protected bool _isReset = false;
    [SerializeField, Header("--- 各種アニメーション変数 ---")]
    protected float _normalScaleDuration = 0.4f;
    [SerializeField]
    protected float _highlightedScaleDuration = 0.2f;
    [SerializeField]
    protected float _pressedScaleDuration = 0f;
    [SerializeField]
    protected float _selectedScaleDuration = 0.2f;
    [SerializeField]
    protected float _disabledScaleDuration = 0.05f;

    [SerializeField]
    protected Ease _normalEase = Ease.InOutSine;
    [SerializeField]
    protected Ease _highlightedEase = Ease.OutBack;
    [SerializeField]
    protected Ease _pressedEase = Ease.InBack;
    [SerializeField]
    protected Ease _selectedEase = Ease.Linear;
    [SerializeField]
    protected Ease _disabledEase = Ease.Linear;

    [SerializeField]
    protected float _normalScaleMultiplier = 1.0f;
    [SerializeField]
    protected float _highlightedScaleMultiplier = 1.1f;
    [SerializeField]
    protected float _pressedScaleMultiplier = 1.05f;
    [SerializeField]
    protected float _selectedScaleMultiplier = 1.1f;
    [SerializeField]
    protected float _disabledScaleMultiplier = 1.0f;

    protected enum AnimatorState : int
    {
        Normal = 0, Highlighted, Pressed, Selected, Disabled
    }
    #endregion

    #region 関数
    /// <summary>
    /// Animatorの再生中のアニメーションを確認します。
    /// </summary>
    /// <returns></returns>
    protected int CheckAnimationState()
    {
        if (_isButton || _animator == null)
        {
            return (int)AnimatorState.Normal;
        }

        foreach (string state in Enum.GetNames(typeof(AnimatorState)))
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName(state))
            {
                return (int)Enum.Parse(typeof(AnimatorState), state);
            }
        }

        return (int)AnimatorState.Normal;
    }


    protected virtual void NormalAnimation()
    {
        if (_i_currentAnimation == _i_lastAnimation)
        {
            return;
        }
        _transform.DOScale(_unitScale, _normalScaleDuration).SetEase(_normalEase);
    }

    protected virtual void HighlightedAnimation()
    {
        if (_i_currentAnimation == _i_lastAnimation)
        {
            return;
        }

        if (_isMonochrome)
        {
            _transform.DOScale(_unitScale * _highlightedScaleMultiplier, _highlightedScaleDuration).SetEase(_highlightedEase);
        }
        else
        {
            _transform.DOScale(_unitScale * _highlightedScaleMultiplier, _highlightedScaleDuration).SetEase(_highlightedEase);
        }
    }

    protected virtual void PressedAnimation()
    {
        if (_i_currentAnimation == _i_lastAnimation)
        {
            return;
        }

        if (_isMonochrome)
        {
            _transform.DOScale(_unitScale * _pressedScaleMultiplier, _pressedScaleDuration).SetEase(_pressedEase);
        }
        else
        {
            _transform.DOScale(_unitScale * _pressedScaleMultiplier, _pressedScaleDuration).SetEase(_pressedEase);
        }
    }

    protected virtual void SelectedAnimation()
    {
        if (_i_currentAnimation == _i_lastAnimation)
        {
            return;
        }

        if (_isMonochrome)
        {
            _transform.DOScale(_unitScale * _selectedScaleMultiplier, _selectedScaleDuration).SetEase(_selectedEase);
        }
        else
        {
            _transform.DOScale(_unitScale * _selectedScaleMultiplier, _selectedScaleDuration).SetEase(_selectedEase);
        }
    }

    protected virtual void DisabledAnimation()
    {
        if (_i_currentAnimation == _i_lastAnimation)
        {
            return;
        }

        if (_isMonochrome)
        {
            _transform.DOScale(_unitScale, _disabledScaleDuration).SetEase(_disabledEase);
        }
        else
        {
            _transform.DOScale(_unitScale, _disabledScaleDuration).SetEase(_disabledEase);
        }
    }
    #endregion

    protected virtual void Start()
    {
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
        if (_transform == null)
        {
            _transform = GetComponent<Transform>();
        }
        if (_unitPosition != _transform.position)
        {
            _unitPosition = _transform.position;
        }
        if (_unitRotation != _transform.eulerAngles)
        {
            _unitRotation = _transform.eulerAngles;
        }
        if (_unitScale != _transform.localScale)
        {
            _unitScale = _transform.localScale;
        }
        if (_isAutoColor)
        {
            _color = ChengeHSV(_color, s: 0.65f, v: 0.7f);
        }

    }

    protected virtual void Update()
    {
        if (_animator == null)
        {
            return;
        }
        
        _i_currentAnimation = CheckAnimationState();
        switch (_i_currentAnimation)
        {
            case (int)AnimatorState.Normal:
                NormalAnimation();
                break;
            case (int)AnimatorState.Highlighted:
                HighlightedAnimation();
                break;
            case (int)AnimatorState.Pressed:
                PressedAnimation();
                break;
            case (int)AnimatorState.Selected:
                SelectedAnimation();
                break;
            case (int)AnimatorState.Disabled:
                DisabledAnimation();
                break;
            default:
                Debug.LogWarning("予期しないアニメーションが参照されました");
                break;
        }
        _i_lastAnimation = _i_currentAnimation;
    }
}
