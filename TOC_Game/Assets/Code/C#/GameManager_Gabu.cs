using UnityEngine;

public class GameManager_Gabu : MonoBehaviour
{

    #region 変数

    public EntityBase player = null;
    public EnemyManager enemyManager = null;

    #endregion

    #region 関数


    #endregion

    private void Start()
    {

    }

    private void Update()
    {
        enemyManager.playerPosition = player.transform.position;
    }
}
