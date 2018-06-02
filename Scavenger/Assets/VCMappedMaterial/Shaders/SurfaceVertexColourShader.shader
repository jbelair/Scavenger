Shader "Vertex Mapped/Colour/Triplanar Axial Surface" 
{
	Properties 
	{
		_Col100 ("1. Tint", Color) = (1,1,1,1)
		_Smooth100("1. Smoothness", Range(0,1)) = 0.5
		_Metal100("1. Metallic", Range(0,1)) = 0.0
		_Tex100 ("1. Material", 2D) = "white" {}
		[Normal]_Tex100_N("1. Normal", 2D) = "bump" {}
		
		_Col75("2. Tint", Color) = (1,1,1,1)
		_Smooth75("2. Smoothness", Range(0,1)) = 0.5
		_Metal75("2. Metallic", Range(0,1)) = 0.0
		_Tex75("2. Material", 2D) = "white" {}
		[Normal]_Tex75_N("2. Normal", 2D) = "bump" {}
		
		_Col50("3. Tint", Color) = (1,1,1,1)
		_Smooth50("3. Smoothness", Range(0,1)) = 0.5
		_Metal50("3. Metallic", Range(0,1)) = 0.0
		_Tex50("3. Material", 2D) = "white" {}
		[Normal]_Tex50_N("3. Normal", 2D) = "bump" {}
		
		_Col25("4. Tint", Color) = (1,1,1,1)
		_Smooth25("4. Smoothness", Range(0,1)) = 0.5
		_Metal25("4. Metallic", Range(0,1)) = 0.0
		_Tex25("4. Material", 2D) = "white" {}
		[Normal]_Tex25_N("4. Normal", 2D) = "bump" {}

		_Col0("5. Tint", Color) = (1,1,1,1)
		_Smooth0("5. Smoothness", Range(0,1)) = 0.5
		_Metal0("5. Metallic", Range(0,1)) = 0.0
		_Tex0("5. Material", 2D) = "white" {}
		[Normal]_Tex0_N("5. Normal", 2D) = "bump" {}

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

		// STANDARD ------------------------------------------------------------------------------------------------------
		struct Input 
		{
			float3 worldPos;
			float3 localPos;
			float3 normal;
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

		struct TriplanarUV 
		{
			float2 pos;
			float3 w;
		};

		float3 GetTriplanarWeights(float3 normal)
		{
			float3 triW = abs(normal);
			return triW / (triW.x + triW.y + triW.z);
		}

		float2 GetTriplanarUV(float3 position, float3 normal) 
		{
			TriplanarUV uv;
			//uv.w = GetTriplanarWeights(normal);
			//uv.pos = position.zy * uv.w.x + position.xz * uv.w.y + position.xy * uv.w.z;
			float3 n = abs(normal);

			if (n.z > 0.7)
			{
				n.x = 0;
				n.y = 0;
				n.z = 1;
				uv.pos = position.xy;
			}
			else if (n.z < -0.7)
			{
				n.x = 0;
				n.y = 0;
				n.z = -1;
				uv.pos = -position.xy;
			}
			else if (n.x > 0.7)
			{
				n.x = 1;
				n.y = 0;
				n.z = 0;
				uv.pos = position.zy;
			}
			else if (n.x < -0.7)
			{
				n.x = -1;
				n.y = 0;
				n.z = 0;
				uv.pos = -position.zy;
			}
			else if (n.y > 0.7)
			{
				n.x = 0;
				n.y = 1;
				n.z = 0;
				uv.pos = position.xz;
			}
			else if (n.y < -0.7)
			{
				n.x = 0;
				n.y = -1;
				n.z = 0;
				uv.pos = -position.xz;
			}
			
			return uv.pos;
		}

		fixed4 SampleAtPosition(int type, float2 pos)
		{
			if (type == 0)
				return tex2D(_Tex100, pos * _Tex100_ST.xy + _Tex100_ST.zw) * _Col100;
			else if (type == 1)
				return tex2D(_Tex75, pos * _Tex75_ST.xy + _Tex75_ST.zw) * _Col75;
			else if (type == 2)
				return tex2D(_Tex50, pos * _Tex50_ST.xy + _Tex50_ST.zw) * _Col50;
			else if (type == 3)
				return tex2D(_Tex25, pos * _Tex25_ST.xy + _Tex25_ST.zw) * _Col25;
			else if (type == 4)
				return tex2D(_Tex0, pos * _Tex0_ST.xy + _Tex0_ST.zw) * _Col0;
			else
				return (1, 1, 1);
		}

		fixed4 SampleNormalAtPosition(int type, float2 pos)
		{
			if (type == 0)
				return tex2D(_Tex100_N, pos * _Tex100_N_ST.xy + _Tex100_N_ST.zw);
			else if (type == 1)
				return tex2D(_Tex75_N, pos * _Tex75_N_ST.xy + _Tex75_N_ST.zw);
			else if (type == 2)
				return tex2D(_Tex50_N, pos * _Tex50_N_ST.xy + _Tex50_N_ST.zw);
			else if (type == 3)
				return tex2D(_Tex25_N, pos * _Tex25_N_ST.xy + _Tex25_N_ST.zw);
			else if (type == 4)
				return tex2D(_Tex0_N, pos * _Tex0_N_ST.xy + _Tex0_N_ST.zw);
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
			
			o.normal = v.normal.xyz;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			float t = Type(IN.colour);
			//TriplanarUV uv = GetTriplanarUV(IN.localPos, IN.normal);
			float2 uv = GetTriplanarUV(IN.localPos, IN.normal);
			fixed4 c = SampleAtPosition(t, uv);///*tex2D (_MainTex, IN.uv_MainTex)*/IN.colour;// *_Color;
			o.Albedo = c.rgb;
			o.Normal = UnpackNormal(SampleNormalAtPosition(t, uv));
			SampleGlow(o, t);
			SampleMetallic(o, t);
			SampleSmoothness(o, t);		
		}
		ENDCG
	}
	FallBack "Diffuse"
}