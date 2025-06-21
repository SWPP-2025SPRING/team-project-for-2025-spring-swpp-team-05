Shader "Custom/ExtremeGaussianBlur"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _BlurSize ("Blur Size", Float) = 1.0
        _WhiteIntensity ("White Overlay", Float) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            ZTest Always Cull Off ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float _BlurSize;
            float _WhiteIntensity;

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata_img v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float2 offset = _BlurSize * _MainTex_TexelSize.xy;

                fixed4 col = fixed4(0, 0, 0, 0);
                for (int x = -3; x <= 3; x++)
                {
                    for (int y = -3; y <= 3; y++)
                    {
                        float2 sampleUV = uv + offset * float2(x, y);
                        col += tex2D(_MainTex, sampleUV);
                    }
                }

                col /= 49.0;

                // 흰색 섞기
                col = lerp(col, fixed4(1, 1, 1, 1), _WhiteIntensity);

                return col;
            }
            ENDCG
        }
    }
}
