Shader "HHHJ/Water/Cubemap"
{
    Properties
    {
		_Color ("Color", Color) = (1,1,1,1)
		_BumpMap ("Bump Map", 2D) = "white" {}
		_FlowMap ("Flow Map", 2D) = "white" {}
		_Speed ("Speed", Vector) = (0, 0, 0, 0)
		_SkyCube ("Sky Cube", Cube) = ""{}
		_FoamColor ("Foam Color", Color) = (1,1,1,1)
		_FoamDepth ("Foam Depth", Float) = 0.3
		_FoamIntensity ("Foam Intensity", Float) = 1
		_Distortion ("Distortion", Range(0, 100)) = 50
    }
	
	
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 1500
		
		Blend Off
		
		GrabPass { "_WaterBackground" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
			#pragma multi_compile_fog

            #include "UnityCG.cginc"
			#include "Lighting.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD1;
                float4 projPos : TEXCOORD2;
				float4 screenPos : TEXCOORD3;
            };
			
            sampler2D _CameraDepthTexture;
			fixed4 _Color;
			half4 _Speed;
			samplerCUBE _SkyCube;
			sampler2D _BumpMap;
			float4 _BumpMap_ST;
			sampler2D _FlowMap;
			float4 _FlowMap_ST;
			half4 _FoamColor;
			float _FoamDepth;
			half _FoamIntensity;
			sampler2D _WaterBackground;
			float4 _WaterBackground_ST;
			float4 _WaterBackground_TexelSize;
			half _Distortion;
			
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.uv.xy = TRANSFORM_TEX(v.uv, _BumpMap) + _Time.x * float2(_Speed.x, _Speed.y);
                o.uv.zw = TRANSFORM_TEX(v.uv, _FlowMap) + _Time.x * float2(_Speed.z, _Speed.w);
				o.projPos = ComputeScreenPos (o.vertex);
				COMPUTE_EYEDEPTH(o.projPos.z);
				o.screenPos = o.vertex;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float sceneZ = max(0, LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.y);
				float diffZ = sceneZ - max(0,i.projPos.z - _ProjectionParams.g) + 0.01;
				
				fixed4 col = lerp(_Color, _FoamColor, saturate(1.0 - diffZ / _FoamDepth) * _FoamIntensity);
				
				float3 N1 = normalize(UnpackNormal(tex2D(_BumpMap, i.uv.xy)));
				float3 N2 = normalize(UnpackNormal(tex2D(_FlowMap, i.uv.zw))); 
				
				float3 N = normalize(N1 + N2);
				float3 L = normalize(UnityWorldSpaceLightDir(i.worldPos));
				float3 V = normalize(UnityWorldSpaceViewDir(i.worldPos));
				
				fixed3 diffuse = col.rgb * saturate(dot(N, -L));
				fixed3 specular = texCUBE(_SkyCube, reflect(-V, N));
				
				i.screenPos = float4(i.screenPos.xy / i.screenPos.w, 0, 0);
                #if UNITY_UV_STARTS_AT_TOP
					i.screenPos.y *= -1;
                #else
					i.screenPos.y *= _ProjectionParams.x;
                #endif
				float2 sceneUVs = i.screenPos.xy * 0.5 + 0.5;
				float2 offset = N.xy * _WaterBackground_TexelSize.xy * _Distortion;
				col.rgb =  lerp(tex2D(_WaterBackground, sceneUVs + offset), diffuse + specular, _Color.a);
				col.a = 1.0;
				
				
				// 片元着色器计算雾效
				float3 clipPos = UnityWorldToClipPos(i.worldPos);
				#if defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2)
					#if (SHADER_TARGET < 30) || defined(SHADER_API_MOBILE)
						UNITY_CALC_FOG_FACTOR_RAW(UNITY_Z_0_FAR_FROM_CLIPSPACE(clipPos.z));
						float1 fogCoord = unityFogFactor;
					#else
						float1 fogCoord = i.vertex.z;
					#endif
				#endif
				
                UNITY_APPLY_FOG(fogCoord, col);
				
				
                return col;
            }
            ENDCG
        }
    }
	
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		
		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
			#include "Lighting.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD1;
            };
			
			fixed4 _Color;
			half4 _Speed;
			
			samplerCUBE _SkyCube;
			
			sampler2D _BumpMap;
			float4 _BumpMap_ST;
			
			sampler2D _FlowMap;
			float4 _FlowMap_ST;
			
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.uv.xy = TRANSFORM_TEX(v.uv, _BumpMap) + _Time.x * float2(_Speed.x, _Speed.y);
                o.uv.zw = TRANSFORM_TEX(v.uv, _FlowMap) + _Time.x * float2(_Speed.z, _Speed.w);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col;
				
				float3 N1 = normalize(UnpackNormal(tex2D(_BumpMap, i.uv.xy)));
				float3 N2 = normalize(UnpackNormal(tex2D(_FlowMap, i.uv.zw))); 
				
				float3 N = normalize(N1 + N2);
				float3 L = normalize(UnityWorldSpaceLightDir(i.worldPos));
				float3 V = normalize(UnityWorldSpaceViewDir(i.worldPos));
				
				// Diffuse
				fixed3 diffuse = _Color.rgb * saturate(dot(N, -L));
				
				// Specular
				fixed3 specular = texCUBE(_SkyCube, reflect(-V, N));
				
				col.rgb = diffuse + specular;
				col.a = _Color.a;
				
                return col;
            }
            ENDCG
        }
    }
}
