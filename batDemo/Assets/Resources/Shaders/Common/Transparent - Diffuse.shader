Shader "HHHJ/Common/Transparent/Diffuse"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
        [Enum(Off, 0, Front, 1, Back, 2)]_Cull ("Cull", float) = 2
    }
    SubShader
    {
		Tags { "RenderType" = "Transparent" "Queue"="Transparent" }
		LOD 150
		
		Cull [_Cull]

		CGPROGRAM
		#pragma surface surf Lambert noforwardadd alpha:blend

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
}