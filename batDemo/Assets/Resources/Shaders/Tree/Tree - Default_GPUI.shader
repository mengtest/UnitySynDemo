Shader "GPUInstancer/HHHJ/Tree/Default"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_Cutoff ("AlphaCutoff", Range(0,1)) = 0.5
    }
	
    SubShader
    {
        Tags { "RenderType"="TransparentCutout" "Queue"="AlphaTest" "IgnoreProjector"="True" }
        LOD 1500
		
        Cull Back
		
		CGPROGRAM
#include "UnityCG.cginc"
#include "./../../../../_3rd/GPUInstancer/Shaders/Include/GPUInstancerInclude.cginc"
#pragma instancing_options procedural:setupGPUI
#pragma multi_compile_instancing
		#pragma surface surf Lambert noforwardadd alphatest:_Cutoff addshadow

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
		
        Cull Front
		CGPROGRAM
#include "UnityCG.cginc"
#include "./../../../../_3rd/GPUInstancer/Shaders/Include/GPUInstancerInclude.cginc"
#pragma instancing_options procedural:setupGPUI
#pragma multi_compile_instancing
		#pragma surface surf Lambert noforwardadd alphatest:_Cutoff addshadow

		sampler2D _MainTex;
		half4 _Color;
		
		struct Input {
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			o.Normal = -o.Normal;
		}
		ENDCG
		
		UsePass "HHHJ/Common/ShadowCaster/ShadowCaster-TwoSide-Cutout"
    }
	
    SubShader
    {
        Tags { "RenderType"="TransparentCutout" "Queue"="AlphaTest" "IgnoreProjector"="True" }
		
        Cull Off
		
		CGPROGRAM
#include "UnityCG.cginc"
#include "./../../../../_3rd/GPUInstancer/Shaders/Include/GPUInstancerInclude.cginc"
#pragma instancing_options procedural:setupGPUI
#pragma multi_compile_instancing
		#pragma surface surf Lambert noforwardadd alphatest:_Cutoff addshadow

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
		
		UsePass "HHHJ/Common/ShadowCaster/ShadowCaster-TwoSide-Cutout"
    }
}
