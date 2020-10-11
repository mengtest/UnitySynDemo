

Shader "Star/Chenbinglin" {
	Properties {
		_Color("Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	}
	CGINCLUDE

				#include "UnityCG.cginc"

				sampler2D _MainTex;
				float4 _MainTex_ST;
				float4 _Color;
				struct appdata {
					float4 vertex : POSITION;
					float2 texcoord : TEXCOORD0;
				};
				
				struct appdata_LM {
					float4 vertex : POSITION;
					float2 texcoord : TEXCOORD0;
					float2 llllm : TEXCOORD1;
				};

				struct v2f {
					float4 vertex : POSITION;
					half2 texcoord : TEXCOORD0;

				};

				struct v2f_LM {
					float4 vertex : POSITION;
					half2 texcoord : TEXCOORD0;
					float2 lllmap : TEXCOORD1;
				};
					  
				v2f vert (appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
					return o;
				}

				fixed4 frag (v2f i) : COLOR   
				{
					fixed4 col = tex2D(_MainTex, i.texcoord)*_Color ;
					return col;
				}
				//star baked show
				v2f_LM vertLM (appdata_LM v)
				{
					v2f_LM o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
					o.lllmap = v.llllm.xy * unity_LightmapST.xy + unity_LightmapST.zw;
					return o;
				}

				fixed4 fragLM (v2f_LM i) : COLOR   
				{
					fixed4 col = tex2D(_MainTex, i.texcoord)*_Color ;
					col.xyz *= DecodeLightmap (UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lllmap));
					return col;
				}

				ENDCG
			

	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 100

		Pass {  
				Tags { "LightMode" = "Vertex" } 
				ZWrite On ZTest On Cull back
				Blend SrcAlpha OneMinusSrcAlpha

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest

				ENDCG
			}
		Pass {
				Tags { "LightMode" = "VertexLM" }
	            ZWrite On ZTest On Cull back
				Blend SrcAlpha OneMinusSrcAlpha
				
				CGPROGRAM
				#pragma vertex vertLM
				#pragma fragment fragLM
				#pragma fragmentoption ARB_precision_hint_fastest
				
				ENDCG
			}
		//star baked show
		Pass {
				Tags { "LightMode" = "VertexLMRGBM" }
	            ZWrite On ZTest On Cull back
				Blend SrcAlpha OneMinusSrcAlpha
				
				CGPROGRAM
				#pragma vertex vertLM
				#pragma fragment fragLM
				#pragma fragmentoption ARB_precision_hint_fastest
				ENDCG
			}
	}

Fallback "Transparent/VertexLit"
}
