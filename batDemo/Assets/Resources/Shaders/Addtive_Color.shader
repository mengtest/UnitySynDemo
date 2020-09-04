﻿Shader "ZCW/Addtive Color"
{
    Properties
    {
		_Color ("Addtive Color", Color) = (1, 1, 1, 1)
		_MainTex ("Main Texture", 2D) = "white" {}
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
		

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
				float2 uv : TEXCOORD;
            };

            struct v2f
            {
			    float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
            };

            fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return lerp(tex2D(_MainTex, i.uv), _Color, _Color.a);
            }
            ENDCG
        }
    }
}
