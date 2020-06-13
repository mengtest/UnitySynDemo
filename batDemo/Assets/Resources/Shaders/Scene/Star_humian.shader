// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Star/heshui" {
    Properties {
    	_Color ("Main Color", Color) = (0,-0.5,1,.1)

        _MainTex ("MainTexture", 2D) = "white" {}
        _Raodong ("Raodong", 2D) = "white" {}
        _tongdao("tongdao",2D)="white" {}

        _texiaoColor("texiao Color",Color)=(1,1,1,1)
        _HeatForce  ("Heat Force", range (-1,1)) = 0.1
        _DirectionUv("Heat.x,Heat.y,texiao.z.texiao.w", Vector) = (.5,.5, 0,2)
    }
    SubShader {
        Tags {"IgnoreProjector"="True"	 "Queue"="Geometry+3"	"RenderType"="Transparent"	}
        LOD 200
         Blend SrcAlpha OneMinusSrcAlpha
		Cull back Lighting Off ZWrite On Fog{ Mode Off}
		ZTest On
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            fixed4 _Color;
            sampler2D _MainTex;  float4 _MainTex_ST;
            sampler2D _Raodong;  float4 _Raodong_ST;
            sampler2D _tongdao;  float4 _tongdao_ST;
            fixed4 _texiaoColor;


            float _HeatForce;
            half4 _DirectionUv;
            
           struct v2f
			{
				float4 pos:POSITION;
				float4 uv:TEXCOORD0;
				float4 ao:TEXCOORD1;
			};

           v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv.xy = TRANSFORM_TEX(v.texcoord.xy,_MainTex) ;
				o.uv.zw =TRANSFORM_TEX(v.texcoord.xy,_Raodong);
				o.ao.xy=TRANSFORM_TEX(v.texcoord.xy,_tongdao);
				return o;
			}

			
            fixed4 frag(v2f  i) : COLOR {
                
                float2 weiyi = (i.uv.xy+_Time*float2(_DirectionUv.x,_DirectionUv.y*.1));
                fixed4 raodong = tex2D(_Raodong,TRANSFORM_TEX(weiyi, _Raodong)).r*_HeatForce;

                fixed4 tex2 = tex2D (_Raodong, i.uv.zw+raodong+_Time*float2(_DirectionUv.z,_DirectionUv.w*.1));
                fixed4 tex3 = tex2D (_Raodong, i.uv.zw+raodong+_Time*float2(-_DirectionUv.z*.7,-_DirectionUv.w*.2));
                float4 aa=tex2D(_MainTex,TRANSFORM_TEX(i.uv.xy+raodong*float2(.2,.2), _MainTex))*2*_Color;//+raodong
               	float4 bb=aa+tex2*tex3*_texiaoColor;
               	float4 cc= tex2D (_tongdao, i.ao.xy);
               	bb.a=cc.r;
                return bb;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
