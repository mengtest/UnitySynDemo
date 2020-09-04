Shader "HHHJ/Architecture/Default"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
        [Enum(Off, 0, Front, 1, Back, 2)]_Cull ("Cull", float) = 2
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 100
		
        Cull [_Cull]
		
		UsePass "HHHJ/Common/Diffuse - HalfLambert/Forward"
		UsePass "HHHJ/Common/Diffuse - HalfLambert/Meta"
		
		UsePass "HHHJ/Common/ShadowCaster/ShadowCaster-Front"
    }
}
