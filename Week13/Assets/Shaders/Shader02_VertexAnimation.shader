Shader "Custom/Shader02_WaveShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _Frequency ("Wave Frequency", Float) = 1.0
        _Amplitude ("Wave Amplitude", Float) = 0.1
        _Speed ("Wave Speed", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _Color;
            float _Frequency;
            float _Amplitude;
            float _Speed;

            v2f vert (appdata v)
            {
                v2f o;
                float3 pos = v.vertex.xyz;
                pos.y += sin(_Frequency * pos.x + _Time.y * _Speed) * _Amplitude;
                o.pos = UnityObjectToClipPos(float4(pos, 1.0));
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.uv);
                return texColor * _Color;
            }
            ENDCG
        }
    }
}
