Shader "Custom/AlphaMask" 
{
	SubShader 
	{
		Tags {"Queue" = "Transparent+10" "RenderType"="Transparent"}
		LOD 200
		
		Blend Zero Zero

        Pass
        {
            ColorMask A
        }
	}
	
	FallBack "Diffuse"
}
