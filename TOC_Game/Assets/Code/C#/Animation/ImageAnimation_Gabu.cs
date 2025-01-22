using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimation_Gabu : UISystem_Gabu
{
    #region 変数
    protected Image image;

    protected Color _normalColor;
    protected Color _highlightedColor;
    protected Color _pressedColor;
    protected Color _selectedColor;
    protected Color _disabledColor;

    #endregion

    #region 関数
    protected override void NormalAnimation()
    {
        if (_i_currentAnimation == _i_lastAnimation)
        {
            return;
        }
        _disabledImage.color = new Color(1f, 1f, 1f, 0f);

        _transform.DOScale(_unitScale * _normalScaleMultiplier, _normalScaleDuration).SetEase(_normalEase);
        image.DOColor(_normalColor, _normalScaleDuration);
    }

    protected override void HighlightedAnimation()
    {
        if (_i_currentAnimation == _i_lastAnimation)
        {
            return;
        }
        _disabledImage.color = new Color(1f, 1f, 1f, 0f);

        _transform.DOScale(_unitScale * _highlightedScaleMultiplier, _highlightedScaleDuration).SetEase(_highlightedEase);
        image.DOColor(_isMonochrome ? SubtractionHSV(_color, 0f, 1f, -0.4f) : SubtractionHSV(_color, 0f, -0.4f, -0.4f), _highlightedScaleDuration);
    }

    protected override void PressedAnimation()
    {
        if (_i_currentAnimation == _i_lastAnimation)
        {
            return;
        }
        _disabledImage.color = new Color(1f, 1f, 1f, 0f);

        _transform.DOScale(_unitScale * _pressedScaleMultiplier, _pressedScaleDuration).SetEase(_pressedEase);
        image.DOColor(_isMonochrome ? SubtractionHSV(_color, 0f, 1f, 0.5f) : SubtractionHSV(_color, 0f, -0.2f, 0.5f), _pressedScaleDuration);
    }

    protected override void SelectedAnimation()
    {
        if (_i_currentAnimation == _i_lastAnimation)
        {
            return;
        }
        _disabledImage.color = new Color(1f, 1f, 1f, 0f);

        _transform.DOScale(_unitScale * _selectedScaleMultiplier, _selectedScaleDuration).SetEase(_selectedEase);
        image.DOColor(_isMonochrome ? SubtractionHSV(_color, 0f, 1f, -0.2f) : SubtractionHSV(_color, 0f, -0.2f, -0.2f), _selectedScaleDuration);
    }

    protected override void DisabledAnimation()
    {
        if (_i_currentAnimation == _i_lastAnimation)
        {
            return;
        }

        _transform.DOScale(_unitScale * _disabledScaleMultiplier, _disabledScaleDuration).SetEase(_disabledEase);
        image.DOColor(_isMonochrome ? SubtractionHSV(_color, 0f, 1f, 0.7f) : SubtractionHSV(_color, 0f, 0.7f, 0.7f), _disabledScaleDuration);

        _disabledImage.DOColor(new Color(1, 1, 1, 0.6f), _disabledScaleDuration).SetEase(_disabledEase);
    }
    #endregion

    // ヌルチェック、数値代入、色代入
    protected override void Start()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }

        base.Start();

        if (!_isReset)
        {
            return;
        }

        // Set default values
        _normalScaleDuration = 0.2f;
        _highlightedScaleDuration = 0.2f;
        _pressedScaleDuration = 0.2f;
        _selectedScaleDuration = 0.2f;
        _disabledScaleDuration = 0.3f;

        _normalScaleMultiplier = 1.0f;
        _highlightedScaleMultiplier = 1.1f;
        _pressedScaleMultiplier = 0.9f;
        _selectedScaleMultiplier = 0.97f;
        _disabledScaleMultiplier = 0.95f;

        // Set default colors
        _normalColor = _color;
        _highlightedColor = SubtractionHSV(_color, 0f, -0.4f, -0.4f);
        _pressedColor = SubtractionHSV(_color, 0f, -0.2f, 0.5f);
        _selectedColor = SubtractionHSV(_color, 0f, -0.2f, -0.2f);
        _disabledColor = SubtractionHSV(_color, 0f, 0.7f, 0.7f);

        // Set default eases
        _normalEase = Ease.InOutSine;
        _highlightedEase = Ease.OutBack;
        _pressedEase = Ease.OutBack;
        _selectedEase = Ease.OutBack;
        _disabledEase = Ease.InOutExpo;

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
