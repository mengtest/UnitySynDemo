Shader "HHHJ/Common/Specular"
{
    Properties {
        _MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}

		_SpecColor ("Specular", Color) = (1,1,1,1)
		_Gloss ("Gloss", Range(0, 1)) = 0.5
		_Shininess ("Shininess", Range (0.03, 1)) = 0.078125
        _Color ("Main Color", Color) = (1,1,1,1)
        [Enum(Off, 0, Front, 1, Back, 2)]_Cull ("Cull", float) = 2
    }

    SubShader {
        Tags { "RenderType" = "Opaque" "Queue" = "Geometry" }
        LOD 200
        
		Cull [_Cull]

        CGPROGRAM
        #pragma surface surf BlinnPhong noforwardadd

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

    FallBack "Mobile/Diffuse"
}
