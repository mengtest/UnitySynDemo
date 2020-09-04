Shader "HHHJ/Character/Default"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 100
		
		UsePass "HHHJ/Common/Texture/Base"
		
		UsePass "HHHJ/Common/ShadowCaster/ShadowCaster-Front"
    }
}
