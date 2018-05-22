Shader "Custom/SurfaceVertexColourShader" 
{
	Properties 
	{
		_Col100 ("Tint", Color) = (1,1,1,1)
		_Tex100 ("Material 1", 2D) = "white" {}
		[Normal]_Tex100_N("Material 1 Normal", 2D) = "bump" {}
		_Smooth100("Smoothness", Range(0,1)) = 0.5
		_Metal100("Metallic", Range(0,1)) = 0.0

		_Col75("Tint", Color) = (1,1,1,1)
		_Tex75("Material 2", 2D) = "white" {}
		[Normal]_Tex75_N("Material 2 Normal", 2D) = "bump" {}
		_Smooth75("Smoothness", Range(0,1)) = 0.5
		_Metal75("Metallic", Range(0,1)) = 0.0

		_Col50("Tint", Color) = (1,1,1,1)
		_Tex50("Material 3", 2D) = "white" {}
		[Normal]_Tex50_N("Material 3 Normal", 2D) = "bump" {}
		_Smooth50("Smoothness", Range(0,1)) = 0.5
		_Metal50("Metallic", Range(0,1)) = 0.0

		_Col25("Tint", Color) = (1,1,1,1)
		_Tex25("Material 4", 2D) = "white" {}
		[Normal]_Tex25_N("Material 4 Normal", 2D) = "bump" {}
		_Smooth25("Smoothness", Range(0,1)) = 0.5
		_Metal25("Metallic", Range(0,1)) = 0.0

		_Col0("Tint", Color) = (1,1,1,1)
		_Tex0("Material 5", 2D) = "white" {}
		[Normal]_Tex0_N("Material 5 Normal", 2D) = "bump" {}
		_Smooth0("Smoothness", Range(0,1)) = 0.5
		_Metal0("Metallic", Range(0,1)) = 0.0

		[HDR]_GlowR("Glow 1", Color) = (1,0,0,1)
		[HDR]_GlowG("Glow 2", Color) = (0,1,0,1)
		[HDR]_GlowB("Glow 3", Color) = (0,0,1,1)
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		fixed4 _Col100;
		sampler2D _Tex100;
		half4 _Tex100_ST;

		sampler2D _Tex100_N;
		half4 _Tex100_N_ST;

		half _Smooth100;
		half _Metal100;

		fixed4 _Col75;
		sampler2D _Tex75;
		half4 _Tex75_ST;

		sampler2D _Tex75_N;
		half4 _Tex75_N_ST;

		sampler2D _Tex75_D;
		half4 _Tex75_D_ST;

		half _Smooth75;
		half _Metal75;

		fixed4 _Col50;
		sampler2D _Tex50;
		half4 _Tex50_ST;

		sampler2D _Tex50_N;
		half4 _Tex50_N_ST;

		half _Smooth50;
		half _Metal50;

		fixed4 _Col25;
		sampler2D _Tex25;
		half4 _Tex25_ST;

		sampler2D _Tex25_N;
		half4 _Tex25_N_ST;

		half _Smooth25;
		half _Metal25;

		fixed4 _Col0;
		sampler2D _Tex0;
		half4 _Tex0_ST;

		sampler2D _Tex0_N;
		half4 _Tex0_N_ST;

		half _Smooth0;
		half _Metal0;

		struct Input 
		{
			float3 worldPos;
			float3 localPos;
			//float3 normal;
			fixed4 colour : COLOR;
			//fixed info;
		};

		fixed4 _GlowR;
		fixed4 _GlowG;
		fixed4 _GlowB;

		int Type(fixed3 colour)
		{
			if (colour.r == colour.g && colour.r == colour.b)
			{
				if (colour.r > 0.753)
					return 0;
				else if (colour.r > 0.502)
					return 1;
				else if (colour.r > 0.251)
					return 2;
				else if (colour.r > 0)
					return 3;
				else
					return 4;
			}
			else
			{
				if (colour.r > 0.5 && colour.g < 0.5 && colour.b < 0.5)
					return 5;
				else if (colour.r < 0.5 && colour.g > 0.5 && colour.b < 0.5)
					return 6;
				else if (colour.r < 0.5 && colour.g < 0.5 && colour.b > 0.5)
					return 7;
				else
					return 8;
			}
		}

		fixed4 SampleAtPosition(int type, float3 pos)
		{
			if (type == 0)
				return tex2D(_Tex100, pos.xy * _Tex100_ST.xy + _Tex100_ST.zw) * _Col100;
			else if (type == 1)
				return tex2D(_Tex75, pos.xy * _Tex75_ST.xy + _Tex75_ST.zw) * _Col75;
			else if (type == 2)
				return tex2D(_Tex50, pos.xy * _Tex50_ST.xy + _Tex50_ST.zw) * _Col50;
			else if (type == 3)
				return tex2D(_Tex25, pos.xy * _Tex25_ST.xy + _Tex25_ST.zw) * _Col25;
			else if (type == 4)
				return tex2D(_Tex0, pos.xy * _Tex0_ST.xy + _Tex0_ST.zw) * _Col0;
			else
				return (1, 1, 1);
		}

		fixed4 SampleNormalAtPosition(int type, float3 pos)
		{
			if (type == 0)
				return tex2D(_Tex100_N, pos.xy * _Tex100_N_ST.xy + _Tex100_N_ST.zw);
			else if (type == 1)
				return tex2D(_Tex75_N, pos.xy * _Tex75_N_ST.xy + _Tex75_N_ST.zw);
			else if (type == 2)
				return tex2D(_Tex50_N, pos.xy * _Tex50_N_ST.xy + _Tex50_N_ST.zw);
			else if (type == 3)
				return tex2D(_Tex25_N, pos.xy * _Tex25_N_ST.xy + _Tex25_N_ST.zw);
			else if (type == 4)
				return tex2D(_Tex0_N, pos.xy * _Tex0_N_ST.xy + _Tex0_N_ST.zw);
			else
				return fixed4(1, 1, 1, 1);
		}

		void SampleMetallic(inout SurfaceOutputStandard o, int type)
		{
			if (type == 0)
				o.Metallic = _Metal100;
			else if (type == 1)
				o.Metallic = _Metal75;
			else if (type == 2)
				o.Metallic = _Metal50;
			else if (type == 3)
				o.Metallic = _Metal25;
			else if (type == 4)
				o.Metallic = _Metal0;
			else
				o.Metallic = 1;
		}

		void SampleSmoothness(inout SurfaceOutputStandard o, int type)
		{
			if (type == 0)
				o.Smoothness = _Smooth100;
			else if (type == 1)
				o.Smoothness = _Smooth75;
			else if (type == 2)
				o.Smoothness = _Smooth50;
			else if (type == 3)
				o.Smoothness = _Smooth25;
			else if (type == 4)
				o.Smoothness = _Smooth0;
			else
				o.Smoothness = 1;
		}

		void SampleGlow(inout SurfaceOutputStandard o, int type)
		{
			if (type == 5)
				o.Emission = _GlowR;
			else if (type == 6)
				o.Emission = _GlowG;
			else if (type == 7)
				o.Emission = _GlowB;
		}

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void vert(inout appdata_full v, out Input o) 
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);

			//o.info = Type(o.colour);
			o.localPos = v.vertex.xyz;
			
			//o.normal = v.normal.xyz;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			float t = Type(IN.colour);
			fixed4 c = SampleAtPosition(t, IN.localPos);///*tex2D (_MainTex, IN.uv_MainTex)*/IN.colour;// *_Color;
			o.Albedo = c.rgb;
			o.Normal = UnpackNormal(SampleNormalAtPosition(t, IN.localPos));
			SampleGlow(o, t);
			SampleMetallic(o, t);
			SampleSmoothness(o, t);		
		}
		ENDCG
	}
	FallBack "Diffuse"
}