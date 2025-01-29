using UnityEngine;

public abstract class ColorSystem : MonoBehaviour
{
    #region 変数

    #endregion

    #region 関数

    /// <summary>
    /// ColorのH,S,Vを変更します
    /// </summary>
    /// <param name="currentColor"></param>
    /// <param name="h"></param>
    /// <param name="s"></param>
    /// <param name="v"></param>
    /// <returns></returns>
    protected Color ChengeHSV(Color currentColor, float h, float s = 0.6f, float v = 0.6f)
    {
        Color newColor;
        Color.RGBToHSV(currentColor, out newColor.r, out newColor.g, out newColor.b);
        newColor = new Color(h, s, v);
        return Color.HSVToRGB(newColor.r, newColor.g, newColor.b);
    }
    /// <summary>
    /// ColorのS,V(彩度,明度)だけを変えます
    /// </summary>
    /// <param name="currentColor"></param>
    /// <param name="s"></param>
    /// <param name="v"></param>
    /// <returns></returns>
    protected Color ChengeHSV(Color currentColor, float s = 0.6f, float v = 0.6f)
    {
        Color newColor;
        Color.RGBToHSV(currentColor, out newColor.r, out newColor.g, out newColor.b);
        newColor = new Color(newColor.r, s, v);
        return Color.HSVToRGB(newColor.r, newColor.g, newColor.b);
    }

    /// <summary>
    /// ColorのS(彩度)だけを変えます
    /// </summary>
    /// <param name="currentColor"></param>
    /// <param name="v"></param>
    /// <returns></returns>
    protected Color ChengeHSV(Color currentColor, float v = 0.6f)
    {
        Color newColor;
        Color.RGBToHSV(currentColor, out newColor.r, out newColor.g, out newColor.b);
        newColor = new Color(newColor.r, newColor.g, v);
        return Color.HSVToRGB(newColor.r, newColor.g, newColor.b);
    }

    /// <summary>
    /// RGBをHSVに変換し引き算を行い再びRGBに戻す関数です。足し算もあります
    /// </summary>
    /// <param name="currentColor"></param>
    /// <param name="sabtractColor"></param>
    /// <returns></returns>
    protected Color SubtractionHSV(Color currentColor, Color sabtractColor)
    {
        Color.RGBToHSV(currentColor, out currentColor.r, out currentColor.g, out currentColor.b);
        Color.RGBToHSV(sabtractColor, out sabtractColor.r, out sabtractColor.g, out sabtractColor.b);
        Color newColor = currentColor - sabtractColor;
        return Color.HSVToRGB(newColor.r, newColor.g, newColor.b);
    }

    /// <summary>
    /// RGBをHSVに変換し引き算を行い再びRGBに戻す関数です。足し算もあります
    /// </summary>
    /// <param name="currentColor"></param>
    /// <param name="h"></param>
    /// <param name="s"></param>
    /// <param name="v"></param>
    /// <returns></returns>
    protected Color SubtractionHSV(Color currentColor, float h, float s, float v)
    {
        Color.RGBToHSV(currentColor, out currentColor.r, out currentColor.g, out currentColor.b);
        Color newColor = currentColor - new Color(h, s, v);
        return Color.HSVToRGB(newColor.r, newColor.g, newColor.b);
    }

    /// <summary>
    /// RGBをHSVに変換し足し算を行い再びRGBに戻す関数です。引き算もあります
    /// </summary>
    /// <param name="currentColor"></param>
    /// <param name="addColor"></param>
    /// <returns></returns>
    protected Color AdditionHSV(Color currentColor, Color addColor)
    {
        Color.RGBToHSV(currentColor, out currentColor.r, out currentColor.g, out currentColor.b);
        Color.RGBToHSV(addColor, out addColor.r, out addColor.g, out addColor.b);
        Color newColor = currentColor + addColor;
        return Color.HSVToRGB(newColor.r, newColor.g, newColor.b);
    }

    /// <summary>
    /// RGBをHSVに変換し足し算を行い再びRGBに戻す関数です。引き算もあります
    /// </summary>
    /// <param name="currentColor"></param>
    /// <param name="h"></param>
    /// <param name="s"></param>
    /// <param name="v"></param>
    /// <returns></returns>
    protected Color AdditionHSV(Color currentColor, float h, float s, float v)
    {
        Color.RGBToHSV(currentColor, out currentColor.r, out currentColor.g, out currentColor.b);
        Color newColor = currentColor + new Color(h, s, v);
        return Color.HSVToRGB(newColor.r, newColor.g, newColor.b);
    }

    public Color AddSaturation(Color currentColor, float s)
    {
        Color.RGBToHSV(currentColor, out currentColor.r, out currentColor.g, out currentColor.b);
        Color newColor = new Color(currentColor.r, currentColor.g + s, currentColor.b);
        return Color.HSVToRGB(newColor.r, newColor.g, newColor.b);
    }

    /// <summary>
    /// RGBをHSVに変換し彩度の引き算を行い再びRGBに戻す関数です。足し算もあります
    /// </summary>
    /// <param name="currentColor"></param>
    /// <param name="s"></param>
    /// <returns></returns>
    public Color SubtractionSaturation(Color currentColor, float s)
    {
        Color.RGBToHSV(currentColor, out currentColor.r, out currentColor.g, out currentColor.b);
        Color newColor = new Color(currentColor.r, currentColor.g - s, currentColor.b);
        return Color.HSVToRGB(newColor.r, newColor.g, newColor.b);
    }

    /// <summary>
    /// RGBをHSVに変換し彩度を変更を行い再びRGBに戻す関数です。引き算もあります
    /// </summary>
    /// <param name="currentColor"></param>
    /// <param name="s"></param>
    /// <returns></returns>
    public Color ChengeSaturation(Color currentColor, float s)
    {
        Color.RGBToHSV(currentColor, out currentColor.r, out currentColor.g, out currentColor.b);
        Color newColor = new Color(currentColor.r, s, currentColor.b);
        return Color.HSVToRGB(newColor.r, newColor.g, newColor.b);
    }

    public float GetSaturation(Color currentColor)
    {
        Color.RGBToHSV(currentColor, out currentColor.r, out currentColor.g, out currentColor.b);
        return currentColor.g;
    }

    public Color AddHue(Color currentColor, float h)
    {
        Color.RGBToHSV(currentColor, out currentColor.r, out currentColor.g, out currentColor.b);
        Color newColor = new Color(currentColor.r + h, currentColor.g, currentColor.b);
        return Color.HSVToRGB(newColor.r, newColor.g, newColor.b);
    }

    public Color SubtractionHue(Color currentColor, float h)
    {
        Color.RGBToHSV(currentColor, out currentColor.r, out currentColor.g, out currentColor.b);
        Color newColor = new Color(currentColor.r - h, currentColor.g, currentColor.b);
        return Color.HSVToRGB(newColor.r, newColor.g, newColor.b);
    }

    public Color AddValue(Color currentColor, float v)
    {
        Color.RGBToHSV(currentColor, out currentColor.r, out currentColor.g, out currentColor.b);
        Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b + v);
        return Color.HSVToRGB(newColor.r, newColor.g, newColor.b);
    }

    public Color SubtractionValue(Color currentColor, float v)
    {
        Color.RGBToHSV(currentColor, out currentColor.r, out currentColor.g, out currentColor.b);
        Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b - v);
        return Color.HSVToRGB(newColor.r, newColor.g, newColor.b);
    }

    #endregion
}
