Shader "Custom/MaskObjectTransparent" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_AddAlpha ("Alpha", Range (0,1)) = 1
	}
	
	SubShader 
	{
		Tags { "Queue"="Transparent+1" "RenderType"="Transparent" }
		LOD 200
		
		Blend SrcAlpha OneMinusSrcAlpha
		ZTest Greater  
		ZWrite Off
		
		CGPROGRAM
		#pragma surface surf Lambert keepalpha

		sampler2D _MainTex;
		float _AddAlpha;

		struct Input 
		{
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a * _AddAlpha;
		}
		
		ENDCG
	} 
	
	FallBack "Diffuse"
}
