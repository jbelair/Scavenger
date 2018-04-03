Shader "Surfaces/Star" 
{
	Properties 
	{
		_Kelvin("Temperature (K)", Range(100,100000)) = 3000
		_KelvinRange("Range (K)", Range(100,20000)) = 500
		_Emissive("Colour Map", 2D) = "white" {}
		_TextureY("500 Kelvin (RGB)", 2D) = "white" {}
		_TextureM("3000 Kelvin (RGB)", 2D) = "white" {}
		_TextureK("5000 Kelvin (RGB)", 2D) = "white" {}
		_TextureG("6000 Kelvin (RGB)", 2D) = "white" {}
		_TextureF("7500 Kelvin (RGB)", 2D) = "white" {}
		_TextureA("10000 Kelvin (RGB)", 2D) = "white" {}
		_TextureB("30000 Start (RGB)", 2D) = "white" {}
		_TextureO("30000+ Start (RGB)", 2D) = "white" {}
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		static const float KELVIN_LOW = 0;
		static const float KELVIN_HIGH = 100000;
		static const float Y = 500;
		static const float M = 3000;
		static const float K = 5000;
		static const float G = 6000;
		static const float F = 7500;
		static const float A = 10000;
		static const float B = 30000;
		static const float O = 100000;

		float _Kelvin;
		float _KelvinRange;
		sampler2D _Emissive;
		sampler2D _TextureY;
		fixed4 _TextureY_ST;
		sampler2D _TextureM;
		fixed4 _TextureM_ST;
		sampler2D _TextureK;
		fixed4 _TextureK_ST;
		sampler2D _TextureG;
		fixed4 _TextureG_ST;
		sampler2D _TextureF;
		fixed4 _TextureF_ST;
		sampler2D _TextureA;
		fixed4 _TextureA_ST;
		sampler2D _TextureB;
		fixed4 _TextureB_ST;
		sampler2D _TextureO;
		fixed4 _TextureO_ST;

		struct Input 
		{
			//float2 uv_TextureY;
			float3 localPos;
		};

		fixed3 SampleAtKelvinEmission(fixed3 emission, float3 uv)
		{
			fixed3 c = fixed3(1, 1, 1);

			float k = _Kelvin + clamp((dot(emission, fixed3(0.33, 0.56, 0.1)) - 0.5) * 2 * _KelvinRange, KELVIN_LOW, KELVIN_HIGH);

			if (k < M)
			{
				if (k < Y)
				{
					c = tex2D(_TextureY, uv.xy * _TextureY_ST.xy + _TextureY_ST.zw).rgb;
				}
				else
				{
					c = lerp(tex2D(_TextureY, uv.xy * _TextureY_ST.xy + _TextureY_ST.zw).rgb, tex2D(_TextureM, uv.xy * _TextureM_ST.xy + _TextureM_ST.zw).rgb, (k - Y) / M);
				}
			}
			else if (k < K)
			{
				c = lerp(tex2D(_TextureM, uv.xy * _TextureM_ST.xy + _TextureM_ST.zw).rgb, tex2D(_TextureK, uv.xy * _TextureK_ST.xy + _TextureK_ST.zw).rgb, (k - M) / K);
			}
			else if (k < G)
			{
				c = lerp(tex2D(_TextureK, uv.xy * _TextureK_ST.xy + _TextureK_ST.zw).rgb, tex2D(_TextureG, uv.xy * _TextureG_ST.xy + _TextureG_ST.zw).rgb, (k - K) / G);
			}
			else if (k < F)
			{
				c = lerp(tex2D(_TextureG, uv.xy * _TextureG_ST.xy + _TextureG_ST.zw).rgb, tex2D(_TextureF, uv.xy * _TextureF_ST.xy + _TextureF_ST.zw).rgb, (k - G) / F);
			}
			else if (k < A)
			{
				c = lerp(tex2D(_TextureF, uv.xy * _TextureF_ST.xy + _TextureF_ST.zw).rgb, tex2D(_TextureA, uv.xy * _TextureA_ST.xy + _TextureA_ST.zw).rgb, (k - F) / A);
			}
			else if (k < B)
			{
				c = lerp(tex2D(_TextureA, uv.xy * _TextureA_ST.xy + _TextureA_ST.zw).rgb, tex2D(_TextureB, uv.xy * _TextureB_ST.xy + _TextureB_ST.zw).rgb, (k - A) / B);
			}
			else
			{
				c = lerp(tex2D(_TextureB, uv.xy * _TextureB_ST.xy + _TextureB_ST.zw).rgb, tex2D(_TextureO, uv.xy * _TextureO_ST.xy + _TextureO_ST.zw).rgb, (k - B) / O);
			}

			float2 kX = float2(k / KELVIN_HIGH, 0);
			c = lerp(tex2D(_Emissive, kX), c * tex2D(_Emissive, kX), uv.z).rgb * 8;

			return c;
		}

		fixed3 SampleAtKelvin(float2 uv)
		{
			fixed3 c = fixed3(1, 1, 1);

			float k = _Kelvin;

			if (k < M)
			{
				if (k < Y)
				{
					c = tex2D(_TextureY, uv * _TextureY_ST.xy + _TextureY_ST.zw).rgb;
				}
				else
				{
					c = lerp(tex2D(_TextureY, uv * _TextureY_ST.xy + _TextureY_ST.zw).rgb, tex2D(_TextureM, uv * _TextureM_ST.xy + _TextureM_ST.zw).rgb, (k - Y) / M);
				}
			}
			else if (k < K)
			{
				c = lerp(tex2D(_TextureM, uv * _TextureM_ST.xy + _TextureM_ST.zw).rgb, tex2D(_TextureK, uv * _TextureK_ST.xy + _TextureK_ST.zw).rgb, (k - M) / K);
			}
			else if (k < G)
			{
				c = lerp(tex2D(_TextureK, uv * _TextureK_ST.xy + _TextureK_ST.zw).rgb, tex2D(_TextureG, uv * _TextureG_ST.xy + _TextureG_ST.zw).rgb, (k - K) / G);
			}
			else if (k < F)
			{
				c = lerp(tex2D(_TextureG, uv * _TextureG_ST.xy + _TextureG_ST.zw).rgb, tex2D(_TextureF, uv * _TextureF_ST.xy + _TextureF_ST.zw).rgb, (k - G) / F);
			}
			else if (k < A)
			{
				c = lerp(tex2D(_TextureF, uv * _TextureF_ST.xy + _TextureF_ST.zw).rgb, tex2D(_TextureA, uv * _TextureA_ST.xy + _TextureA_ST.zw).rgb, (k - F) / A);
			}
			else if (k < B)
			{
				c = lerp(tex2D(_TextureA, uv * _TextureA_ST.xy + _TextureA_ST.zw).rgb, tex2D(_TextureB, uv * _TextureB_ST.xy + _TextureB_ST.zw).rgb, (k - A) / B);
			}
			else
			{
				c = lerp(tex2D(_TextureB, uv * _TextureB_ST.xy + _TextureB_ST.zw).rgb, tex2D(_TextureO, uv * _TextureO_ST.xy + _TextureO_ST.zw).rgb, (k - B) / O);
			}

			c *= tex2D(_Emissive, float2(k / KELVIN_HIGH, 0));

			return c;
		}

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		/*fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			fixed4 c;
			c.rgb = s.Albedo;
			c.a = s.Alpha;
			return c;
		}*/

		void vert(inout appdata_full v, out Input o) 
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.localPos = float3(v.texcoord.xy, pow(1-abs(v.vertex.y), 3));//v.vertex.xy;//v.texcoord;//v.vertex.xyz;//normalize(v.vertex.xyz);//v.normal.xyz;//v.vertex.xyz - (dot(v.normal.xyz, (v.vertex.xyz - v.normal.xyz))) * v.normal.xyz;
		}

		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			// Albedo comes from a texture tinted by color
			o.Albedo = SampleAtKelvin(IN.localPos);//*/IN.uv_TextureY);
			o.Emission = SampleAtKelvinEmission(o.Albedo, IN.localPos);//*/IN.uv_TextureY);
			// Metallic and smoothness come from slider variables
			o.Metallic = 0;//_Metallic;
			o.Smoothness = 0;//_Glossiness;
			o.Alpha = 1;//c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
