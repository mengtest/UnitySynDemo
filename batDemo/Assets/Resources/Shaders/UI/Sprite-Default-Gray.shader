// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "HHHJ/Sprites/Default-Gray"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
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
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
			CGPROGRAM
            #pragma vertex SpriteGrayVert
            #pragma fragment SpriteGrayFrag
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile_local _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #pragma multi_compile_local _ GRAY_SCALE_CHANNEL
            #include "UnitySprites.cginc"
			
			struct appdata_t_gray
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				#if GRAY_SCALE_CHANNEL
				float2 texcoord1 : TEXCOORD1;
				#endif
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f_gray
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				#if GRAY_SCALE_CHANNEL
				float2 texcoord1 : TEXCOORD2;
				#endif
				UNITY_VERTEX_OUTPUT_STEREO
			};
			
			v2f_gray SpriteGrayVert(appdata_t_gray IN)
			{
				v2f_gray OUT;
			
				UNITY_SETUP_INSTANCE_ID (IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				
				OUT.vertex = UnityFlipSprite(IN.vertex, _Flip);
				OUT.vertex = UnityObjectToClipPos(OUT.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color * _RendererColor;
			
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif
				
				#if GRAY_SCALE_CHANNEL
				OUT.texcoord1 = IN.texcoord1;
				#endif
			
				return OUT;
			}
			
			fixed4 SpriteGrayFrag(v2f_gray IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
				
				float cc = c.r * 0.299 + c.g * 0.518 + c.b * 0.184;
				#if GRAY_SCALE_CHANNEL
				c.rgb = lerp(c.rgb, cc, clamp(IN.texcoord1.x, 0, 1));
				#else
				c.rgb = cc;
				#endif
				
				c.rgb *= c.a;
				return c;
			}
			
			ENDCG
        }
    }
}
