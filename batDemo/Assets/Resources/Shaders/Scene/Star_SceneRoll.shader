Shader "Star/SceneRoll" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Emission("Emission",2D)="white"{}
		_Allnumbers("X(speed),Y(speed),Z(light),W(wind)", Vector) = (0, 1, 1, 1)
		
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		sampler2D _Emission;
		fixed4 _Allnumbers;
		
		struct Input {
			float2 uv_MainTex;
			float2 uv_Emission;
			
		};

		void surf (Input IN, inout SurfaceOutput o) {
			  
			IN.uv_Emission += fixed2(_Allnumbers.x * _Time.y, _Allnumbers.y * _Time.y*0.1); 
			//IN.uv_MainTex.x+=_Time.x*_ScrollX;
			float jjyy= sin(_Time.y*_Allnumbers.w)*0.15+_Allnumbers.z;
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			half4 b = tex2D (_Emission, IN.uv_Emission);
			o.Emission=b.rgb*c.rgb*jjyy;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
