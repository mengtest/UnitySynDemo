// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Star/Sea" {
	
Properties {
	_MainColor ("Base Ocean Color", Color) = (0.1137,0.4,0.31,1)
	_MainAlpha("Base Alpha",Range(0,1)) = 0.8
	_Normal("Water", 2D) = "bump" {}

	_ReflectionTex("Reflection Texture",2D) = "white"{}
	_ReflectionBias("Reflection Bias",float) = 0.01
	_ReflectionAlpha("Reflection Alpha", Range(0,1)) = 1.0
	_DirectionUv("Wet scroll direction (speed)", Vector) = (-0.1,-0.4, 0,2)
	_TexAtlasTiling("Tex atlas tiling (uv)", Vector) = (0,0, 3,6)
	
}

CGINCLUDE	

#include "UnityCG.cginc"

fixed4 _MainColor;
half	_MainAlpha;
sampler2D _Normal;

sampler2D _ReflectionTex;
half _ReflectionBias;
half _ReflectionAlpha;

half4 _DirectionUv;
half4 _TexAtlasTiling;


struct v2f_full
{
	half4 pos : SV_POSITION;
	half4 normalScrollUv : TEXCOORD0;	
	half4 testUV : TEXCOORD3;
};

ENDCG 

SubShader {
	Tags {"RenderType"="Transparent" "Queue"="Transparent"} 
	 
	LOD 200

	Pass {
		Blend SrcAlpha OneMinusSrcAlpha
		//Blend SrcAlpha One
		ZTest LEqual
		ZWrite Off
		Cull back
		Fog{Color (0,0,0,0) }
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma fragmentoption ARB_precision_hint_fastest 
		
		v2f_full vert (appdata_full v) 
		{
			v2f_full o;
			o.pos = UnityObjectToClipPos (v.vertex);
			o.testUV = v.texcoord;
			o.normalScrollUv.xyzw = v.texcoord.xyxy * _TexAtlasTiling + frac((_Time.xxxx) * _DirectionUv.xyxy * _DirectionUv.w);
			
			return o; 
		}
				
		fixed4 frag (v2f_full i) : COLOR0
		{	
			fixed4 normalMap_zw = tex2D(_Normal, i.normalScrollUv.zw);
			fixed4 rtReflNorm = (normalMap_zw - 0.5) * _ReflectionBias;
			fixed4 rtRefl = tex2D(_ReflectionTex ,i.testUV.xy+ rtReflNorm.xy);
			fixed t_ShadowAlpha = rtRefl.a * _ReflectionAlpha;
			
			
			fixed4 baseColor 	= _MainColor * (1 ) + (rtRefl * t_ShadowAlpha);
			baseColor.a =  _MainAlpha;
			return baseColor;
		}	
		
		ENDCG
	}	
	
} 

FallBack "Diffuse"
}

