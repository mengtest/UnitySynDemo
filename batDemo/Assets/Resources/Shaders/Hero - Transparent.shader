Shader "HHHJ/Hero Transparent"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color ("叠加颜色", Color) = (1,1,1,1)
		_Specular ("高光颜色", Color) = (0,0,0,0)
		_Gloss ("高光强度", Range(0.1,32)) = 1
		_RimPower("边缘高光强度", Range(0, 8.0)) = 0
		_RimColor ("边缘高光颜色", Color) = (1,1,1,1)
		_Saturation ("饱满度", Range(0, 5)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
		
		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
			#include "Lighting.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
				float4 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			half4 _Color;
			half4 _Specular;
			half _Gloss;
			half _RimPower;
			half4 _RimColor;
			half _Saturation;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.worldNormal = mul(unity_ObjectToWorld, v.normal); //v.normal;
                UNITY_TRANSFER_FOG(o,o.vertex);
				
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				fixed4 c = tex2D(_MainTex, i.uv);
				
				fixed3 viewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
				fixed3 lightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
				fixed3 normalDir = normalize(i.worldNormal);
				
				// 饱满度
				half luminance = 0.2125 * c.r + 0.7154 * c.g + 0.0721 * c.b;
				c.rgb = lerp(half3(luminance, luminance, luminance), c.rgb, _Saturation);
				
				// 高光
				fixed3 h = normalize(lightDir + viewDir);
				fixed3 specular = _LightColor0.rgb * _Specular.rgb * pow(max(0, dot(normalDir, h)), _Gloss);
				c.rgb = c.rgb * (fixed3(1, 1, 1) + specular);
				
				// 边缘光
				float rim = 1 - saturate(dot(normalDir, viewDir));
				c.rgb += _RimColor * pow(rim, 1/_RimPower);
				
				c = c * _Color;
				
                UNITY_APPLY_FOG(i.fogCoord, c);
                return c;
            }
            ENDCG
        }
    }
}
