using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class TextAnimation_Gabu : UISystem_Gabu
{

    #region 変数
    private TextMeshProUGUI tmp;
    #endregion

    #region 関数
    protected override void NormalAnimation()
    {
        base.NormalAnimation();
        if (_i_currentAnimation == _i_lastAnimation)
        {
            return;
        }
        _transform.DOScale(_unitScale, duration: 0.4f);
        tmp.DOColor(_color, duration: 0.4f);
    }
    protected override void HighlightedAnimation()
    {
        base.HighlightedAnimation();
        if (_i_currentAnimation == _i_lastAnimation)
        {
            return;
        }

        if (_isMonochrome)
        {
            _transform.DOScale(_unitScale * 1.1f, duration: 0.2f);
            tmp.DOColor(SubtractionHSV(_color, 0f, 1f, -0.4f), duration: 0.2f);
        }
        else
        {
            _transform.DOScale(_unitScale * 1.1f, duration: 0.2f);
            tmp.DOColor(SubtractionHSV(_color, 0f, -0.4f, -0.4f), duration: 0.2f);
        }
    }
    protected override void PressedAnimation()
    {
        base.PressedAnimation();
        if (_i_currentAnimation == _i_lastAnimation)
        {
            return;
        }

        if (_isMonochrome)
        {
            _transform.DOScale(_unitScale * 1.05f, duration: 0f);
            tmp.DOColor(SubtractionHSV(_color, 0f, 1f, 0.5f), duration: 0f);
        }
        else
        {
            _transform.DOScale(_unitScale * 1.05f, duration: 0f);
            tmp.DOColor(SubtractionHSV(_color, 0f, -0.2f, 0.5f), duration: 0f);
        }
    }
    protected override void SelectedAnimation()
    {
        base.SelectedAnimation();
        if (_i_currentAnimation == _i_lastAnimation)
        {
            return;
        }

        if (_isMonochrome)
        {
            _transform.DOScale(_unitScale * 1.1f, duration: 0.2f);
            tmp.DOColor(SubtractionHSV(_color, 0f, 1f, -0.2f), duration: 0.2f);
        }
        else
        {
            _transform.DOScale(_unitScale * 1.1f, duration: 0.2f);
            tmp.DOColor(SubtractionHSV(_color, 0f, -0.2f, -0.2f), duration: 0.2f);
        }
    }
    protected override void DisabledAnimation()
    {
        base.DisabledAnimation();
        if (_i_currentAnimation == _i_lastAnimation)
        {
            return;
        }

        if (_isMonochrome)
        {
            _transform.DOScale(_unitScale, duration: 0.05f);
            tmp.DOColor(SubtractionHSV(_color, 0f, 1f, 0.7f), duration: 0.05f);
        }
        else
        {
            _transform.DOScale(_unitScale, duration: 0.05f);
            tmp.DOColor(SubtractionHSV(_color, 0f, 0.7f, 0.7f), duration: 0.05f);
        }
    }
    #endregion

    protected override void Start()
    {
        if (tmp == null)
        {
            tmp = GetComponent<TextMeshProUGUI>();
        }
        base.Start();
    }
}
