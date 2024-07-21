Shader "Custom/Shader01_MyFirstShader"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
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
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 color : COLOR;
            };

            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                //o.color = float4(v.vertex.xyz * 0.1, 1.0); // Change color based on position
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return _Color; // Apply the color property
            }
            ENDCG
        }
    }
}
