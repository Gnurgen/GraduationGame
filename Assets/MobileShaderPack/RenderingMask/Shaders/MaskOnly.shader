Shader "Custom/MaskOnly" 
{
	Properties 
	{
	}

	SubShader 
	{
		Tags {"RenderType"="Opaque"}
		LOD 200
		
		ZWrite On

		BindChannels 
		{
			Bind "Vertex", vertex
			Bind "TexCoord", texcoord
			Bind "Color", color
		}

        Pass
        {
            colormask 0
            ZWrite On
        }
	}
}
