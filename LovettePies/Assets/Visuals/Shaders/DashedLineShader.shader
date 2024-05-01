Shader "Unlit/DashedLineShader"
{
    Properties
    {
        _MainTex ("Albedo (RGD)", 2D) = "white" {}

        _OneDashLength("One Dash Length", float) = 1
        _Length("Current Length", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="TransparentCutout" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
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
            float4 _MainTex_ST;

            float _OneDashLength;
            float _Length;

            float ceiling(float x) {
                return -floor(-x);
            }

            v2f vert (appdata v)
            {
                int Repeatition = 1;
                if (_Length > _OneDashLength)
                {
                    Repeatition = ceiling(_Length/ _OneDashLength) + 1;
                }

                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv.y *= Repeatition;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                col.a = step(frac(i.uv.y / 2), .5);
                clip(col.a - 1.0);
                return col;
            }
            ENDCG
        }
    }
}
