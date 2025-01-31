using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEngine.InputSystem.InputSettings;
using static DBManager_Gabu;

public class EnemyManager : MonoBehaviour
{
    #region 変数

    [SerializeField, Header("生成範囲")]
    public readonly float generationRange = 29f;
    [SerializeField, Header("生成しない範囲")]
    private readonly float notgenerationRange = 5f; // 敵の生成範囲
    public ComputeShader computeShader;  // Compute Shaderをアタッチ
    public readonly uint enemyCount = 500;         // 敵の数
    public float moveSpeed = 0.8f;       // 敵の移動速度
    public readonly uint maxShots = 255;

    private ComputeBuffer enemyBuffer;   // 敵位置情報用のComputeBuffer

    private ComputeBuffer scaleBuffer;   // 敵スケール情報用のComputeBuffer

    private ComputeBuffer stopProbabilities;// 敵の停止確率情報用のComputeBuffer

    private ComputeBuffer boundsBuffer;  // 座標制限用のComputeBuffer

    private ComputeBuffer restrictionsBuffer; // 敵の数用のComputeBuffer

    private ComputeBuffer randomBuffer;  // ランダム用のComputeBuffer
    private uint[] random;

    private ComputeBuffer randomValuesBuffer; // ランダム値用のComputeBuffer
    private float[] randomValues;

    private ComputeBuffer attackBuffer;  // 攻撃用のComputeBuffer

    private ComputeBuffer takeDamageBuffer;    // ダメージ処理用のバッファ

    private ComputeBuffer ShotBuffer;  // ショット用のComputeBuffer
    private Vector2[] shotPosition;
    private GameObject[] shot;

    private ComputeBuffer shotScaleBuffer;  // ショットスケール用のComputeBuffer
    private float[] shotScales;

    private ComputeBuffer attackedShotBuffer;  // 攻撃されたショット用のComputeBuffer
    private int[] attackedShotFrags;

    public EntityBase player;           // プレイヤー
    public Vector2 playerPosition = Vector2.zero;  // プレイヤーの位置
    private float detectionRange = 5f;  // プレイヤーの検出範囲
    private float attackRange = 0.7f;      // プレイヤーの攻撃範囲
    private uint OperationID;           // 行動する敵の範囲
    private Vector2[] enemyPositions;   // CPU上での敵位置情報
    private float[] enemyScales;        // CPU上での敵スケール情報
    private float[] probabilities;      // CPU上での敵の停止確率情報
    private Vector2[] moveRestrictions;     // CPU上での敵の数情報
    public GameObject enemyPrefab;      // 敵のプレハブ
    public float[] scales =
        {
        0.42f,0.42f,0.42f,0.42f,0.42f,0.42f,0.42f,0.42f,0.42f,0.42f,0.42f,0.42f,0.42f,
        0.84f,0.84f,0.84f,0.84f,0.84f,0.84f,0.84f,
        1.68f,1.68f,1.68f,
        3.36f
        };                         // 敵のスケール
    public float[] bounds = { 30f };   // 座標制限

    private GameObject[] enemies;   // 敵オブジェクトの配列

    int kernel;
    int[] shotTempArray;
    int[] tempArray;

    #endregion


    #region 関数


    private void SetAttack(int[] targetEnemies)
    {
        for (int i = 0; i < targetEnemies.Length; i++)
        {
            if (i >= enemyCount)
            {
                continue;
            }
            if (targetEnemies[i] == 0)
            {
                continue;
            }
            Enemy_Gabu entity = enemies[i].GetComponent<Enemy_Gabu>();
            if (entity.atkCT > 0)
            {
                continue;
            }
            Debug.Log("攻撃してるよ:" + i);
            entity.atkCT = entity.atkSpeed;
            player.TakeDamage(entity.atk, entity.level, entity.criticalChance, entity.criticalDamage, entity.Buff);
        }
    }


    public void TakeDamage(int[] IDs)
    {
        for (int i = 0; i < IDs.Length; i++)
        {
            if (i >= enemyCount)
            {
                continue;
            }
            if (IDs[i] == 0)
            {
                continue;
            }
            Debug.Log("ダメージを受けてるよ:" + i);
            enemies[i].GetComponent<Enemy_Gabu>().TakeDamage(
            player.atk, player.level, player.criticalChance, player.criticalDamage, player.Buff);
        }
    }

    public void ResetEnemy(uint ID)
    {
        Debug.Log("テッテレー、死んじゃいました（胡桃風）:" + ID);
        DB.playerDBs[DB.AccountID].money += (uint)enemies[ID].GetComponent<Enemy_Gabu>().level;
        float direction = UnityEngine.Random.Range(-1f, 1f); // -1 から 1 の範囲でランダムに方向を決定
        Vector2 position = new Vector2(direction * generationRange, (1 - Mathf.Abs(direction)) * generationRange);
        enemyPositions[ID] = position;
        enemies[(int)ID].transform.position = position;
        player.GetComponent<Player_Gabu>().UpdateMoney();
    }

    public void SetShot(GameObject newShot)
    {
        for (int i = 0; i < maxShots; i++)
        {
            if (shot[i] == null)
            {
                shot[i] = newShot;
                attackedShotFrags[i] = 0;
                Debug.Log("ショット追加を" + i + "に追加");
                return;

            }
        }
        Debug.Log("なんと弾数が最大に！！？しかたないから最初の弾を消して新しい弾を追加");
        // すべての弾が埋まっている場合は、最初の弾を削除して新しい弾を追加
        Destroy(shot[0]);
        shot[0] = newShot;
    }

    public void RmoveShot(GameObject target)
    {
        int index = Array.IndexOf(shot, target);
        Debug.Log("ショット取り除き" + index);
        if (index < 0)
        {
            Debug.Log("ショットが見つかりません");
            return;
        }
        if (index >= maxShots)
        {
            Debug.LogError("ショットの長さが異常です");
            return;
        }
        attackedShotFrags[index] = 0;
        shot[index] = null;
    }

    #endregion


    void Start()
    {
        // 敵の初期位置とスケールを設定
        enemyPositions = new Vector2[enemyCount];
        enemyScales = new float[enemyCount];
        probabilities = new float[enemyCount];
        enemies = new GameObject[enemyCount];
        moveRestrictions = new Vector2[enemyCount];
        random = new uint[enemyCount];
        randomValues = new float[enemyCount];
        OperationID = 0;
        tempArray = new int[enemyCount];
        shotTempArray = new int[maxShots];
        shot = new GameObject[maxShots];
        shotPosition = new Vector2[maxShots];
        shotScales = new float[maxShots];
        attackedShotFrags = new int[maxShots];
        //shotCounts = 0;
        for (int i = 0; i < maxShots; i++)
        {
            shotTempArray[i] = 0;
            shot[i] = null;
            shotPosition[i] = Vector2.zero;
            shotScales[i] = 0;
            attackedShotFrags[i] = 0;
        }
        for (int i = 0; i < enemyCount; i++)
        {
            tempArray[i] = 0;
        }

        for (int i = 0; i < enemyCount; i++)
        {
            // ランダムな初期位置（例：-10〜10の範囲）
            enemyPositions[i] = new Vector2(UnityEngine.Random.Range(Mathf.Abs(generationRange) * -1, Mathf.Abs(generationRange)), UnityEngine.Random.Range(Mathf.Abs(generationRange) * -1, Mathf.Abs(generationRange)));

            // notgenerationRangeの範囲内に生成された場合はenemyPositionsを再設定
            if ((enemyPositions[i].x < notgenerationRange) && (enemyPositions[i].x > -notgenerationRange)
                && (enemyPositions[i].y < notgenerationRange) && (enemyPositions[i].y > -notgenerationRange))
            {
                enemyPositions[i].x = Mathf.Abs(notgenerationRange) * Mathf.Sign(enemyPositions[i].x);
                enemyPositions[i].y = Mathf.Abs(notgenerationRange) * Mathf.Sign(enemyPositions[i].y);
            }

            // プレハブをインスタンス化してリストに保存
            GameObject enemy = Instantiate(enemyPrefab, enemyPositions[i], Quaternion.identity);

            // ランダムなスケールを設定
            float scale = scales[UnityEngine.Random.Range(0, scales.Length)]; // スケールを配列からランダムに設定
            enemy.transform.localScale = new Vector3(scale, scale, 1f);

            // スケールを保存
            enemyScales[i] = scale;

            // 停止確率を設定
            probabilities[i] = 0f;

            // Enemy_Gabuスクリプトを取得
            Enemy_Gabu enemy_Gabu = enemy.GetComponent<Enemy_Gabu>();
            enemy_Gabu.ID = (uint)i;
            enemy_Gabu.enemyManager = this;
            enemy_Gabu.level = 1 + (int)(scale / 0.21f);
            enemy_Gabu.Buff = UnityEngine.Random.Range(0, 100);
            enemy_Gabu.entityUIBase.normalScale = Vector3.one * scale;
            enemy_Gabu.atk = enemy_Gabu.level;
            enemy_Gabu.maxHP = enemy_Gabu.level;
            enemy_Gabu.currentHP = enemy_Gabu.maxHP;

            enemies[i] = enemy;
        }

        // ComputeBufferの設定
        enemyBuffer = new ComputeBuffer((int)enemyCount, sizeof(float) * 2); // 2Dなのでfloat2
        enemyBuffer.SetData(enemyPositions);

        scaleBuffer = new ComputeBuffer((int)enemyCount, sizeof(float)); // スケールはfloat1
        scaleBuffer.SetData(enemyScales);

        stopProbabilities = new ComputeBuffer((int)enemyCount, sizeof(float)); // 停止確率はfloat1
        stopProbabilities.SetData(probabilities);

        boundsBuffer = new ComputeBuffer(1, sizeof(float)); // 座標範囲はfloat2
        boundsBuffer.SetData(bounds); // 座標範囲の最小値

        restrictionsBuffer = new ComputeBuffer((int)enemyCount, sizeof(float) * 2); // 敵の数はfloat2
        restrictionsBuffer.SetData(moveRestrictions);

        randomBuffer = new ComputeBuffer((int)enemyCount, sizeof(uint)); // ランダムはfloat1
        randomBuffer.SetData(random);

        randomValuesBuffer = new ComputeBuffer((int)enemyCount, sizeof(float)); // ランダム値はint1
        randomValuesBuffer.SetData(randomValues);

        attackBuffer = new ComputeBuffer((int)enemyCount, sizeof(int)); // 攻撃情報はint1
        attackBuffer.SetData(tempArray);

        takeDamageBuffer = new ComputeBuffer((int)enemyCount, sizeof(int)); // 攻撃情報はint1
        takeDamageBuffer.SetData(tempArray);

        ShotBuffer = new ComputeBuffer((int)maxShots, sizeof(float) * 2); // 攻撃情報はint1
        ShotBuffer.SetData(shotPosition);

        attackedShotBuffer = new ComputeBuffer((int)maxShots, sizeof(int)); // 攻撃情報はint1
        attackedShotBuffer.SetData(shotTempArray);

        shotScaleBuffer = new ComputeBuffer((int)maxShots, sizeof(float)); // 攻撃情報はint1
        shotScaleBuffer.SetData(shotScales);

    }

    void Update()
    {
        OperationID = (uint)Mathf.Repeat(OperationID + 1, 6);
        kernel = computeShader.FindKernel("CSMain");

        if (kernel < 0)
        {
            Debug.LogError("Invalid kernel index");
            return;
        }

        // Compute Shaderに変数をセット
        computeShader.SetBuffer(kernel, "enemyPositions", enemyBuffer);
        computeShader.SetBuffer(kernel, "enemyScales", scaleBuffer);
        computeShader.SetFloat("deltaTime", Time.deltaTime);
        computeShader.SetFloat("moveSpeed", moveSpeed);
        computeShader.SetInt("enemyCount", (int)enemyCount); // 敵の数を渡す
        computeShader.SetInt("operationID", (int)OperationID);
        computeShader.SetBuffer(kernel, "stopProbabilities", stopProbabilities);
        computeShader.SetBuffer(kernel, "bounds", boundsBuffer);
        computeShader.SetBuffer(kernel, "moveRestrictions", restrictionsBuffer);
        computeShader.SetBuffer(kernel, "random", randomBuffer);
        computeShader.SetBuffer(kernel, "randomValues", randomValuesBuffer);
        computeShader.SetFloats("playerPosition", new float[] { playerPosition.x, playerPosition.y });
        computeShader.SetFloat("detectionRange", detectionRange);
        computeShader.SetFloat("attackRange", attackRange);
        computeShader.SetInt("shotCount", (int)maxShots);
        computeShader.SetBuffer(kernel, "attackIDs", attackBuffer);
        computeShader.SetBuffer(kernel, "takeDamageIDs", takeDamageBuffer);
        computeShader.SetBuffer(kernel, "attackedShotIDs", attackedShotBuffer);


        // 弾の数と配置を教える、あとスケールも
        for (int i = 0; i < shot.Length; i++)
        {
            if (shot[i] == null)
            {
                continue;
            }
            shotPosition[i] = shot[i].transform.position;
            shotScales[i] = shot[i].transform.localScale.x;
        }
        ShotBuffer.SetData(shotPosition);
        shotScaleBuffer.SetData(shotScales);
        computeShader.SetBuffer(kernel, "shotPosition", ShotBuffer);
        computeShader.SetBuffer(kernel, "shotScale", shotScaleBuffer);

        // Compute Shaderを実行
        // 敵の攻撃処理
        attackBuffer.GetData(tempArray);
        SetAttack(tempArray);

        // 敵の攻撃を受ける処理
        takeDamageBuffer.GetData(tempArray);
        TakeDamage(tempArray);

        // ヒット済みの弾の処理
        attackedShotBuffer.GetData(shotTempArray);
        for (int i = 0; shotTempArray.Length > i; i++)
        {
            if (i >= maxShots)
            {
                Debug.LogError("attackedShotBufferの長さが異常です");
                break;
            }
            if (shot[i] == null)
            {
                continue;
            }
            if (shotTempArray[i] == 0)
            {
                continue;
            }
            shot[i].GetComponent<Shot_Gabu>().OnAttacked();
        }

        computeShader.Dispatch(kernel, Mathf.CeilToInt(enemyCount / 1f), 1, 1);

        // ComputeBufferからデータを取得
        enemyBuffer.GetData(enemyPositions);

        // 敵オブジェクトの位置を更新
        for (int i = 0; i < enemyCount; i++)
        {
            enemies[i].transform.position = new Vector3(enemyPositions[i].x, enemyPositions[i].y, 0);
        }
    }
    void OnDestroy()
    {
        // ComputeBufferを解放
        if (enemyBuffer != null) enemyBuffer.Release();
        if (scaleBuffer != null) scaleBuffer.Release();
        if (stopProbabilities != null) stopProbabilities.Release();
        if (boundsBuffer != null) boundsBuffer.Release();
        if (restrictionsBuffer != null) restrictionsBuffer.Release();
        if (randomBuffer != null) randomBuffer.Release();
        if (randomValuesBuffer != null) randomValuesBuffer.Release();
        if (attackBuffer != null) attackBuffer.Release();
        if (takeDamageBuffer != null) takeDamageBuffer.Release();
        if (ShotBuffer != null) ShotBuffer.Release();
        if (shotScaleBuffer != null) shotScaleBuffer.Release();
        if (attackedShotBuffer != null) attackedShotBuffer.Release();
    }
}
