// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Blue/Character/BaseNoShadow" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_MainColor ("Main Color", Color) = (1,1,1,1)
	    _SpecularColor ("Specular Color", Color) = (0.6,0.6,0.6,1)
	    _SpecularPow("Specular Pow" , float) = 0
	    _LightDir("Light Direction" , Vector) = (0,0,0,1)
	    
	    _ShadowDir("Shadow Direction", Vector) = (0, 1, 1, 1)
		_ShadowColor("Shadow Color", Color) = (0, 0, 0, 0.3)
		_ShadowAlpha("Shadow Alpha", float) = 0.3
		_ShadowHeight("Shadow Height", float) = 0
		_EdgeWidth("Edge Width", float) = 0
		_EdgePower("Edge Power", float) = 1
		_EdgeColor("Edge Color", Color) = (0, 0, 0, 1)
	}
	
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
	
		Pass 
		{
			Name "FORWARD"
			Tags { "LightMode" = "ForwardBase" }

			CGPROGRAM

			#pragma vertex vert_surf
			#pragma fragment frag_surf

			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _MainColor;
			fixed4 _SpecularColor;
			half _SpecularPow;
			half4 _LightDir;
			
			half 		_EdgeWidth;
            half 		_EdgePower;
            half4 		_EdgeColor;
			
			struct VertexInput 
            {
                float4 vertex 	: POSITION;
                float3 normal 	: NORMAL;
                float4 uv 		: TEXCOORD0;
            };
            
			struct VertexOutput
			{
				float4 pos : SV_POSITION;
			  	half2 uv : TEXCOORD0; // _MainTex
			  	half3 normal : TEXCOORD1;
			  	fixed3 vlight : TEXCOORD2; // ambient/SH/vertexlights
			  	half3 posWorld : TEXCOORD3;
			};

			// vertex shader
			VertexOutput vert_surf (VertexInput v) 
			{
			  	VertexOutput o;

			  	o.pos = UnityObjectToClipPos (v.vertex);
			  	o.uv.xy = v.uv;
			  	o.normal = normalize(mul((float3x3)unity_ObjectToWorld, SCALED_NORMAL));
			  	o.vlight = ShadeSH9 (float4(o.normal,1.0));
			  	o.posWorld 	= mul(unity_ObjectToWorld, v.vertex).xyz;
			  
			  	return o;
			}

			// fragment shader
			fixed4 frag_surf (VertexOutput IN) : COLOR 
			{
				IN.normal = normalize(IN.normal);
			  	fixed4 texColor = tex2D(_MainTex , TRANSFORM_TEX(IN.uv, _MainTex)) * _MainColor;

			  	fixed4 finalColor = texColor;
			  
			  	half spec = max(dot(IN.normal , normalize(_LightDir.xyz)),0.001);
			  	finalColor.rgb += texColor * (_SpecularColor.rgb*8*spec*pow(spec , _SpecularPow)+min(_SpecularPow/10,0.3)+IN.vlight);
			  	
			  	half3 view_dir = normalize(_WorldSpaceCameraPos.xyz - IN.posWorld);
			  	
			  	float edgeWeight=_EdgeWidth-dot(view_dir,IN.normal);
            	edgeWeight*=_EdgePower;
            	edgeWeight=clamp(edgeWeight, 0, 1);
            	finalColor.rgb=lerp(finalColor.rgb, _EdgeColor.rgb, edgeWeight);
            	

			  	return finalColor;
			}

			ENDCG

		}
    }
	FallBack "Diffuse"
}

