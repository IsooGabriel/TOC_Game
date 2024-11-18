using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager2D : MonoBehaviour
{
    public float generationRange = 15f;
    public ComputeShader computeShader;  // Compute Shaderをアタッチ
    public int enemyCount = 100;         // 敵の数
    public float moveSpeed = 1.0f;       // 敵の移動速度
    private ComputeBuffer enemyBuffer;   // 敵位置情報用のComputeBuffer
    private ComputeBuffer scaleBuffer;   // 敵スケール情報用のComputeBuffer
    private ComputeBuffer stopProbabilities;// 敵の停止確率情報用のComputeBuffer
    private ComputeBuffer boundsBuffer;  // 座標制限用のComputeBuffer
    private ComputeBuffer restrictionsBuffer; // 敵の数用のComputeBuffer
    private Vector2[] enemyPositions;   // CPU上での敵位置情報
    private float[] enemyScales;        // CPU上での敵スケール情報
    private float[] probabilities;      // CPU上での敵の停止確率情報
    private Vector2[] moveRestrictions;     // CPU上での敵の数情報
    public GameObject enemyPrefab;      // 敵のプレハブ
    public float[] scales =
        {0.42f,0.42f,0.42f,0.42f,0.42f,0.42f,0.42f,0.42f,0.42f,
        0.84f,0.84f,0.84f,
        1.68f};                         // 敵のスケール
    public float[] bounds = { 25f };   // 座標制限
    private List<GameObject> enemies;   // 敵オブジェクトのリスト

    void Start()
    {
        // 敵の初期位置とスケールを設定
        enemyPositions = new Vector2[enemyCount];
        enemyScales = new float[enemyCount];
        probabilities = new float[enemyCount];
        enemies = new List<GameObject>();
        moveRestrictions = new Vector2[enemyCount];

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

            // 停止確率を設定
            probabilities[i] = 0.5f;

            enemies.Add(enemy);
        }

        // ComputeBufferの設定
        enemyBuffer = new ComputeBuffer(enemyCount, sizeof(float) * 2); // 2Dなのでfloat2
        enemyBuffer.SetData(enemyPositions);

        scaleBuffer = new ComputeBuffer(enemyCount, sizeof(float)); // スケールはfloat1
        scaleBuffer.SetData(enemyScales);

        stopProbabilities = new ComputeBuffer(enemyCount, sizeof(float)); // 停止確率はfloat1
        stopProbabilities.SetData(probabilities);

        boundsBuffer = new ComputeBuffer(1, sizeof(float)); // 座標範囲はfloat2
        boundsBuffer.SetData(bounds); // 座標範囲の最小値

        restrictionsBuffer = new ComputeBuffer(enemyCount, sizeof(float) * 2); // 敵の数はfloat2
        restrictionsBuffer.SetData(moveRestrictions);
    }

    void Update()
    {
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
        computeShader.SetInt("enemyCount", enemyCount); // 敵の数を渡す
        computeShader.SetBuffer(kernel, "stopProbabilities", stopProbabilities);
        computeShader.SetBuffer(kernel, "bounds", boundsBuffer);
        computeShader.SetBuffer(kernel, "moveRestrictions", restrictionsBuffer);

        // Compute Shaderを実行
        computeShader.Dispatch(kernel, Mathf.CeilToInt(enemyCount / 1f), 1, 1);

        // ComputeBufferからデータを取得
        enemyBuffer.GetData(enemyPositions);

        // 敵オブジェクトの位置を更新
        for (int i = 0; i < enemyCount; i++)
        {
            enemies[i].transform.position = new Vector3(enemyPositions[i].x, enemyPositions[i].y, 0);
        }

        stopProbabilities.GetData(probabilities);
        foreach (float probability in probabilities)
        {
            Debug.Log(probability);
        }
    }

    void OnDestroy()
    {
        // ComputeBufferを解放
        if (enemyBuffer != null) enemyBuffer.Release();
        if (scaleBuffer != null) scaleBuffer.Release();
    }
}
