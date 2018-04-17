// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Surfaces/Planet" {
	Properties{
		_AtmosphereRadius("Atmosphere Radius", Float) = 1
		_AtmosphereDensity("Atmosphere Density", Float) = 1
		_AtmosphereIntensity("Atmosphere Intensity", Float) = 4
		_PlanetRadius("Planet Radius", Float) = 0.95
		_PlanetDensity("Planet Density", Float) = 0.8
		_PlanetCentre("Planet Centre", Vector) = (0,0,0,1)
		_AtmosphereMap("Atmosphere Map", 2D) = "black" {}
		_Phase("Atmosphere Phase", Vector) = (0.75,0.825,1,1)
		_ViewSamples("View Samples", Int) = 1
		_LightSamples("Light Samples", Int) = 4

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
			half NdotL = dot(s.Normal, gi.light.dir);//lightDir);
			half diff = NdotL * 0.5 + 0.5;
			half4 c = LightingStandard(s, viewDir, gi);

			//float3 H = normalize(gi.light.dir + s.Normal * 0.5);
			//float I = pow(saturate(dot(viewDir, -H)), 1) * 2;
			float I = 1 - abs(dot(gi.light.dir, s.Normal));
			I *= pow(abs(min(0, dot(viewDir, gi.light.dir))), 4.0);
			I = saturate(pow(I, 2));

			half VdotL = dot(s.Normal, gi.light.dir);
			float scat = saturate(VdotL + 1) + I;
			c.rgb = (c + s.Albedo * gi.light.color * float3(pow(diff, _Phase.r), pow(diff, _Phase.g), pow(diff, _Phase.b))) / 2;// * dot(viewDir, -gi.light.dir) * (1 - saturate(dot(s.Normal, gi.light.dir))) * abs(dot(s.Normal, viewDir));;// *lerp(float3(pow(scat, 0.5), pow(scat, 0.75), pow(scat, 0.9)), gi.light.color * 16, saturate(I));// *lerp(float3(1, 1, 1), float3(pow(I, 0.5), pow(I, 0.75), pow(I, 0.9)), (I + scat));
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
			//o.normal = v.normal;
		}

		void surf(Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
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
			// Metallic and smoothness come from slider variables
			o.Metallic = ms.x;
			o.Smoothness = ms.y;
			//o.Alpha = c.a;
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
			
			half NdotL = dot(s.Normal, gi.light.dir);//lightDir);
			half diff = NdotL * 0.5 + 0.5;
			half4 c;

			//float3 H = normalize(gi.light.dir + s.Normal * 0.5);
			//float I = pow(saturate(dot(viewDir, -H)), 1) * 2;
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
				//s.Normal + -viewDir * scattering / 2;
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

			//density = scattering - planet;
			density = abs(scattering - planet * _PlanetDensity);
			//density = scattering;
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

				//if (planet > 0)
				//	break;

				float lightSamples = d / _LightSamples;
				for (int j = 0; j < _LightSamples; j++)
				{
					float direction = dot(s.Normal, -gi.light.dir);
					//direction = pow(abs(direction), 1 / _AtmosphereDensity) * ((direction > 0) ? 1 : -1);
					direction = pow(direction / 2 + 0.5, _AtmosphereDensity);
					//direction = direction / 2 + 0.5;
					directionality += lightSamples * direction;
					//directionality += min(0, dot(gi.light.dir + viewDir, -s.Normal)) * pow(min((dot(viewDir, -gi.light.dir) / 2 + 0.5), 0.5), 0.01);
					//directionality += min(0, dot(gi.light.dir + viewDir, -s.Normal)) * pow(min((dot(viewDir, -gi.light.dir) / 2 + 0.5), 0.5), 0.01);

					//directionality += lightSamples * (dot(gi.light.dir + s.Normal, gi.light.dir)) * dot(gi.light.dir + viewDir, -s.Normal);

					//directionality += lightSamples * (dot(gi.light.dir + s.Normal, gi.light.dir) * (max(0, dot(normalize(gi.light.dir + viewDir), s.Normal)) + max(0, dot(normalize(gi.light.dir + viewDir), -s.Normal))));
				}

				//density += d;
			}
			directionality = saturate(directionality / _LightSamples);

			//directionality = directionality + 0.1 * dot(viewDir, -gi.light.dir) * (1 - saturate(dot(s.Normal, gi.light.dir))) * abs(dot(s.Normal, viewDir));

			//if (directionality > density)
			//	density *= directionality;

			half VdotL = dot(s.Normal, gi.light.dir);

			float freznel = pow(saturate(abs(dot(s.Normal, viewDir))), 1); /*used to be 5*/
			float scat = saturate(VdotL + I);

			//density -= planet;
			//directionality *= density;
			density += density * directionality;

			//float density = saturate(scattering * (abs(dot(-viewDir, gi.light.dir)) / 2 + 0.5) * (dot(s.Normal, gi.light.dir) / 2 + 0.5) - planet);// *(dot(s.Normal, gi.light.dir) / 2 + 0.5));
			//float density = saturate((scattering - planet)) * pow((dot(viewDir, gi.light.dir) / 2 + 0.5), 0.05); /* (saturate(dot(-viewDir, gi.light.dir)) / 2 + 0.5)*/;// -planet;
			//float directionality = /*density */ density * saturate(pow((dot(-viewDir, gi.light.dir) / 2 + 0.5) * pow((dot(s.Normal, -gi.light.dir) / 2 + 0.5), 2), 2));
			//density = max(abs(dot(s.Normal, viewDir)) * saturate(1 - dot(s.Normal, gi.light.dir)), density);
			//directionality = lerp(directionality, density, freznel);

			//c.rgb = gi.light.color.rgb * (float3(pow(density, 1 / _Phase.r), pow(density, 1 / _Phase.g), pow(density, 1 / _Phase.b)) * float3(pow(directionality, _Phase.r), pow(directionality, _Phase.g), pow(directionality, _Phase.b))) * _AtmosphereIntensity;
			c.rgb = gi.light.color.rgb * tex2D(_AtmosphereMap, float2(density, directionality)) * _AtmosphereIntensity;
			//*(1 + _AtmosphereIntensity * density * (1 - abs(dot(s.Normal, -gi.light.dir))) * saturate(dot(-gi.light.dir, viewDir)));
			//c.rgb = gi.light.color.rgb * tex2D(_AtmosphereMap, float2(density, directionality)) * _AtmosphereIntensity;
			// * (dot(s.Normal, -gi.light.dir) / 2 + 0.5);
			//float3(pow(density, 2) + pow(directionality, 0.25), pow(density, 0.9) + pow(directionality, 0.9), pow(density, 0.75) - pow(directionality, 1)) / 2;
			//saturate(dot(viewDir, -gi.light.dir)) * (1 - abs(dot(s.Normal, gi.light.dir))), 0);
			//float3(scattering, planet, 0) / 10;
			//s.Albedo * gi.light.color.rgb * diff * lerp(float3(0, 0, 0), lerp(lerp(float3(pow(freznel, 0.9), pow(freznel, 0.75), pow(freznel, 0.5)), float3(pow(scat, 0.5), pow(scat, 0.75), pow(scat, 0.9)), freznel), gi.light.color * 16, saturate(I - (1 - freznel))), min(1 - I, pow(1 - freznel, 2)));
			// *lerp(float3(1, 1, 1), float3(pow(I, 0.5), pow(I, 0.75), pow(I, 0.9)), (I + scat));
			c.a = s.Alpha;

			//c = float4(density, directionality * density, 0, 1);

			return c;
		}

		void LightingWrapScattering_GI(SurfaceOutputStandard s, UnityGIInput data, inout UnityGI gi)
		{
			LightingStandard_GI(s, data, gi);
		}

		struct Input {
			float3 worldPos;
			//float3 centre;
		};

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			v.vertex.xyz += v.normal * (_AtmosphereRadius - _PlanetRadius);
			//o.centre = mul(unity_ObjectToWorld, half4(0, 0, 0, 1));
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
