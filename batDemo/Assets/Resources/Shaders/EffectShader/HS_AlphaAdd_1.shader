// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Blue/Effect/HS_AlphaAdd_1" {
Properties {

	_Color("Color", Color) = (1,1,1,1)
	_MainTex ("Base layer (RGB)", 2D) = "white" {}
	_MMultiplier ("Layer Multiplier", Float) = 2.0
}
	
SubShader {
	Tags { "Queue"="Transparent2" "IgnoreProjector"="True" "RenderType"="Transparent" }
	
	Blend SrcAlpha OneMinusSrcAlpha
	Blend SrcAlpha One
	Cull Off Lighting Off ZWrite Off
	ZTest Off
	Fog { Color (0,0,0,0) }
	
	Pass {
	CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
	#include "UnityCG.cginc"
	sampler2D _MainTex; float4 _MainTex_ST;
	float _MMultiplier;
	float4 _Color;

		struct v2f {
			float4 pos : SV_POSITION;
			float4 uv : TEXCOORD0;
			fixed4 color : TEXCOORD1;
		};

		v2f vert (appdata_full v)
		{
			v2f o = (v2f)0;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.uv.xy = TRANSFORM_TEX(v.texcoord.xy,_MainTex) ;
			o.color = _MMultiplier * _Color*v.color ;
			return o;
		}	
		fixed4 frag (v2f i) : COLOR
		{
			fixed4 o;
			fixed4 tex = tex2D (_MainTex, i.uv.xy);
			o = tex * i.color;
			return o;
		}
		ENDCG 
	}	
}
}
