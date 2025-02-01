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

    private ComputeBuffer enemyBuffer;   // 敵位置情報用のComputeBuffer

    private ComputeBuffer scaleBuffer;   // 敵スケール情報用のComputeBuffer

    private ComputeBuffer stopProbabilities;// 敵の停止確率情報用のComputeBuffer

    private ComputeBuffer boundsBuffer;  // 座標制限用のComputeBuffer

    private ComputeBuffer restrictionsBuffer; // 敵の数用のComputeBuffer

    private ComputeBuffer randomBuffer;  // ランダム用のComputeBuffer
    private uint[] random;

    private ComputeBuffer randomValuesBuffer; // ランダム値用のComputeBuffer
    private float[] randomValues;

    public EntityBase player;           // プレイヤー
    public Vector2 playerPosition = Vector2.zero;  // プレイヤーの位置
    private float detectionRange = 5f;  // プレイヤーの検出範囲
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

    #endregion


    #region 関数




    public void ResetEnemy(uint ID)
    {
        Debug.Log("テッテレー、死んじゃいました（胡桃風）:" + ID);
        float direction = UnityEngine.Random.Range(-1f, 1f); // -1 から 1 の範囲でランダムに方向を決定
        Vector2 position = new Vector2(direction * generationRange + player.transform.position.x, (1 - Mathf.Abs(direction)) * generationRange + player.transform.position.y);
        enemyPositions[ID] = position;
        enemies[(int)ID].transform.position = position;
        enemies[(int)ID].GetComponent<EntityUIBase>().Reset();

        Player_Gabu playerSystem = player.GetComponent<Player_Gabu>();
        if (playerSystem.currentHP > 0)
        {
            DB.playerDBs[DB.AccountID].money += (uint)enemies[ID].GetComponent<Enemy_Gabu>().level;
            playerSystem.UpdateMoney();
        }
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
        //shotCounts = 0;

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

    }

    void Update()
    {
        // 敵オブジェクトの位置を更新
        for (int i = 0; i < enemyCount; i++)
        {
            enemies[i].transform.position = new Vector3(enemyPositions[i].x, enemyPositions[i].y, 0);
        }

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


        computeShader.Dispatch(kernel, Mathf.CeilToInt(enemyCount / 1f), 1, 1);

        // ComputeBufferからデータを取得
        enemyBuffer.GetData(enemyPositions);


    }

    public void LateUpdate()
    {
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
    }
}
