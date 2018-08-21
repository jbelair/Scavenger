Shader "Surfaces/Planet" 
{
	Properties
	{
		[MaterialToggle]_EnableBack("Back Enabled", Float) = 1
		[MaterialToggle]_EnableMid("Mid Enabled", Float) = 1
		[MaterialToggle]_EnableFront("Front Enabled", Float) = 1
		[MaterialToggle]_EnablePoles("Poles Enabled", Float) = 1
		[MaterialToggle]_EnableClouds("Clouds Enabled", Float) = 1

		_KelvinMinMax("Kelvin Maximums", Vector) = (273,300,500,1000)
		_Kelvin("Kelvin", Float) = 0
		_KelvinRange("Kelvin Range", Float) = 0

		_Elevation("Water Level", Range(0.001,1)) = 0.5
		_ElevationSlope("Water Slope", Range(0.01,1)) = 0.05
		_Elevation_Mask("Elevation Map (R)", 2D) = "black" {}

		_Phase("Land Phase", Vector) = (1,0.825,0.75,0.33)
		_PhaseClouds("Cloud Phase", Vector) = (0.5,1,0.75,0.42)
		_PhaseWater("Water Phase", Vector) = (1,0.825,0.75,0.33)

		_CloudsSlope("Cloud Slope", Range(0.01,1)) = 0.1
		_CloudsSpeed("Cloud Speed", Float) = 0.001
		_CloudsTempScale("Cloud Speed Scaler (Kelvin)", Float) = 0.01
		_CloudsTurbulence("Cloud Turbulence", Range(0.001,0.1)) = 0.02
		_CloudsTurbScale("Cloud Turbulence Scale", Float) = 1
		_CloudsTurbOctaves("Cloud Turbulence Octaves", Float) = 4

		_PoleSlope("Pole Slope", Range(0.01,1)) = 0.1

		[Normal]_TexBack_Norm("Back Normal (BUMP)", 2D) = "bump" {}
		[Normal]_TexMid_Norm("Mid Normal (BUMP)", 2D) = "bump" {}
		[Normal]_TexFront_Norm("Front Normal (BUMP)", 2D) = "bump" {}
		[Normal]_TexPole_Norm("Pole Normal (BUMP)", 2D) = "bump" {}
		[Normal]_TexClouds_Norm("Cloud Normal (BUMP)", 2D) = "bump" {}

		// COLD
		// WATER
		_TexWaterCold("COLD Water", 2D) = "white" {}
		_TexWaterCold_Tint("COLD Water Tint", Color) = (1,1,1,1)
		[HDR]_TexWaterCold_Emissive("COLD Water Emissive", Color) = (0,0,0,0)
		[NoScaleOffset]_TexWaterCold_MSE("COLD Water MSE (RGB)", 2D) = "black" {}
		// BACK
		_TexBackCold("COLD Back (RGB)", 2D) = "white" {}
		[HDR]_TexBackCold_Emissive("COLD Back Emissive", Color) = (0,0,0,0)
		[NoScaleOffset]_TexBackCold_MSE("COLD Back MSE (RGB)", 2D) = "black" {}
		_TexBackCold_NStr("COLD Back Normal Strength", Range(0,10)) = 1
		
		// MID
		_TexMidCold_Mask("COLD Mid Mask (R)", 2D) = "black" {}
		_TexMidCold("COLD Mid (RGB)", 2D) = "white" {}
		[HDR]_TexMidCold_Emissive("COLD Mid Emissive", Color) = (0,0,0,0)
		[NoScaleOffset]_TexMidCold_MSE("COLD Mid MSE (RGB)", 2D) = "black" {}
		_TexMidCold_NStr("COLD Mid Normal Strength", Range(0,10)) = 1
		
		// FRONT
		_TexFrontCold_Mask("COLD Front Mask (R)", 2D) = "black" {}
		_TexFrontCold("COLD Front (RGB)", 2D) = "white" {}
		[HDR]_TexFrontCold_Emissive("COLD Front Emissive (RGB)", Color) = (0,0,0,0)
		[NoScaleOffset]_TexFrontCold_MSE("COLD Front MSE", 2D) = "black" {}
		_TexFrontCold_NStr("COLD Front Normal Strength", Range(0,10)) = 1
		
		// POLES
		_TexPoleCold_Mask("COLD North Polar Mask (R)", 2D) = "black" {}
		_TexPoleCold("COLD North Pole (RGB)", 2D) = "white" {}
		[HDR]_TexPoleCold_Emissive("COLD North Pole Emissive", Color) = (0,0,0,0)
		[NoScaleOffset]_TexPoleCold_MSE("COLD North Pole MSE (RGB)", 2D) = "grey" {}
		_TexPoleCold_NStr("COLD North Pole Normal Strength", Range(0,10)) = 1
		
		// CLOUDS
		_TexCloudsCold_Colour("COLD Cloud Colour", Color) = (1,1,1,1)
		_TexCloudsCold("COLD Clouds (R)", 2D) = "black" {}
		_TexCloudsCold_NStr("COLD Clouds Normal Strength", Float) = 1

		// WARM
		// WATER
		_TexWaterWarm("WARM Water", 2D) = "white" {}
		_TexWaterWarm_Tint("WARM Water Tint", Color) = (1, 1, 1, 1)
		[HDR]_TexWaterWarm_Emissive("WARM Water Emissive", Color) = (0, 0, 0, 0)
		[NoScaleOffset]_TexWaterWarm_MSE("WARM Water MSE (RGB)", 2D) = "black" {}
		// BACK
		_TexBackWarm("WARM Back (RGB)", 2D) = "white" {}
		[HDR]_TexBackWarm_Emissive("WARM Back Emissive", Color) = (0,0,0,0)
		[NoScaleOffset]_TexBackWarm_MSE("WARM Back MSE (RGB)", 2D) = "black" {}
		_TexBackWarm_NStr("WARM Back Normal Strength", Range(0,10)) = 1
		// MID
		_TexMidWarm_Mask("WARM Mid Mask (R)", 2D) = "black" {}
		_TexMidWarm("WARM Mid (RGB)", 2D) = "white" {}
		[HDR]_TexMidWarm_Emissive("WARM Mid Emissive", Color) = (0,0,0,0)
		[NoScaleOffset]_TexMidWarm_MSE("WARM Mid MSE (RGB)", 2D) = "black" {}
		_TexMidWarm_NStr("WARM Mid Normal Strength", Range(0,10)) = 1
		// FRONT
		_TexFrontWarm_Mask("WARM Front Mask (R)", 2D) = "black" {}
		_TexFrontWarm("WARM Front (RGB)", 2D) = "white" {}
		[HDR]_TexFrontWarm_Emissive("WARM Front Emissive", Color) = (0,0,0,0)
		[NoScaleOffset]_TexFrontWarm_MSE("WARM Front MSE (RGB)", 2D) = "black" {}
		_TexFrontWarm_NStr("WARM Front Normal Strength", Range(0,10)) = 1
		// POLES
		_TexPoleWarm_Mask("WARM North Polar Mask (R)", 2D) = "black" {}
		_TexPoleWarm("WARM North Pole (RGB)", 2D) = "white" {}
		[HDR]_TexPoleWarm_Emissive("WARM North Pole Emissive", Color) = (0,0,0,0)
		[NoScaleOffset]_TexPoleWarm_MSE("WARM North Pole MSE (RGB)", 2D) = "grey" {}
		_TexPoleWarm_NStr("WARM North Pole Normal Strength", Range(0,10)) = 1
		// CLOUDS
		_TexCloudsWarm_Colour("WARM Cloud Colour", Color) = (1,1,1,1)
		_TexCloudsWarm("WARM Clouds (R)", 2D) = "black" {}
		_TexCloudsWarm_NStr("WARM Clouds Normal Strength", Float) = 1

		// HOT
		// WATER
		_TexWaterHot("HOT Water", 2D) = "white" {}
		_TexWaterHot_Tint("HOT Water Tint", Color) = (1, 1, 1, 1)
		[HDR]_TexWaterHot_Emissive("HOT Water Emissive", Color) = (0, 0, 0, 0)
		[NoScaleOffset]_TexWaterHot_MSE("HOT Water MSE (RGB)", 2D) = "black" {}
		// BACK
		_TexBackHot("HOT Back (RGB)", 2D) = "white" {}
		[HDR]_TexBackHot_Emissive("HOT Back Emissive", Color) = (0,0,0,0)
		[NoScaleOffset]_TexBackHot_MSE("HOT Back MSE (RGB)", 2D) = "grey" {}
		_TexBackHot_NStr("HOT Back Normal Strength", Range(0,10)) = 1
		// MID
		_TexMidHot_Mask("HOT Mid Mask", 2D) = "black" {}
		_TexMidHot("HOT Mid (RGB)", 2D) = "white" {}
		[HDR]_TexMidHot_Emissive("HOT Mid Emissive", Color) = (0,0,0,0)
		[NoScaleOffset]_TexMidHot_MSE("HOT Mid MSE (RGB)", 2D) = "black" {}
		_TexMidHot_NStr("HOT Mid Normal Strength", Range(0,10)) = 1
		// FRONT
		_TexFrontHot_Mask("HOT Front Mask (R)", 2D) = "black" {}
		_TexFrontHot("HOT Front Hot (RGB)", 2D) = "white" {}
		[HDR]_TexFrontHot_Emissive("HOT Front Emissive", Color) = (0,0,0,0)
		[NoScaleOffset]_TexFrontHot_MSE("HOT Front MSE (RGB)", 2D) = "black" {}
		_TexFrontHot_NStr("HOT Front Normal Strength", Range(0,10)) = 1
		// POLES
		_TexPoleHot_Mask("HOT North Polar Mask (R)", 2D) = "black" {}
		_TexPoleHot("HOT North Pole (RGB)", 2D) = "white" {}
		[HDR]_TexPoleHot_Emissive("HOT North Pole Emissive", Color) = (0, 0, 0, 0)
		[NoScaleOffset]_TexPoleHot_MSE("HOT North Pole MSE (RGB)", 2D) = "grey" {}
		_TexPoleHot_NStr("HOT North Pole Normal Strength", Range(0, 10)) = 1
		// CLOUDS
		_TexCloudsHot_Colour("HOT Cloud Colour", Color) = (1, 1, 1, 1)
		_TexCloudsHot("HOT Clouds (R)", 2D) = "black" {}
		_TexCloudsHot_NStr("HOT Clouds Normal Strength", Float) = 1
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 400

		/***********************************************************************************************************************************************************************************************************
		WATER
		***********************************************************************************************************************************************************************************************************/

		CGPROGRAM
		#pragma surface surf WrapScatteringWater fullforwardshadows vertex:vert
		#include "Assets/Surfaces/Shaders/surfaceLighting.cginc"
		#include "Assets/Surfaces/Shaders/noise.cginc"
		#include "Assets/Surfaces/Shaders/planetHelper.cginc"

		sampler2D _TexWaterCold;
		float4 _TexWaterCold_ST;
		half3 _TexWaterCold_Tint;
		float3 _TexWaterCold_Emissive;
		sampler2D _TexWaterCold_MSE;

		sampler2D _TexWaterWarm;
		float4 _TexWaterWarm_ST;
		half3 _TexWaterWarm_Tint;
		float3 _TexWaterWarm_Emissive;
		sampler2D _TexWaterWarm_MSE;

		sampler2D _TexWaterHot;
		float4 _TexWaterHot_ST;
		float3 _TexWaterHot_Emissive;
		half3 _TexWaterHot_Tint;
		sampler2D _TexWaterHot_MSE;

		struct Input
		{
			float3 localPos;
			float2 texturePos;
		};

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.localPos = v.vertex.xyz;
			o.texturePos = v.texcoord.xy;
		}

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			float4 a = 0;
			float3 mse = 0;
			float3 e = 0;
			float3 eC = 0;
			float fade = abs(IN.localPos.y);

			float k = _Kelvin + _KelvinRange * ((1 - fade) * 2 - 1) * (1 - tex2D(_Elevation_Mask, IN.texturePos * _Elevation_Mask_ST.xy + _Elevation_Mask_ST.zw));

			float2 samplePos = IN.texturePos;

			if (k <= _KelvinMinMax.x)
			{
				a.rgb = tex2D(_TexWaterCold, samplePos * _TexWaterCold_ST.xy + _TexWaterCold_ST.zw).rgb * _TexWaterCold_Tint;
				a.rgb = lerp(a.rgb, _TexWaterCold_Tint, fade);

				mse = tex2D(_TexWaterCold_MSE, samplePos * _TexWaterCold_ST.xy + _TexWaterCold_ST.zw);
				e = mse.z * _TexWaterCold_Emissive;
				eC = _TexWaterCold_Emissive;
			}
			else if (k <= _KelvinMinMax.y)
			{
				// Calculate how warm the planet is, counteracting the cold
				float t = coldToWarm(k);

				a.rgb = tex2D(_TexWaterCold, samplePos * _TexWaterCold_ST.xy + _TexWaterCold_ST.zw).rgb * _TexWaterCold_Tint;
				a.rgb = lerp(a.rgb, _TexWaterCold_Tint, fade);
				float3 aWarm = tex2D(_TexWaterWarm, samplePos * _TexWaterWarm_ST.xy + _TexWaterWarm_ST.zw).rgb * _TexWaterWarm_Tint;
				aWarm.rgb = lerp(aWarm.rgb, _TexWaterWarm_Tint, fade);
				a.rgb = lerp(a.rgb, aWarm, t);

				mse = tex2D(_TexWaterCold_MSE, samplePos * _TexWaterCold_ST.xy + _TexWaterCold_ST.zw);
				e = mse.z * _TexWaterCold_Emissive;
				float3 mseWarm = tex2D(_TexWaterWarm_MSE, samplePos * _TexWaterWarm_ST.xy + _TexWaterWarm_ST.zw);
				mse = lerp(mse, mseWarm, t);
				e = lerp(e, mseWarm.z * _TexWaterWarm_Emissive, t);
				eC = lerp(_TexWaterCold_Emissive, _TexWaterWarm_Emissive, t);
			}
			else if (k <= _KelvinMinMax.z)
			{
				// Calculate how hot the planet is, counteracting the warm
				float t = warmToHot(k);

				a.rgb = tex2D(_TexWaterWarm, samplePos * _TexWaterWarm_ST.xy + _TexWaterWarm_ST.zw).rgb * _TexWaterWarm_Tint;
				a.rgb = lerp(a.rgb, _TexWaterWarm_Tint, fade);
				float3 aHot = tex2D(_TexWaterHot, samplePos * _TexWaterHot_ST.xy + _TexWaterHot_ST.zw).rgb * _TexWaterHot_Tint;
				aHot.rgb = lerp(aHot.rgb, _TexWaterHot_Tint, fade);
				a.rgb = lerp(a.rgb, aHot, t);

				mse = tex2D(_TexWaterWarm_MSE, samplePos * _TexWaterWarm_ST.xy + _TexWaterWarm_ST.zw);
				e = mse.z * _TexWaterWarm_Emissive;
				float3 mseHot = tex2D(_TexWaterHot_MSE, samplePos * _TexWaterHot_ST.xy + _TexWaterHot_ST.zw);
				mse = lerp(mse, mseHot, t);
				e = lerp(e, mseHot.z * _TexWaterHot_Emissive * k / _KelvinMinMax.w, t);
				eC = lerp(_TexWaterWarm_Emissive, _TexWaterHot_Emissive * k / _KelvinMinMax.w, t);
			}
			else
			{
				a.rgb = tex2D(_TexWaterHot, samplePos * _TexWaterHot_ST.xy + _TexWaterHot_ST.zw).rgb * _TexWaterHot_Tint;
				a.rgb = lerp(a.rgb, _TexWaterHot_Tint, fade);

				mse = tex2D(_TexWaterHot_MSE, samplePos * _TexWaterHot_ST.xy + _TexWaterHot_ST.zw);
				e = mse.z * _TexWaterHot_Emissive * k / _KelvinMinMax.w;
				eC = _TexWaterHot_Emissive * k / _KelvinMinMax.w;
			}

			o.Albedo = a;
			o.Metallic = lerp(mse.x, 0.5, fade);
			o.Smoothness = lerp(mse.y, 0.5, fade);
			o.Emission = lerp(e, eC, fade);
		}

		ENDCG

		/***********************************************************************************************************************************************************************************************************
		BACK
		***********************************************************************************************************************************************************************************************************/

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf WrapScattering fullforwardshadows vertex:vert alpha:fade
		#include "Assets/Surfaces/Shaders/surfaceLighting.cginc"
		#include "Assets/Surfaces/Shaders/planetHelper.cginc"

		float _EnableBack;

		sampler2D _TexBack_Norm;
		float4 _TexBack_Norm_ST;

		// _KelvinMinMax.x
		sampler2D _TexBackCold;
		float4 _TexBackCold_ST;
		// RGB emissive colour
		half3 _TexBackCold_Emissive;
		// RGB encoded to Metallic Smoothness Emissive
		sampler2D _TexBackCold_MSE;
		// Scalar normal strength modifier;
		float _TexBackCold_NStr;

		// _KelvinMinMax.y
		sampler2D _TexBackWarm;
		float4 _TexBackWarm_ST;
		// RGB emissive colour
		half3 _TexBackWarm_Emissive;
		// RGB encoded to Metallic Smoothness Emissive
		sampler2D _TexBackWarm_MSE;
		// Scalar normal strength modifier;
		float _TexBackWarm_NStr;

		// _KelvinMinMax.z
		sampler2D _TexBackHot;
		float4 _TexBackHot_ST;
		// RGB emissive colour
		half3 _TexBackHot_Emissive;
		// RGB encoded to Metallic Smoothness Emissive
		sampler2D _TexBackHot_MSE;
		// Scalar normal strength modifier;
		float _TexBackHot_NStr;

		struct Input 
		{
			float3 localPos;
			float2 texturePos;
		};

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.localPos = v.vertex.xyz;
			o.texturePos = v.texcoord.xy;
		}

		void surf(Input IN, inout SurfaceOutputStandard o) 
		{
			float4 a = 0;
			float3 n = UnpackNormal(tex2D(_TexBack_Norm, IN.texturePos * _TexBack_Norm_ST.xy + _TexBack_Norm_ST.zw));
			float3 mse = 0;
			float3 e = 0;

			if (_EnableBack == 1)
			{
				float k = _Kelvin + _KelvinRange * ((1 - abs(IN.localPos.y)) * 2 - 1) * (1 - tex2D(_Elevation_Mask, IN.texturePos * _Elevation_Mask_ST.xy + _Elevation_Mask_ST.zw));

				if (k <= _KelvinMinMax.x)
				{
					a = tex2D(_TexBackCold, IN.texturePos * _TexBackCold_ST.xy + _TexBackCold_ST.zw);

					n.xy *= _TexBackCold_NStr;

					mse = tex2D(_TexBackCold_MSE, IN.texturePos * _TexBackCold_ST.xy + _TexBackCold_ST.zw);

					e = pow(1 - mse.z, 2) * _TexBackCold_Emissive;
				}
				else if (k <= _KelvinMinMax.y)
				{
					// Calculate how warm the planet is, counteracting the cold
					float t = coldToWarm(k);
					a = tex2D(_TexBackCold, IN.texturePos * _TexBackCold_ST.xy + _TexBackCold_ST.zw);
					a = lerp(a, tex2D(_TexBackWarm, IN.texturePos * _TexBackWarm_ST.xy + _TexBackWarm_ST.zw), t);

					n.xy *= lerp(_TexBackCold_NStr, _TexBackWarm_NStr, t);

					mse = tex2D(_TexBackCold_MSE, IN.texturePos * _TexBackCold_ST.xy + _TexBackCold_ST.zw);
					e = pow(1 - mse.z, 2) * _TexBackCold_Emissive;
					float3 mseWarm = tex2D(_TexBackWarm_MSE, IN.texturePos * _TexBackWarm_ST.xy + _TexBackWarm_ST.zw);
					mse = lerp(mse, mseWarm, t);
					e = lerp(e, pow((1 - mseWarm.z), 2) * _TexBackWarm_Emissive, t);
				}
				else if (k <= _KelvinMinMax.z)
				{
					// Calculate how hot the planet is, counteracting the warm
					float t = warmToHot(k);
					a = tex2D(_TexBackWarm, IN.texturePos * _TexBackWarm_ST.xy + _TexBackWarm_ST.zw);
					a = lerp(a, tex2D(_TexBackHot, IN.texturePos * _TexBackHot_ST.xy + _TexBackHot_ST.zw), t);

					n.xy *= lerp(_TexBackWarm_NStr, _TexBackHot_NStr, t);

					mse = tex2D(_TexBackWarm_MSE, IN.texturePos * _TexBackWarm_ST.xy + _TexBackWarm_ST.zw);
					e = pow(1 - mse.z, 2) * _TexBackWarm_Emissive;
					float3 mseHot = tex2D(_TexBackHot_MSE, IN.texturePos * _TexBackHot_ST.xy + _TexBackHot_ST.zw);
					mse = lerp(mse, mseHot, t);
					float eRamp = pow(1 - mseHot.z, 2);
					e = lerp(e, eRamp * _TexBackHot_Emissive * k / _KelvinMinMax.w, t);
				}
				else
				{
					a = tex2D(_TexBackHot, IN.texturePos * _TexBackHot_ST.xy + _TexBackHot_ST.zw);

					n.xy *= _TexBackHot_NStr;

					mse = tex2D(_TexBackHot_MSE, IN.texturePos * _TexBackHot_ST.xy + _TexBackHot_ST.zw);

					float eRamp = pow(1 - mse.z, 2);
					e = eRamp * _TexBackHot_Emissive * k / _KelvinMinMax.w;
				}

				a.a = a.a * (1 - waterElevation(mse.b, IN.texturePos));
				a.a = a.a;

				if (n.z < 0)
					n.z *= -1;
			}

			o.Albedo = a;
			o.Alpha = a.a;
			o.Normal = normalize(n);
			o.Metallic = mse.x;
			o.Smoothness = mse.y;
			o.Emission = e;
		}
		ENDCG

		/***********************************************************************************************************************************************************************************************************
		MID
		***********************************************************************************************************************************************************************************************************/

		CGPROGRAM
		#pragma surface surf WrapScattering fullforwardshadows vertex:vert alpha:fade
		#include "Assets/Surfaces/Shaders/surfaceLighting.cginc"
		#include "Assets/Surfaces/Shaders/planetHelper.cginc"

		float _EnableMid;

		sampler2D _TexMid_Norm;
		float4 _TexMid_Norm_ST;

		sampler2D _TexMidCold;
		float4 _TexMidCold_ST;
		sampler2D _TexMidCold_Mask;
		float4 _TexMidCold_Mask_ST;
		// RGB emissive colour
		half3 _TexMidCold_Emissive;
		// RGB encoded to Metallic Smoothness Emissive
		sampler2D _TexMidCold_MSE;
		// Scalar normal strength modifier;
		float _TexMidCold_NStr;
		

		sampler2D _TexMidWarm;
		float4 _TexMidWarm_ST;
		sampler2D _TexMidWarm_Mask;
		float4 _TexMidWarm_Mask_ST;
		// RGB emissive colour
		half3 _TexMidWarm_Emissive;
		// RGB encoded to Metallic Smoothness Emissive
		sampler2D _TexMidWarm_MSE;
		// Scalar normal strength modifier;
		float _TexMidWarm_NStr;

		sampler2D _TexMidHot;
		float4 _TexMidHot_ST;
		sampler2D _TexMidHot_Mask;
		float4 _TexMidHot_Mask_ST;
		// RGB emissive colour
		half3 _TexMidHot_Emissive;
		// RGB encoded to Metallic Smoothness Emissive
		sampler2D _TexMidHot_MSE;
		// Scalar normal strength modifier;
		float _TexMidHot_NStr;

		struct Input
		{
			float3 localPos;
			float2 texturePos;
		};

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.localPos = v.vertex.xyz;
			o.texturePos = v.texcoord.xy;
		}

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			float4 a = 0;
			float3 n = UnpackNormal(tex2D(_TexMid_Norm, IN.texturePos * _TexMid_Norm_ST.xy + _TexMid_Norm_ST.zw));
			float3 mse = 0;
			float3 e = 0;

			if (_EnableMid == 1)
			{
				float k = _Kelvin + _KelvinRange * ((1 - abs(IN.localPos.y)) * 2 - 1) * (1 - tex2D(_Elevation_Mask, IN.texturePos * _Elevation_Mask_ST.xy + _Elevation_Mask_ST.zw));

				if (k <= _KelvinMinMax.x)
				{
					a = tex2D(_TexMidCold, IN.texturePos * _TexMidCold_ST.xy + _TexMidCold_ST.zw);
					a.a = tex2D(_TexMidCold_Mask, IN.texturePos * _TexMidCold_Mask_ST.xy + _TexMidCold_Mask_ST.zw).r;

					n.xy *= _TexMidCold_NStr;

					mse = tex2D(_TexMidCold_MSE, IN.texturePos * _TexMidCold_ST.xy + _TexMidCold_ST.zw);
					e = pow(1 - mse.z, 2) * _TexMidCold_Emissive;
				}
				else if (k <= _KelvinMinMax.y)
				{
					// Calculate how warm the planet is, counteracting the cold
					float t = coldToWarm(k);

					a = tex2D(_TexMidCold, IN.texturePos * _TexMidCold_ST.xy + _TexMidCold_ST.zw);
					a = lerp(a, tex2D(_TexMidWarm, IN.texturePos * _TexMidWarm_ST.xy + _TexMidWarm_ST.zw), t);
					a.a = tex2D(_TexMidCold_Mask, IN.texturePos * _TexMidCold_Mask_ST.xy + _TexMidCold_Mask_ST.zw).r;
					a.a = lerp(a.a, tex2D(_TexMidWarm_Mask, IN.texturePos * _TexMidWarm_Mask_ST.xy + _TexMidWarm_Mask_ST.zw).r, t);

					n.xy *= lerp(_TexMidCold_NStr, _TexMidWarm_NStr, t);

					mse = tex2D(_TexMidCold_MSE, IN.texturePos * _TexMidCold_ST.xy + _TexMidCold_ST.zw);
					e = pow(1 - mse.z, 2) * _TexMidCold_Emissive;
					float3 mseWarm = tex2D(_TexMidWarm_MSE, IN.texturePos * _TexMidWarm_ST.xy + _TexMidWarm_ST.zw);
					mse = lerp(mse, mseWarm, t);
					e = lerp(e, pow(1 - mseWarm.z, 2) * _TexMidWarm_Emissive, t);
				}
				else if (k <= _KelvinMinMax.z)
				{
					// Calculate how hot the planet is, counteracting the warm
					float t = warmToHot(k);

					a = tex2D(_TexMidWarm, IN.texturePos * _TexMidWarm_ST.xy + _TexMidWarm_ST.zw);
					a = lerp(a, tex2D(_TexMidHot, IN.texturePos * _TexMidHot_ST.xy + _TexMidHot_ST.zw), t);
					a.a = tex2D(_TexMidWarm_Mask, IN.texturePos * _TexMidWarm_Mask_ST.xy + _TexMidWarm_Mask_ST.zw).r;
					a.a = lerp(a.a, tex2D(_TexMidHot_Mask, IN.texturePos * _TexMidHot_Mask_ST.xy + _TexMidHot_Mask_ST.zw).r, t);

					n.xy *= lerp(_TexMidWarm_NStr, _TexMidHot_NStr, t);

					mse = tex2D(_TexMidWarm_MSE, IN.texturePos * _TexMidWarm_ST.xy + _TexMidWarm_ST.zw);
					e = pow(1 - mse.z, 2) * _TexMidWarm_Emissive;
					float3 mseHot = tex2D(_TexMidHot_MSE, IN.texturePos * _TexMidHot_ST.xy + _TexMidHot_ST.zw);
					mse = lerp(mse, mseHot, t);
					float eRamp = pow(1 - mseHot.z, 2);
					e = lerp(e, eRamp * _TexMidHot_Emissive * k / _KelvinMinMax.w, t);
				}
				else
				{
					a = tex2D(_TexMidHot, IN.texturePos * _TexMidHot_ST.xy + _TexMidHot_ST.zw);
					a.a = tex2D(_TexMidHot_Mask, IN.texturePos * _TexMidHot_Mask_ST.xy + _TexMidHot_Mask_ST.zw).r;

					n.xy *= _TexMidHot_NStr;

					mse = tex2D(_TexMidHot_MSE, IN.texturePos * _TexMidHot_ST.xy + _TexMidHot_ST.zw);
					float eRamp = pow(1 - mse.z, 2);
					e = eRamp * _TexMidHot_Emissive * k / _KelvinMinMax.w;
				}

				a.a = a.a * (1 - waterElevation(mse.b, IN.texturePos));

				if (n.z < 0)
					n.z *= -1;
			}

			o.Albedo = a;
			o.Alpha = a.a;
			o.Normal = normalize(n);
			o.Metallic = mse.x;
			o.Smoothness = mse.y;
			o.Emission = e;
		}
		ENDCG

		/***********************************************************************************************************************************************************************************************************
		FRONT
		***********************************************************************************************************************************************************************************************************/

		CGPROGRAM
		#pragma surface surf WrapScattering fullforwardshadows vertex:vert alpha:fade
		#include "Assets/Surfaces/Shaders/surfaceLighting.cginc"
		#include "Assets/Surfaces/Shaders/planetHelper.cginc"

		float _EnableFront;

		sampler2D _TexFront_Norm;
		float4 _TexFront_Norm_ST;

		// _KelvinMinMax.x
		sampler2D _TexFrontCold;
		float4 _TexFrontCold_ST;
		sampler2D _TexFrontCold_Mask;
		float4 _TexFrontCold_Mask_ST;
		// RGB emissive colour
		half3 _TexFrontCold_Emissive;
		// RGB encoded to Metallic Smoothness Emissive
		sampler2D _TexFrontCold_MSE;
		// Scalar normal strength modifier;
		float _TexFrontCold_NStr;

		// _KelvinMinMax.y
		sampler2D _TexFrontWarm;
		float4 _TexFrontWarm_ST;
		sampler2D _TexFrontWarm_Mask;
		float4 _TexFrontWarm_Mask_ST;
		// RGB emissive colour
		half3 _TexFrontWarm_Emissive;
		// RGB encoded to Metallic Smoothness Emissive
		sampler2D _TexFrontWarm_MSE;
		// Scalar normal strength modifier;
		float _TexFrontWarm_NStr;

		// _KelvinMinMax.z
		sampler2D _TexFrontHot;
		float4 _TexFrontHot_ST;
		sampler2D _TexFrontHot_Mask;
		float4 _TexFrontHot_Mask_ST;
		// RGB emissive colour
		half3 _TexFrontHot_Emissive;
		// RGB encoded to Metallic Smoothness Emissive
		sampler2D _TexFrontHot_MSE;
		// Scalar normal strength modifier;
		float _TexFrontHot_NStr;

		struct Input
		{
			float3 localPos;
			float2 texturePos;
		};

			void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.localPos = v.vertex.xyz;
			o.texturePos = v.texcoord.xy;
		}

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			float4 a = 0;
			float3 n = UnpackNormal(tex2D(_TexFront_Norm, IN.texturePos * _TexFront_Norm_ST.xy + _TexFront_Norm_ST.zw));
			float3 mse = 0;
			float3 e = 0;

			if (_EnableFront == 1)
			{
				float k = _Kelvin + _KelvinRange * ((1 - abs(IN.localPos.y)) * 2 - 1) * (1 - tex2D(_Elevation_Mask, IN.texturePos * _Elevation_Mask_ST.xy + _Elevation_Mask_ST.zw));

				if (k <= _KelvinMinMax.x)
				{
					a = tex2D(_TexFrontCold, IN.texturePos * _TexFrontCold_ST.xy + _TexFrontCold_ST.zw);
					a.a = tex2D(_TexFrontCold_Mask, IN.texturePos * _TexFrontCold_Mask_ST.xy + _TexFrontCold_Mask_ST.zw).r;

					n.xy *= _TexFrontCold_NStr;

					mse = tex2D(_TexFrontCold_MSE, IN.texturePos * _TexFrontCold_ST.xy + _TexFrontCold_ST.zw);
					e = pow(1 - mse.z, 2) * _TexFrontCold_Emissive;
				}
				else if (k <= _KelvinMinMax.y)
				{
					// Calculate how warm the planet is, counteracting the cold
					float t = coldToWarm(k);

					a = tex2D(_TexFrontCold, IN.texturePos * _TexFrontCold_ST.xy + _TexFrontCold_ST.zw);
					a = lerp(a, tex2D(_TexFrontWarm, IN.texturePos * _TexFrontWarm_ST.xy + _TexFrontWarm_ST.zw), t);
					a.a = tex2D(_TexFrontCold_Mask, IN.texturePos * _TexFrontCold_Mask_ST.xy + _TexFrontCold_Mask_ST.zw).r;
					a.a = lerp(a.a, tex2D(_TexFrontWarm_Mask, IN.texturePos * _TexFrontWarm_Mask_ST.xy + _TexFrontWarm_Mask_ST.zw).r, t);

					n.xy *= lerp(_TexFrontCold_NStr, _TexFrontWarm_NStr, t);

					mse = tex2D(_TexFrontCold_MSE, IN.texturePos * _TexFrontCold_ST.xy + _TexFrontCold_ST.zw);
					e = pow(1 - mse.z, 2) * _TexFrontCold_Emissive;
					float3 mseWarm = tex2D(_TexFrontWarm_MSE, IN.texturePos * _TexFrontWarm_ST.xy + _TexFrontWarm_ST.zw);
					mse = lerp(mse, mseWarm, t);
					e = lerp(e, pow(1 - mseWarm.z, 2) * _TexFrontWarm_Emissive, t);
				}
				else if (k <= _KelvinMinMax.z)
				{
					// Calculate how hot the planet is, counteracting the warm
					float t = warmToHot(k);

					a = tex2D(_TexFrontWarm, IN.texturePos * _TexFrontWarm_ST.xy + _TexFrontWarm_ST.zw);
					a = lerp(a, tex2D(_TexFrontHot, IN.texturePos * _TexFrontHot_ST.xy + _TexFrontHot_ST.zw), t);
					a.a = tex2D(_TexFrontWarm_Mask, IN.texturePos * _TexFrontWarm_Mask_ST.xy + _TexFrontWarm_Mask_ST.zw).r;
					a.a = lerp(a.a, tex2D(_TexFrontHot_Mask, IN.texturePos * _TexFrontHot_Mask_ST.xy + _TexFrontHot_Mask_ST.zw).r, t);

					n.xy *= lerp(_TexFrontWarm_NStr, _TexFrontHot_NStr, t);

					mse = tex2D(_TexFrontWarm_MSE, IN.texturePos * _TexFrontWarm_ST.xy + _TexFrontWarm_ST.zw);
					e = pow(1 - mse.z, 2) * _TexFrontWarm_Emissive;
					float3 mseHot = tex2D(_TexFrontHot_MSE, IN.texturePos * _TexFrontHot_ST.xy + _TexFrontHot_ST.zw);
					mse = lerp(mse, mseHot, t);
					float eRamp = pow(1 - mseHot.z, 2);
					e = lerp(e, eRamp * _TexFrontHot_Emissive * k / _KelvinMinMax.w, t);
				}
				else
				{
					a = tex2D(_TexFrontHot, IN.texturePos * _TexFrontHot_ST.xy + _TexFrontHot_ST.zw);
					a.a = tex2D(_TexFrontHot_Mask, IN.texturePos * _TexFrontHot_Mask_ST.xy + _TexFrontHot_Mask_ST.zw).r;

					n.xy *= _TexFrontHot_NStr;

					mse = tex2D(_TexFrontHot_MSE, IN.texturePos * _TexFrontHot_ST.xy + _TexFrontHot_ST.zw);
					float eRamp = pow(1 - mse.z, 2);
					e = eRamp * _TexFrontHot_Emissive * k / _KelvinMinMax.w;
				}

				a.a = a.a * (1 - waterElevation(mse.b, IN.texturePos));

				if (n.z < 0)
					n.z *= -1;
			}

			o.Albedo = a;
			o.Alpha = a.a;
			o.Normal = normalize(n);
			o.Metallic = mse.x;
			o.Smoothness = mse.y;
			o.Emission = e;
		}
		ENDCG

		/***********************************************************************************************************************************************************************************************************
		POLAR LAYERS
		***********************************************************************************************************************************************************************************************************/

		CGPROGRAM
		#pragma surface surf WrapScattering fullforwardshadows vertex:vert alpha:fade
		#include "Assets/Surfaces/Shaders/surfaceLighting.cginc"
		#include "Assets/Surfaces/Shaders/planetHelper.cginc"

		float _EnablePoles;

		sampler2D _TexPole_Norm;
		float4 _TexPole_Norm_ST;

		// _KelvinMinMax.x
		sampler2D _TexPoleCold;
		float4 _TexPoleCold_ST;
		sampler2D _TexPoleCold_Mask;
		float4 _TexPoleCold_Mask_ST;
		/*sampler2D _TexPoleCold_SMask;
		float4 _TexPoleCold_SMask_ST;*/
		// RGB emissive colour
		half3 _TexPoleCold_Emissive;
		// RGB encoded to Metallic Smoothness Emissive
		sampler2D _TexPoleCold_MSE;
		// Scalar normal strength modifier;
		float _TexPoleCold_NStr;

		// _KelvinMinMax.y
		sampler2D _TexPoleWarm;
		float4 _TexPoleWarm_ST;
		sampler2D _TexPoleWarm_Mask;
		float4 _TexPoleWarm_Mask_ST;
		/*sampler2D _TexPoleWarm_SMask;
		float4 _TexPoleWarm_SMask_ST;*/
		// RGB emissive colour
		half3 _TexPoleWarm_Emissive;
		// RGB encoded to Metallic Smoothness Emissive
		sampler2D _TexPoleWarm_MSE;
		// Scalar normal strength modifier;
		float _TexPoleWarm_NStr;

		// _KelvinMinMax.z
		sampler2D _TexPoleHot;
		float4 _TexPoleHot_ST;
		sampler2D _TexPoleHot_Mask;
		float4 _TexPoleHot_Mask_ST;
		/*sampler2D _TexPoleHot_SMask;
		float4 _TexPoleHot_SMask_ST;*/
		// RGB emissive colour
		half3 _TexPoleHot_Emissive;
		// RGB encoded to Metallic Smoothness Emissive
		sampler2D _TexPoleHot_MSE;
		// Scalar normal strength modifier;
		float _TexPoleHot_NStr;

		struct Input
		{
			float3 localPos;
			float2 texturePos;
		};

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.localPos = v.vertex.xyz;
			float2 vertex = v.vertex.xz;
			o.texturePos = vertex + float2(0.5, 0.5);
		}

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			float4 a = 0;
			float3 n = UnpackNormal(tex2D(_TexPole_Norm, IN.texturePos * _TexPole_Norm_ST.xy + _TexPole_Norm_ST.zw));
			float3 mse = 0;
			float3 e = 0;

			if (_EnablePoles == 1)
			{
				float fade = IN.localPos.y;

				float k = _Kelvin + _KelvinRange * ((1 - abs(fade)) * 2 - 1);

				if (k < _KelvinMinMax.x)
				{
					a = tex2D(_TexPoleCold, IN.texturePos * _TexPoleCold_ST.xy + _TexPoleCold_ST.zw);
					a.a = tex2D(_TexPoleCold_Mask, IN.texturePos * _TexPoleCold_Mask_ST.xy + _TexPoleCold_Mask_ST.zw).r;
					
					n.xy *= _TexPoleCold_NStr;

					mse = tex2D(_TexPoleCold_MSE, IN.texturePos * _TexPoleCold_ST.xy + _TexPoleCold_ST.zw);
					e = pow(1 - mse.z, 2) * _TexPoleCold_Emissive;
				}
				else if (k < _KelvinMinMax.y)
				{
					// Calculate how warm the planet is, counteracting the cold
					float t = coldToWarm(k);

					a = tex2D(_TexPoleCold, IN.texturePos * _TexPoleCold_ST.xy + _TexPoleCold_ST.zw);
					a = lerp(a, tex2D(_TexPoleWarm, IN.texturePos * _TexPoleWarm_ST.xy + _TexPoleWarm_ST.zw), t);
					a.a = tex2D(_TexPoleCold_Mask, IN.texturePos * _TexPoleCold_Mask_ST.xy + _TexPoleCold_Mask_ST.zw).r;
					a.a = lerp(a.a, tex2D(_TexPoleWarm_Mask, IN.texturePos * _TexPoleWarm_Mask_ST.xy + _TexPoleWarm_Mask_ST.zw).r, t);
					
					n.xy *= lerp(_TexPoleCold_NStr, _TexPoleWarm_NStr, t);

					mse = tex2D(_TexPoleCold_MSE, IN.texturePos * _TexPoleCold_ST.xy + _TexPoleCold_ST.zw);
					e = pow(1 - mse.z, 2) * _TexPoleCold_Emissive;
					float3 mseWarm = tex2D(_TexPoleWarm_MSE, IN.texturePos * _TexPoleWarm_ST.xy + _TexPoleWarm_ST.zw);
					mse = lerp(mse, mseWarm, t);
					e = lerp(e, pow(1 - mseWarm.z, 2) * _TexPoleWarm_Emissive, t);
				}
				else if (k < _KelvinMinMax.z)
				{
					// Calculate how hot the planet is, counteracting the warm
					float t = warmToHot(k);

					a = tex2D(_TexPoleWarm, IN.texturePos * _TexPoleWarm_ST.xy + _TexPoleWarm_ST.zw);
					a = lerp(a, tex2D(_TexPoleHot, IN.texturePos * _TexPoleHot_ST.xy + _TexPoleHot_ST.zw), t);
					a.a = tex2D(_TexPoleWarm_Mask, IN.texturePos * _TexPoleWarm_Mask_ST.xy + _TexPoleWarm_Mask_ST.zw).r;
					a.a = lerp(a.a, tex2D(_TexPoleHot_Mask, IN.texturePos * _TexPoleHot_Mask_ST.xy + _TexPoleHot_Mask_ST.zw).r, t);

					n.xy *= lerp(_TexPoleWarm_NStr, _TexPoleHot_NStr, t);

					mse = tex2D(_TexPoleWarm_MSE, IN.texturePos * _TexPoleWarm_ST.xy + _TexPoleWarm_ST.zw);
					e = pow(1 - mse.z, 2) * _TexPoleWarm_Emissive;
					float3 mseHot = tex2D(_TexPoleHot_MSE, IN.texturePos * _TexPoleHot_ST.xy + _TexPoleHot_ST.zw);
					mse = lerp(mse, mseHot, t);
					e = lerp(e, pow(1 - mseHot.z, 2) * _TexPoleHot_Emissive, t);
				}
				else
				{
					a = tex2D(_TexPoleHot, IN.texturePos * _TexPoleHot_ST.xy + _TexPoleHot_ST.zw);
					a.a = tex2D(_TexPoleHot_Mask, IN.texturePos * _TexPoleHot_Mask_ST.xy + _TexPoleHot_Mask_ST.zw).r;

					n.xy *= _TexPoleHot_NStr;

					mse = tex2D(_TexPoleHot_MSE, IN.texturePos * _TexPoleHot_ST.xy + _TexPoleHot_ST.zw);
					e = pow(1 - mse.z, 2) * _TexPoleHot_Emissive * _Kelvin / _KelvinMinMax.z;
				}

				a.a = sin(a.a * 3.14152 / 2);

				if (a.a > 0.5)
					a.a = 1;
				else
					a.a = saturate(lerp(0, 1, (a.a - 0.4) / 0.1));

				if (n.z < 0)
					n.z *= -1;
			}

			o.Albedo = a;
			o.Alpha = a.a;
			o.Normal = normalize(n);
			o.Metallic = mse.x;
			o.Smoothness = mse.y;
			o.Emission = e;
		}
		ENDCG

		/***********************************************************************************************************************************************************************************************************
		CLOUDS
		***********************************************************************************************************************************************************************************************************/

		CGPROGRAM
		#pragma surface surf WrapScatteringClouds fullforwardshadows vertex:vert alpha:fade
		#include "Assets/Surfaces/Shaders/surfaceLighting.cginc"
		#include "Assets/Surfaces/Shaders/noise.cginc"
		#include "Assets/Surfaces/Shaders/planetHelper.cginc"

		float _EnableClouds;
			
		float _CloudsSlope;
		float _CloudsSpeed;
		float _CloudsTempScale;
		float _CloudsTurbulence;
		float _CloudsTurbScale;
		float _CloudsTurbOctaves;

		sampler2D _TexClouds_Norm;
		float4 _TexClouds_Norm_ST;

		half3 _TexCloudsCold_Colour;
		sampler2D _TexCloudsCold;
		float4 _TexCloudsCold_ST;
		float _TexCloudsCold_NStr;

		half3 _TexCloudsWarm_Colour;
		sampler2D _TexCloudsWarm;
		float4 _TexCloudsWarm_ST;
		float _TexCloudsWarm_NStr;

		half3 _TexCloudsHot_Colour;
		sampler2D _TexCloudsHot;
		float4 _TexCloudsHot_ST;
		float _TexCloudsHot_NStr;

		struct Input
		{
			float3 localPos;
			float2 texturePos;
		};

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.localPos = v.vertex.xyz;
			o.texturePos = v.texcoord.xy;
		}

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			float4 a = float4(1,1,1,0);

			float timeScale = _Time.x * _CloudsSpeed *_CloudsTempScale * _Kelvin;
			float2 time = float2(timeScale, 0);
			float3 n = UnpackNormal(tex2D(_TexClouds_Norm, (IN.texturePos + time) * _TexClouds_Norm_ST.xy + _TexClouds_Norm_ST.zw + time));

			float3 e = 0;

			if (_EnableClouds == 1)
			{
				float fade = 1 - abs(IN.localPos.y);

				float aFade = pow(fade, 0.5);

				float k = _Kelvin + _KelvinRange * (fade * 2 - 1) * (1 - tex2D(_Elevation_Mask, IN.texturePos * _Elevation_Mask_ST.xy + _Elevation_Mask_ST.zw));

				float elevation = surfaceElevation(0, IN.texturePos);

				float3 nRot = float3(timeScale * 0, 0, timeScale * 0);
				float noise = snoise4(float4((IN.localPos.xyz - nRot) * _CloudsTurbScale, timeScale * 10), _CloudsTurbOctaves) * 2 - 1;
				elevation = 1 - abs((_Elevation - elevation) / _Elevation);
				elevation = saturate(elevation + abs(noise));

				float2 samplePos = IN.texturePos + IN.localPos.xz * noise * _CloudsTurbulence + time;

				if (k <= _KelvinMinMax.x)
				{
					a.rgb = _TexCloudsCold_Colour;
					a.a = tex2D(_TexCloudsCold, (samplePos + time) * _TexCloudsCold_ST.xy + _TexCloudsCold_ST.zw + time) * saturate(_Kelvin / _KelvinMinMax.x);

					n.xy *= _TexCloudsCold_NStr;
				}
				else if (k <= _KelvinMinMax.y)
				{
					// Calculate how warm the planet is, counteracting the cold
					float t = coldToWarm(k);

					a.rgb = _TexCloudsCold_Colour;
					a.a = tex2D(_TexCloudsCold, (samplePos + time) * _TexCloudsCold_ST.xy + _TexCloudsCold_ST.zw);
					a.rgb = lerp(a.rgb, _TexCloudsWarm_Colour, t);
					a.a = lerp(a.a, tex2D(_TexCloudsWarm, (samplePos + time) * _TexCloudsWarm_ST.xy + _TexCloudsWarm_ST.zw), t);

					n.xy *= lerp(_TexCloudsCold_NStr, _TexCloudsWarm_NStr, t);
				}
				else if (k <= _KelvinMinMax.z)
				{
					// Calculate how hot the planet is, counteracting the warm
					float t = warmToHot(k);

					a.rgb = _TexCloudsWarm_Colour;
					a.a = tex2D(_TexCloudsWarm, (samplePos + time) * _TexCloudsWarm_ST.xy + _TexCloudsWarm_ST.zw);
					a.rgb = lerp(a.rgb, _TexCloudsHot_Colour, t);
					a.a = lerp(a.a, tex2D(_TexCloudsHot, (samplePos + time) * _TexCloudsHot_ST.xy + _TexCloudsHot_ST.zw), t);

					n.xy *= lerp(_TexCloudsWarm_NStr, _TexCloudsHot_NStr, t);
				}
				else
				{
					a.rgb = _TexCloudsHot_Colour;
					a.a = tex2D(_TexCloudsHot, (samplePos + time) * _TexCloudsHot_ST.xy + _TexCloudsHot_ST.zw) * saturate(1 - ((k - _KelvinMinMax.z) / (_KelvinMinMax.w - _KelvinMinMax.z)));

					n.xy *= _TexCloudsHot_NStr;
				}

				float poleFade = 1 - pow(fade, 0.2);
				a.a = lerp(a.a, saturate(noise * 0.5 + 0.5), poleFade);

				a.a = saturate(a.a * (noise * 0.5 + 0.5) / _CloudsSlope) * aFade;
				a.a = lerp(a.a, 1, saturate((k - _KelvinMinMax.z) / (_KelvinMinMax.w - _KelvinMinMax.z)));

				n = lerp(normalize(n), float3(0, 0, 1), 1 - pow(fade, 0.75));
				if (n.z < 0)
					n.z *= -1;
			}

			o.Albedo = a.rgb;
			o.Alpha = a.a;
			o.Normal = n;
			o.Metallic = 0;
			o.Smoothness = 0;
			o.Emission = e;
		}

		ENDCG
	}
	FallBack "Diffuse"
}
