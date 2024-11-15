using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BaseDivision_Gabu : MonoBehaviour
{
    public float maxSize = 64;
    GameObject childerenSpace;
    string s_name = "Base";

    #region 関数

    /// <summary>
    /// 倍化の術
    /// </summary>
    public void Doublication()
    {
        transform.localScale *= 2;
        transform.localPosition *= 2;
    }

    /// <summary>
    /// 倍化の術、お前は一体いつから、倍化の術では縮小できないと錯覚していた？
    /// </summary>
    /// <param name="n">かける数</param>
    public void Doublication(float n)
    {
        transform.localScale *= n;
        transform.localPosition *= n;
    }

    /// <summary>
    /// 複製の術
    /// </summary>
    public void Duplication()
    {
        GameObject child = Instantiate(gameObject, transform.position, transform.rotation, childerenSpace.transform);
        child.GetComponent<BaseDivision_Gabu>().Doublication(0.5f);
        child.transform.parent = transform.parent.transform;
        child.transform.localPosition = Vector2.zero;
        Division(child);
    }

    /// <summary>
    /// 分裂の術
    /// </summary>
    public void Division(GameObject child)
    {
        Vector2 direction = GetAngleWithinRange(gameObject);
        child.transform.position = PositionCalculator(direction, transform.localScale.x);
    }

    public Vector2 PositionCalculator(Vector2 direction, float size)
    {
        float magnification = size / 4;
        Debug.Log($"direction: {direction.magnitude}, *: {magnification}, size: {size}");
        Vector2 localPosition = magnification * direction;
        Vector2 worldPosition = (Vector2)transform.position + localPosition;
        return worldPosition;
    }

    /// <summary>
    /// ターゲットのローカルポジションに基づき、角度が基準角度の±45度範囲以外である場合、その角度を-180〜180度範囲に収まるように調整して返します。
    /// </summary>
    /// <returns>±45度範囲外の角度を調整した結果の角度。範囲内の場合はNaNを返します。</returns>
    public Vector2 GetAngleWithinRange(GameObject target)
    {
        // ターゲットのローカルポジションを取得
        Vector3 localPos = target.transform.localPosition * -1;

        if (localPos == Vector3.zero)
        {
            return Random.insideUnitCircle.normalized;
        }

        // x, yから角度を計算（0度が右方向、正の値は反時計回り）
        float angle = Mathf.Atan2(localPos.y, localPos.x) * Mathf.Rad2Deg;

        // ±45度範囲内の角度をランダムに生成
        float randomAngle = Random.Range(-90f, 90f);

        // 角度を調整
        angle += randomAngle;

        // 調整後の角度を -180〜180 度に収める
        angle = Mathf.Repeat(angle + 180f, 360f) - 180f;

        // 調整された角度を正規化されたVector2に変換
        float radian = angle * Mathf.Deg2Rad;
        Vector2 normalizedDirection = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian)).normalized;

        return normalizedDirection * -1;
    }


    #endregion

    void Start()
    {
        if (gameObject.name != s_name)
        {
            gameObject.name = s_name;
        }
        if (childerenSpace == null)
        {
            childerenSpace = gameObject.transform.parent.gameObject;
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            Doublication();
            Duplication();
        }
    }

}
