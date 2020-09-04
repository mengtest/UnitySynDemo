// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable

// Upgrade NOTE: commented out 'sampler2D unity_Lightmap', a built-in variable

Shader "HHHJ/ShadowMap/Texture - Opaque"
{
	Properties {
		_MainTex ("Main Tex", 2D) = "white" {}
		[Enum(Off,0,front,1,back,2)] _Cull ("Cull", Int) = 2
	}

	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 150
		Cull [_Cull]
		ZTest LEqual
		ZWrite On

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ SHADOWMODE_CAMERA_DEPTH SHADOWMODE_DEPTH_ONLY
			
			#include "UnityCG.cginc"
			
			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD;
				float3 normal : NORMAL;
			};
			
			struct v2f {
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 proj : TEXCOORD1;
				float3 screenNormal : TEXCOORD2;
			};
			
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			
			uniform float4x4 DepthMap_Mat;
			uniform sampler2D DepthMap_Texture;
			uniform float DepthMap_Bias;
			uniform float4 DepthMap_Color;
			uniform float DepthMap_FactorSlope;
			
			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				float4x4 mat = mul(DepthMap_Mat, unity_ObjectToWorld);
				o.proj = mul(mat, v.vertex);
				o.screenNormal = mul(mat, v.normal); 
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target {
				fixed4 col = tex2D(_MainTex, i.uv);
				
				float4 proj = i.proj;
				proj.xy = proj.xy * 0.5 + proj.w * 0.5;
				#if UNITY_UV_START_AT_TOP
				proj.y = proj.w - proj.y;
				#endif
				proj.xy /= proj.w;
				float depth = proj.z / proj.w;
					
				#if defined(SHADER_API_MOBILE) || defined(SHADER_API_GLCORE)
					depth = depth / 2 + 0.5;
				#endif
				
				if (proj.x > 0 && proj.x < 1 && proj.y > 0 && proj.y < 1 && depth > 0 && depth < 1)
				{
					float camDepth = DecodeFloatRGBA(tex2D(DepthMap_Texture, proj.xy));
					
					#if defined(SHADOWMODE_CAMERA_DEPTH)
						col.rgb = (depth - camDepth) * 1000;
					#elif defined(SHADOWMODE_DEPTH_ONLY)
						col.rgb = depth;
					#else
						float3 normal = normalize(i.screenNormal.xyz);
						float slope = abs(atan(normal.y / sqrt(1 - normal.y * normal.y)));
						float shadowBias = DepthMap_FactorSlope * slope + DepthMap_Bias;
						
						#if defined(SHADER_API_MOBILE) || defined(SHADER_API_GLCORE)
							if (depth - camDepth > shadowBias)
						#else
							if (camDepth - depth > shadowBias)
						#endif
							{
								col.rgb *= DepthMap_Color;
							}
					#endif	
				}
				return col;
			}
			ENDCG
		}
	}

	Fallback "Mobile/VertexLit"
}
