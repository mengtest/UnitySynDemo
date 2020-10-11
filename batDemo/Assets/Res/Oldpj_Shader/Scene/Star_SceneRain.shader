Shader "Star/SceneRain" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		
		_Raodong1 ("Raodong1", 2D) = "white" {}
		_RaodongDir("Fangxiang(xy) Raodong(w)" , Vector) = (1,1,0,.1)
	}
	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 200
		CGPROGRAM
		//#pragma surface surf Lambert alpha 
		#pragma surface surf Lambert decal:add
		fixed4 _Color;
		sampler2D _MainTex;
		sampler2D _Raodong1;
		half4 _RaodongDir;
		
		struct Input {
			float2 uv_MainTex;
			float2 uv_Raodong1;
			float4 color : COLOR;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			
			IN.uv_Raodong1.x += _Time.x*_RaodongDir.x;
			IN.uv_Raodong1.y += _Time.y*_RaodongDir.y*.1;
			
			fixed4 c1 = tex2D(_Raodong1, IN.uv_Raodong1);
    		fixed4 c0 = tex2D(_MainTex, IN.uv_MainTex);
    		IN.uv_MainTex.x+=c0.r*c1.r*_RaodongDir.w;
    		IN.uv_MainTex.y+=c0.g*c1.g*_RaodongDir.w;
			
			half4 c =tex2D(_MainTex, IN.uv_MainTex)*_Color;
			o.Emission = c.rgb*2;
			
			
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
