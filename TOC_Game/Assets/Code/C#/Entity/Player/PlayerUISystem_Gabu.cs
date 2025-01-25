using UnityEngine;
using TMPro;

public class PlayerUISystem_Gabu : EntityUIBase
{
    public SpriteRenderer spriteRenderer = null;
    public TextMeshProUGUI ammoTmpro = null;
    public GameObject messageTemprete = null;

    public void UpdateAmmo(long ammo)
    {
        ammoTmpro.text = ammo.ToString();
    }

    public override void TakeDamage()
    {
    }

    public override void Die()
    {

    }

    public override void Attack()
    {
    }


    public override void Buffed(float value)
    {

    }

    public override void Dash(Quaternion direction)
    {
    }

    public override void stopDash()
    {
    }

    public void Reroll()
    {
    }

    public void Aim()
    {

    }
}
