// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:6261,x:32949,y:32764,varname:node_6261,prsc:2|emission-4155-OUT;n:type:ShaderForge.SFN_Tex2d,id:3069,x:32220,y:32707,ptovrint:False,ptlb:diffuse tex,ptin:_diffusetex,varname:node_3042,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-8898-OUT;n:type:ShaderForge.SFN_Multiply,id:2178,x:32418,y:32859,varname:node_2178,prsc:2|A-3069-RGB,B-9449-RGB,C-1406-RGB,D-1819-R;n:type:ShaderForge.SFN_Color,id:9449,x:32220,y:32886,ptovrint:False,ptlb:diffuse color,ptin:_diffusecolor,varname:node_6028,prsc:2,glob:False,taghide:False,taghdr:True,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:4155,x:32666,y:32919,varname:node_4155,prsc:2|A-3069-A,B-1406-A,C-2178-OUT,D-5515-OUT;n:type:ShaderForge.SFN_Tex2d,id:1819,x:32220,y:33185,ptovrint:False,ptlb:mask,ptin:_mask,varname:node_8258,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_VertexColor,id:1406,x:32220,y:33033,varname:node_1406,prsc:2;n:type:ShaderForge.SFN_Time,id:3626,x:31351,y:32840,varname:node_3626,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:8991,x:31364,y:32774,ptovrint:False,ptlb:Uspeed,ptin:_Uspeed,varname:node_8766,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:2054,x:31338,y:33011,ptovrint:False,ptlb:Vspeed,ptin:_Vspeed,varname:node_7972,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:611,x:31589,y:32819,varname:node_611,prsc:2|A-8991-OUT,B-3626-T;n:type:ShaderForge.SFN_Multiply,id:3696,x:31542,y:32977,varname:node_3696,prsc:2|A-3626-T,B-2054-OUT;n:type:ShaderForge.SFN_Append,id:4886,x:31798,y:32861,varname:node_4886,prsc:2|A-611-OUT,B-3696-OUT;n:type:ShaderForge.SFN_TexCoord,id:4032,x:31364,y:32618,varname:node_4032,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Add,id:8898,x:32007,y:32739,varname:node_8898,prsc:2|A-4032-UVOUT,B-4886-OUT;n:type:ShaderForge.SFN_Tex2d,id:8721,x:31426,y:33258,ptovrint:False,ptlb:disslove tex,ptin:_disslovetex,varname:node_1205,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_TexCoord,id:4699,x:31417,y:33616,varname:node_4699,prsc:2,uv:1,uaff:True;n:type:ShaderForge.SFN_ValueProperty,id:4366,x:31437,y:33440,ptovrint:False,ptlb:soft,ptin:_soft,varname:node_8389,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Vector1,id:1014,x:31417,y:33540,varname:node_1014,prsc:2,v1:-1.5;n:type:ShaderForge.SFN_Multiply,id:6748,x:31701,y:33349,varname:node_6748,prsc:2|A-8721-R,B-4366-OUT;n:type:ShaderForge.SFN_Lerp,id:8420,x:31715,y:33589,varname:node_8420,prsc:2|A-4366-OUT,B-1014-OUT,T-4699-U;n:type:ShaderForge.SFN_Subtract,id:8908,x:31944,y:33498,varname:node_8908,prsc:2|A-6748-OUT,B-8420-OUT;n:type:ShaderForge.SFN_Clamp01,id:5515,x:32225,y:33543,varname:node_5515,prsc:2|IN-8908-OUT;proporder:3069-9449-1819-8721-4366-8991-2054;pass:END;sub:END;*/

Shader "ZCW/1add" {
    Properties {
		[hideininspector]_MainTex("Texture", 2D) = "white" {}

        _diffusetex ("diffuse tex", 2D) = "white" {}
        [HDR]_diffusecolor ("diffuse color", Color) = (0.5,0.5,0.5,1)
        _mask ("mask", 2D) = "white" {}
        _disslovetex ("disslove tex", 2D) = "white" {}
        _soft ("soft", Float ) = 0
        _Uspeed ("Uspeed", Float ) = 0
        _Vspeed ("Vspeed", Float ) = 0
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 200
        Pass {
            Blend One One
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            CBUFFER_START(UnityPerMaterial)
            uniform sampler2D _diffusetex; uniform float4 _diffusetex_ST;
            uniform float4 _diffusecolor;
            uniform sampler2D _mask; uniform float4 _mask_ST;
            uniform float _Uspeed;
            uniform float _Vspeed;
            uniform sampler2D _disslovetex; uniform float4 _disslovetex_ST;
            uniform float _soft;
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
                float4 node_3626 = _Time;
                float2 node_8898 = (i.uv0+float2((_Uspeed*node_3626.g),(node_3626.g*_Vspeed)));
                float4 _diffusetex_var = tex2D(_diffusetex,TRANSFORM_TEX(node_8898, _diffusetex));
                float4 _mask_var = tex2D(_mask,TRANSFORM_TEX(i.uv0, _mask));
                float4 _disslovetex_var = tex2D(_disslovetex,TRANSFORM_TEX(i.uv0, _disslovetex));
                float3 emissive = (_diffusetex_var.a*i.vertexColor.a*(_diffusetex_var.rgb*_diffusecolor.rgb*i.vertexColor.rgb*_mask_var.r)*saturate(((_disslovetex_var.r*_soft)-lerp(_soft,(-1.5),i.uv1.r))));
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
