// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Blue/Effect/Wsequence_Alpha" {  
Properties {  
      _TintColor("Color", Color) = (1,1,1,1)
	_MMultiplier ("Layer Multiplier", Float) = 2.0
   
    _MainTex ("Particle Texture", 2D) = "white" {}  
    _SizeX ("SizeX", Float) = 4  
    _SizeY ("SizeY", Float) = 4  
    _Speed ("Speed", Float) = 200  
}  
  
Category {  
    Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }  
   	Blend SrcAlpha OneMinusSrcAlpha
	Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
      
    SubShader {  
        Pass {  
          
            CGPROGRAM  
            #pragma vertex vert  
            #pragma fragment frag  
            #pragma multi_compile_particles  
 
            #include "UnityCG.cginc"  
  
            sampler2D _MainTex;  
            fixed4  _TintColor;  
            fixed _SizeX;  
            fixed _SizeY;  
            fixed _Speed;  
            float _MMultiplier;
            struct appdata_t {  
                float4 vertex : POSITION;  
                fixed4 color : COLOR;  
                float2 texcoord : TEXCOORD0;  
            };  
  
            struct v2f {  
                float4 vertex : POSITION;  
                fixed4 color : COLOR;  
                float2 texcoord : TEXCOORD0;  
            };  
              
            float4 _MainTex_ST;  
  
            v2f vert (appdata_t v)  
            {  
                v2f o;  
                o.vertex = UnityObjectToClipPos(v.vertex);  
               
                o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);  
                o.color =  _TintColor * v.color;
                return o;  
            }  
              
            fixed4 frag (v2f i) : COLOR  
            {  
				
	            int index = floor(_Time .x * _Speed);   
	            int indexX=fmod (_Time.x*_Speed,_SizeX);  
	            int indexY=fmod ((_Time.x*_Speed)/_SizeX,_SizeY);  
	   			//int indexY = index/_SizeX;   //get y number
              	//int indexX = index-indexY*_SizeX;   //get x number

	            fixed2 seqUV = float2((i.texcoord.x) /_SizeX, (i.texcoord.y)/_SizeY);  
	            seqUV.x += indexX/_SizeX;  
	            seqUV.y -= indexY/_SizeY;  
	            fixed4 o=tex2D(_MainTex, seqUV)*i.color *_MMultiplier;
	            return o;  
            }  
            ENDCG   
        }  
    }     
}  
}  