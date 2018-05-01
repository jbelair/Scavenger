Shader "Surfaces/Star"
{
	Properties
	{
		_AtmosphereRadius("Atmosphere Radius", Float) = 1
		_AtmosphereDensity("Atmosphere Density", Float) = 2
		_AtmosphereIntensity("Atmosphere Intensity", Float) = 1
		_PlanetRadius("Planet Radius", Float) = 0.95
		_PlanetDensity("Planet Density", Float) = 1
		_PlanetCentre("Planet Centre", Vector) = (0,0,0,1)
		[NoScaleOffset]_AtmosphereMap("Atmosphere Map", 2D) = "black" {}
		_Phase("Atmosphere Phase", Vector) = (0.75,0.825,1,1)

		_Kelvin("Temperature (K)", Range(1,100000)) = 3000
		_KelvinRange("Range (K)", Range(1,100000)) = 500
		_KelvinMax("Maximum (K)", Float) = 100000
		_HDR("Emissive HDR Intensity", Range(1,8)) = 8
		[NoScaleOffset]_Emissive("Colour Map", 2D) = "white" {}
		_Spin("Spin", Float) = 0.01
		_Turbulence("Turbulence", Float) = 0.01
		_Octaves("Turbulence Octaves", Int) = 2
		_Scattering("Gas Scattering", Range(0,1)) = 0.1
		_Texture("Gas Giant (RGB)", 2D) = "white" {}
		_Gasses("Gas Tint Map (RGB)", 2D) = "white" {}
		_TextureY("500 Kelvin (RGB)", 2D) = "white" {}
		_TextureL("2000 Kelvin (RGB)", 2D) = "white" {}
		_TextureM("3000 Kelvin (RGB)", 2D) = "white" {}
		_TextureF("7500 Kelvin (RGB)", 2D) = "white" {}
		_TextureO("30000+ Kelvin (RGB)", 2D) = "white" {}
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf WrapScattering vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 4.6

		static const float Y = 500;
		static const float L = 2000;
		static const float M = 3000;
		static const float F = 7500;
		static const float O = 100000;

		float _Kelvin;
		float _KelvinRange;
		float _KelvinMax;
		float _HDR;
		sampler1D _Emissive;
		sampler1D _Gasses;
		float _Scattering;
		float _Spin;
		float _Turbulence;
		int _Octaves;
		sampler2D _Texture;
		fixed4 _Texture_ST;
		sampler2D _TextureY;
		fixed4 _TextureY_ST;
		sampler2D _TextureL;
		fixed4 _TextureL_ST;
		sampler2D _TextureM;
		fixed4 _TextureM_ST;
		sampler2D _TextureF;
		fixed4 _TextureF_ST;
		sampler2D _TextureO;
		fixed4 _TextureO_ST;

		fixed4 _Phase;

		float time = 0;

		struct Input
		{
			float3 localPos;
			float3 texturePos;
			float3 viewDir;
			float3 normal;
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
		// Description : Array and textureless GLSL 2D/3D/4D simplex 
		//               noise functions.
		//      Author : Ian McEwan, Ashima Arts.
		//  Maintainer : stegu
		//     Lastmod : 20110822 (ijm)
		//     License : Copyright (C) 2011 Ashima Arts. All rights reserved.
		//               Distributed under the MIT License. See LICENSE file.
		//               https://github.com/ashima/webgl-noise
		//               https://github.com/stegu/webgl-noise
		// 
		// CONVERTED TO HLSL BY AIDAN DEARING

		float4 mod289(float4 x) {
			return x - floor(x * (1.0 / 289.0)) * 289.0;
		}

		float mod289(float x) {
			return x - floor(x * (1.0 / 289.0)) * 289.0;
		}

		float4 permute(float4 x) {
			return mod289(((x*34.0) + 1.0)*x);
		}

		float permute(float x) {
			return mod289(((x*34.0) + 1.0)*x);
		}

		float4 taylorInvSqrt(float4 r)
		{
			return 1.79284291400159 - 0.85373472095314 * r;
		}

		float taylorInvSqrt(float r)
		{
			return 1.79284291400159 - 0.85373472095314 * r;
		}

		float4 grad4(float j, float4 ip)
		{
			const float4 ones = float4(1.0, 1.0, 1.0, -1.0);
			float4 p, s;

			p.xyz = floor(frac(float3(j,j,j) * ip.xyz) * 7.0) * ip.z - 1.0;
			p.w = 1.5 - dot(abs(p.xyz), ones.xyz);
			s = float4(p < float4(0.0, 0.0, 0.0, 0.0));// p < float4(0.0, 0.0, 0.0, 0.0), p < float4(0.0, 0.0, 0.0, 0.0), p < float4(0.0, 0.0, 0.0, 0.0));
			p.xyz = p.xyz + (s.xyz*2.0 - 1.0) * s.www;

			return p;
		}

		// (sqrt(5) - 1)/4 = F4, used once below
		#define F4 0.309016994374947451

		float snoise(float4 v)
		{
			const float4  C = float4(0.138196601125011,  // (5 - sqrt(5))/20  G4
				0.276393202250021,  // 2 * G4
				0.414589803375032,  // 3 * G4
				-0.447213595499958); // -1 + 4 * G4

									 // First corner
			float4 i = floor(v + dot(v, float4(F4,F4,F4,F4)));
			float4 x0 = v - i + dot(i, C.xxxx);

			// Other corners

			// Rank sorting originally contributed by Bill Licea-Kane, AMD (formerly ATI)
			float4 i0;
			float3 isX = step(x0.yzw, x0.xxx);
			float3 isYZ = step(x0.zww, x0.yyz);
			//  i0.x = dot( isX, vec3( 1.0 ) );
			i0.x = isX.x + isX.y + isX.z;
			i0.yzw = 1.0 - isX;
			//  i0.y += dot( isYZ.xy, vec2( 1.0 ) );
			i0.y += isYZ.x + isYZ.y;
			i0.zw += 1.0 - isYZ.xy;
			i0.z += isYZ.z;
			i0.w += 1.0 - isYZ.z;

			// i0 now contains the unique values 0,1,2,3 in each channel
			float4 i3 = clamp(i0, 0.0, 1.0);
			float4 i2 = clamp(i0 - 1.0, 0.0, 1.0);
			float4 i1 = clamp(i0 - 2.0, 0.0, 1.0);

			//  x0 = x0 - 0.0 + 0.0 * C.xxxx
			//  x1 = x0 - i1  + 1.0 * C.xxxx
			//  x2 = x0 - i2  + 2.0 * C.xxxx
			//  x3 = x0 - i3  + 3.0 * C.xxxx
			//  x4 = x0 - 1.0 + 4.0 * C.xxxx
			float4 x1 = x0 - i1 + C.xxxx;
			float4 x2 = x0 - i2 + C.yyyy;
			float4 x3 = x0 - i3 + C.zzzz;
			float4 x4 = x0 + C.wwww;

			// Permutations
			i = mod289(i);
			float j0 = permute(permute(permute(permute(i.w) + i.z) + i.y) + i.x);
			float4 j1 = permute(permute(permute(permute(
				i.w + float4(i1.w, i2.w, i3.w, 1.0))
				+ i.z + float4(i1.z, i2.z, i3.z, 1.0))
				+ i.y + float4(i1.y, i2.y, i3.y, 1.0))
				+ i.x + float4(i1.x, i2.x, i3.x, 1.0));

			// Gradients: 7x7x6 points over a cube, mapped onto a 4-cross polytope
			// 7*7*6 = 294, which is close to the ring size 17*17 = 289.
			float4 ip = float4(1.0 / 294.0, 1.0 / 49.0, 1.0 / 7.0, 0.0);

			float4 p0 = grad4(j0, ip);
			float4 p1 = grad4(j1.x, ip);
			float4 p2 = grad4(j1.y, ip);
			float4 p3 = grad4(j1.z, ip);
			float4 p4 = grad4(j1.w, ip);

			// Normalise gradients
			float4 norm = taylorInvSqrt(float4(dot(p0, p0), dot(p1, p1), dot(p2, p2), dot(p3, p3)));
			p0 *= norm.x;
			p1 *= norm.y;
			p2 *= norm.z;
			p3 *= norm.w;
			p4 *= taylorInvSqrt(dot(p4, p4));

			// Mix contributions from the five corners
			float3 m0 = max(0.6 - float3(dot(x0, x0), dot(x1, x1), dot(x2, x2)), 0.0);
			float2 m1 = max(0.6 - float2(dot(x3, x3), dot(x4, x4)), 0.0);
			m0 = m0 * m0;
			m1 = m1 * m1;
			return 49.0 * (dot(m0*m0, float3(dot(p0, x0), dot(p1, x1), dot(p2, x2)))
				+ dot(m1*m1, float2(dot(p3, x3), dot(p4, x4))));

		}

		float snoise4(float4 pos, int octaves)
		{
			float noise = 0;//snoise(pos);
			for (int i = 0; i < octaves; i++)
			{
				noise += snoise(pos * pow(2.0, i));
			}
			noise /= octaves;
			return noise;
		}

		float SampleKelvin(float3 colour)
		{
			return clamp(_Kelvin + (dot(colour, fixed3(0.33, 0.56, 0.1)) - 0.5) * 2 * _KelvinRange, _Kelvin - _KelvinRange, _Kelvin + _KelvinRange);
		}

		float3 SampleAtKelvinEmission(fixed3 emission, float4 uv)
		{
			fixed3 c = fixed3(1, 1, 1);

			float k = clamp(_Kelvin + (dot(emission, fixed3(0.33, 0.56, 0.1)) - 0.5) * 2 * _KelvinRange, 0, _KelvinMax);

			if (k < L)
			{
				if (k < Y)
				{
					c = tex2D(_TextureY, uv.xy * _TextureY_ST.xy + _TextureY_ST.zw).rgb;
				}
				else
				{
					c = lerp(tex2D(_TextureY, uv.xy * _TextureY_ST.xy + _TextureY_ST.zw).rgb, tex2D(_TextureL, uv.xy * _TextureL_ST.xy + _TextureL_ST.zw).rgb, (k - Y) / (L - Y));
				}
			}
			else if (k < M)
			{
				c = lerp(tex2D(_TextureL, uv.xy * _TextureL_ST.xy + _TextureL_ST.zw).rgb, tex2D(_TextureM, uv.xy * _TextureM_ST.xy + _TextureM_ST.zw).rgb, (k - L) / (M - L));
			}
			else if (k < F)
			{
				c = lerp(tex2D(_TextureM, uv.xy * _TextureM_ST.xy + _TextureM_ST.zw).rgb, tex2D(_TextureF, uv.xy * _TextureF_ST.xy + _TextureF_ST.zw).rgb, (k - M) / (F - M));
			}
			else
			{
				c = lerp(tex2D(_TextureF, uv.xy * _TextureF_ST.xy + _TextureF_ST.zw).rgb, tex2D(_TextureO, uv.xy * _TextureO_ST.xy + _TextureO_ST.zw).rgb, (k - F) / (O - F));
			}

			k = SampleKelvin(c);

			float kX = k / _KelvinMax;
			c = lerp(tex1D(_Emissive, (_Kelvin + _KelvinRange) / _KelvinMax), c * tex1D(_Emissive, kX), min(clamp(uv.z,0,1), 1 - uv.w)).rgb * _HDR;

			return c;
		}

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		#include "UnityPBSLighting.cginc"
		inline fixed4 LightingWrapScattering(SurfaceOutputStandard s, fixed3 viewDir, UnityGI gi)
		{
			half NdotL = dot(s.Normal, gi.light.dir);
			half diff = NdotL * 0.5 + 0.5;
			half4 c = LightingStandard(s, viewDir, gi);
			
			float I = 1 - abs(dot(gi.light.dir, s.Normal));
			I *= pow(abs(min(0, dot(viewDir, gi.light.dir))), 4.0);
			I = saturate(pow(I, 2));

			half VdotL = dot(s.Normal, gi.light.dir);
			float scat = saturate(VdotL + 1) + I;
			c.rgb = (c + s.Albedo * gi.light.color * float3(pow(diff, _Phase.r), pow(diff, _Phase.g), pow(diff, _Phase.b))) / 2;
			c.a = s.Alpha;

			return c;
		}

		void LightingWrapScattering_GI(SurfaceOutputStandard s, UnityGIInput data, inout UnityGI gi)
		{
			LightingStandard_GI(s, data, gi);
		}

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.localPos = v.vertex.xyz;
			o.texturePos = float3(v.texcoord.xy, 1 - pow(abs(v.vertex.y) * 2, 8));
			o.normal = v.normal;
		}

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			float4 pD = float4(1 - pow(_Kelvin / _KelvinMax, 0.1), pow(_Kelvin / _KelvinMax, 0.5) * 128, _Kelvin / _KelvinMax * 256, pow(_Kelvin / _KelvinMax, 2) * 512);

			float3 rgbNoise = float3(0, 0, 0);

			if (_Turbulence > 0)
			{
				rgbNoise = float3(snoise4(float4(IN.localPos * 16, _Time.x * _Spin), _Octaves), snoise4(float4(IN.localPos * 8, _Time.x * _Spin), _Octaves), snoise4(float4(IN.localPos * 128, _Time.x * _Spin), _Octaves)) * _Turbulence;
				rgbNoise.r = cos((rgbNoise.r - 0.5) * 6.2831853);
				rgbNoise.g = sin((rgbNoise.g - 0.5) * 6.2831853);
				rgbNoise.b = (rgbNoise.b - 0.5);
			}

			float freznel = saturate(dot(normalize(IN.viewDir), IN.normal));
			float4 sampleAt = float4(IN.texturePos, lerp(0, pow(freznel, pD.x), pD.x));
			sampleAt += float4(rgbNoise.r * rgbNoise.b, rgbNoise.g * rgbNoise.b, 0, 0);
			sampleAt += float4(_Time.x * _Spin, 0, 0, 0);

			float3 albedo = tex2D(_Texture, sampleAt.xy * _Texture_ST.xy + _Texture_ST.zw);
			// magnitude of 1,1,1 is √3 ~ 1.732051 (1.7320508...)
			float3 temperature = lerp(1, dot(albedo, float3(0.33, 0.56, 0.11)) / (8 * 1.732051), 1 - pD.x);
			// magnitude of 1,1,1 is √3 ~ 1.732051 (1.7320508...)
			o.Albedo = temperature * lerp((tex1D(_Gasses, length(albedo) / 1.732051) + tex1D(_Gasses, 1) * freznel + tex1D(_Gasses, 1) * IN.texturePos.z) / 2, tex1D(_Gasses, 1), 1 - IN.texturePos.z);
			o.Emission = SampleAtKelvinEmission(albedo, sampleAt);
			o.Emission = o.Emission + float3(pD.x, pD.x, pD.x) * o.Albedo * (1 - freznel) * _Scattering;

			o.Alpha = 1;
		}
		ENDCG

		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		LOD 200
		Cull Back
		Blend One One

		CGPROGRAM
		#pragma surface surf WrapScattering vertex:vert

		float _AtmosphereRadius;
		float _AtmosphereDensity;
		float _AtmosphereIntensity;
		float _PlanetRadius;
		float _PlanetDensity;
		float4 _PlanetCentre;

		sampler2D _AtmosphereMap;

		float3 _Phase;

		bool rayIntersect
		(
			// Ray
			float3 O, // Origin
			float3 D, // Direction

					  // Sphere
			float3 C, // Centre
			float R,	// Radius
			out float AO, // First intersection time
			out float BO  // Second intersection time
		)
		{
			float3 L = C - O;
			float DT = dot(L, D);
			float R2 = R * R;

			float CT2 = dot(L, L) - DT*DT;

			// Intersection point outside the circle
			if (CT2 > R2)
				return false;

			float AT = sqrt(R2 - CT2);
			float BT = AT;

			AO = DT - AT;
			BO = DT + BT;
			return true;
		}

		#include "UnityPBSLighting.cginc"
		inline fixed4 LightingWrapScattering(SurfaceOutputStandard s, fixed3 viewDir, UnityGI gi)
		{
			half4 c;

			float density = 0;
			float directionality = 0;

			float entry = 0;
			float exit = 0;
			float scattering = 0;
			if (rayIntersect(s.Normal, -viewDir, _PlanetCentre, _AtmosphereRadius, entry, exit));
			{
				scattering = exit - entry;
			}
			scattering = saturate(scattering);


			float entryP = 0;
			float exitP = 0;
			float planet = 0;
			if (rayIntersect(s.Normal, -viewDir, _PlanetCentre, _PlanetRadius / (_AtmosphereRadius + (_AtmosphereRadius - _PlanetRadius)), entryP, exitP));
			{
				planet = exitP - entryP;
			}
			planet = saturate(planet);

			density = abs(scattering - planet * _PlanetDensity);
			density = pow(density, _AtmosphereDensity);
			density = max(density, 1 * dot(viewDir, -gi.light.dir) * (saturate(dot(s.Normal, gi.light.dir))) * abs(dot(s.Normal, viewDir)));

			float entryD = 0;
			float exitD = 0;

			float3 p = s.Normal + -viewDir;

			rayIntersect(p, -gi.light.dir, _PlanetCentre, _AtmosphereRadius, entryD, exitD);

			directionality = dot(s.Normal, -gi.light.dir);

			if (directionality > 0)
				directionality = pow(directionality, _AtmosphereDensity);
			else
				directionality = pow(abs(directionality), _AtmosphereDensity) * -1;

			//directionality = pow(directionality / 2 + 0.5, _AtmosphereDensity);
			directionality = directionality / 2 + 0.5;

			c.rgb = gi.light.color.rgb * tex2D(_AtmosphereMap, float2(density, directionality)) * _AtmosphereIntensity;

			c.a = s.Alpha;

			return c;
		}

		void LightingWrapScattering_GI(SurfaceOutputStandard s, UnityGIInput data, inout UnityGI gi)
		{
			LightingStandard_GI(s, data, gi);
		}

		struct Input {
			float3 worldPos;
			float3 centre;
		};

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			v.vertex.xyz += v.normal * (_AtmosphereRadius - _PlanetRadius);
			o.centre = mul(unity_ObjectToWorld, half4(0, 0, 0, 1));
		}

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = float3(1, 1, 1);
			o.Alpha = 1;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
