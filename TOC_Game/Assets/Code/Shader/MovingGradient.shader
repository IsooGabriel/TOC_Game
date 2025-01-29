Shader "Custom/CenterGradientUI"
{
    Properties
    {
        _Color1 ("Color 1", Color) = (0.0, 0.396, 0.322, 1.0) // #006552
        _Color2 ("Color 2", Color) = (0.0, 1.0, 0.831, 1.0)   // #00FFD4
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Transparent" "IgnoreProjector"="True" "Canvas"="UI" }
        LOD 100
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            fixed4 _Color1;
            fixed4 _Color2;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Calculate gradient factor from UV
                float gradientFactor = abs(i.uv.x - 0.5) * 2.0; // Central gradient spreading both sides
                return lerp(_Color1, _Color2, gradientFactor);
            }
            ENDCG
        }
    }
    FallBack "Unlit/Transparent"
}
