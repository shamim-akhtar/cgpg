Shader "Custom/Shader04_PerlinNoiseCloud"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
        _Frequency ("Frequency", Float) = 1.0
        _Amplitude ("Amplitude", Float) = 0.5
        _Speed ("Speed", Float) = 1.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float4 _Color;
            float _Frequency;
            float _Amplitude;
            float _Speed;

            float2 hash(float2 p) {
                return frac(sin(float2(dot(p, float2(127.1, 311.7)), dot(p, float2(269.5, 183.3)))) * 43758.5453);
            }

            // Perlin noise function
            float noise(float2 p) {
                float2 i = floor(p);
                float2 f = frac(p);

                float2 u = f * f * (3.0 - 2.0 * f);

                float n = dot(hash(i + float2(0.0, 0.0)), f - float2(0.0, 0.0));
                float n1 = dot(hash(i + float2(1.0, 0.0)), f - float2(1.0, 0.0));
                float n2 = dot(hash(i + float2(0.0, 1.0)), f - float2(0.0, 1.0));
                float n3 = dot(hash(i + float2(1.0, 1.0)), f - float2(1.0, 1.0));

                return lerp(lerp(n, n1, u.x), lerp(n2, n3, u.x), u.y);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv * _Frequency + _Time.y * _Speed;
                float n = noise(uv) * _Amplitude;
                float alpha = saturate(n); // Make sure alpha is between 0 and 1
                return float4(_Color.rgb, alpha) * _Color.a;
            }
            ENDCG
        }
    }
}
