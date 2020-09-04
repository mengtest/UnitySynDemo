

Shader "Star/linshi" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
	_WaveSpeed ("Wave Speed", Vector) = (1,1,1,1)
	_WaveSize ("Wave Size", float) = 0.1
	_WavePhase ("Wave Phase", float) = 0.02
}

SubShader {
	Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
	LOD 100
	ZWrite On ZTest LEqual Cull back
	Pass {  
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;
			fixed _Cutoff;
			//#ifndef LIGHTMAP_OFF
			//sampler2D unity_Lightmap;	 
			// float4 unity_LightmapST;
			//#endif

			struct v2f {
				float4 vertex : POSITION;
				half2 texcoord : TEXCOORD0;
				//#ifndef LIGHTMAP_OFF
				//float2 lmap : TEXCOORD1;
				//#endif
			};

			float4 _WaveSpeed;
			float _WaveSize;
			float _WavePhase;
				  
			v2f vert (appdata_full v)
			{
				v2f o;
				float3 waves = _WaveSize * v.color;
				float3 freq = _Time.y * _WaveSpeed.xyz;
				v.vertex.xyz += cos(freq + v.vertex.xyz * _WavePhase) * waves;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				
				//lightmap
				//#ifndef LIGHTMAP_OFF
				//o.lmap = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				//#endif
				return o;
			}

			fixed4 frag (v2f i) : COLOR
			{
				fixed4 col = tex2D(_MainTex, i.texcoord) ;
				//#ifndef LIGHTMAP_OFF
				//col.xyz *= DecodeLightmap (UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lmap));
				//#endif
				clip( col.a - _Cutoff );
				return col*_Color;
			}
			ENDCG
		}
}

Fallback "Transparent/VertexLit"
}
