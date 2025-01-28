using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    #region 変数

    [SerializeField, Header("生成範囲")]
    public readonly float generationRange = 15f;
    [SerializeField, Header("生成しない範囲")]
    private readonly float notgenerationRange = 5f; // 敵の生成範囲
    public ComputeShader computeShader;  // Compute Shaderをアタッチ
    public readonly uint enemyCount = 10;         // 敵の数
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

    private ComputeBuffer resetBuffer;  // リセット用のComputeBuffer
    private List<uint> resetEnemy;

    private ComputeBuffer attackBuffer;  // 攻撃用のComputeBuffer
    private List<uint> attackEnemy;

    // カウンタ取得用のバッファ
    private ComputeBuffer countBuffer;
    private int[] count = new int[1];

    public EntityBase player;           // プレイヤー
    public Vector2 playerPosition = Vector2.zero;  // プレイヤーの位置
    private float detectionRange = 10f;  // プレイヤーの検出範囲
    private float attackRange = 0.95f;      // プレイヤーの攻撃範囲
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

    int kernel;

    #endregion


    #region 関数


    private void SetAttack(List<uint> targetEnemies)
    {
        for (int i = 1; i < targetEnemies.Count; i++)
        {
            if (targetEnemies[(int)i] >= enemyCount)
            {
                continue;
            }
            uint ID = targetEnemies[(int)i];
            Enemy_Gabu entity = enemies[(int)ID].GetComponent<Enemy_Gabu>();
            player.TakeDamage(entity.atk, entity.level, entity.criticalChance, entity.criticalDamage, entity.Buff);
        }
    }

    public void ResetEnemy(uint ID)
    {

        float direction = Random.Range(-1f, 1f); // -1 から 1 の範囲でランダムに方向を決定
        Vector2 position = new Vector2(direction * generationRange, (1 - Mathf.Abs(direction)) * generationRange);
        enemyPositions[ID] = position;
        enemies[(int)ID].transform.position = position;

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
        resetEnemy = new List<uint>();
        attackEnemy = new List<uint>();
        attackEnemy.Add(0); // 要素のないリストはBufferに渡せないのでダミーを追加

        for (int i = 0; i < enemyCount; i++)
        {
            // ランダムな初期位置（例：-10〜10の範囲）
            enemyPositions[i] = new Vector2(Random.Range(Mathf.Abs(generationRange) * -1, Mathf.Abs(generationRange)), Random.Range(Mathf.Abs(generationRange) * -1, Mathf.Abs(generationRange)));

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
            float scale = scales[Random.Range(0, scales.Length)]; // スケールを配列からランダムに設定
            enemy.transform.localScale = new Vector3(scale, scale, 1f);

            // スケールを保存
            enemyScales[i] = scale;

            // 停止確率を設定
            probabilities[i] = i % 3 == 0 ? 0.01f : 0.01f;

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
        attackBuffer.SetData(attackEnemy);

        countBuffer = new ComputeBuffer(1, sizeof(int), ComputeBufferType.Raw); // カウンタ取得用のバッファ
    }

    void Update()
    {
        OperationID = (uint)Mathf.Repeat(OperationID++, 10);

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
        computeShader.SetBuffer(kernel, "attackIDs", attackBuffer);


        // Compute Shaderを実行
        computeShader.Dispatch(kernel, Mathf.CeilToInt(enemyCount / 1f), 1, 1);

        // ComputeBufferからデータを取得
        enemyBuffer.GetData(enemyPositions);

        // 敵オブジェクトの位置を更新
        for (int i = 0; i < enemyCount; i++)
        {
            enemies[i].transform.position = new Vector3(enemyPositions[i].x, enemyPositions[i].y, 0);
        }

        // GPUでの処理が完了した後に、カウンタを取得
        ComputeBuffer.CopyCount(attackBuffer, countBuffer, 0);
        countBuffer.GetData(count);
        if (count[0] > 1)
        {
            uint[] tempArray = new uint[count[0]];
            attackBuffer.GetData(tempArray, 0, 0, count[0]);
            if (attackEnemy != null)
            {
                SetAttack(attackEnemy);
                attackEnemy.Clear();
                attackEnemy.Add(0); // 要素のないリストはBufferに渡せないのでダミーを追加
                computeShader.SetBuffer(kernel, "attackIDs", attackBuffer);
            }
        }

    }

}
