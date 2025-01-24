using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyUISystem : EntityUIBase
{

    #region 関数

    public override void Dash(Quaternion direction)
    {
        return;
    }
    public override void stopDash()
    {
        return;
    }
    public override void Buffed(long value)
    {
    }

    private void Start()
    {
        normalSaturation = GetSaturation(entityImage.color);
    }

    #endregion
}
