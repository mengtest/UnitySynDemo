// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:33209,y:32712,varname:node_9361,prsc:2|emission-4321-OUT,alpha-7111-OUT;n:type:ShaderForge.SFN_Tex2d,id:411,x:32560,y:32839,ptovrint:False,ptlb:diffuse tex,ptin:_diffusetex,varname:node_411,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-1451-OUT;n:type:ShaderForge.SFN_Color,id:8008,x:32560,y:32677,ptovrint:False,ptlb:diffuse color,ptin:_diffusecolor,varname:node_8008,prsc:2,glob:False,taghide:False,taghdr:True,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_VertexColor,id:4279,x:32560,y:32540,varname:node_4279,prsc:2;n:type:ShaderForge.SFN_Multiply,id:4321,x:32913,y:32681,varname:node_4321,prsc:2|A-4279-RGB,B-411-RGB,C-8008-RGB;n:type:ShaderForge.SFN_Multiply,id:7111,x:32985,y:32957,varname:node_7111,prsc:2|A-4279-A,B-8008-A,C-411-A,D-576-R,E-2146-OUT;n:type:ShaderForge.SFN_Tex2d,id:576,x:32560,y:33014,ptovrint:False,ptlb:mask,ptin:_mask,varname:node_576,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:1996,x:32133,y:33122,ptovrint:False,ptlb:dissolve,ptin:_dissolve,varname:node_1996,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-9733-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1006,x:32133,y:33301,ptovrint:False,ptlb:soft,ptin:_soft,varname:node_1006,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Vector1,id:4894,x:32133,y:33371,varname:node_4894,prsc:2,v1:-1.5;n:type:ShaderForge.SFN_TexCoord,id:5920,x:32133,y:33440,varname:node_5920,prsc:2,uv:1,uaff:True;n:type:ShaderForge.SFN_Multiply,id:5936,x:32367,y:33193,varname:node_5936,prsc:2|A-1996-R,B-1006-OUT;n:type:ShaderForge.SFN_Lerp,id:717,x:32367,y:33382,varname:node_717,prsc:2|A-1006-OUT,B-4894-OUT,T-5920-U;n:type:ShaderForge.SFN_Subtract,id:3372,x:32566,y:33308,varname:node_3372,prsc:2|A-5936-OUT,B-717-OUT;n:type:ShaderForge.SFN_Clamp01,id:2146,x:32741,y:33308,varname:node_2146,prsc:2|IN-3372-OUT;n:type:ShaderForge.SFN_Time,id:9178,x:31814,y:32862,varname:node_9178,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:3482,x:31814,y:32757,ptovrint:False,ptlb:diffuse U,ptin:_diffuseU,varname:node_3482,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:6497,x:31814,y:33030,ptovrint:False,ptlb:diffuse V,ptin:_diffuseV,varname:node_6497,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:7609,x:31976,y:32757,varname:node_7609,prsc:2|A-3482-OUT,B-9178-T;n:type:ShaderForge.SFN_Multiply,id:4166,x:31995,y:32883,varname:node_4166,prsc:2|A-9178-T,B-6497-OUT;n:type:ShaderForge.SFN_Append,id:4438,x:32153,y:32853,varname:node_4438,prsc:2|A-7609-OUT,B-4166-OUT;n:type:ShaderForge.SFN_TexCoord,id:5210,x:32153,y:32697,varname:node_5210,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Add,id:1451,x:32381,y:32778,varname:node_1451,prsc:2|A-5210-UVOUT,B-4438-OUT;n:type:ShaderForge.SFN_Time,id:5027,x:31591,y:33292,varname:node_5027,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:9440,x:31591,y:33240,ptovrint:False,ptlb:diss U,ptin:_dissU,varname:node_9440,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:4024,x:31591,y:33443,ptovrint:False,ptlb:diss V,ptin:_dissV,varname:node_4024,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:885,x:31758,y:33267,varname:node_885,prsc:2|A-9440-OUT,B-5027-T;n:type:ShaderForge.SFN_Multiply,id:8494,x:31758,y:33383,varname:node_8494,prsc:2|A-5027-T,B-4024-OUT;n:type:ShaderForge.SFN_TexCoord,id:6725,x:31748,y:33112,varname:node_6725,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Append,id:6858,x:31934,y:33291,varname:node_6858,prsc:2|A-885-OUT,B-8494-OUT;n:type:ShaderForge.SFN_Add,id:9733,x:31961,y:33122,varname:node_9733,prsc:2|A-6725-UVOUT,B-6858-OUT;proporder:411-8008-3482-6497-576-1996-1006-9440-4024;pass:END;sub:END;*/

Shader "ZCW/1alpha" {
    Properties {
		[hideininspector]_MainTex("Texture", 2D) = "white" {}

        _diffusetex ("diffuse tex", 2D) = "white" {}
        [HDR]_diffusecolor ("diffuse color", Color) = (0.5,0.5,0.5,1)
        _diffuseU ("diffuse U", Float ) = 0
        _diffuseV ("diffuse V", Float ) = 0
        _mask ("mask", 2D) = "white" {}
        _dissolve ("dissolve", 2D) = "white" {}
        _soft ("soft", Float ) = 0
        _dissU ("diss U", Float ) = 0
        _dissV ("diss V", Float ) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
        }
        Pass {
            Blend SrcAlpha OneMinusSrcAlpha
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
            uniform sampler2D _dissolve; uniform float4 _dissolve_ST;
            uniform float _soft;
            uniform float _diffuseU;
            uniform float _diffuseV;
            uniform float _dissU;
            uniform float _dissV;
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
                float4 node_9178 = _Time;
                float2 node_1451 = (i.uv0+float2((_diffuseU*node_9178.g),(node_9178.g*_diffuseV)));
                float4 _diffusetex_var = tex2D(_diffusetex,TRANSFORM_TEX(node_1451, _diffusetex));
                float3 emissive = (i.vertexColor.rgb*_diffusetex_var.rgb*_diffusecolor.rgb);
                float3 finalColor = emissive;
                float4 _mask_var = tex2D(_mask,TRANSFORM_TEX(i.uv0, _mask));
                float4 node_5027 = _Time;
                float2 node_9733 = (i.uv0+float2((_dissU*node_5027.g),(node_5027.g*_dissV)));
                float4 _dissolve_var = tex2D(_dissolve,TRANSFORM_TEX(node_9733, _dissolve));
                return fixed4(finalColor,(i.vertexColor.a*_diffusecolor.a*_diffusetex_var.a*_mask_var.r*saturate(((_dissolve_var.r*_soft)-lerp(_soft,(-1.5),i.uv1.r)))));
            }
            ENDCG
        }
    }
}
