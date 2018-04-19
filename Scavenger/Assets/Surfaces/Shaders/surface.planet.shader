﻿// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Surfaces/Planet" {
	Properties{
		_AtmosphereRadius("Atmosphere Radius", Float) = 1
		_AtmosphereDensity("Atmosphere Density", Float) = 2
		_AtmosphereIntensity("Atmosphere Intensity", Float) = 1
		_PlanetRadius("Planet Radius", Float) = 0.95
		_PlanetDensity("Planet Density", Float) = 1
		_PlanetCentre("Planet Centre", Vector) = (0,0,0,1)
		_AtmosphereMap("Atmosphere Map", 2D) = "black" {}
		_Phase("Atmosphere Phase", Vector) = (0.75,0.825,1,1)
		_ViewSamples("View Samples", Int) = 2
		_LightSamples("Light Samples", Int) = 2

		_TexMain("Main Texture", 2D) = "white" {}
		_NorMain("Main Normal Map", 2D) = "bump" {}
		_MSMain("Main Metallic & Smoothness Map", 2D) = "black" {}
		[HDR]_EmiMainColour("Main Emission Colour", Color) = (1,1,1,1)
		_EmiMain("Main Emission Map", 2D) = "black" {}
		_PoleScale("Poles Scale", Range(0.1,1)) = 0.5
		_TexPoleN("North Pole Texture", 2D) = "white" {}
		_NorPoleN("North Pole Normal Map", 2D) = "bump" {}
		_MSPoleN("North Pole Metallic & Smoothness Map", 2D) = "black" {}
		[HDR]_EmiPoleNColour("North Pole Emission Colour", Color) = (1,1,1,1)
		_EmiPoleN("North Pole Emission Map", 2D) = "black" {}
		_TexPoleS("South Pole Texture", 2D) = "white" {}
		_NorPoleS("South Pole Normal Map", 2D) = "bump" {}
		_MSPoleS("South Pole Metallic & Smoothness Map", 2D) = "black" {}
		[HDR]_EmiPoleSColour("South Pole Emission Colour", Color) = (1,1,1,1)
		_EmiPoleS("South Pole Emission Map", 2D) = "black" {}
	}
		SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 400

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf WrapScattering fullforwardshadows vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		// Primary Texture
		sampler2D _TexMain;
		float4 _TexMain_ST;
		// Primary Normal Map
		sampler2D _NorMain;
		float4 _NorMain_ST;
		// Primary Metallic and Smoothness Map
		sampler2D _MSMain;
		float4 _MSMain_ST;
		// Primary Emissive Map
		sampler2D _EmiMain;
		float4 _EmiMain_ST;
		float4 _EmiMainColour;
		// Pole Scale
		float _PoleScale;
		// North Pole Texture (Uses alpha to map transition between primary and north pole)
		sampler2D _TexPoleN;
		float4 _TexPoleN_ST;
		// North Pole Normal Map
		sampler2D _NorPoleN;
		float4 _NorPoleN_ST;
		// North Pole Metallic and Smoothness Map
		sampler2D _MSPoleN;
		float4 _MSPoleN_ST;
		// North Pole Emissive Map
		sampler2D _EmiPoleN;
		float4 _EmiPoleN_ST;
		float4 _EmiPoleNColour;
		// South Pole Texture
		sampler2D _TexPoleS;
		float4 _TexPoleS_ST;
		// South Pole Normal Map
		sampler2D _NorPoleS;
		float4 _NorPoleS_ST;
		// South Pole Metallic and Smoothness Map
		sampler2D _MSPoleS;
		float4 _MSPoleS_ST;
		// South Pole Emissive Map
		sampler2D _EmiPoleS;
		float4 _EmiPoleS_ST;
		float4 _EmiPoleSColour;

		struct Input {
			float3 localPos;
			float2 texturePos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		fixed4 _Phase;

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
			o.texturePos = v.texcoord.xy;
		}

		void surf(Input IN, inout SurfaceOutputStandard o) 
		{
			fixed4 c = 0;
			fixed4 e = 0;
			float3 n = 0;
			float2 ms = 0;
			if (IN.localPos.y < 0.5 - _PoleScale / 2 && IN.localPos.y > -0.5 + (_PoleScale / 2))
			{
				c = tex2D(_TexMain, IN.texturePos * _TexMain_ST.xy + _TexMain_ST.zw);
				e = tex2D(_EmiMain, IN.texturePos * _EmiMain_ST.xy + _EmiMain_ST.zw) * _EmiMainColour;
				n = UnpackNormal(tex2D(_NorMain, IN.texturePos * _NorMain_ST.xy + _NorMain_ST.zw));
				ms = tex2D(_MSMain, IN.texturePos * _MSMain_ST.xy + _MSMain_ST.zw).rg;
			}
			else
			{
				float scale = sin(_PoleScale);
				if (IN.localPos.y >= 0.5 - _PoleScale / 2)
				{
					c = tex2D(_TexPoleN, ((IN.localPos.xz / scale) + 0.5) * _TexPoleN_ST.xy + _TexPoleN_ST.zw);
					e = tex2D(_EmiPoleN, ((IN.localPos.xz / scale) + 0.5) * _EmiPoleN_ST.xy + _EmiPoleN_ST.zw) * _EmiPoleNColour;
					n = UnpackNormal(tex2D(_NorPoleN, ((IN.localPos.xz / scale) + 0.5) * _NorPoleN_ST.xy + _NorPoleN_ST.zw));
					ms = tex2D(_MSPoleN, IN.texturePos * _MSPoleN_ST.xy + _MSPoleN_ST.zw).rg;

					e = lerp(tex2D(_EmiMain, IN.texturePos* _EmiMain_ST.xy + _EmiMain_ST.zw) * _EmiMainColour, e, c.a);
					n = lerp(UnpackNormal(tex2D(_NorMain, IN.texturePos * _NorMain_ST.xy + _NorMain_ST.zw)), n, c.a);
					ms = lerp(tex2D(_MSMain, IN.texturePos* _MSMain_ST.xy + _MSMain_ST.zw).rg, ms, c.a);
					c = lerp(tex2D(_TexMain, IN.texturePos * _TexMain_ST.xy + _TexMain_ST.zw), c, c.a);
						
				}
				else if (IN.localPos.y <= -0.5 + (_PoleScale / 2))
				{
					c = tex2D(_TexPoleS, ((IN.localPos.xz / scale) + 0.5) * _TexPoleS_ST.xy + _TexPoleS_ST.zw);
					e = tex2D(_EmiPoleS, ((IN.localPos.xz / scale) + 0.5) * _EmiPoleS_ST.xy + _EmiPoleS_ST.zw) * _EmiPoleSColour;
					n = UnpackNormal(tex2D(_NorPoleS, ((IN.localPos.xz / scale) + 0.5) * _NorPoleS_ST.xy + _NorPoleS_ST.zw));
					ms = tex2D(_MSPoleS, IN.texturePos * _MSPoleS_ST.xy + _MSPoleS_ST.zw).rg;

					e = lerp(tex2D(_EmiMain, IN.texturePos* _EmiMain_ST.xy + _EmiMain_ST.zw) * _EmiMainColour, e, c.a);
					n = lerp(UnpackNormal(tex2D(_NorMain, IN.texturePos * _NorMain_ST.xy + _NorMain_ST.zw)), n, c.a);
					ms = lerp(tex2D(_MSMain, IN.texturePos* _MSMain_ST.xy + _MSMain_ST.zw).rg, ms, c.a);
					c = lerp(tex2D(_TexMain, IN.texturePos * _TexMain_ST.xy + _TexMain_ST.zw), c, c.a);
				}
			}
			o.Albedo = c.rgb;
			o.Emission = lerp(float3(0,0,0),e.rgb,e.a);
			o.Normal = n;
			o.Metallic = ms.x;
			o.Smoothness = ms.y;
		}
		ENDCG

		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
		LOD 600
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

		float _HEIGHT_RAY = 0.7994;
		float _HEIGHT_MIE = 0.1200;
		float3 _BETA_RAY = float3(3.8e-6, 13.5e-6, 33.1e-6);
		float3 _BETA_MIE = 21e-6; 

		sampler2D _AtmosphereMap;

		float _ViewSamples;
		float _LightSamples;

		float _RayScaleHeight;

		float _ScatteringCoefficient;

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
			
			half NdotL = dot(s.Normal, gi.light.dir);
			half diff = NdotL * 0.5 + 0.5;
			half4 c;

			float I = 1 - abs(dot(gi.light.dir, s.Normal));
			I *= pow(abs(min(0, dot(viewDir, gi.light.dir))), 4.0);
			I = saturate(pow(I, 0.5));

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

			float viewSample = scattering / _ViewSamples;
			for (int i = 0; i < _ViewSamples; i++)
			{
				float entryD = 0;
				float exitD = 0;

				float3 p = s.Normal + -viewDir * viewSample * i;

				rayIntersect(p, -gi.light.dir, _PlanetCentre, _AtmosphereRadius, entryD, exitD);

				float d = exitD - entryD;

				float lightSamples = d / _LightSamples;
				for (int j = 0; j < _LightSamples; j++)
				{
					float direction = dot(s.Normal, -gi.light.dir);
					direction = pow(direction / 2 + 0.5, _AtmosphereDensity);
					directionality += lightSamples * direction;
				}
			}
			directionality = saturate(directionality / _LightSamples);

			half VdotL = dot(s.Normal, gi.light.dir);

			float freznel = pow(saturate(abs(dot(s.Normal, viewDir))), 1);
			float scat = saturate(VdotL + I);

			directionality *= density;
			density += density * directionality;

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
		};

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			v.vertex.xyz += v.normal * (_AtmosphereRadius - _PlanetRadius);
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
