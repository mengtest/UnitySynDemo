// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/Sprites/TextureCompress_fore 1"
{
	Properties
	{
		_MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		_RGBAlpha ("RGBAlpha", 2D) = "white" {}
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off	
		Lighting Off
		ZWrite off
		ZTest LEqual 
		//AlphaTest Greater 0.5
		Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha
	

		Pass
		{
		CGPROGRAM
			#pragma vertex vert 
			#pragma fragment frag
			#pragma multi_compile DUMMY PIXELSNAP_ON
			#include "UnityCG.cginc"
			
			fixed4 _Color;
			sampler2D _MainTex;
			sampler2D _RGBAlpha;
			float4 _ClipRange0 = float4(0.0, 0.0, 1.0, 1.0);
			float4 _ClipArgs0 = float4(1000.0, 1000.0, 0.0, 1.0);
						
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
				float2 worldPos : TEXCOORD1;
			};
			

			
			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;

				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif
				
				OUT.worldPos = IN.vertex.xy * _ClipRange0.zw + _ClipRange0.xy;

				return OUT;
			}

			fixed4 frag(v2f IN) : COLOR
			{
				float2 factor = (float2(1.0, 1.0) - abs(IN.worldPos)) * _ClipArgs0.xy;

				float4 AlphaTex = tex2D(_RGBAlpha,IN.texcoord);
				
				float4 a = tex2D(_MainTex, IN.texcoord);
				float fade = clamp(min(factor.x, factor.y), 0.0, 1.0);

				if(IN.color.r < 0.01 && IN.color.g < 0.01 && IN.color.b < 0.01)
				{
					fixed3 gray =  dot(a.rgb,fixed3(0.3,0.59,0.1));
					// lerp (color1 , color2 , blend_value) modify blend_value
					a.rgb = lerp(a.rgb,gray,0.88);
				}
				else
				{
					a *= IN.color;
				}

				a.a *= AlphaTex.x * IN.color.a;
				a.a *= fade;
				//a.rgb = lerp(half3(0.0, 0.0, 0.0), a.rgb, fade);
				return a;
			}
		ENDCG
		}
	}
}
