Shader "HHHJ/Weapon/Default - Transparent"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
		
		UsePass "HHHJ/Common/Transparent/Texture/Base"
		
		UsePass "HHHJ/Common/ShadowCaster/ShadowCaster-Front"
    }
}
