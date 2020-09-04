Shader "HHHJ/Common/Diffuse - HalfLambert"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
        [Enum(Off, 0, Front, 1, Back, 2)]_Cull ("Cull", float) = 2
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 100
		
        Cull [_Cull]
		
		CGPROGRAM
		#pragma surface surf HalfLambert noforwardadd

		sampler2D _MainTex;
		half4 _Color;
		
		inline fixed4 UnityHalfLambertLight (SurfaceOutput s, UnityLight light)
		{
			fixed diff = 0.5f + dot(s.Normal, light.dir) * 0.5f;
		
			fixed4 c;
			c.rgb = s.Albedo * light.color * diff;
			c.a = s.Alpha;
			return c;
		}
		
		inline fixed4 LightingHalfLambert (SurfaceOutput s, UnityGI gi)
		{
			fixed4 c;
			
			c = UnityHalfLambertLight (s, gi.light);
		
			#ifdef UNITY_LIGHT_FUNCTION_APPLY_INDIRECT
				c.rgb += s.Albedo * gi.indirect.diffuse;
			#endif
		
			return c;
		} 
		
		inline void LightingHalfLambert_GI (
			SurfaceOutput s,
			UnityGIInput data,
			inout UnityGI gi)
		{
			gi = UnityGlobalIllumination (data, 1.0, s.Normal);
		}
		
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
