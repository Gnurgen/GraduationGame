Shader "Water" 
{
	Properties 
	{
	    _ReflectionTex ("Reflection Texture", 2D) = "white" {}
		_RenderTex ("Render Texture, Show Refraction", 2D) = "white" {}
		_DistortTex ("Distort Texture", 2D) = "white" {}
		
		_DistortValue ("Distortion", Float) = 30
		
		_RefrReflBlendValue ("Refraction reflection blending", range(0, 1.0)) = 0.5

        _DistortScale ("Distort Scale", Float) = 1.5
		_DistortSpeedOnX ("Distort Speed X", Float) = 6
		_DistortSpeedOnY ("Distort Speed Y", Float) = 6
		
		_RfractionTexOffsetX ("Refraction Texture Offset On X", Float) = 0
		_RfractionTexOffsetY ("Refraction Texture Offset On Y", Float) = 0
	}

	Category 
	{
		Tags { "Queue"="Geometry" "RenderType"="Opaque" }

		SubShader
		{
			Pass 
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#include "UnityCG.cginc"

				struct appdata_t
				{
					float4 vertex : POSITION;
					float2 texcoord: TEXCOORD0;
				};

				struct v2f 
				{
					float4 vertex : POSITION;
					float4 uvRtt : TEXCOORD0;
					float2 uvDistort : TEXCOORD1;
					//float2 uvRefl : TEXCOORD2;
				};

                sampler2D _ReflectionTex;
				sampler2D _RenderTex;
                sampler2D _DistortTex;
                
				float _DistortValue;
				float4 _DistortTex_ST;
				float4 _ReflTex_ST;
				//float4 _RenderTex_TexelSize;
				float _RefrReflBlendValue;
				half _DistortScale;
				half _DistortSpeedOnX;
			    half _DistortSpeedOnY;
				half _RfractionTexOffsetX;
				half _RfractionTexOffsetY;

				v2f vert (appdata_t v)
				{
					v2f o;
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
					
					#if UNITY_UV_STARTS_AT_TOP
					float flip = -1.0;
					#else
					float flip = 1.0;
					#endif
					o.uvRtt.xy = ComputeScreenPos(o.vertex);//(float2(o.vertex.x, o.vertex.y * flip) + o.vertex.w) * 0.5;
					o.uvRtt.zw = o.vertex.zw;
					
					//o.uvRefl = ComputeScreenPos(o.vertex);//TRANSFORM_TEX( v.texcoord, _ReflTex);
					
					o.uvDistort = TRANSFORM_TEX(v.texcoord * float2(_DistortScale, _DistortScale), _DistortTex);
					o.uvDistort.x += _Time.x * _DistortSpeedOnX;
					o.uvDistort.y += _Time.x * _DistortSpeedOnY;
				
					return o;
				}

				half4 frag( v2f i ) : COLOR
				{
					float2 distortion =  tex2D(_DistortTex, i.uvDistort).rg - half2(0.5, 0.5);
					distortion = distortion * _DistortValue;// * _RenderTex_TexelSize.xy;

                    float4 reflUv = i.uvRtt;
                    reflUv.xy = distortion * reflUv.z + reflUv.xy;
					half4 refl = tex2Dproj(_ReflectionTex, UNITY_PROJ_COORD(reflUv));//i.uvRefl + distortion);
					
					if(refl.a < 0.01) 
					{
					    distortion = half2(0, 0);
					}
					
					if(refl.a > 0.01) 
					{
					    i.uvRtt.xy += half2(_RfractionTexOffsetX, _RfractionTexOffsetY);
					}
					
					i.uvRtt.xy = distortion * i.uvRtt.z + i.uvRtt.xy;
					
					half4 col = tex2Dproj(_RenderTex, UNITY_PROJ_COORD(i.uvRtt));
					
					half4 fColor = lerp(col, refl, _RefrReflBlendValue * refl);
					if(refl.a < 0.01) 
					{
					    fColor = col;
					}
					
					return fColor;
				}
				ENDCG
			}
		}
	}
}
