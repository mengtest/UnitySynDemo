// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.36 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.36;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:32869,y:32289,varname:node_3138,prsc:2|emission-6151-OUT,custl-117-OUT,alpha-6644-OUT;n:type:ShaderForge.SFN_Tex2d,id:3755,x:32790,y:32999,ptovrint:False,ptlb:Mask Texture,ptin:_MaskTexture,varname:node_3755,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_If,id:5746,x:32149,y:32951,varname:node_5746,prsc:2|A-654-OUT,B-3755-R,GT-6272-OUT,EQ-6272-OUT,LT-3073-OUT;n:type:ShaderForge.SFN_Vector1,id:6272,x:32410,y:33096,varname:node_6272,prsc:2,v1:1;n:type:ShaderForge.SFN_Vector1,id:3073,x:32410,y:33221,varname:node_3073,prsc:2,v1:0;n:type:ShaderForge.SFN_Tex2d,id:5620,x:31863,y:32531,ptovrint:False,ptlb:Diffuse Texture,ptin:_MainTex,varname:_node_3755_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:6731,x:31483,y:32881,varname:node_6731,prsc:2|A-5620-A,B-6450-OUT;n:type:ShaderForge.SFN_Color,id:7519,x:31867,y:32262,ptovrint:False,ptlb:Diffuse Color,ptin:_TintColor,varname:node_7519,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.6985294,c2:0.6985294,c3:0.6985294,c4:1;n:type:ShaderForge.SFN_Multiply,id:3808,x:31439,y:32419,varname:node_3808,prsc:2|A-7519-RGB,B-5620-RGB;n:type:ShaderForge.SFN_Multiply,id:6644,x:31268,y:32755,varname:node_6644,prsc:2|A-7519-A,B-6731-OUT;n:type:ShaderForge.SFN_ValueProperty,id:329,x:32661,y:32887,ptovrint:False,ptlb:N_mask,ptin:_N_mask,varname:node_329,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.3;n:type:ShaderForge.SFN_If,id:5217,x:32129,y:33205,varname:node_5217,prsc:2|A-654-OUT,B-2022-OUT,GT-6272-OUT,EQ-6272-OUT,LT-3073-OUT;n:type:ShaderForge.SFN_Add,id:2022,x:32548,y:33159,varname:node_2022,prsc:2|A-3755-R,B-5828-OUT;n:type:ShaderForge.SFN_ValueProperty,id:5828,x:32779,y:33257,ptovrint:False,ptlb:N_BY_KD,ptin:_N_BY_KD,varname:node_5828,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.01;n:type:ShaderForge.SFN_Subtract,id:1274,x:31919,y:33102,varname:node_1274,prsc:2|A-5746-OUT,B-5217-OUT;n:type:ShaderForge.SFN_Color,id:9508,x:31919,y:33271,ptovrint:False,ptlb:C_BYcolor,ptin:_C_BYcolor,varname:node_9508,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Multiply,id:7346,x:31725,y:33178,varname:node_7346,prsc:2|A-1274-OUT,B-9508-RGB;n:type:ShaderForge.SFN_ValueProperty,id:8447,x:31919,y:33450,ptovrint:False,ptlb:N_BY_QD,ptin:_N_BY_QD,varname:node_8447,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:3;n:type:ShaderForge.SFN_Multiply,id:7666,x:31370,y:33152,varname:node_7666,prsc:2|A-7346-OUT,B-8447-OUT;n:type:ShaderForge.SFN_Add,id:6450,x:31687,y:33031,varname:node_6450,prsc:2|A-5746-OUT,B-1274-OUT;n:type:ShaderForge.SFN_Multiply,id:4059,x:31215,y:32519,varname:node_4059,prsc:2|A-3808-OUT,B-6450-OUT;n:type:ShaderForge.SFN_Add,id:1014,x:31035,y:32663,varname:node_1014,prsc:2|A-4059-OUT,B-7666-OUT;n:type:ShaderForge.SFN_VertexColor,id:8945,x:32661,y:32699,varname:node_8945,prsc:2;n:type:ShaderForge.SFN_Multiply,id:654,x:32377,y:32890,varname:node_654,prsc:2|A-8945-A,B-329-OUT;n:type:ShaderForge.SFN_Vector1,id:9158,x:31295,y:32976,varname:node_9158,prsc:2,v1:0.4;n:type:ShaderForge.SFN_Multiply,id:6151,x:30848,y:32593,varname:node_6151,prsc:2|A-7519-A,B-1014-OUT;n:type:ShaderForge.SFN_Multiply,id:117,x:30951,y:32892,varname:node_117,prsc:2|A-7519-A,B-7666-OUT;proporder:7519-5620-329-3755-9508-8447-5828;pass:END;sub:END;*/

Shader "Blue/Effect/Dissolution_add" {
    Properties {
        _TintColor ("Diffuse Color", Color) = (0.6985294,0.6985294,0.6985294,1)
        _MainTex ("Diffuse Texture", 2D) = "white" {}
        _N_mask ("N_mask", Float ) = 0.3
        _MaskTexture ("Mask Texture", 2D) = "white" {}
        _C_BYcolor ("C_BYcolor", Color) = (1,0,0,1)
        _N_BY_QD ("N_BY_QD", Float ) = 3
        _N_BY_KD ("N_BY_KD", Float ) = 0.01
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            ZWrite Off
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform sampler2D _MaskTexture; uniform float4 _MaskTexture_ST;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _TintColor;
            uniform float _N_mask;
            uniform float _N_BY_KD;
            uniform float4 _C_BYcolor;
            uniform float _N_BY_QD;
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
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float node_654 = (i.vertexColor.a*_N_mask);
                float4 _MaskTexture_var = tex2D(_MaskTexture,TRANSFORM_TEX(i.uv0, _MaskTexture));
                float node_5746_if_leA = step(node_654,_MaskTexture_var.r);
                float node_5746_if_leB = step(_MaskTexture_var.r,node_654);
                float node_3073 = 0.0;
                float node_6272 = 1.0;
                float node_5746 = lerp((node_5746_if_leA*node_3073)+(node_5746_if_leB*node_6272),node_6272,node_5746_if_leA*node_5746_if_leB);
                float node_5217_if_leA = step(node_654,(_MaskTexture_var.r+_N_BY_KD));
                float node_5217_if_leB = step((_MaskTexture_var.r+_N_BY_KD),node_654);
                float node_1274 = (node_5746-lerp((node_5217_if_leA*node_3073)+(node_5217_if_leB*node_6272),node_6272,node_5217_if_leA*node_5217_if_leB));
                float node_6450 = (node_5746+node_1274);
                float3 node_7666 = ((node_1274*_C_BYcolor.rgb)*_N_BY_QD);
                float3 emissive = (_TintColor.a*(((_TintColor.rgb*_MainTex_var.rgb)*node_6450)+node_7666));
                float3 finalColor = emissive + (_TintColor.a*node_7666);
                return fixed4(finalColor,(_TintColor.a*(_MainTex_var.a*node_6450)));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
