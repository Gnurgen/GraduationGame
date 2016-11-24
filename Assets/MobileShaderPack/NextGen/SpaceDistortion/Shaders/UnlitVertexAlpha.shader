Shader "Custom/UnlitVertexAlpha" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	
	SubShader
	{
		Tags {"Queue"="Transparent" "RenderType"="Transparent" }
		LOD 200
		
		Cull Off
		CGPROGRAM
		#pragma surface surf Unlit alpha keepalpha

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float4 color: COLOR;
		};
		
		half4 LightingUnlit (SurfaceOutput s, half3 lightDir, half atten) 
		{
           half4 c;
           c.rgb = s.Albedo;
           c.a = s.Alpha;
           return c;
         }

		void surf (Input IN, inout SurfaceOutput o) 
		{
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a * IN.color.a;
		}
		ENDCG
	} 
	
	FallBack "Diffuse"
}
