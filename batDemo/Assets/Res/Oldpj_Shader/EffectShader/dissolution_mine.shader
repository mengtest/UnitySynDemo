// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Blue/Effect/WDestroy" 
{
	Properties 
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_DetailTex ("Detail(RGB)", 2D) = "white" {}
		_StartAmount("StartAmount", float) = 0.05
		_StartColor("StartColor",Color)=(1,0,0,1)
		_Liangdu("Power",float)=1

	}
	SubShader 
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Pass 
		{
		  
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off Lighting Off ZWrite Off  Fog{ Mode Off}

			CGPROGRAM
			#pragma vertex vert_surf
			#pragma fragment frag_surf
			#include "UnityCG.cginc"
			
			sampler2D _MainTex; float4 _MainTex_ST;
			sampler2D _DetailTex; float4 _DetailTex_ST;
			fixed4 _Color;
			half _StartAmount;

			fixed4 _StartColor;
			half _Liangdu;
									
			struct VertexInput 
            {
                float4 vertex 	: POSITION;
                float4 uv 		: TEXCOORD0;
                float4 vertexColor : COLOR;
            };
			struct VertexOutput
			{
				float4 pos : SV_POSITION;
			  	half2 uv : TEXCOORD0; // _MainTex
			  	float4 vertexColor : COLOR;
			};
			VertexOutput vert_surf (VertexInput v) 
			{
			  	VertexOutput o=(VertexOutput)0;
			  	o.pos = UnityObjectToClipPos (v.vertex);
			  	o.uv.xy = v.uv;
			  	o.vertexColor = v.vertexColor;
			  	return o;
			}

			fixed4 frag_surf (VertexOutput IN) : COLOR 
			{
			  	fixed4 texColor = tex2D(_MainTex , TRANSFORM_TEX(IN.uv, _MainTex));
				fixed4 delColor = tex2D(_DetailTex , TRANSFORM_TEX(IN.uv, _DetailTex));
			  	
			  	fixed2 clipTemp;
			  	fixed change=1-_Color.a*IN.vertexColor.a;
			  	clipTemp.x = change;
			  	clipTemp.y = delColor.r - change;
			  	
			  //	texColor.rgb  *= (step(clipTemp.y, _StartAmount)*(_StartColor*4*clipTemp.y/_StartAmount) +1)*_Color*IN.vertexColor;
				clip(clipTemp);
			  	return texColor*_Liangdu*_Color*IN.vertexColor;
			}
			ENDCG
		}
    }
	FallBack "Diffuse"
}