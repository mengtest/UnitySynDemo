// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Blue/Effect/HS_newtest" {  
Properties {  
    _Color("Color", Color) = (1,1,1,1)
    _MainTex ("Particle Texture", 2D) = "white" {}  
    _SizeX ("SizeX", Float) = 4  
    _SizeY ("SizeY", Float) = 4  
}  
Category {  
    Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	
	Blend SrcAlpha OneMinusSrcAlpha
	//Blend SrcAlpha One
	Cull Off Lighting Off ZWrite Off 
      
    SubShader {  
        Pass {  
          
            CGPROGRAM  
            #pragma vertex vert  
            #pragma fragment frag  
            #pragma multi_compile_particles  
            #include "UnityCG.cginc"  
  
            sampler2D _MainTex;  
            fixed4 _Color;  
            fixed _SizeX;  
            fixed _SizeY;  
              
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
                o.color = v.color*_Color;  
                o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);  
                return o;  
            }  
              
            fixed4 frag (v2f i) : COLOR  
            {  
			  int index =(_SizeX*_SizeY-1)*_Color.a+_SizeX;//_TintColor.a eaqules range move
              int indexY = index/_SizeX;   //get y number
              int indexX = index-indexY*_SizeX;   //get x number
  
              fixed2 seqUV = float2((i.texcoord.x) /_SizeX, (i.texcoord.y)/_SizeY);  
              seqUV.x += indexX/_SizeX;  
              seqUV.y -= indexY/_SizeY;  
			  fixed4 tex = tex2D (_MainTex, seqUV)*float4(i.color.rgb,1);
              return tex;
            }  
            ENDCG   
        }  
    }     
}  
}  