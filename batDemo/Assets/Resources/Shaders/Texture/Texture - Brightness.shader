Shader "HHHJ/Unlit/Texture/Brightness"
{

    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Brightness ("Brightness", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
		
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD;
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};
			
			half _Brightness;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				return col * _Brightness;
			}
			
			ENDCG
		}
    }
	
    FallBack "Unlit/Texture"
}
