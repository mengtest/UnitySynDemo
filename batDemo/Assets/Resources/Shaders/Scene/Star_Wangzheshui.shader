

Shader "Star/Wangzheshui" {
	Properties {
		_Splat0 ("Base (RGB)", 2D) = "white" {}
		_addtiveValue("addtive Water Value", Float)= 0.3	
		_RaodongGround ("Raodong Ground Value", Float) = 0.3
		_DirectionUv("X Y(Raodong speed) Z W(addtive speed X)", Vector) = (1,0,1,0)
		_RaodongTex ("Raodong(RGB)", 2D) = "white" {}

		_maskColor("Mask Color", Color) = (1,1,1,1)
		_MaskImage("Water Mask(R)", 2D) = "black" {}
	}
	CGINCLUDE

				#include "UnityCG.cginc"
				sampler2D 	_Splat0;     float4  _Splat0_ST;
				float _addtiveValue;
				float _RaodongGround;
				half4 _DirectionUv;
				sampler2D _RaodongTex;  float4 _RaodongTex_ST;
				fixed4 _maskColor;
				sampler2D _MaskImage; float4 _MaskImage_ST;

				struct appdata {
					float4 vertex : POSITION;
					float2 texcoord : TEXCOORD0;
					float2 texcoord1 : TEXCOORD1;
					fixed4 color : COLOR0;
				};

				struct v2f {
					float4 pos : SV_POSITION;
					float4 uv0 : TEXCOORD0;
					float4 uv1 : TEXCOORD1;
					float2 uv2 : TEXCOORD2;
					fixed4 color : COLOR0;
				};

				v2f vert (appdata v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.uv0.xy = TRANSFORM_TEX(v.texcoord, _Splat0);
					o.uv1.xy = TRANSFORM_TEX(v.texcoord1, _RaodongTex) + frac(half2(_DirectionUv.z, _DirectionUv.w) * _Time.x);
					o.uv1.zw = TRANSFORM_TEX(v.texcoord1, _RaodongTex) + frac(half2(_DirectionUv.x, _DirectionUv.y) * _Time.x);
					o.uv2 = TRANSFORM_TEX(v.texcoord1, _MaskImage);
					o.color = v.color;
					return o;
				}

				fixed4 frag (v2f i) : COLOR   
				{
					fixed mask = tex2D(_MaskImage, i.uv2).r/3;
					fixed3 twist = tex2D(_RaodongTex, i.uv1.zw).rgb;
					half2 uv = twist.b * _addtiveValue;
					half2 uv1 = (twist.b * _RaodongGround ) * mask;

					fixed3 addtex = tex2D(_RaodongTex, i.uv1.xy + uv).rgb;
					fixed3 lay1 = tex2D(_Splat0, i.uv0.xy + uv1).rgb;
					fixed3 texcol = lerp(lay1  + mask*_maskColor.rgb , addtex, mask);
					return fixed4(texcol*2,mask*4);
				}
				ENDCG
			
	SubShader {
		Tags {"Queue"="Geometry+3"  "RenderType"="Opaque"}
		LOD 100
		Blend SrcAlpha OneMinusSrcAlpha
		Cull back Lighting Off ZWrite On Fog{ Mode Off}

		Pass {  
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				ENDCG
			}
	}
Fallback Off
}
