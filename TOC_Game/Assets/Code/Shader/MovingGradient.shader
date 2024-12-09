Shader "UI/SingleColorMovingGradient"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (0, 0.396, 0.321, 1) // 006552 �� RGB �l
        _Speed ("Gradient Speed", Float) = 1.0
    }
    SubShader
    {
        Tags {"Queue"="Overlay" "IgnoreProjector"="True" "RenderType"="Transparent"}
        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            
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

            float _Speed;
            float4 _BaseColor;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // ���I�Ȏ��ԃx�[�X�̕�Ԓl
                float t = abs(sin(_Time.y * _Speed));

                // ��F�� HSV �ɕϊ�
                float3 baseHSV = RGBToHSV(_BaseColor.rgb);

                // �ʓx�Ɩ��x�͈̔�
                float minS = 0.396; // 006552 �̍ʓx
                float maxS = 1.0;   // 00FFD4 �̍ʓx
                float minV = 0.321; // 006552 �̖��x
                float maxV = 0.831; // 00FFD4 �̖��x

                // �ʓx�Ɩ��x�̕��
                float s = lerp(minS, maxS, t);
                float v = lerp(minV, maxV, t);

                // �V�����F���v�Z
                float3 resultRGB = HSVToRGB(float3(baseHSV.r, s, v));

                return float4(resultRGB, 1.0);
            }

            // RGB to HSV conversion function
            float3 RGBToHSV(float3 rgb)
            {
                float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                float4 p = rgb.g < rgb.b ? float4(rgb.bg, K.wz) : float4(rgb.gb, K.xy);
                float4 q = rgb.r < p.x ? float4(p.xyw, rgb.r) : float4(rgb.r, p.yzx);

                float d = q.x - min(q.w, q.y);
                float e = 1.0e-10;
                return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
            }

            // HSV to RGB conversion function
            float3 HSVToRGB(float3 hsv)
            {
                float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
                float3 p = abs(frac(hsv.xxx + K.xyz) * 6.0 - K.www);
                return hsv.z * lerp(K.xxx, saturate(p - K.xxx), hsv.y);
            }
            ENDCG
        }
    }
}
