Shader "CustomRenderTexture/ImageOutline"
{
     Properties {
        _MainTex ("Base (RGB), Alpha (A)", 2D) = "white" {}
        _BlurColor ("Blur Color", Color) = (1, 1, 1, 1)
        _BlurSize ("Blur Size", float) = 1
    }

    // passで共通処理をまとめる
    CGINCLUDE
    struct appdata {
        float4 vertex   : POSITION;
        float2 texcoord : TEXCOORD0;
    };

    struct v2f {
        float4 vertex   : SV_POSITION;
        half2 texcoord  : TEXCOORD0;
        float4 worldPosition : TEXCOORD1;
    };

    #include "UnityCG.cginc"

    sampler2D _MainTex;
    // MainTexのサイズを扱う
    // x : 1.0 / width
    // y : 1.0 / height
    // z : width
    // w : height
    float4 _MainTex_TexelSize;
    fixed4 _BlurColor;
    float _BlurSize;

    v2f vert (appdata v) {
        v2f o;
        o.worldPosition = v.vertex;
        o.vertex = UnityObjectToClipPos(o.worldPosition);
        o.texcoord = v.texcoord;
        return o;
    }

    fixed4 frag(v2f v) : SV_Target {
        half4 color = (tex2D(_MainTex, v.texcoord));
        return color;
    }

    fixed4 frag_blur (v2f v) : SV_Target {
        // -1 ~ 1でforを回す
        int k = 1;
        float2 blurSize = _BlurSize * _MainTex_TexelSize.xy;
        float blurAlpha = 0;
        float2 tempCoord = float2(0,0);
        float tempAlpha;
        // xy方向にblurSize分ズラし、アルファ値を計算
        for (int px = -k; px <= k; px++) {
            for (int py = -k; py <= k; py++) {
                tempCoord = v.texcoord;
                tempCoord.x += px * blurSize.x;
                tempCoord.y += py * blurSize.y;
                tempAlpha = tex2D(_MainTex, tempCoord).a;
                blurAlpha += tempAlpha;
            }
        }

        half4 blurColor = _BlurColor;
        blurColor.a *= blurAlpha;
        return blurColor;
    }
    ENDCG

    SubShader {
        Tags {
            "Queue"="Transparent"
            // プロジェクターの影響を受けないように
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            // マテリアルのincpecterでの表示をplaneに
            "PreviewType"="Plane"
            // spriteに不具合がある場合にfalseになる
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        // CanvasのRenderModeによって動的に変化
        // Screen Space - Overlay : Always
        // Screen Space - Camera : LEqual
        // World Space : LEqual
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag_blur
            ENDCG
        }

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            ENDCG
        }
    }
}
