// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "hongpeiSea" {1
    Properties {
    	_Color ("Main Color", Color) = (0,-0.5,1,.1) //alpha include wave color
        _MainTex ("MainTexture", 2D) = "white" {}
        _DirectionUv("X Y(texUV) Z W(RaodongUV)", Vector) = (0.1,0.1,-0.1,-0.1)

        _WaveColor ("Normal Color", Color) = (0, 0.15, 0.115, 1)
        _WaveMap ("Normal Map", 2D) = "bump" {}
        _WaveUv("X(liangdu) Y(Heat) Z W(WaveUV)", Vector) = (1,-0.05,0.01,-0.015)
    }
    CGINCLUDE

			#include "UnityCG.cginc"
   			#include "Lighting.cginc"

            fixed4 _Color;
            sampler2D _MainTex;  float4 _MainTex_ST;
            half4 _DirectionUv;

            fixed4 _WaveColor;
			sampler2D _WaveMap;			float4 _WaveMap_ST;
			half4 _WaveUv;

            struct appdata {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT; 
				float4 texcoord : TEXCOORD0;
			};

			 struct appdata_LM {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT; 
				float4 texcoord : TEXCOORD0;
				float2 llllm : TEXCOORD1;
			};

           struct v2f
			{
				float4 pos:SV_POSITION;
				float4 scrPos : TEXCOORD0;
				float4 uv:TEXCOORD1;
				float4 TtoW0 : TEXCOORD2;  
				float4 TtoW1 : TEXCOORD3;  
				float4 TtoW2 : TEXCOORD4; 
			};

			struct v2f_LM
			{
				float4 pos:SV_POSITION;
				float4 scrPos : TEXCOORD0;
				float4 uv:TEXCOORD1;
				float4 TtoW0 : TEXCOORD2;  
				float4 TtoW1 : TEXCOORD3;  
				float4 TtoW2 : TEXCOORD4; 
				float2 lllmap : TEXCOORD5;
			};

           v2f vert(appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.scrPos = ComputeGrabScreenPos(o.pos);
				o.uv.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.uv.zw = TRANSFORM_TEX(v.texcoord, _WaveMap);

				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;  
				fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);  
				fixed3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);  
				fixed3 worldBinormal = cross(worldNormal, worldTangent) * v.tangent.w; 
				o.TtoW0 = float4(worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x);  
				o.TtoW1 = float4(worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y);  
				o.TtoW2 = float4(worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z);  
				return o;
			}

            fixed4 frag(v2f  i) : COLOR {
                
                float2 niudong = (i.uv.xy+_Time*float2(_DirectionUv.z,_DirectionUv.w*.1));
                float2 finally = (i.uv.xy+tex2D(_MainTex,TRANSFORM_TEX(niudong, _MainTex)).x*_WaveUv.y);
                float4 aa=tex2D(_MainTex,TRANSFORM_TEX(finally, _MainTex)+float2(_DirectionUv.x, _DirectionUv.y/30) * _Time)* _WaveUv.x*_Color;


                float3 worldPos = float3(i.TtoW0.w, i.TtoW1.w, i.TtoW2.w);
                fixed3 viewDir = normalize(UnityWorldSpaceViewDir(worldPos));
                float2 speed = _Time.y * float2(_WaveUv.z, _WaveUv.w);
                // Get the normal in tangent space
				fixed3 bump1 = UnpackNormal(tex2D(_WaveMap, i.uv.zw + speed)).rgb;
				fixed3 bump2 = UnpackNormal(tex2D(_WaveMap, i.uv.zw - speed)).rgb;
				fixed3 bump = normalize(bump1 + bump2);
				bump = normalize(half3(dot(i.TtoW0.xyz, bump), dot(i.TtoW1.xyz, bump), dot(i.TtoW2.xyz, bump)));
				fixed fresnel = pow(1 - saturate(dot(viewDir, bump)), 8);
				fixed3 finalColor =  _WaveColor.rgb * fresnel*1;//*X add number
				fixed4 col=fixed4(aa.xyz+finalColor, aa.w);
				return col;
            }
            //star baked show
            v2f_LM vertLM (appdata_LM v)
			{
				v2f_LM o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.scrPos = ComputeGrabScreenPos(o.pos);
				o.uv.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.uv.zw = TRANSFORM_TEX(v.texcoord, _WaveMap);

				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;  
				fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);  
				fixed3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);  
				fixed3 worldBinormal = cross(worldNormal, worldTangent) * v.tangent.w; 
				o.TtoW0 = float4(worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x);  
				o.TtoW1 = float4(worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y);  
				o.TtoW2 = float4(worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z);  

				o.lllmap = v.llllm.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				return o;
			}

			fixed4 fragLM (v2f_LM i) : COLOR    {
                float2 niudong = (i.uv.xy+_Time*float2(_DirectionUv.z,_DirectionUv.w*.1));
                float2 finally = (i.uv.xy+tex2D(_MainTex,TRANSFORM_TEX(niudong, _MainTex)).x*_WaveUv.y);
                float4 aa=tex2D(_MainTex,TRANSFORM_TEX(finally, _MainTex)+float2(_DirectionUv.x, _DirectionUv.y/30) * _Time)* _WaveUv.x*_Color;


                float3 worldPos = float3(i.TtoW0.w, i.TtoW1.w, i.TtoW2.w);
                fixed3 viewDir = normalize(UnityWorldSpaceViewDir(worldPos));
                float2 speed = _Time.y * float2(_WaveUv.z, _WaveUv.w);
                // Get the normal in tangent space
				fixed3 bump1 = UnpackNormal(tex2D(_WaveMap, i.uv.zw + speed)).rgb;
				fixed3 bump2 = UnpackNormal(tex2D(_WaveMap, i.uv.zw - speed)).rgb;
				fixed3 bump = normalize(bump1 + bump2);
				bump = normalize(half3(dot(i.TtoW0.xyz, bump), dot(i.TtoW1.xyz, bump), dot(i.TtoW2.xyz, bump)));
				fixed fresnel = pow(1 - saturate(dot(viewDir, bump)), 8);
				fixed3 finalColor =  _WaveColor.rgb * fresnel*1;//*X add number
				fixed4 col=fixed4(aa.xyz+finalColor, aa.w);
				col.xyz *= DecodeLightmap (UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lllmap));
				return col;
            }

            ENDCG

     SubShader {
	    	Tags{"IgnoreProjector"="True" "Queue"="Transparent" "RenderType"="Transparent" }
	        LOD 200


	        Pass {
	        	Tags { "LightMode" = "Vertex" } 
	        	Lighting Off ZWrite Off cull back
	       		Blend SrcAlpha OneMinusSrcAlpha
	            CGPROGRAM
	            #pragma vertex vert
	            #pragma fragment frag
	            #pragma multi_compile_fwdbase
	            #pragma fragmentoption ARB_precision_hint_fastest
	            ENDCG
			}

			Pass {
				Tags { "LightMode" = "VertexLM" }
				Lighting Off ZWrite Off cull back
	       		Blend SrcAlpha OneMinusSrcAlpha
				CGPROGRAM
				#pragma vertex vertLM
				#pragma fragment fragLM
				#pragma multi_compile_fwdbase
	            #pragma fragmentoption ARB_precision_hint_fastest
				ENDCG
			}
			//star baked show
			Pass {
				Tags { "LightMode" = "VertexLMRGBM" }
	            ZWrite On ZTest LEqual Cull back
				Blend SrcAlpha OneMinusSrcAlpha
				
				CGPROGRAM
				#pragma vertex vertLM
				#pragma fragment fragLM
				#pragma fragmentoption ARB_precision_hint_fastest
				ENDCG
			}

    }
    FallBack "Diffuse"
}
