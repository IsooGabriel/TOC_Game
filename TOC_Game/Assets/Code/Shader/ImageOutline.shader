Shader "CustomRenderTexture/ImageOutline"
{
     Properties {
        _MainTex ("Base (RGB), Alpha (A)", 2D) = "white" {}
        _BlurColor ("Blur Color", Color) = (1, 1, 1, 1)
        _BlurSize ("Blur Size", float) = 1
    }

    // pass�ŋ��ʏ������܂Ƃ߂�
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
    // MainTex�̃T�C�Y������
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
        // -1 ~ 1��for����
        int k = 1;
        float2 blurSize = _BlurSize * _MainTex_TexelSize.xy;
        float blurAlpha = 0;
        float2 tempCoord = float2(0,0);
        float tempAlpha;
        // xy������blurSize���Y�����A�A���t�@�l���v�Z
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
            // �v���W�F�N�^�[�̉e�����󂯂Ȃ��悤��
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            // �}�e���A����incpecter�ł̕\����plane��
            "PreviewType"="Plane"
            // sprite�ɕs�������ꍇ��false�ɂȂ�
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        // Canvas��RenderMode�ɂ���ē��I�ɕω�
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
