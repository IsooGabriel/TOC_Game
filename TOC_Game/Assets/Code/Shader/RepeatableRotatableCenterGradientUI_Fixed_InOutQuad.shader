Shader "Custom/RepeatableRotatableCenterGradientUI_Fixed_InOutQuad"
{
    Properties
    {
        _Color1 ("Color 1", Color) = (0.0, 0.396, 0.322, 1.0) // #006552
        _Color2 ("Color 2", Color) = (0.0, 1.0, 0.831, 1.0)   // #00FFD4
        _Rotation ("Rotation (Degrees)", Range(0, 360)) = 0.0 // グラデーションの回転角
        _MainTex ("Mask Texture", 2D) = "white" {}            // マスク用テクスチャ
        _Center ("Center Offset", Vector) = (0.5, 0.5, 0, 0)  // グラデーション中心のオフセット
        _Repeat ("Repeat Count", Float) = 1.0                 // グラデーションの反復回数
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
            float _Rotation;
            sampler2D _MainTex;
            float2 _Center;
            float _Repeat;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // マスクテクスチャのアルファ値を取得
                float maskAlpha = tex2D(_MainTex, i.uv).a;

                // 回転角をラジアンに変換
                float angle = radians(_Rotation);

                // 回転行列を定義
                float2x2 rotationMatrix = float2x2(
                    cos(angle), -sin(angle),
                    sin(angle), cos(angle)
                );

                // UV座標を中心オフセット基準に変換し、回転を適用
                float2 centeredUV = i.uv - _Center;
                float2 rotatedUV = mul(rotationMatrix, centeredUV) * _Repeat; // 反復回数を適用

                // 反復時に偶数・奇数で反転
                float repeatedUV = frac(rotatedUV.x);
                float flip = step(0.5, repeatedUV); // 0～0.5ではそのまま、0.5～1.0では反転
                float adjustedUV = lerp(repeatedUV, 1.0 - repeatedUV, flip);

                // InOutQuadグラデーションの計算
                float gradientFactor;
                if (adjustedUV < 0.5)
                {
                    // 前半: 加速
                    gradientFactor = 2.0 * adjustedUV * adjustedUV;
                }
                else
                {
                    // 後半: 減速
                    gradientFactor = -1.0 + (4.0 - 2.0 * adjustedUV) * adjustedUV;
                }

                // グラデーション色の補間
                fixed4 color = lerp(_Color1, _Color2, saturate(gradientFactor));

                // アルファマスクを適用
                color.a *= maskAlpha;

                return color;
            }
            ENDCG
        }
    }
    FallBack "Unlit/Transparent"
}
