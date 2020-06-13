// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'



Shader "Blue/Character/BaseCube_Normal" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_CtrlTex ("Spec(R) Cube(B)", 2D) = "white" {}
		_MainColor ("Main Color", Color) = (0.913,0.913,0.913,0)
	    _SpecularColor ("Specular Color", Color) = (0,0,0,0)
	    _SpecularPow("Specular Pow" , float) = -0.6
		_NormalMap("Normal Map", 2D) = "bump"{}
	    _CubeTex ("Cube Map", CUBE) = "" {}
    	_CubeValue("Cube Value",float)=0
	    _LightDir("Light Direction" , Vector) =  (-2.3,3,-2.7,1)
	    
	    _ShadowDir("Shadow Direction", Vector) =  (-2.3,3,-2.7,1)
		_ShadowColor("Shadow Color", Color) = (0, 0, 0, 0)
		_ShadowAlpha("Shadow Alpha", float) = 0
		_ShadowHeight("Shadow Height", float) = 0.4
		_EdgeWidth("Edge Width", float) = 0
		_EdgePower("Edge Power", float) = 0
		_EdgeColor("Edge Color", Color) = (0, 0, 0, 1)
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
			
			sampler2D 	_NormalMap;
			float4 		_NormalMap_ST;
			half 		_EdgeWidth;
            half 		_EdgePower;
            half4 		_EdgeColor;
			
			struct VertexInput 
            {
                float4 vertex 	: POSITION;
                float3 normal 	: NORMAL;
                float4 uv 		: TEXCOORD0;
				float4 tangent 	: TANGENT;
            };
            
			struct VertexOutput
			{
				float4 pos : SV_POSITION;
			  	half2 uv : TEXCOORD0; // _MainTex
			  	half3 normal : TEXCOORD1;
			  	fixed3 vlight : TEXCOORD2; // ambient/SH/vertexlights
			  	half3 worldRefl : TEXCOORD3;
			  	half3 posWorld : TEXCOORD4;
				float3 tangent 	: TEXCOORD5;
                float3 binormal	: TEXCOORD6;
			};

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
			  	
			//  	float3 viewDir = -ObjSpaceViewDir(v.vertex);
		//	  	float3 viewRefl = reflect (viewDir, v.normal);
			  	o.worldRefl = mul ((float3x3)unity_ObjectToWorld, viewRefl);

				o.tangent 	= mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 )).xyz;
            	o.binormal	= cross(o.normal, o.tangent) * v.tangent.w;
			  	return o;
			}

			fixed4 frag_surf (VertexOutput IN) : COLOR 
			{
				
			  	fixed4 texColor = tex2D(_MainTex , TRANSFORM_TEX(IN.uv, _MainTex)) * _MainColor;
			  	fixed4 control = tex2D(_CtrlTex , TRANSFORM_TEX(IN.uv, _CtrlTex));
			  	fixed4 finalColor = texColor;
				IN.normal = normalize(IN.normal);

				float3x3 tangent_mat = float3x3(IN.tangent, IN.binormal, IN.normal);
				float3 normal = UnpackNormal(tex2D(_NormalMap, TRANSFORM_TEX(IN.uv, _NormalMap))).rgb;
            	normal = normalize(mul(normal, tangent_mat));
			  	half spec = max(dot(normal, normalize(_LightDir.xyz)),0.001);

				half3 view_dir = normalize(_WorldSpaceCameraPos.xyz - IN.posWorld);
			  	finalColor.rgb +=control.r *  _SpecularColor.rgb*2*pow(spec , _SpecularPow);
				
				float3 viewReflectDirection = reflect( -view_dir, normal );
			  	finalColor.rgb += texColor*control.b *texCUBE (_CubeTex, viewReflectDirection)*_CubeValue;

			  	float edgeWeight=_EdgeWidth-dot(view_dir,normal);
            	edgeWeight*=_EdgePower;
            	edgeWeight=clamp(edgeWeight, 0, 1);
            	finalColor.rgb=lerp(finalColor.rgb, _EdgeColor.rgb, edgeWeight);

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

