Shader "Custom/SpaceDistortion" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_DistortionTex ("Dist (RGB)", 2D) = "white" {}
		_distortionPower (" factor", Float) = 0.02
	}
	
	SubShader
    {
        Pass
        {
            Tags {"RenderType"="Opaque"}
            Lighting Off
           
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
               
                #pragma fragmentoption ARB_fog_exp2
                #pragma fragmentoption ARB_precision_hint_fastest
               
                #include "UnityCG.cginc"
              
                uniform sampler2D _MainTex;
                uniform float4 _MainTex_ST;
		        uniform sampler2D _DistortionTex;
		        uniform half _distortionPower;
               
                struct v2f
                {
                    float4 pos : SV_POSITION;
                    float2 texcoord : TEXCOORD0;
                    float2 distortTexCoord: TEXCOORD1;
                };

                v2f vert(appdata_base v)
                {
                    v2f o;
                   
                    o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
                    o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                    o.distortTexCoord = o.texcoord;
                    #if UNITY_UV_STARTS_AT_TOP
                    float scale = -1.0;
                    #else
                    float scale = 1.0;
                    #endif
                    o.distortTexCoord.y *= scale;
                   
                    return o;
                }
              
                float4 frag(v2f i) : COLOR
                {
                    half4 c = tex2D (_MainTex, i.texcoord);
                    
                    half4 d = tex2D (_DistortionTex, i.distortTexCoord);
                    half2 offset = (d.rg - half2(0.5, 0.5)) * d.a * _distortionPower;
                    half4 ac = tex2D (_MainTex, i.texcoord + offset * (1.0 - c.a));
					
					if(ac.a > 0.0) 
					{
					    ac = c;
					}
					return ac;
                }
            ENDCG
        }
    }
	FallBack "Diffuse"
}
