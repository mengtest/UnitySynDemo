Shader "HHHJ/Common/Transparent/Specular"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_SpecColor ("Specular", Color) = (1,1,1,1)
		_Gloss ("Gloss", Range(0, 1)) = 0.5
		_Shininess ("Shininess", Range(0.03, 1)) = 0.078125
        [Enum(Off, 0, Front, 1, Back, 2)]_Cull ("Cull", float) = 2
    }
    SubShader
    {
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		LOD 150
		
		Cull [_Cull]

		CGPROGRAM
        #pragma surface surf BlinnPhong noforwardadd alpha:blend

        sampler2D _MainTex;
        half _Shininess;
		half _Gloss;
		half4 _Color;
		
		struct Input {
			float2 uv_MainTex;
		};

        void surf (Input IN, inout SurfaceOutput o) {
            fixed4 tex = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = tex.rgb;
            o.Gloss = _Gloss;
            o.Alpha = tex.a;
            o.Specular = _Shininess;
        }
        ENDCG
    }
}