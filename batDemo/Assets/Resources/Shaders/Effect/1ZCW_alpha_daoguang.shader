// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:8909,x:33562,y:32945,varname:node_8909,prsc:2|emission-7862-OUT,alpha-3585-OUT;n:type:ShaderForge.SFN_Tex2d,id:3925,x:32885,y:32848,ptovrint:False,ptlb:diffuse,ptin:_diffuse,varname:node_3925,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-9840-OUT;n:type:ShaderForge.SFN_Color,id:9168,x:32885,y:33039,ptovrint:False,ptlb:color,ptin:_color,varname:node_9168,prsc:2,glob:False,taghide:False,taghdr:True,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_VertexColor,id:4125,x:32885,y:33190,varname:node_4125,prsc:2;n:type:ShaderForge.SFN_Multiply,id:7862,x:33117,y:32974,varname:node_7862,prsc:2|A-3925-RGB,B-4125-RGB,C-9168-RGB;n:type:ShaderForge.SFN_Append,id:4702,x:32643,y:33006,varname:node_4702,prsc:2|A-8037-Z,B-8037-W;n:type:ShaderForge.SFN_TexCoord,id:7856,x:32407,y:32749,varname:node_7856,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Add,id:9840,x:32701,y:32848,varname:node_9840,prsc:2|A-7856-UVOUT,B-4702-OUT;n:type:ShaderForge.SFN_Multiply,id:3585,x:33187,y:33307,varname:node_3585,prsc:2|A-9340-R,B-7593-OUT,C-4125-A,D-3925-A;n:type:ShaderForge.SFN_Tex2d,id:9340,x:33025,y:33320,ptovrint:False,ptlb:mask,ptin:_mask,varname:node_9340,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_TexCoord,id:4991,x:32245,y:33731,varname:node_4991,prsc:2,uv:1,uaff:True;n:type:ShaderForge.SFN_Tex2d,id:5887,x:32254,y:33353,ptovrint:False,ptlb:dissolve,ptin:_dissolve,varname:node_5887,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-3200-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8930,x:32254,y:33555,ptovrint:False,ptlb:soft,ptin:_soft,varname:node_8930,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Vector1,id:8694,x:32254,y:33647,varname:node_8694,prsc:2,v1:-1.5;n:type:ShaderForge.SFN_Multiply,id:2265,x:32477,y:33449,varname:node_2265,prsc:2|A-5887-R,B-8930-OUT;n:type:ShaderForge.SFN_Lerp,id:8854,x:32546,y:33700,varname:node_8854,prsc:2|A-8930-OUT,B-8694-OUT,T-4991-U;n:type:ShaderForge.SFN_Subtract,id:2093,x:32747,y:33662,varname:node_2093,prsc:2|A-2265-OUT,B-8854-OUT;n:type:ShaderForge.SFN_Clamp01,id:7593,x:32979,y:33662,varname:node_7593,prsc:2|IN-2093-OUT;n:type:ShaderForge.SFN_TexCoord,id:8037,x:32282,y:32981,varname:node_8037,prsc:2,uv:1,uaff:True;n:type:ShaderForge.SFN_Time,id:3457,x:32402,y:33243,varname:node_3457,prsc:2;n:type:ShaderForge.SFN_TexCoord,id:7980,x:32710,y:33168,varname:node_7980,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_ValueProperty,id:2070,x:32446,y:33193,ptovrint:False,ptlb:dissu,ptin:_dissu,varname:node_2070,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:6961,x:32550,y:33243,varname:node_6961,prsc:2|A-2070-OUT,B-3457-T;n:type:ShaderForge.SFN_ValueProperty,id:7882,x:32402,y:33404,ptovrint:False,ptlb:dissv,ptin:_dissv,varname:node_7882,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:3349,x:32573,y:33370,varname:node_3349,prsc:2|A-3457-T,B-7882-OUT;n:type:ShaderForge.SFN_Append,id:580,x:32710,y:33324,varname:node_580,prsc:2|A-6961-OUT,B-3349-OUT;n:type:ShaderForge.SFN_Add,id:3200,x:32814,y:33393,varname:node_3200,prsc:2|A-7980-UVOUT,B-580-OUT;proporder:3925-9168-9340-5887-8930-2070-7882;pass:END;sub:END;*/

Shader "ZCW/1alpha_daoguang" {
    Properties {
		[hideininspector]_MainTex("Texture", 2D) = "white" {}

        _diffuse ("diffuse", 2D) = "white" {}
        [HDR]_color ("color", Color) = (0.5,0.5,0.5,1)
        _mask ("mask", 2D) = "white" {}
        _dissolve ("dissolve", 2D) = "white" {}
        _soft ("soft", Float ) = 0
        _dissu ("dissu", Float ) = 0
        _dissv ("dissv", Float ) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 200
        Pass {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            CBUFFER_START(UnityPerMaterial)
            uniform sampler2D _diffuse; uniform float4 _diffuse_ST;
            uniform float4 _color;
            uniform sampler2D _mask; uniform float4 _mask_ST;
            uniform sampler2D _dissolve; uniform float4 _dissolve_ST;
            uniform float _soft;
            uniform float _dissu;
            uniform float _dissv;
            CBUFFER_END

            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
////// Emissive:
                float2 node_9840 = (i.uv0+float2(i.uv1.b,i.uv1.a));
                float4 _diffuse_var = tex2D(_diffuse,TRANSFORM_TEX(node_9840, _diffuse));
                float3 emissive = (_diffuse_var.rgb*i.vertexColor.rgb*_color.rgb);
                float3 finalColor = emissive;
                float4 _mask_var = tex2D(_mask,TRANSFORM_TEX(i.uv0, _mask));
                float4 node_3457 = _Time;
                float2 node_3200 = (i.uv0+float2((_dissu*node_3457.g),(node_3457.g*_dissv)));
                float4 _dissolve_var = tex2D(_dissolve,TRANSFORM_TEX(node_3200, _dissolve));
                return fixed4(finalColor,(_mask_var.r*saturate(((_dissolve_var.r*_soft)-lerp(_soft,(-1.5),i.uv1.r)))*i.vertexColor.a*_diffuse_var.a));
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
