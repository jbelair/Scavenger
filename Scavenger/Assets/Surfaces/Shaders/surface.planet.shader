Shader "Surfaces/Planet" {
	Properties{
		_AtmosphereRadius("Atmosphere Radius", Float) = 1.1
		_PlanetRadius("Planet Radius", Float) = 1
		_PlanetCentre("Planet Centre", Vector) = (0,0,0,1)
		_ViewSamples("View Samples", Int) = 4
		_LightSamples("Light Samples", Int) = 4
		_ScatteringCoefficient("Scattering Coefficient", Float) = 1
		_RayScaleHeight("Ray Height", Float) = 0.1
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf WrapScattering fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		#include "UnityPBSLighting.cginc"
		inline fixed4 LightingWrapScattering(SurfaceOutputStandard s, fixed3 viewDir, UnityGI gi)
		{
			half NdotL = dot(s.Normal, gi.light.dir);//lightDir);
			half diff = NdotL * 0.5 + 0.5;
			half4 c;

			//float3 H = normalize(gi.light.dir + s.Normal * 0.5);
			//float I = pow(saturate(dot(viewDir, -H)), 1) * 2;
			float I = 1 - abs(dot(gi.light.dir, s.Normal));
			I *= pow(abs(min(0, dot(viewDir, gi.light.dir))), 4.0);
			I = saturate(pow(I, 2));

			half VdotL = dot(s.Normal, gi.light.dir);
			float scat = saturate(VdotL + 1) + I;
			c.rgb = s.Albedo * gi.light.color.rgb * diff * lerp(float3(pow(scat, 0.5), pow(scat, 0.75), pow(scat, 0.9)), gi.light.color * 16, saturate(I));// *lerp(float3(1, 1, 1), float3(pow(I, 0.5), pow(I, 0.75), pow(I, 0.9)), (I + scat));
			c.a = s.Alpha;

			return c;
		}

		void LightingWrapScattering_GI(SurfaceOutputStandard s, UnityGIInput data, inout UnityGI gi)
		{
			LightingStandard_GI(s, data, gi);
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG

		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
		LOD 200
		Cull Back
		Blend One One

		CGPROGRAM
		#pragma surface surf WrapScattering vertex:vert

		float _AtmosphereRadius;
		float _PlanetRadius;
		float4 _PlanetCentre;

		float _ViewSamples;
		float _LightSamples;

		float _RayScaleHeight;

		float _ScatteringCoefficient;

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
			half NdotL = dot(s.Normal, gi.light.dir);//lightDir);
			half diff = NdotL * 0.5 + 0.5;
			half4 c;

			//float3 H = normalize(gi.light.dir + s.Normal * 0.5);
			//float I = pow(saturate(dot(viewDir, -H)), 1) * 2;
			float I = 1 - abs(dot(gi.light.dir, s.Normal));
			I *= pow(abs(min(0, dot(viewDir, gi.light.dir))), 4.0);
			I = saturate(pow(I, 0.5));

			float entry = 0;
			float exit = 0;
			float scattering = 0;
			if (rayIntersect(s.Normal, -viewDir, _PlanetCentre, _AtmosphereRadius / _AtmosphereRadius, entry, exit));
			{	
				scattering = exit - entry;
			}
			scattering = saturate(scattering);

			float entryP = 0;
			float exitP = 0;
			float planet = 0;
			if (rayIntersect(s.Normal, -viewDir, _PlanetCentre, _PlanetRadius / _AtmosphereRadius, entryP, exitP));
			{
				planet = exitP - entryP;
			}
			planet = saturate(planet);

			half VdotL = dot(s.Normal, gi.light.dir);

			float freznel = pow(saturate(abs(dot(s.Normal, viewDir))), 5);
			float scat = saturate(VdotL + I);

			//float density = saturate(scattering * (abs(dot(-viewDir, gi.light.dir)) / 2 + 0.5) * (dot(s.Normal, gi.light.dir) / 2 + 0.5) - planet);// *(dot(s.Normal, gi.light.dir) / 2 + 0.5));
			float density = saturate(scattering * (saturate(dot(-viewDir, gi.light.dir)) / 2 + 0.5) - planet);
			float directionality = density * saturate(dot(-viewDir, gi.light.dir)) * saturate(dot(s.Normal, -gi.light.dir));

			c.rgb = float3(pow(density,2) + pow(directionality, 0.25),pow(density,0.9) + pow(directionality, 0.9),pow(density,0.75) - pow(directionality, 1)) / 2;//saturate(dot(viewDir, -gi.light.dir)) * (1 - abs(dot(s.Normal, gi.light.dir))), 0);//float3(scattering, planet, 0) / 10;//s.Albedo * gi.light.color.rgb * diff * lerp(float3(0, 0, 0), lerp(lerp(float3(pow(freznel, 0.9), pow(freznel, 0.75), pow(freznel, 0.5)), float3(pow(scat, 0.5), pow(scat, 0.75), pow(scat, 0.9)), freznel), gi.light.color * 16, saturate(I - (1 - freznel))), min(1 - I, pow(1 - freznel, 2)));// *lerp(float3(1, 1, 1), float3(pow(I, 0.5), pow(I, 0.75), pow(I, 0.9)), (I + scat));
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
			// Albedo comes from a texture tinted by color
			//fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = float3(1, 1, 1);//c.rgb;
			// Metallic and smoothness come from slider variables
			//o.Metallic = _Metallic;
			//o.Smoothness = _Glossiness;
			o.Alpha = 1;//.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
