// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:33430,y:32684,varname:node_9361,prsc:2|emission-4390-OUT,alpha-3205-OUT,voffset-7334-OUT;n:type:ShaderForge.SFN_NormalVector,id:3730,x:32439,y:32960,prsc:2,pt:False;n:type:ShaderForge.SFN_TexCoord,id:5037,x:32000,y:32992,varname:node_5037,prsc:2,uv:1,uaff:True;n:type:ShaderForge.SFN_Multiply,id:7334,x:33093,y:33118,varname:node_7334,prsc:2|A-3730-OUT,B-5037-W,C-4456-RGB;n:type:ShaderForge.SFN_Tex2d,id:4456,x:32439,y:33286,ptovrint:False,ptlb:diffuse,ptin:_diffuse,varname:node_4456,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-8781-OUT;n:type:ShaderForge.SFN_Lerp,id:4390,x:32981,y:32570,varname:node_4390,prsc:2|A-8307-OUT,B-434-OUT,T-4391-OUT;n:type:ShaderForge.SFN_Color,id:7045,x:32764,y:32509,ptovrint:False,ptlb:fire1,ptin:_fire1,varname:node_7045,prsc:2,glob:False,taghide:False,taghdr:True,tagprd:False,tagnsco:False,tagnrm:False,c1:0.509804,c2:0.6078432,c3:0.5607843,c4:1;n:type:ShaderForge.SFN_Color,id:2711,x:32504,y:32503,ptovrint:False,ptlb:smoke,ptin:_smoke,varname:node_2711,prsc:2,glob:False,taghide:False,taghdr:True,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Step,id:4391,x:32596,y:32847,varname:node_4391,prsc:2|A-5037-Z,B-4456-R;n:type:ShaderForge.SFN_Multiply,id:3570,x:32734,y:33126,varname:node_3570,prsc:2|A-4456-R,B-9904-OUT;n:type:ShaderForge.SFN_ValueProperty,id:9904,x:32669,y:33337,ptovrint:False,ptlb:line,ptin:_line,varname:node_9904,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1.1;n:type:ShaderForge.SFN_Step,id:6580,x:32879,y:33018,varname:node_6580,prsc:2|A-5037-Z,B-3570-OUT;n:type:ShaderForge.SFN_Lerp,id:8307,x:33196,y:32412,varname:node_8307,prsc:2|A-7045-RGB,B-5050-RGB,T-6580-OUT;n:type:ShaderForge.SFN_Color,id:5050,x:32734,y:32886,ptovrint:False,ptlb:fire2,ptin:_fire2,varname:node_5050,prsc:2,glob:False,taghide:False,taghdr:True,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Tex2d,id:3835,x:32003,y:32427,ptovrint:False,ptlb:diss,ptin:_diss,varname:node_3835,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_TexCoord,id:9192,x:31654,y:33027,varname:node_9192,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_ValueProperty,id:9725,x:31615,y:33220,ptovrint:False,ptlb:u,ptin:_u,varname:node_9725,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:8762,x:31600,y:33426,ptovrint:False,ptlb:v,ptin:_v,varname:node_8762,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:8958,x:31781,y:33220,varname:node_8958,prsc:2|A-9725-OUT,B-186-T;n:type:ShaderForge.SFN_Multiply,id:7023,x:31781,y:33359,varname:node_7023,prsc:2|A-186-T,B-8762-OUT;n:type:ShaderForge.SFN_Append,id:3917,x:32025,y:33269,varname:node_3917,prsc:2|A-8958-OUT,B-7023-OUT;n:type:ShaderForge.SFN_Time,id:186,x:31615,y:33281,varname:node_186,prsc:2;n:type:ShaderForge.SFN_Add,id:8781,x:32242,y:33247,varname:node_8781,prsc:2|A-9192-UVOUT,B-3917-OUT;n:type:ShaderForge.SFN_Multiply,id:434,x:32764,y:32406,varname:node_434,prsc:2|A-1941-RGB,B-2711-RGB,C-4925-RGB,D-4925-A;n:type:ShaderForge.SFN_Tex2d,id:1941,x:32504,y:32309,ptovrint:False,ptlb:smoketex,ptin:_smoketex,varname:node_1941,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_VertexColor,id:4925,x:32736,y:32201,varname:node_4925,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:2528,x:32003,y:32636,ptovrint:False,ptlb:soft,ptin:_soft,varname:node_2528,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Vector1,id:9473,x:32003,y:32701,varname:node_9473,prsc:2,v1:-1.5;n:type:ShaderForge.SFN_Lerp,id:4057,x:32190,y:32667,varname:node_4057,prsc:2|A-2528-OUT,B-9473-OUT,T-5037-U;n:type:ShaderForge.SFN_Multiply,id:2708,x:32177,y:32493,varname:node_2708,prsc:2|A-3835-R,B-2528-OUT;n:type:ShaderForge.SFN_Subtract,id:868,x:32407,y:32667,varname:node_868,prsc:2|A-2708-OUT,B-4057-OUT;n:type:ShaderForge.SFN_Clamp01,id:3205,x:32660,y:32686,varname:node_3205,prsc:2|IN-868-OUT;proporder:4456-7045-2711-9904-5050-3835-9725-8762-1941-2528;pass:END;sub:END;*/

Shader "ZCW/bomb" {
    Properties {
        _diffuse ("diffuse", 2D) = "white" {}
        [HDR]_fire1 ("fire1", Color) = (0.509804,0.6078432,0.5607843,1)
        [HDR]_smoke ("smoke", Color) = (0.5,0.5,0.5,1)
        _line ("line", Float ) = 1.1
        [HDR]_fire2 ("fire2", Color) = (0.5,0.5,0.5,1)
        _diss ("diss", 2D) = "white" {}
        _u ("u", Float ) = 0
        _v ("v", Float ) = 0
        _smoketex ("smoketex", 2D) = "white" {}
        _soft ("soft", Float ) = 0
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
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma target 3.0
            uniform sampler2D _diffuse; uniform float4 _diffuse_ST;
            uniform float4 _fire1;
            uniform float4 _smoke;
            uniform float _line;
            uniform float4 _fire2;
            uniform sampler2D _diss; uniform float4 _diss_ST;
            uniform float _u;
            uniform float _v;
            uniform sampler2D _smoketex; uniform float4 _smoketex_ST;
            uniform float _soft;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_186 = _Time;
                float2 node_8781 = (o.uv0+float2((_u*node_186.g),(node_186.g*_v)));
                float4 _diffuse_var = tex2Dlod(_diffuse,float4(TRANSFORM_TEX(node_8781, _diffuse),0.0,0));
                v.vertex.xyz += (v.normal*o.uv1.a*_diffuse_var.rgb);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float4 node_186 = _Time;
                float2 node_8781 = (i.uv0+float2((_u*node_186.g),(node_186.g*_v)));
                float4 _diffuse_var = tex2D(_diffuse,TRANSFORM_TEX(node_8781, _diffuse));
                float4 _smoketex_var = tex2D(_smoketex,TRANSFORM_TEX(i.uv0, _smoketex));
                float3 emissive = lerp(lerp(_fire1.rgb,_fire2.rgb,step(i.uv1.b,(_diffuse_var.r*_line))),(_smoketex_var.rgb*_smoke.rgb*i.vertexColor.rgb*i.vertexColor.a),step(i.uv1.b,_diffuse_var.r));
                float3 finalColor = emissive;
                float4 _diss_var = tex2D(_diss,TRANSFORM_TEX(i.uv0, _diss));
                fixed4 finalRGBA = fixed4(finalColor,saturate(((_diss_var.r*_soft)-lerp(_soft,(-1.5),i.uv1.r))));
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma target 3.0
            uniform sampler2D _diffuse; uniform float4 _diffuse_ST;
            uniform float _u;
            uniform float _v;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float4 uv1 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_186 = _Time;
                float2 node_8781 = (o.uv0+float2((_u*node_186.g),(node_186.g*_v)));
                float4 _diffuse_var = tex2Dlod(_diffuse,float4(TRANSFORM_TEX(node_8781, _diffuse),0.0,0));
                v.vertex.xyz += (v.normal*o.uv1.a*_diffuse_var.rgb);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
