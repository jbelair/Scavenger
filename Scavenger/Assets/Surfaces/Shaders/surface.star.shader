Shader "Surfaces/Star" 
{
	Properties 
	{
		_Kelvin("Temperature (K)", Range(1,100000)) = 3000
		_KelvinRange("Range (K)", Range(1,100000)) = 500
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
			float3 localPos;
			float3 texturePos;
		};

		//
		// Noise Shader Library for Unity - https://github.com/keijiro/NoiseShader
		//
		// Original work (webgl-noise) Copyright (C) 2011 Stefan Gustavson
		// Translation and modification was made by Keijiro Takahashi.
		//
		// This shader is based on the webgl-noise GLSL shader. For further details
		// of the original shader, please see the following description from the
		// original source code.
		//

		//
		// GLSL textureless classic 3D noise "cnoise",
		// with an RSL-style periodic variant "pnoise".
		// Author:  Stefan Gustavson (stefan.gustavson@liu.se)
		// Version: 2011-10-11
		//
		// Many thanks to Ian McEwan of Ashima Arts for the
		// ideas for permutation and gradient selection.
		//
		// Copyright (c) 2011 Stefan Gustavson. All rights reserved.
		// Distributed under the MIT license. See LICENSE file.
		// https://github.com/ashima/webgl-noise
		//
		float3 mod(float3 x, float3 y)
		{
			return x - y * floor(x / y);
		}

		float3 mod289(float3 x)
		{
			return x - floor(x / 289.0) * 289.0;
		}

		float4 mod289(float4 x)
		{
			return x - floor(x / 289.0) * 289.0;
		}

		float4 permute(float4 x)
		{
			return mod289(((x*34.0) + 1.0)*x);
		}

		float4 taylorInvSqrt(float4 r)
		{
			return (float4)1.79284291400159 - r * 0.85373472095314;
		}

		float3 fade(float3 t) {
			return t*t*t*(t*(t*6.0 - 15.0) + 10.0);
		}

		// Classic Perlin noise
		float cnoise(float3 P)
		{
			float3 Pi0 = floor(P); // Integer part for indexing
			float3 Pi1 = Pi0 + (float3)1.0; // Integer part + 1
			Pi0 = mod289(Pi0);
			Pi1 = mod289(Pi1);
			float3 Pf0 = frac(P); // Fractional part for interpolation
			float3 Pf1 = Pf0 - (float3)1.0; // Fractional part - 1.0
			float4 ix = float4(Pi0.x, Pi1.x, Pi0.x, Pi1.x);
			float4 iy = float4(Pi0.y, Pi0.y, Pi1.y, Pi1.y);
			float4 iz0 = (float4)Pi0.z;
			float4 iz1 = (float4)Pi1.z;

			float4 ixy = permute(permute(ix) + iy);
			float4 ixy0 = permute(ixy + iz0);
			float4 ixy1 = permute(ixy + iz1);

			float4 gx0 = ixy0 / 7.0;
			float4 gy0 = frac(floor(gx0) / 7.0) - 0.5;
			gx0 = frac(gx0);
			float4 gz0 = (float4)0.5 - abs(gx0) - abs(gy0);
			float4 sz0 = step(gz0, (float4)0.0);
			gx0 -= sz0 * (step((float4)0.0, gx0) - 0.5);
			gy0 -= sz0 * (step((float4)0.0, gy0) - 0.5);

			float4 gx1 = ixy1 / 7.0;
			float4 gy1 = frac(floor(gx1) / 7.0) - 0.5;
			gx1 = frac(gx1);
			float4 gz1 = (float4)0.5 - abs(gx1) - abs(gy1);
			float4 sz1 = step(gz1, (float4)0.0);
			gx1 -= sz1 * (step((float4)0.0, gx1) - 0.5);
			gy1 -= sz1 * (step((float4)0.0, gy1) - 0.5);

			float3 g000 = float3(gx0.x, gy0.x, gz0.x);
			float3 g100 = float3(gx0.y, gy0.y, gz0.y);
			float3 g010 = float3(gx0.z, gy0.z, gz0.z);
			float3 g110 = float3(gx0.w, gy0.w, gz0.w);
			float3 g001 = float3(gx1.x, gy1.x, gz1.x);
			float3 g101 = float3(gx1.y, gy1.y, gz1.y);
			float3 g011 = float3(gx1.z, gy1.z, gz1.z);
			float3 g111 = float3(gx1.w, gy1.w, gz1.w);

			float4 norm0 = taylorInvSqrt(float4(dot(g000, g000), dot(g010, g010), dot(g100, g100), dot(g110, g110)));
			g000 *= norm0.x;
			g010 *= norm0.y;
			g100 *= norm0.z;
			g110 *= norm0.w;

			float4 norm1 = taylorInvSqrt(float4(dot(g001, g001), dot(g011, g011), dot(g101, g101), dot(g111, g111)));
			g001 *= norm1.x;
			g011 *= norm1.y;
			g101 *= norm1.z;
			g111 *= norm1.w;

			float n000 = dot(g000, Pf0);
			float n100 = dot(g100, float3(Pf1.x, Pf0.y, Pf0.z));
			float n010 = dot(g010, float3(Pf0.x, Pf1.y, Pf0.z));
			float n110 = dot(g110, float3(Pf1.x, Pf1.y, Pf0.z));
			float n001 = dot(g001, float3(Pf0.x, Pf0.y, Pf1.z));
			float n101 = dot(g101, float3(Pf1.x, Pf0.y, Pf1.z));
			float n011 = dot(g011, float3(Pf0.x, Pf1.y, Pf1.z));
			float n111 = dot(g111, Pf1);

			float3 fade_xyz = fade(Pf0);
			float4 n_z = lerp(float4(n000, n100, n010, n110), float4(n001, n101, n011, n111), fade_xyz.z);
			float2 n_yz = lerp(n_z.xy, n_z.zw, fade_xyz.y);
			float n_xyz = lerp(n_yz.x, n_yz.y, fade_xyz.x);
			return 2.2 * n_xyz;
		}

		// Classic Perlin noise, periodic variant
		float pnoise(float3 P, float3 rep)
		{
			float3 Pi0 = mod(floor(P), rep); // Integer part, modulo period
			float3 Pi1 = mod(Pi0 + (float3)1.0, rep); // Integer part + 1, mod period
			Pi0 = mod289(Pi0);
			Pi1 = mod289(Pi1);
			float3 Pf0 = frac(P); // Fractional part for interpolation
			float3 Pf1 = Pf0 - (float3)1.0; // Fractional part - 1.0
			float4 ix = float4(Pi0.x, Pi1.x, Pi0.x, Pi1.x);
			float4 iy = float4(Pi0.y, Pi0.y, Pi1.y, Pi1.y);
			float4 iz0 = (float4)Pi0.z;
			float4 iz1 = (float4)Pi1.z;

			float4 ixy = permute(permute(ix) + iy);
			float4 ixy0 = permute(ixy + iz0);
			float4 ixy1 = permute(ixy + iz1);

			float4 gx0 = ixy0 / 7.0;
			float4 gy0 = frac(floor(gx0) / 7.0) - 0.5;
			gx0 = frac(gx0);
			float4 gz0 = (float4)0.5 - abs(gx0) - abs(gy0);
			float4 sz0 = step(gz0, (float4)0.0);
			gx0 -= sz0 * (step((float4)0.0, gx0) - 0.5);
			gy0 -= sz0 * (step((float4)0.0, gy0) - 0.5);

			float4 gx1 = ixy1 / 7.0;
			float4 gy1 = frac(floor(gx1) / 7.0) - 0.5;
			gx1 = frac(gx1);
			float4 gz1 = (float4)0.5 - abs(gx1) - abs(gy1);
			float4 sz1 = step(gz1, (float4)0.0);
			gx1 -= sz1 * (step((float4)0.0, gx1) - 0.5);
			gy1 -= sz1 * (step((float4)0.0, gy1) - 0.5);

			float3 g000 = float3(gx0.x, gy0.x, gz0.x);
			float3 g100 = float3(gx0.y, gy0.y, gz0.y);
			float3 g010 = float3(gx0.z, gy0.z, gz0.z);
			float3 g110 = float3(gx0.w, gy0.w, gz0.w);
			float3 g001 = float3(gx1.x, gy1.x, gz1.x);
			float3 g101 = float3(gx1.y, gy1.y, gz1.y);
			float3 g011 = float3(gx1.z, gy1.z, gz1.z);
			float3 g111 = float3(gx1.w, gy1.w, gz1.w);

			float4 norm0 = taylorInvSqrt(float4(dot(g000, g000), dot(g010, g010), dot(g100, g100), dot(g110, g110)));
			g000 *= norm0.x;
			g010 *= norm0.y;
			g100 *= norm0.z;
			g110 *= norm0.w;
			float4 norm1 = taylorInvSqrt(float4(dot(g001, g001), dot(g011, g011), dot(g101, g101), dot(g111, g111)));
			g001 *= norm1.x;
			g011 *= norm1.y;
			g101 *= norm1.z;
			g111 *= norm1.w;

			float n000 = dot(g000, Pf0);
			float n100 = dot(g100, float3(Pf1.x, Pf0.y, Pf0.z));
			float n010 = dot(g010, float3(Pf0.x, Pf1.y, Pf0.z));
			float n110 = dot(g110, float3(Pf1.x, Pf1.y, Pf0.z));
			float n001 = dot(g001, float3(Pf0.x, Pf0.y, Pf1.z));
			float n101 = dot(g101, float3(Pf1.x, Pf0.y, Pf1.z));
			float n011 = dot(g011, float3(Pf0.x, Pf1.y, Pf1.z));
			float n111 = dot(g111, Pf1);

			float3 fade_xyz = fade(Pf0);
			float4 n_z = lerp(float4(n000, n100, n010, n110), float4(n001, n101, n011, n111), fade_xyz.z);
			float2 n_yz = lerp(n_z.xy, n_z.zw, fade_xyz.y);
			float n_xyz = lerp(n_yz.x, n_yz.y, fade_xyz.x);
			return 2.2 * n_xyz;
		}

		float SampleKelvin(float3 colour)
		{
			return clamp(_Kelvin + (dot(colour, fixed3(0.33, 0.56, 0.1)) - 0.5) * 2 * _KelvinRange, _Kelvin - _KelvinRange, _Kelvin + _KelvinRange);
		}

		float3 SampleAtKelvinEmission(fixed3 emission, float3 uv)
		{
			fixed3 c = fixed3(1, 1, 1);

			float k = clamp(_Kelvin + (dot(emission, fixed3(0.33, 0.56, 0.1)) - 0.5) * 2 * _KelvinRange, KELVIN_LOW, KELVIN_HIGH);

			if (k < M)
			{
				if (k < Y)
				{
					c = tex2D(_TextureY, uv.xy * _TextureY_ST.xy + _TextureY_ST.zw).rgb;
				}
				else
				{
					c = lerp(tex2D(_TextureY, uv.xy * _TextureY_ST.xy + _TextureY_ST.zw).rgb, tex2D(_TextureM, uv.xy * _TextureM_ST.xy + _TextureM_ST.zw).rgb, (k - Y) / (M - Y));
				}
			}
			else if (k < K)
			{
				c = lerp(tex2D(_TextureM, uv.xy * _TextureM_ST.xy + _TextureM_ST.zw).rgb, tex2D(_TextureK, uv.xy * _TextureK_ST.xy + _TextureK_ST.zw).rgb, (k - M) / (K - M));
			}
			else if (k < G)
			{
				c = lerp(tex2D(_TextureK, uv.xy * _TextureK_ST.xy + _TextureK_ST.zw).rgb, tex2D(_TextureG, uv.xy * _TextureG_ST.xy + _TextureG_ST.zw).rgb, (k - K) / (G - K));
			}
			else if (k < F)
			{
				c = lerp(tex2D(_TextureG, uv.xy * _TextureG_ST.xy + _TextureG_ST.zw).rgb, tex2D(_TextureF, uv.xy * _TextureF_ST.xy + _TextureF_ST.zw).rgb, (k - G) / (F - G));
			}
			else if (k < A)
			{
				c = lerp(tex2D(_TextureF, uv.xy * _TextureF_ST.xy + _TextureF_ST.zw).rgb, tex2D(_TextureA, uv.xy * _TextureA_ST.xy + _TextureA_ST.zw).rgb, (k - F) / (A - F));
			}
			else if (k < B)
			{
				c = lerp(tex2D(_TextureA, uv.xy * _TextureA_ST.xy + _TextureA_ST.zw).rgb, tex2D(_TextureB, uv.xy * _TextureB_ST.xy + _TextureB_ST.zw).rgb, (k - A) / (B - A));
			}
			else
			{
				c = lerp(tex2D(_TextureB, uv.xy * _TextureB_ST.xy + _TextureB_ST.zw).rgb, tex2D(_TextureO, uv.xy * _TextureO_ST.xy + _TextureO_ST.zw).rgb, (k - B) / (O - B));
			}

			k = SampleKelvin(c);

			float2 kX = float2(k / KELVIN_HIGH, 0);
			c = lerp(tex2D(_Emissive, kX), c * tex2D(_Emissive, kX), uv.z).rgb * 8;

			return c;
		}

		float3 SampleAtKelvin(float3 uv, float3 localPos)
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
					c = lerp(tex2D(_TextureY, uv * _TextureY_ST.xy + _TextureY_ST.zw).rgb, tex2D(_TextureM, uv * _TextureM_ST.xy + _TextureM_ST.zw).rgb, (k - Y) / (M - Y));
				}
			}
			else if (k < K)
			{
				c = lerp(tex2D(_TextureM, uv * _TextureM_ST.xy + _TextureM_ST.zw).rgb, tex2D(_TextureK, uv * _TextureK_ST.xy + _TextureK_ST.zw).rgb, (k - M) / (K - M));
			}
			else if (k < G)
			{
				c = lerp(tex2D(_TextureK, uv * _TextureK_ST.xy + _TextureK_ST.zw).rgb, tex2D(_TextureG, uv * _TextureG_ST.xy + _TextureG_ST.zw).rgb, (k - K) / (G - K));
			}
			else if (k < F)
			{
				c = lerp(tex2D(_TextureG, uv * _TextureG_ST.xy + _TextureG_ST.zw).rgb, tex2D(_TextureF, uv * _TextureF_ST.xy + _TextureF_ST.zw).rgb, (k - G) / (F - G));
			}
			else if (k < A)
			{
				c = lerp(tex2D(_TextureF, uv * _TextureF_ST.xy + _TextureF_ST.zw).rgb, tex2D(_TextureA, uv * _TextureA_ST.xy + _TextureA_ST.zw).rgb, (k - F) / (A - F));
			}
			else if (k < B)
			{
				c = lerp(tex2D(_TextureA, uv * _TextureA_ST.xy + _TextureA_ST.zw).rgb, tex2D(_TextureB, uv * _TextureB_ST.xy + _TextureB_ST.zw).rgb, (k - A) / (B - A));
			}
			else
			{
				c = lerp(tex2D(_TextureB, uv * _TextureB_ST.xy + _TextureB_ST.zw).rgb, tex2D(_TextureO, uv * _TextureO_ST.xy + _TextureO_ST.zw).rgb, (k - B) / (O - B));
			}

			float l = pow(_Kelvin / KELVIN_HIGH, 0.1) * 20;//log10(_Kelvin);
			//k = clamp(SampleKelvin(c) * cnoise(localPos.xyz * l + float3(_Time.x,0,_Time.x) * l), _Kelvin - _KelvinRange, _Kelvin + _KelvinRange);
			k = SampleKelvin(c) + _KelvinRange * (cnoise(localPos.xyz * l + float3(0, _Time.x, 0) * l) - 0.5) * 2;
			//k = SampleKelvin(c) + _KelvinRange * (cnoise(float3(uv.xy,_Time.x) * l) - 0.5) * 2;

			//c *= tex2D(_Emissive, float2(k / KELVIN_HIGH, 0));
			float2 kX = float2(k / KELVIN_HIGH, 0);
			c = lerp(tex2D(_Emissive, kX), c * tex2D(_Emissive, kX), uv.z).rgb * 8;

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
			o.localPos = v.vertex.xyz;
			o.texturePos = float3(v.texcoord.xy, pow(1-abs(v.vertex.y), 3));
		}

		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			// Albedo comes from a texture tinted by color
			//o.Albedo = SampleAtKelvin(IN.localPos);//*/IN.uv_TextureY);
			o.Albedo = float3(0, 0, 0);
			//o.Emission = SampleAtKelvin(IN.localPos);
			float4 pD = float4(1 - pow(_Kelvin / KELVIN_HIGH, 0.01), pow(_Kelvin / KELVIN_HIGH, 0.5) * 64, _Kelvin / KELVIN_HIGH * 128, pow(_Kelvin / KELVIN_HIGH, 2) * 256);
			float p = cnoise(IN.localPos * float3(0, 8, 0) + float3(_Time.x, 0, 0)) * pD.x;
			p += cnoise(IN.localPos * pD.y + float3(0, _Time.x * pD.y / 8, 0)) * pD.y / 64;
			p += cnoise(IN.localPos * pD.z + float3(0, _Time.x * pD.z / 8, 0)) * pD.z / 128;
			p += cnoise(IN.localPos * pD.w + float3(0, _Time.x * pD.w / 8, 0)) * pD.w / 256;
			//p *= 8;
			float3 c = tex2D(_Emissive, float2((p * _KelvinRange + _Kelvin) / KELVIN_HIGH, 0)) * 8;//SampleAtKelvin(IN.texturePos, IN.localPos);// *pow(2, log10(_Kelvin));// +float3(0.1, 0.1, 0) * cnoise(IN.localPos + _Time.x));
			//float k = pow(SampleKelvin(c) / KELVIN_HIGH, 0.1);// / log10(_Kelvin);// *10;
			//float3 kX = float3(_CosTime.w * k * _SinTime.x, _SinTime.w * k * _CosTime.y, 0);
			o.Emission = c;//SampleAtKelvinEmission(c, IN.texturePos);// +float3(k, k, 0) * cnoise(float3(IN.texturePos.xy, _Time.x) * pow(2, log10(_Kelvin))) - float3(-0.5, -0.5, 0));// +float3(0.1, 0.1, 0) * cnoise(IN.localPos + _Time.x));// +kX);
			//o.Emission = SampleAtKelvinEmission(o.Albedo, IN.localPos);//*/IN.uv_TextureY);
			// Metallic and smoothness come from slider variables
			o.Metallic = 1;//_Metallic;
			o.Smoothness = 0;//_Glossiness;
			o.Alpha = 1;//c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
