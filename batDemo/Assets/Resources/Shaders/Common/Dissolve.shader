Shader "HHHJ/Common/Dissolve"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_DissolveFactor ("Dissolve Factor", Range(0, 1)) = 1
		_DissolveTexture ("Dissolve Texture", 2D) = "white" {}
		_EdgeColor ("Edge Color", Color) = (1,1,1,1)
		_EdgeWidth ("Edge Width", Range(0, 0.5)) = 0.1
        [Enum(Off, 0, Front, 1, Back, 2)]_Cull ("Cull", float) = 2
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="AlphaTest"}
        LOD 100
		
		Cull [_Cull]

        Pass
        {
			Name "Base"
			
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			sampler2D _DissolveTexture;
			half _DissolveFactor;
			fixed _EdgeWidth;
			half4 _EdgeColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
				fixed3 dissolveCol = tex2D(_DissolveTexture, i.uv);
				half diff = _DissolveFactor - dissolveCol.r;
				clip(diff);
				col.rgb = lerp(col.rgb, _EdgeColor, saturate(1 - diff / _EdgeWidth));
				
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
