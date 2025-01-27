using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyManager2D : MonoBehaviour
{
    #region 変数

    public float generationRange = 15f;
    public ComputeShader computeShader;  // Compute Shaderをアタッチ
    public uint enemyCount = 100;         // 敵の数
    public float moveSpeed = 1.0f;       // 敵の移動速度

    private ComputeBuffer enemyBuffer;   // 敵位置情報用のComputeBuffer

    private ComputeBuffer scaleBuffer;   // 敵スケール情報用のComputeBuffer

    private ComputeBuffer stopProbabilities;// 敵の停止確率情報用のComputeBuffer

    private ComputeBuffer boundsBuffer;  // 座標制限用のComputeBuffer

    private ComputeBuffer restrictionsBuffer; // 敵の数用のComputeBuffer

    private ComputeBuffer randomBuffer;  // ランダム用のComputeBuffer
    private uint[] random;

    private ComputeBuffer randomValuesBuffer; // ランダム値用のComputeBuffer
    private float[] randomValues;

    private ComputeBuffer survivalBuffer;  // 敵の生存情報用のComputeBuffer
    public bool[] enemySurvival;

    private ComputeBuffer resetBuffer;  // リセット用のComputeBuffer
    private List<uint> resetEnemy;

    public Vector2 playerPosition = Vector2.zero;  // プレイヤーの位置
    private float detectionRange = 10f;  // プレイヤーの検出範囲
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
    public float[] bounds = { 25f };   // 座標制限
    private GameObject[] enemies;   // 敵オブジェクトの配列
    private EntityBase[] enemyEntity; // 敵のEntityBaseの配列

    #endregion

    #region 関数

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
        enemySurvival = new bool[enemyCount];
        OperationID = 0;
        resetEnemy = new List<uint>();

        for (int i = 0; i < enemyCount; i++)
        {
            // ランダムな初期位置（例：-10〜10の範囲）
            enemyPositions[i] = new Vector2(Random.Range(Mathf.Abs(generationRange) * -1, Mathf.Abs(generationRange)), Random.Range(Mathf.Abs(generationRange) * -1, Mathf.Abs(generationRange)));

            // プレハブをインスタンス化してリストに保存
            GameObject enemy = Instantiate(enemyPrefab, enemyPositions[i], Quaternion.identity);

            // ランダムなスケールを設定
            float scale = scales[Random.Range(0, scales.Length)]; // スケールを配列からランダムに設定
            enemy.transform.localScale = new Vector3(scale, scale, 1f);

            // スケールを保存
            enemyScales[i] = scale;

            // 生存情報を保存
            enemySurvival[i] = true;

            // 停止確率を設定
            probabilities[i] = i % 3 == 0 ? 0.01f : 0.9f;

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
        
        survivalBuffer = new ComputeBuffer((int)enemyCount, sizeof(bool)); // 生存情報はbool1
        survivalBuffer.SetData(enemySurvival);

        resetBuffer = new ComputeBuffer((int)resetEnemy.Count, sizeof(uint)); // リセット情報はuint1
        resetBuffer.SetData(resetEnemy.ToArray());
    }

    void Update()
    {
        OperationID = (uint)Mathf.Repeat(OperationID++,10);

        int kernel = computeShader.FindKernel("CSMain");

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
        computeShader.SetBuffer(kernel, "survival", survivalBuffer);


        // Compute Shaderを実行
        computeShader.Dispatch(kernel, Mathf.CeilToInt(enemyCount / 1f), 1, 1);

        // ComputeBufferからデータを取得
        enemyBuffer.GetData(enemyPositions);

        // 敵オブジェクトの位置を更新
        for (int i = 0; i < enemyCount; i++)
        {
            enemies[i].transform.position = new Vector3(enemyPositions[i].x, enemyPositions[i].y, 0);
        }

        // デバッグ用    
        stopProbabilities.GetData(probabilities);
        randomValuesBuffer.GetData(randomValues);
        restrictionsBuffer.GetData(moveRestrictions);
        for (int i = 0; i < probabilities.Length; i++)
        {
            Debug.Log("proba:" + probabilities[i]);
            Debug.Log("random: " + randomValues[i]);
            Debug.Log("restrictions: " + moveRestrictions[i]);  
        }

        if(resetEnemy != null)
        {
            resetEnemies(resetEnemy);
            computeShader.SetBuffer(kernel, "reset", resetBuffer);
        }
    }

    private void resetEnemies(List<uint> enemies)
    {
        for (int i = 0;i < enemies.Count; i++)
        {
            uint ID = enemies[(int)i]; 
            float direction = Random.Range(-1f, 1f); // -1 から 1 の範囲でランダムに方向を決定
            Vector2 position = new Vector2(direction * generationRange, (1 - Mathf.Abs(direction)) * generationRange);
            enemyPositions[ID] = position;
            enemyEntity[(int)ID].transform.position = position;
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

    #endregion
}
