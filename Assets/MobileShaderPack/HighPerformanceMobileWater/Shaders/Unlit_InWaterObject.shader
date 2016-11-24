﻿Shader "Unlit_InWaterObject"
 {
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		
		_WaterPlane ("Water Plane Position Y", float) = 0.0
		_UnderWaterColor ("Under Water Color", Color) = (0.0, 0.0, 1, 1)
		
		_WaterColorStart ("Position Y To Start Have Under Water Color", float) = 0.1
		_WaterColorEnd ("Position Y To Have Deepest Under Water Color", float) = 1.0
		_WaterColorMax ("Maxium Depth To Have Deepest Under Water Color", float) = 1.1
	}
	
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		
		#pragma surface surf Unlit nolightmap nodirlightmap noforwardadd 
		
		sampler2D _MainTex;
		
		float _WaterPlane;
		float4 _UnderWaterColor;
		float _WaterColorStart;
		float _WaterColorEnd;
		float _WaterColorMax;
		
		struct Input
		{
			half2 uv_MainTex : TEXCOORD0;
			float3 worldPos;
		};
		
		
		half4 LightingUnlit (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
		{	
		    half4 col = half4(s.Albedo, 1.0);
			return col;
		}
		
		void surf (Input IN, inout SurfaceOutput o)
		{
			half4 c = tex2D(_MainTex, IN.uv_MainTex);
			
			o.Albedo = c.rgb;
			
			if(IN.worldPos.y < _WaterPlane)
			{
			    float waterFactor = clamp(_WaterPlane - IN.worldPos.y, _WaterColorStart, _WaterColorEnd) / _WaterColorMax;
			    half4 waterColor = lerp(half4(1,1,1,1) ,_UnderWaterColor, waterFactor);
			    o.Albedo = o.Albedo * waterColor.rgb; //lerp(o.Albedo, _UnderWaterColor, waterFactor);
			}
			
			o.Alpha = 1;
		}
		ENDCG
	}
	
	Fallback "VertexLit"
}
