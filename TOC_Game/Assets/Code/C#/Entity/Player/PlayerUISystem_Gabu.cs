using UnityEngine;
using TMPro;

public class PlayerUISystem_Gabu : MonoBehaviour
{
    public TextMeshProUGUI ammoTmpro = null;
    public GameObject messageTemprete = null;

    public void UpdateAmmo(long ammo)
    {
        ammoTmpro.text = ammo.ToString();
    }

    public void Die()
    {

    }
}
