﻿Shader "Hidden/Applibot/Particles/OverDrawer"
{
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"  "IgnoreProjector"="True"}
        LOD 100

        Pass
        {
            Cull Back
            ZWrite Off
            ZTest LEqual 
            Lighting Off
            Blend One One

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return half4(0.01, 0.01, 0.01, 1);
            }
            ENDCG
        }
    }
}
