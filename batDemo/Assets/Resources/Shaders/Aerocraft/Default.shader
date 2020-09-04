Shader "HHHJ/Aerocraft/Default"
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
        Tags { "RenderType"="Opaque" "Queue"="Transparent" }
        LOD 100
		
		Cull [_Cull]
		
		UsePass "HHHJ/Common/Dissolve/Base"
    }
}
