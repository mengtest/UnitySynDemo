// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Star/newSea" {
    Properties {
    	_Color ("Main Color", Color) = (0,-0.5,1,.1)
    	jialiang  ("jialiang", range (0,5)) = 2
        _MainTex ("MainTexture", 2D) = "white" {}
        _HeatForce  ("Heat Force", range (-.5,.5)) = 0.03
        _DirectionUv("X Y(texUV) Z W(RaodongUV)", Vector) = (0.1,0.1,-0.1,-0.1)
    }
    SubShader {
        Tags {"IgnoreProjector"="True"	 "Queue"="Transparent"	"RenderType"="Transparent"	}
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha
		Lighting Off ZWrite Off Fog{ Mode Off}
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _Color;
            float jialiang;
            sampler2D _MainTex;  float4 _MainTex_ST;
            float _HeatForce;
            half4 _DirectionUv;
            
           struct v2f
			{
				float4 pos:POSITION;
				float4 uv:TEXCOORD0;
			};

           v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				return o;
			}

            fixed4 frag(v2f  i) : COLOR {
                
                float2 niudong = (i.uv.rg+_Time*float2(_DirectionUv.z,_DirectionUv.w*.1));
                float2 finally = (i.uv.rg+tex2D(_MainTex,TRANSFORM_TEX(niudong, _MainTex)).r*_HeatForce);
                float4 aa=tex2D(_MainTex,TRANSFORM_TEX(finally, _MainTex)+float2(_DirectionUv.x, _DirectionUv.y/30) * _Time);
                
                return aa* jialiang*_Color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
