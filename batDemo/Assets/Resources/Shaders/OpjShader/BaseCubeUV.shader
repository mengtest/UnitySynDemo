﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

//#warning Upgrade NOTE: unity_Scale shader variable was removed; replaced 'unity_Scale.w' with '1.0'

Shader "Blue/Character/BaseCubeUV" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_CtrlTex ("Spec(R) UV(G) Cube(B)", 2D) = "white" {}
		_MainColor ("Main Color", Color) = (1,1,1,1)
	    _SpecularColor ("Specular Color", Color) = (0.6,0.6,0.6,1)
	    _SpecularPow("Specular Pow" , float) = 0
	    _CubeTex ("Cube Map", CUBE) = "" {}
    	_CubeValue("Cube Value",float)=1
	    _LightDir("Light Direction" , Vector) = (0,0,0,1)
	    
	    _ShadowDir("Shadow Direction", Vector) = (0, 1, 1, 1)
		_ShadowColor("Shadow Color", Color) = (0, 0, 0, 0.3)
		_ShadowAlpha("Shadow Alpha", float) = 0.3
		_ShadowHeight("Shadow Height", float) = 0
		_EdgeWidth("Edge Width", float) = 0
		_EdgePower("Edge Power", float) = 1
		_EdgeColor("Edge Color", Color) = (0, 0, 0, 1)
		
		_ScrollTex("UV Anim Map", 2D) = "white"{}
		_ScrollColor("UV Color", Color) = (1, 1, 1, 1)
		_ScrollPower("UV Power", float) = 0
		_ScrollX("Anim X", float) = 0
		_ScrollY("Anim Y", float) = 0
	}
	
	SubShader 
	{
		Tags { "Queue" = "Transparent-10" }
	
		Pass 
		{
			Name "FORWARD"
			Tags { "LightMode" = "ForwardBase" }

			CGPROGRAM
			#pragma vertex vert_surf
			#pragma fragment frag_surf
			#pragma target 3.0

			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _MainColor;
			sampler2D _CtrlTex;
			float4 _CtrlTex_ST;
			fixed4 _SpecularColor;
			half _SpecularPow;
			samplerCUBE _CubeTex;
			half _CubeValue;
			half4 _LightDir;
			
			half 		_EdgeWidth;
            half 		_EdgePower;
            half4 		_EdgeColor;
            
            sampler2D 	_ScrollTex;
            float4 		_ScrollTex_ST;
            fixed4 		_ScrollColor;
            half 		_ScrollPower;
            half 		_ScrollX;
            half 		_ScrollY;
			
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
			  	half3 worldRefl : TEXCOORD3;
			  	half3 posWorld : TEXCOORD4;
			};

			// vertex shader
			VertexOutput vert_surf (VertexInput v) 
			{
			  	VertexOutput o;

			  	o.pos = UnityObjectToClipPos (v.vertex);
			  	o.uv = v.uv.xy;
			  	o.normal = normalize(mul((float3x3)unity_ObjectToWorld, SCALED_NORMAL));
			  	o.vlight = ShadeSH9 (float4(o.normal,1.0));
			  	o.posWorld 	= mul(unity_ObjectToWorld, v.vertex).xyz;
			  	
			  	float3 viewDir = normalize(-ObjSpaceViewDir(v.vertex));
			  	float3 viewRefl = reflect (viewDir, normalize(v.normal));
			  	
		//	  	float3 viewDir = -ObjSpaceViewDir(v.vertex);
	//		  	float3 viewRefl = reflect (viewDir, v.normal);
			  	o.worldRefl = mul ((float3x3)unity_ObjectToWorld, viewRefl);
			  
			  	return o;
			}

			// fragment shader
			fixed4 frag_surf (VertexOutput IN) : COLOR 
			{
				IN.normal = normalize(IN.normal);
			  	fixed4 texColor = tex2D(_MainTex , TRANSFORM_TEX(IN.uv, _MainTex)) * _MainColor;
			  	fixed4 control = tex2D(_CtrlTex , TRANSFORM_TEX(IN.uv, _CtrlTex));
			  	
			  	fixed4 finalColor = texColor;
			  
			  	half spec = max(dot(IN.normal , normalize(_LightDir.xyz)),0.001);
			  	finalColor.rgb += texColor.rgb * control.r * (_SpecularColor.rgb*8*spec*pow(spec , _SpecularPow)+min(_SpecularPow/10,0.3)+IN.vlight);
			  	finalColor.rgb +=texColor* control.b *texCUBE (_CubeTex, IN.worldRefl).rgb*_CubeValue;
			  	
			  	half3 view_dir = normalize(_WorldSpaceCameraPos.xyz - IN.posWorld);
			  	
			  	float edgeWeight=_EdgeWidth-dot(view_dir,IN.normal);
            	edgeWeight*=_EdgePower;
            	edgeWeight=clamp(edgeWeight, 0, 1);
            	finalColor.rgb=lerp(finalColor.rgb, _EdgeColor.rgb, edgeWeight);
            	
            	IN.uv.x += _ScrollX*_Time.x;
            	IN.uv.y += _ScrollY*_Time.x;
            	fixed4 scroll_col = tex2D(_ScrollTex, TRANSFORM_TEX(IN.uv, _ScrollTex));
            	scroll_col.rgb *= _ScrollPower * scroll_col.a * control.g;
            	scroll_col *= _ScrollColor;
            		
            	finalColor.rgb += scroll_col.rgb;
            	       	
			  	return finalColor;
			}

			ENDCG

		}
		Pass 
		{
			Tags { "RenderType"="Opaque"} 
			Blend SrcAlpha OneMinusSrcAlpha
			Stencil
			{  
			    Ref 2
			    Comp NotEqual  
			    Pass Replace
			}
			
			Fog{Mode off}
			
			CGPROGRAM
			#pragma vertex vert 
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			struct vertexOutput 
			{
				float4 pos : SV_POSITION;
				float2 xy : TEXCOORD0;
			};

			fixed4 _ShadowColor;
			half _ShadowAlpha;
			half _ShadowHeight;
			fixed4 _ShadowDir;
			
			vertexOutput vert(appdata_base v)
			{
				float4 vt;
				vertexOutput output;
				
				output.xy=float2(0,0);
				
				vt= mul(unity_ObjectToWorld, v.vertex);
				vt.xyz*=1.0;
				output.xy.y=vt.y/vt.w;
				
				fixed3 lightDir=-float3(_ShadowDir.x*1000,_ShadowDir.y*1000,_ShadowDir.z*1000);
				//v.vertex.xyz-float3(_ShadowDir.x*1000,_ShadowDir.y*1000,_ShadowDir.z*1000);


				vt.xz=vt.xz-((vt.y-_ShadowHeight)/lightDir.y)*lightDir.xz;
				vt.y=0.001+_ShadowHeight;
				vt=mul(unity_WorldToObject,vt);
				
				output.pos=UnityObjectToClipPos(vt);
				
				return output;
			}
	 		float4 frag(vertexOutput input) : COLOR 
			{
				if (input.xy.y<_ShadowHeight)
				{
					discard;
				}
				return float4(_ShadowColor.xyz, _ShadowAlpha);
			}
	 		ENDCG 
		}
    }
	FallBack "Diffuse"
}

