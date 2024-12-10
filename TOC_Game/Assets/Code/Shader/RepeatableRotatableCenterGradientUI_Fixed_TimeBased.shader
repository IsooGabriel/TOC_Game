Shader "Custom/RepeatableRotatableCenterGradientUI_Fixed_TimeBased"
{
    Properties
    {
        _Color1 ("Color 1", Color) = (0.0, 0.396, 0.322, 1.0) // #006552
        _Color2 ("Color 2", Color) = (0.0, 1.0, 0.831, 1.0)   // #00FFD4
        _Rotation ("Rotation (Degrees)", Range(0, 360)) = 0.0 // �O���f�[�V�����̉�]�p
        _MainTex ("Mask Texture", 2D) = "white" {}            // �}�X�N�p�e�N�X�`��
        _CenterSpeed ("Center Speed", Float) = 0.5           // �O���f�[�V�������S�̓����̑��x
        _Repeat ("Repeat Count", Float) = 1.0                 // �O���f�[�V�����̔�����
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
            float _CenterSpeed;
            float _Repeat;

            // ���ԂɊ�Â��ăO���f�[�V�������S�𓮂���
            float2 GetDynamicCenter(float time)
            {
                float xOffset = time * _CenterSpeed;
                float yOffset = 0;
                return float2(xOffset, yOffset);
            }

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // �}�X�N�e�N�X�`���̃A���t�@�l���擾
                float maskAlpha = tex2D(_MainTex, i.uv).a;

                // ���ԂɊ�Â��ăO���f�[�V�������S�𓮂���
                float time = _Time.y;  // �O���[�o�����Ԃ��擾
                float2 dynamicCenter = GetDynamicCenter(time);

                // ��]�p�����W�A���ɕϊ�
                float angle = radians(_Rotation);

                // ��]�s����`
                float2x2 rotationMatrix = float2x2(
                    cos(angle), -sin(angle),
                    sin(angle), cos(angle)
                );

                // UV���W�𒆐S�I�t�Z�b�g��ɕϊ����A��]��K�p
                float2 centeredUV = i.uv - dynamicCenter;
                float2 rotatedUV = mul(rotationMatrix, centeredUV) * _Repeat; // �����񐔂�K�p

                // �������ɋ����E��Ŕ��]
                float repeatedUV = frac(rotatedUV.x);
                float flip = step(0.5, repeatedUV); // 0�`0.5�ł͂��̂܂܁A0.5�`1.0�ł͔��]
                float adjustedUV = lerp(repeatedUV, 1.0 - repeatedUV, flip);

                // �O���f�[�V�����W���̌v�Z
                float gradientFactor = abs(adjustedUV - 0.5) * 2.0;

                // �O���f�[�V�����F�̕��
                fixed4 color = lerp(_Color1, _Color2, gradientFactor);

                // �A���t�@�}�X�N��K�p
                color.a *= maskAlpha;

                return color;
            }
            ENDCG
        }
    }
    FallBack "Unlit/Transparent"
}
