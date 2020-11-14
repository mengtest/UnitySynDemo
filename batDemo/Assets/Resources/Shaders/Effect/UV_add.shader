// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:7621,x:33534,y:32750,varname:node_7621,prsc:2|emission-577-OUT;n:type:ShaderForge.SFN_Tex2d,id:8567,x:32382,y:32676,ptovrint:False,ptlb:diffuse,ptin:_diffuse,varname:node_8567,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-521-OUT;n:type:ShaderForge.SFN_Multiply,id:616,x:32664,y:32908,varname:node_616,prsc:2|A-8567-RGB,B-3769-RGB,C-4313-RGB,D-5176-R,E-4313-A;n:type:ShaderForge.SFN_Color,id:3769,x:32382,y:32854,ptovrint:False,ptlb:node_3769,ptin:_node_3769,varname:node_3769,prsc:2,glob:False,taghide:False,taghdr:True,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_VertexColor,id:4313,x:32382,y:32998,varname:node_4313,prsc:2;n:type:ShaderForge.SFN_Tex2d,id:5176,x:32382,y:33182,ptovrint:False,ptlb:mask,ptin:_mask,varname:node_5176,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-3847-OUT;n:type:ShaderForge.SFN_Multiply,id:577,x:32905,y:32827,varname:node_577,prsc:2|A-8567-A,B-3769-A,C-616-OUT;n:type:ShaderForge.SFN_Time,id:2895,x:31253,y:33171,varname:node_2895,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:6278,x:31139,y:33102,ptovrint:False,ptlb:U,ptin:_U,varname:node_6278,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:5510,x:31198,y:33393,ptovrint:False,ptlb:V,ptin:_V,varname:node_5510,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:6986,x:31547,y:33106,varname:node_6986,prsc:2|A-6278-OUT,B-2895-T;n:type:ShaderForge.SFN_Multiply,id:7632,x:31562,y:33268,varname:node_7632,prsc:2|A-2895-T,B-5510-OUT;n:type:ShaderForge.SFN_Append,id:6336,x:31742,y:33177,varname:node_6336,prsc:2|A-6986-OUT,B-7632-OUT;n:type:ShaderForge.SFN_TexCoord,id:4744,x:31689,y:32997,varname:node_4744,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Add,id:838,x:31994,y:32900,varname:node_838,prsc:2|A-4744-UVOUT,B-6336-OUT;n:type:ShaderForge.SFN_Tex2d,id:8988,x:31718,y:32606,ptovrint:False,ptlb:noise,ptin:_noise,varname:node_8988,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-631-OUT;n:type:ShaderForge.SFN_TexCoord,id:172,x:31313,y:32430,varname:node_172,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_ValueProperty,id:7258,x:31054,y:32501,ptovrint:False,ptlb:U2,ptin:_U2,varname:node_7258,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:8502,x:31040,y:32847,ptovrint:False,ptlb:V2,ptin:_V2,varname:node_8502,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Time,id:4161,x:31018,y:32606,varname:node_4161,prsc:2;n:type:ShaderForge.SFN_Multiply,id:7178,x:31257,y:32606,varname:node_7178,prsc:2|A-7258-OUT,B-4161-T;n:type:ShaderForge.SFN_Multiply,id:7557,x:31273,y:32767,varname:node_7557,prsc:2|A-4161-T,B-8502-OUT;n:type:ShaderForge.SFN_Add,id:631,x:31603,y:32461,varname:node_631,prsc:2|A-172-UVOUT,B-8981-OUT;n:type:ShaderForge.SFN_Append,id:8981,x:31471,y:32666,varname:node_8981,prsc:2|A-7178-OUT,B-7557-OUT;n:type:ShaderForge.SFN_ValueProperty,id:3016,x:31743,y:32793,ptovrint:False,ptlb:noise pow,ptin:_noisepow,varname:node_3016,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:5618,x:31959,y:32698,varname:node_5618,prsc:2|A-8988-R,B-3016-OUT;n:type:ShaderForge.SFN_Add,id:521,x:32168,y:32736,varname:node_521,prsc:2|A-5618-OUT,B-838-OUT;n:type:ShaderForge.SFN_Time,id:9065,x:31690,y:33491,varname:node_9065,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:6160,x:31576,y:33422,ptovrint:False,ptlb:maskU,ptin:_maskU,varname:_U_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:7942,x:31635,y:33713,ptovrint:False,ptlb:maskV,ptin:_maskV,varname:_V_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:4270,x:31984,y:33426,varname:node_4270,prsc:2|A-6160-OUT,B-9065-T;n:type:ShaderForge.SFN_Multiply,id:9546,x:31999,y:33588,varname:node_9546,prsc:2|A-9065-T,B-7942-OUT;n:type:ShaderForge.SFN_Append,id:8571,x:32172,y:33467,varname:node_8571,prsc:2|A-4270-OUT,B-9546-OUT;n:type:ShaderForge.SFN_Add,id:3847,x:32200,y:33215,varname:node_3847,prsc:2|A-4744-UVOUT,B-8571-OUT;proporder:8567-3769-5176-6278-5510-8988-7258-8502-3016-6160-7942;pass:END;sub:END;*/

Shader "ZCW/UV_add" {
    Properties {
        _diffuse ("diffuse", 2D) = "white" {}
        [HDR]_node_3769 ("node_3769", Color) = (0.5,0.5,0.5,1)
        _mask ("mask", 2D) = "white" {}
        _U ("U", Float ) = 0
        _V ("V", Float ) = 0
        _noise ("noise", 2D) = "white" {}
        _U2 ("U2", Float ) = 0
        _V2 ("V2", Float ) = 0
        _noisepow ("noise pow", Float ) = 0
        _maskU ("maskU", Float ) = 0
        _maskV ("maskV", Float ) = 0
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 200
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            //#pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _diffuse; uniform float4 _diffuse_ST;
            uniform float4 _node_3769;
            uniform sampler2D _mask; uniform float4 _mask_ST;
            uniform float _U;
            uniform float _V;
            uniform sampler2D _noise; uniform float4 _noise_ST;
            uniform float _U2;
            uniform float _V2;
            uniform float _noisepow;
            uniform float _maskU;
            uniform float _maskV;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
////// Emissive:
                float4 node_4161 = _Time;
                float2 node_631 = (i.uv0+float2((_U2*node_4161.g),(node_4161.g*_V2)));
                float4 _noise_var = tex2D(_noise,TRANSFORM_TEX(node_631, _noise));
                float4 node_2895 = _Time;
                float2 node_521 = ((_noise_var.r*_noisepow)+(i.uv0+float2((_U*node_2895.g),(node_2895.g*_V))));
                float4 _diffuse_var = tex2D(_diffuse,TRANSFORM_TEX(node_521, _diffuse));
                float4 node_9065 = _Time;
                float2 node_3847 = (i.uv0+float2((_maskU*node_9065.g),(node_9065.g*_maskV)));
                float4 _mask_var = tex2D(_mask,TRANSFORM_TEX(node_3847, _mask));
                float3 emissive = (_diffuse_var.a*_node_3769.a*(_diffuse_var.rgb*_node_3769.rgb*i.vertexColor.rgb*_mask_var.r*i.vertexColor.a));
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
