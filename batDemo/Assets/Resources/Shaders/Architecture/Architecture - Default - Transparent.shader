Shader "HHHJ/Architecture/Default - Transparent"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
        [Enum(Off, 0, Front, 1, Back, 2)]_Cull ("Cull", float) = 2
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
		
        Cull [_Cull]
		
		UsePass "HHHJ/Common/Transparent/Diffuse/Forward"
		UsePass "HHHJ/Common/Transparent/Diffuse/Meta"
		
		UsePass "HHHJ/Common/ShadowCaster/ShadowCaster-Front"
    }
}
