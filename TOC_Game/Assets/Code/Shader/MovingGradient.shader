Shader "UI/MonochromeMovingGradient"
{

    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ScrollSpeed ("Scroll Speed", float) = 1.0
        _Scroll ("Scroll Direction", float2) = (1, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

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

            sampler2D _MainTex;
            float4 _MainTex_ST; // Unity�������Ő�������e�N�X�`����ST�i�X�P�[���ƃI�t�Z�b�g�j
            float _ScrollSpeed; // �X�N���[�����x
            float2 _Scroll; // �X�N���[������

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // ���ԂɊ�Â�UV�I�t�Z�b�g�v�Z
                float2 uv = i.uv;
                uv.x += _ScrollSpeed * _Time * _Scroll.x; // X�����ɃX�N���[��
                uv.y += _ScrollSpeed * _Time * _Scroll.y; // Y�����ɃX�N���[��
                uv = frac(uv); // UV��0-1�͈͓��ɐ���

                // �e�N�X�`���T���v�����O
                fixed4 col = tex2D(_MainTex, uv);
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"

}
