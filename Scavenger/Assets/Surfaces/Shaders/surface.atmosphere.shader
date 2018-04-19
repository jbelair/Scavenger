// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Surfaces/Atmosphere" 
{
	Properties
	{
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
	}
	SubShader
	{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
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
