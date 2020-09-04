// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Star/SeaUnder" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Normal("Normal", 2D) = "bump" {}
		_TexAtlasTiling("xy for TexAtlasTiling.xy/zw , zw for Direction.xy" , Vector) = (1,1,1,1)
		_ReflectionAlpha("Reflection Alpha" , Range(0,1.0)) = 0.5
		_ReflectionBias("Reflection Bias" , float) = 0.02
	}
	
	CGINCLUDE
	
	#include "UnityCG.cginc"
	sampler2D _MainTex;
	sampler2D _Normal;
	
	float4 _TexAtlasTiling;
	half _ReflectionAlpha;
	half _ReflectionBias;

	struct v2f_My
	{
		half4 pos : SV_POSITION;
		half4 uv1 : TEXCOORD0;
	};
	ENDCG
	SubShader {
		Tags { "RenderType"="Transparent" "Queue"="Transparent+2" }
		LOD 200
		
		Blend SrcAlpha OneMinusSrcAlpha
		ZTest LEqual
		ZWrite Off
		Cull back		
		
		pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			
			v2f_My vert(appdata_full v)
			{
				v2f_My o;
				o.pos = UnityObjectToClipPos( v.vertex);
				o.uv1 = v.texcoord.xyxy * _TexAtlasTiling.xyxy + frac((_Time.xxxx) * _TexAtlasTiling.zwzw);
				o.uv1.zw = v.texcoord.xy;
				return o;			
				
			}
			
			fixed4 frag(v2f_My i) : COLOR
			{
				fixed4 normalMap_zw = tex2D(_Normal, i.uv1.xy);
				
				fixed4 rtReflNorm = (normalMap_zw - 0.5) * _ReflectionBias;
				fixed4 rtRefl = tex2D(_MainTex ,i.uv1.zw+ rtReflNorm.xy);
				fixed t_ShadowAlpha = rtRefl.a * _ReflectionAlpha;
				rtRefl.a = t_ShadowAlpha;
				
				return rtRefl*2;
			}
			
			
			ENDCG  
		}
		
	} 
	FallBack "Diffuse"
}
