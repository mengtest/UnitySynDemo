Shader "HHHJ/Common/Cutout/Diffuse"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_Cutoff ("AlphaCutoff", Range(0,1)) = 0.5
        [Enum(Off, 0, Front, 1, Back, 2)]_Cull ("Cull", float) = 2
    }
    SubShader
    {
		Tags { "RenderType"="TransparentCutout" "Queue"="AlphaTest" "IgnoreProjector"="True" }
		LOD 150

		Cull [_Cull]

		CGPROGRAM
		#pragma surface surf Lambert noforwardadd alphatest:_Cutoff

		sampler2D _MainTex;
		half4 _Color;
		
		struct Input {
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
    }
	
	Fallback "Legacy Shaders/Transparent/Cutout/Diffuse"
}