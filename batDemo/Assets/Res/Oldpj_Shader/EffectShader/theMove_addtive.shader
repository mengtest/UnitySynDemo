// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Blue/Effect/theMove_addtive"
{
	Properties
	{
		_Color("Base Color", Color) = (1,1,1,1)
		_ColorMuti ("ColorMuti", Float) = 3
		_Maintex("Base(RGB)", 2D) = "white" {}
		_DetailTex ("2nd layer (RGB)", 2D) = "white" {}
		_ScrollUv("Base layer X,Y 2nd layer Z,W", Vector) = (1,-0.05,0.01,-0.015)
		_theMove ("theMove", 2D) = "white" {}
		_ScrollX ("theMove Scroll speed X", Float) = 0.0
		_ScrollY ("theMove Scroll speed Y", Float) = 0.3
		_MoveXYZ("_theMove X,Y,Z(just 1 and 0) _MMultiplier W", Vector) = (1,0,1,1)
	}
	
	SubShader
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha One
		Cull Off Lighting Off ZWrite Off Fog{ Mode Off}
	LOD 100
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
			float4 _Color;
			float _ColorMuti;
			uniform sampler2D _Maintex; uniform float4 _Maintex_ST;
			uniform sampler2D _DetailTex; uniform float4 _DetailTex_ST;
			half4 _ScrollUv;
			uniform sampler2D _theMove; uniform float4 _theMove_ST;
			float _ScrollX;
			float _ScrollY;
			half4 _MoveXYZ;
            struct VertexOutput 
            {
                float4 pos : SV_POSITION;
                float4 uv : TEXCOORD0;
                fixed4 color : TEXCOORD1;
                float4 uv1 : TEXCOORD2;
            };
			
			VertexOutput vert (appdata_full v) 
			{
                VertexOutput o;

                o.uv1.xy=TRANSFORM_TEX(v.texcoord1,_theMove);
                float2 mover = (o.uv1.xy+_Time.g*float2(_ScrollX,_ScrollY));
                float2 _theMove_var=tex2Dlod(_theMove,float4(TRANSFORM_TEX(mover, _theMove),0,0))*_MoveXYZ.w*v.color.rgb;//关键性参数tex2dlod，使点扭曲
                v.vertex.xyz+=float3(_theMove_var.r*_MoveXYZ.x,_theMove_var.r*_MoveXYZ.y,_theMove_var.r*_MoveXYZ.z);//移动方向的设置,外部好像没法更改,我好像有办法了，都乘以1，或者0

                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv.xy = TRANSFORM_TEX(v.texcoord.xy,_Maintex) + frac(float2(_ScrollUv.x, _ScrollUv.y) * _Time);
                o.uv.zw = TRANSFORM_TEX(v.texcoord.xy,_DetailTex) + frac(float2(_ScrollUv.z, _ScrollUv.w) * _Time);
                o.color = _ColorMuti * _Color;
                return o;
            }
            
            fixed4 frag(VertexOutput i) : COLOR 
            {
               fixed4 o;
               fixed4 tex = tex2D (_Maintex, i.uv.xy);
			   fixed4 tex2 = tex2D (_DetailTex, i.uv.zw);
			
			   o = tex * tex2*i.color ;
               return o;
            }
			
			ENDCG
		}
		
	}
	FallBack "Diffuse"
}