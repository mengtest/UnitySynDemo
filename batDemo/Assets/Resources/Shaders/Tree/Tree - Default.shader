Shader "HHHJ/Tree/Default"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_Cutoff ("AlphaCutoff", Range(0,1)) = 0.5
        [Enum(Off, 0, Front, 1, Back, 2)]_Cull ("Cull", float) = 2
    }
    SubShader
    {
        Tags { "RenderType"="TransparentCutout" "Queue"="AlphaTest" "IgnoreProjector"="True" }
        LOD 100
		
        Cull [_Cull]
		
		UsePass "HHHJ/Common/Cutout/Diffuse/Forward"
		UsePass "HHHJ/Common/Cutout/Diffuse/Prepass"
		UsePass "HHHJ/Common/Cutout/Diffuse/Deferred"
		UsePass "HHHJ/Common/Cutout/Diffuse/Meta"
		
		UsePass "HHHJ/Common/ShadowCaster/ShadowCaster-Front-Cutout"
    }
}
