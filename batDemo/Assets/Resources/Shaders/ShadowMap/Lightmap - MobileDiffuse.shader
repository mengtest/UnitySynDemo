// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable

// Upgrade NOTE: commented out 'sampler2D unity_Lightmap', a built-in variable

Shader "HHHJ/Lightmap/Mobile/Diffuse"
{
	Properties{
		_MainTex("Main Tex", 2D) = "white" {}
	}

		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 150

			CGPROGRAM
			#pragma surface surf Lambert noforwardadd.

			sampler2D _MainTex;
			struct Input {
				float2 uv_MainTex;
			};
			void surf(Input IN, inout SurfaceOutput o) {
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
				o.Albedo = c.rgb;
				o.Alpha = c.a;
			}
			ENDCG

				// Pass to render object as a shadow caster
				Pass
			{
					Name "ShadowCaster"
					Tags { "LightMode" = "ShadowCaster" }

					ZWrite On ZTest LEqual Cull Back

					CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#pragma target 2.0
					#pragma multi_compile_shadowcaster
					#include "UnityCG.cginc"

					struct v2f {
							V2F_SHADOW_CASTER;
							UNITY_VERTEX_OUTPUT_STEREO
					};

					v2f vert(appdata_base v)
					{
							v2f o;
							UNITY_SETUP_INSTANCE_ID(v);
							UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
							TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
							return o;
					}

					float4 frag(v2f i) : SV_Target
					{
							SHADOW_CASTER_FRAGMENT(i)
					}
					ENDCG
			}
	}



		Fallback "Mobile/VertexLit"
}
