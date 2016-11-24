Shader "Custom/MaskTransparent" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Opaque ("Alpha", Range (0,1)) = 1
	}
	
	SubShader
	{
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		LOD 200
		
		ZWrite On
		Blend SrcAlpha OneMinusSrcAlpha 
		
		
		CGPROGRAM
		#pragma surface surf Lambert keepalpha

		sampler2D _MainTex;
		float _Opaque;

		struct Input 
		{
			float2 uv_MainTex;
		};
		
		void surf (Input IN, inout SurfaceOutput o)
		{
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a * _Opaque;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
