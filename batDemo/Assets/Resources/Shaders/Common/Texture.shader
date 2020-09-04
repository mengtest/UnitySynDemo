﻿Shader "HHHJ/Common/Texture"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
        [Enum(Off, 0, Front, 1, Back, 2)]_Cull ("Cull", float) = 2
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 100
        
		Cull [_Cull]

		Pass
		{
			Name "Base"
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD;
			};
			
			struct v2f {
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};
			
			
			sampler2D _MainTex;
			float4 _MainTex_ST;
			half4 _Color;
			
			v2f vert(appdata v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target {
				fixed4 col = tex2D(_MainTex, i.uv) * _Color;
				return col;
			}
			ENDCG
		}
    }
}
