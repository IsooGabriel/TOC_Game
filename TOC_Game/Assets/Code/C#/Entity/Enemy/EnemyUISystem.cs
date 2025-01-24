using UnityEngine;
using UnityEngine.UI;

public class EnemyUISystem : EntityUIBase
{
    public override void Dash(Quaternion direction)
    {
        // ダッシュ時のUI処理
    }
    public override void stopDash()
    {
        // ダッシュ停止時のUI処理
    }
    public override void TakeDamage()
    {
        // ダメージを受けた時のUI処理
    }
    public override void Attack()
    {
        // 攻撃時のUI処理
    }
    public override void Die()
    {
        // 死亡時のUI処理
    }
    public override void Buffed(long value)
    {
        // バフ時のUI処理
    }

}
