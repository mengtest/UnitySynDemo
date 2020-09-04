// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Star/Scenebuliding"
{
	Properties
	{
		_Color("Base Color", Color) = (1,1,1,1)
		_Maintex("Base(RGB)", 2D) = "white" {}
		_SpecularColor ("Specular Color", Color) = (0.6,0.6,0.6,1)
	    _SpecularPow("Specular Pow" , float) = 0
	    _LightDir("Light Direction" , Vector) = (-3,2,1,1)
	}
	
	SubShader
	{
		tags{"RenderType" = "Opaque" }
		Pass
		{
			Tags { "LightMode" = "ForwardBase" }
			CGPROGRAM
			#pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
			
			fixed4 _Color;
			uniform sampler2D _Maintex; uniform float4 _Maintex_ST;
			fixed4 _SpecularColor;
			half _SpecularPow;
			half4 _LightDir;
			
			struct VertexInput 
			{
                float4 vertex : POSITION;
                float3 normal 	: NORMAL; //output not normally
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput 
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                half3 normal : TEXCOORD1;
			  	fixed3 vlight : TEXCOORD2; // ambient/SH/vertexlights
            };
			
			VertexOutput vert (VertexInput v) 
			{
                VertexOutput o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord0;
                o.normal = normalize(mul((float3x3)unity_ObjectToWorld, SCALED_NORMAL));
                o.vlight = ShadeSH9 (float4(o.normal,1.0));
               	
                return o;
            }
            
            fixed4 frag(VertexOutput i) : COLOR 
            {
                float4 Main = tex2D(_Maintex,TRANSFORM_TEX(i.uv, _Maintex))*_Color;
                fixed4 finalTex = Main;
                
                half spec = max(dot(i.normal , normalize(_LightDir.xyz)),0.001);
                finalTex.rgb += Main * (_SpecularColor.rgb*8*spec*pow(spec , _SpecularPow)+min(_SpecularPow/10,0.3)+i.vlight);
                
                return finalTex;
            }
			ENDCG
		}
	}
	FallBack "Diffuse"
}