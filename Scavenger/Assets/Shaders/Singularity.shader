Shader "Custom/Singularity" 
{
	Properties 
	{
		[HDR]_Colour("Freznel Colour", Color) = (1,1,1,1)
		_RimPower("Freznel Power", Float) = 1
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf NoLighting NoForwardAdd

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		half4 LightingNoLighting(SurfaceOutput s, half3 lightDir, half atten) 
		{
			half4 c;
			c.rgb = s.Albedo;
			c.a = s.Alpha;
			return c;
		}

		struct Input 
		{
			float3 viewDir;
		};

		fixed4 _Colour;
		float _RimPower;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutput o) 
		{
			// Albedo comes from a texture tinted by color
			half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
			o.Albedo = _Colour.rgb * pow(rim, _RimPower);
			o.Emission = _Colour.rgb * pow(rim, _RimPower);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
