Shader "Surfaces/Planet" 
{
	Properties
	{
		_Kelvin("Kelvin", Float) = 0
		_Phase("Land Phase", 2D) = "white" {}
		_PhaseClouds("Cloud Phase", 2D) = "white" {}

		// COLD
		_TexBackCold("COLD Back (RGB)", 2D) = "white" {}
		[HDR]_TexBackCold_Emissive("COLD Back Emissive", Color) = (0,0,0,0)
		[NoScaleOffset]_TexBackCold_MSE("COLD Back MSE (RGB)", 2D) = "grey" {}
		_TexBackCold_NStr("COLD Back Normal Strength", Range(0,10)) = 1
		[Normal]_TexBackCold_Norm("COLD Back Normal (BUMP)", 2D) = "bump" {}

		_TexMidCold_Mask("COLD Mid Mask (R)", 2D) = "black" {}
		_TexMidCold("COLD Mid (RGB)", 2D) = "white" {}
		[HDR]_TexMidCold_Emissive("COLD Mid Emissive", Color) = (0,0,0,0)
		[NoScaleOffset]_TexMidCold_MSE("COLD Mid MSE (RGB)", 2D) = "grey" {}
		_TexMidCold_NStr("COLD Mid Normal Strength", Range(0,10)) = 1
		[Normal]_TexMidCold_Norm("COLD Mid Normal (BUMP)", 2D) = "bump" {}

		_TexFrontCold_Mask("COLD Front Mask (R)", 2D) = "black" {}
		_TexFrontCold("COLD Front (RGB)", 2D) = "white" {}
		[HDR]_TexFrontCold_Emissive("COLD Front Emissive (RGB)", Color) = (0,0,0,0)
		[NoScaleOffset]_TexFrontCold_MSE("COLD Front MSE", 2D) = "grey" {}
		_TexFrontCold_NStr("COLD Front Normal Strength", Range(0,10)) = 1
		[Normal]_TexFrontCold_Norm("COLD Front Normal (BUMP)", 2D) = "bump" {}

		// WARM
		_TexBackWarm("WARM Back (RGB)", 2D) = "white" {}
		[HDR]_TexBackWarm_Emissive("WARM Back Emissive", Color) = (0,0,0,0)
		[NoScaleOffset]_TexBackWarm_MSE("WARM Back MSE (RGB)", 2D) = "grey" {}
		_TexBackWarm_NStr("WARM Back Normal Strength", Range(0,10)) = 1
		[Normal]_TexBackWarm_Norm("WARM Back Normal (BUMP)", 2D) = "bump" {}

		_TexMidWarm_Mask("WARM Mid Mask (R)", 2D) = "black" {}
		_TexMidWarm("WARM Mid (RGB)", 2D) = "white" {}
		[HDR]_TexMidWarm_Emissive("WARM Mid Emissive", Color) = (0,0,0,0)
		[NoScaleOffset]_TexMidWarm_MSE("WARM Mid MSE (RGB)", 2D) = "grey" {}
		_TexMidWarm_NStr("WARM Mid Normal Strength", Range(0,10)) = 1
		[Normal]_TexMidWarm_Norm("WARM Mid Normal (BUMP)", 2D) = "bump" {}
		
		_TexFrontWarm_Mask("WARM Front Mask (R)", 2D) = "black" {}
		_TexFrontWarm("WARM Front (RGB)", 2D) = "white" {}
		[HDR]_TexFrontWarm_Emissive("WARM Front Emissive", Color) = (0,0,0,0)
		[NoScaleOffset]_TexFrontWarm_MSE("WARM Front MSE (RGB)", 2D) = "grey" {}
		_TexFrontWarm_NStr("WARM Front Normal Strength", Range(0,10)) = 1
		[Normal]_TexFrontWarm_Norm("WARM Front Normal (BUMP)", 2D) = "bump" {}

		// HOT
		_TexBackHot("HOT Back (RGB)", 2D) = "white" {}
		[HDR]_TexBackHot_Emissive("HOT Back Emissive", Color) = (0,0,0,0)
		[NoScaleOffset]_TexBackHot_MSE("HOT Back MSE (RGB)", 2D) = "grey" {}
		_TexBackHot_NStr("HOT Back Normal Strength", Range(0,10)) = 1
		[Normal]_TexBackHot_Norm("HOT Back Normal (BUMP)", 2D) = "bump" {}

		_TexMidHot_Mask("HOT Mid Mask", 2D) = "black" {}
		_TexMidHot("HOT Mid (RGB)", 2D) = "white" {}
		[HDR]_TexMidHot_Emissive("HOT Mid Emissive", Color) = (0,0,0,0)
		[NoScaleOffset]_TexMidHot_MSE("HOT Mid MSE (RGB)", 2D) = "grey" {}
		_TexMidHot_NStr("HOT Mid Normal Strength", Range(0,10)) = 1
		[Normal]_TexMidHot_Norm("HOT Mid Normal (BUMP)", 2D) = "bump" {}

		_TexFrontHot_Mask("HOT Front Mask (R)", 2D) = "black" {}
		_TexFrontHot("HOT Front Hot (RGB)", 2D) = "white" {}
		[HDR]_TexFrontHot_Emissive("HOT Front Emissive", Color) = (0,0,0,0)
		[NoScaleOffset]_TexFrontHot_MSE("HOT Front MSE (RGB)", 2D) = "grey" {}
		_TexFrontHot_NStr("HOT Front Normal Strength", Range(0,10)) = 1
		[Normal]_TexFrontHot_Norm("HOT Front Normal (BUMP)", 2D) = "bump" {}

		// POLES
		_TexPoleCold_Mask("COLD Polar Mask (R)", 2D) = "black" {}
		_TexPoleCold("COLD Pole (RGB)", 2D) = "white" {}
		[HDR]_TexPoleCold_Emissive("COLD Pole Emissive", Color) = (0,0,0,0)
		[NoScaleOffset]_TexPoleCold_MSE("COLD Pole MSE (RGB)", 2D) = "grey" {}
		_TexPoleCold_NStr("COLD Pole Normal Strength", Range(0,10)) = 1
		[Normal]_TexPoleCold_Norm("COLD Pole Normal (BUMP)", 2D) = "bump" {}

		_TexPoleWarm_Mask("WARM Polar Mask (R)", 2D) = "black" {}
		_TexPoleWarm("WARM Pole (RGB)", 2D) = "white" {}
		[HDR]_TexPoleWarm_Emissive("WARM Pole Emissive", Color) = (0,0,0,0)
		[NoScaleOffset]_TexPoleWarm_MSE("WARM Pole MSE (RGB)", 2D) = "grey" {}
		_TexPoleWarm_NStr("WARM Pole Normal Strength", Range(0,10)) = 1
		[Normal]_TexPoleWarm_Norm("WARM Pole Normal (BUMP)", 2D) = "bump" {}

		_TexPoleHot_Mask("HOT Polar Mask (R)", 2D) = "black" {}
		_TexPoleHot("HOT Pole (RGB)", 2D) = "white" {}
		[HDR]_TexPoleHot_Emissive("HOT Pole Emissive", Color) = (0,0,0,0)
		[NoScaleOffset]_TexPoleHot_MSE("HOT Pole MSE (RGB)", 2D) = "grey" {}
		_TexPoleHot_NStr("HOT Pole Normal Strength", Range(0,10)) = 1
		[Normal]_TexPoleHot_Norm("HOT Pole Normal (BUMP)", 2D) = "bump" {}

		// CLOUDS
		_CloudsSpeed("Cloud Speed", Float) = 0.01
		_CloudsTempScale("Cloud Speed Scaler (Kelvin)", Float) = 0.01

		_TexCloudsCold_Colour("COLD Cloud Colour", Color) = (1,1,1,1)
		_TexCloudsCold("COLD Clouds (R)", 2D) = "black" {}
		_TexCloudsCold_NStr("COLD Clouds Normal Strength", Float) = 1
		[Normal]_TexCloudsCold_Norm("COLD Clouds Normal (BUMP)", 2D) = "bump" {}

		_TexCloudsWarm_Colour("WARM Cloud Colour", Color) = (1,1,1,1)
		_TexCloudsWarm("WARM Clouds (R)", 2D) = "black" {}
		_TexCloudsWarm_NStr("COLD Clouds Normal Strength", Float) = 1
		[Normal]_TexCloudsWarm_Norm("WARM Clouds Normal (BUMP)", 2D) = "bump" {}

		_TexCloudsHot_Colour("HOT Cloud Colour", Color) = (1, 1, 1, 1)
		_TexCloudsHot("HOT Clouds (R)", 2D) = "black" {}
		_TexCloudsHot_NStr("COLD Clouds Normal Strength", Float) = 1
		[Normal]_TexCloudsHot_Norm("HOT Clouds Normal (BUMP)", 2D) = "bump" {}
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 400

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf WrapScattering fullforwardshadows vertex:vert
		#include "Assets/Surfaces/Shaders/surfaceLighting.cginc"

		static const float COLD = 100;
		static const float WARM = 290;
		static const float HOT = 400;

		float _Kelvin;

		// COLD
		sampler2D _TexBackCold;
		float4 _TexBackCold_ST;
		// RGB emissive colour
		half3 _TexBackCold_Emissive;
		// RGB encoded to Metallic Smoothness Emissive
		sampler2D _TexBackCold_MSE;
		// Scalar normal strength modifier;
		float _TexBackCold_NStr;
		sampler2D _TexBackCold_Norm;
		float4 _TexBackCold_Norm_ST;

		// WARM
		sampler2D _TexBackWarm;
		float4 _TexBackWarm_ST;
		// RGB emissive colour
		half3 _TexBackWarm_Emissive;
		// RGB encoded to Metallic Smoothness Emissive
		sampler2D _TexBackWarm_MSE;
		// Scalar normal strength modifier;
		float _TexBackWarm_NStr;
		sampler2D _TexBackWarm_Norm;
		float4 _TexBackWarm_Norm_ST;

		// HOT
		sampler2D _TexBackHot;
		float4 _TexBackHot_ST;
		// RGB emissive colour
		half3 _TexBackHot_Emissive;
		// RGB encoded to Metallic Smoothness Emissive
		sampler2D _TexBackHot_MSE;
		// Scalar normal strength modifier;
		float _TexBackHot_NStr;
		sampler2D _TexBackHot_Norm;
		float4 _TexBackHot_Norm_ST;

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
			float3 a = 0;
			float3 n = (0,0,1);
			float3 mse = 0;
			float3 e = 0;

			if (_Kelvin < COLD)
			{
				a = tex2D(_TexBackCold, IN.texturePos * _TexBackCold_ST.xy + _TexBackCold_ST.zw);

				n = UnpackNormal(tex2D(_TexBackCold_Norm, IN.texturePos * _TexBackCold_Norm_ST.xy + _TexBackCold_Norm_ST.zw));
				n.xy *= _TexBackCold_NStr;

				mse = tex2D(_TexBackCold_MSE, IN.texturePos * _TexBackCold_ST.xy + _TexBackCold_ST.zw);

				e = mse.z * _TexBackCold_Emissive;
			}
			else if (_Kelvin < WARM)
			{
				// Calculate how warm the planet is, counteracting the cold
				float t = (_Kelvin - COLD) / (WARM - COLD);
				a = tex2D(_TexBackCold, IN.texturePos * _TexBackCold_ST.xy + _TexBackCold_ST.zw);
				a = lerp(a, tex2D(_TexBackWarm, IN.texturePos * _TexBackWarm_ST.xy + _TexBackWarm_ST.zw), t);

				n = UnpackNormal(tex2D(_TexBackCold_Norm, IN.texturePos * _TexBackCold_Norm_ST.xy + _TexBackCold_Norm_ST.zw));
				n.xy *= _TexBackCold_NStr;
				float3 goalN = UnpackNormal(tex2D(_TexBackWarm_Norm, IN.texturePos * _TexBackWarm_Norm_ST.xy + _TexBackWarm_Norm_ST.zw));
				goalN.xy *= _TexBackWarm_NStr;

				n = lerp(n, goalN, t);

				mse = tex2D(_TexBackCold_MSE, IN.texturePos * _TexBackCold_ST.xy + _TexBackCold_ST.zw);
				e = mse.z * _TexBackCold_Emissive;
				float3 mseWarm = tex2D(_TexBackWarm_MSE, IN.texturePos * _TexBackWarm_ST.xy + _TexBackWarm_ST.zw);
				mse = lerp(mse, mseWarm, t);
				e = lerp(e, mseWarm.z * _TexBackWarm_Emissive, t);
			}
			else if (_Kelvin < HOT)
			{
				// Calculate how hot the planet is, counteracting the warm
				float t = (_Kelvin - WARM) / (HOT - WARM);
				a = tex2D(_TexBackWarm, IN.texturePos * _TexBackWarm_ST.xy + _TexBackWarm_ST.zw);
				a = lerp(a, tex2D(_TexBackHot, IN.texturePos * _TexBackHot_ST.xy + _TexBackHot_ST.zw), t);

				n = UnpackNormal(tex2D(_TexBackWarm_Norm, IN.texturePos * _TexBackWarm_Norm_ST.xy + _TexBackWarm_Norm_ST.zw));
				n.xy *= _TexBackWarm_NStr;
				float3 goalN = UnpackNormal(tex2D(_TexBackHot_Norm, IN.texturePos * _TexBackHot_Norm_ST.xy + _TexBackHot_Norm_ST.zw));
				goalN.xy *= _TexBackHot_NStr;

				n = lerp(n, goalN, t);

				mse = tex2D(_TexBackWarm_MSE, IN.texturePos * _TexBackWarm_ST.xy + _TexBackWarm_ST.zw);
				e = mse.z * _TexBackWarm_Emissive;
				float3 mseHot = tex2D(_TexBackHot_MSE, IN.texturePos * _TexBackHot_ST.xy + _TexBackHot_ST.zw);
				mse = lerp(mse, mseHot, t);
				e = lerp(e, mseHot.z * _TexBackHot_Emissive, t);
			}
			else
			{
				a = tex2D(_TexBackHot, IN.texturePos * _TexBackHot_ST.xy + _TexBackHot_ST.zw);

				n = UnpackNormal(tex2D(_TexBackHot_Norm, IN.texturePos * _TexBackHot_Norm_ST.xy + _TexBackHot_Norm_ST.zw));
				n.xy *= _TexBackHot_NStr;

				mse = tex2D(_TexBackHot_MSE, IN.texturePos * _TexBackHot_ST.xy + _TexBackHot_ST.zw);

				e = mse.z * _TexBackHot_Emissive * _Kelvin / HOT;
			}

			o.Albedo = a;
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
		
		static const float COLD = 100;
		static const float WARM = 290;
		static const float HOT = 400;

		float _Kelvin;

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
		sampler2D _TexMidCold_Norm;
		float4 _TexMidCold_Norm_ST;

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
		sampler2D _TexMidWarm_Norm;
		float4 _TexMidWarm_Norm_ST;

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
		sampler2D _TexMidHot_Norm;
		float4 _TexMidHot_Norm_ST;

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
			float3 n = (0, 0, 1);
			float3 mse = 0;
			float3 e = 0;

			if (_Kelvin < COLD)
			{
				a = tex2D(_TexMidCold, IN.texturePos * _TexMidCold_ST.xy + _TexMidCold_ST.zw);
				a.a = tex2D(_TexMidCold_Mask, IN.texturePos * _TexMidCold_Mask_ST.xy + _TexMidCold_Mask_ST.zw).r;

				n = UnpackNormal(tex2D(_TexMidCold_Norm, IN.texturePos * _TexMidCold_Norm_ST.xy + _TexMidCold_Norm_ST.zw));
				n.xy *= _TexMidCold_NStr;

				mse = tex2D(_TexMidCold_MSE, IN.texturePos * _TexMidCold_ST.xy + _TexMidCold_ST.zw);
				e = mse.z * _TexMidCold_Emissive;
			}
			else if (_Kelvin < WARM)
			{
				// Calculate how warm the planet is, counteracting the cold
				float t = (_Kelvin - COLD) / (WARM - COLD);

				a = tex2D(_TexMidCold, IN.texturePos * _TexMidCold_ST.xy + _TexMidCold_ST.zw);
				a = lerp(a, tex2D(_TexMidWarm, IN.texturePos * _TexMidWarm_ST.xy + _TexMidWarm_ST.zw), t);
				a.a = tex2D(_TexMidCold_Mask, IN.texturePos * _TexMidCold_Mask_ST.xy + _TexMidCold_Mask_ST.zw).r;
				a.a = lerp(a.a, tex2D(_TexMidWarm_Mask, IN.texturePos * _TexMidWarm_Mask_ST.xy + _TexMidWarm_Mask_ST.zw).r, t);

				n = UnpackNormal(tex2D(_TexMidCold_Norm, IN.texturePos * _TexMidCold_Norm_ST.xy + _TexMidCold_Norm_ST.zw));
				n.xy *= _TexMidCold_NStr;
				float3 goalN = UnpackNormal(tex2D(_TexMidWarm_Norm, IN.texturePos * _TexMidWarm_Norm_ST.xy + _TexMidWarm_Norm_ST.zw));
				goalN.xy *= _TexMidWarm_NStr;

				n = lerp(n, goalN, t);

				mse = tex2D(_TexMidCold_MSE, IN.texturePos * _TexMidCold_ST.xy + _TexMidCold_ST.zw);
				e = mse.z * _TexMidCold_Emissive;
				float3 mseWarm = tex2D(_TexMidWarm_MSE, IN.texturePos * _TexMidWarm_ST.xy + _TexMidWarm_ST.zw);
				mse = lerp(mse, mseWarm, t);
				e = lerp(e, mseWarm.z * _TexMidWarm_Emissive, t);
			}
			else if (_Kelvin < HOT)
			{
				// Calculate how hot the planet is, counteracting the warm
				float t = (_Kelvin - WARM) / (HOT - WARM);

				a = tex2D(_TexMidWarm, IN.texturePos * _TexMidWarm_ST.xy + _TexMidWarm_ST.zw);
				a = lerp(a, tex2D(_TexMidHot, IN.texturePos * _TexMidHot_ST.xy + _TexMidHot_ST.zw), t);
				a.a = tex2D(_TexMidWarm_Mask, IN.texturePos * _TexMidWarm_Mask_ST.xy + _TexMidWarm_Mask_ST.zw).r;
				a.a = lerp(a.a, tex2D(_TexMidHot_Mask, IN.texturePos * _TexMidHot_Mask_ST.xy + _TexMidHot_Mask_ST.zw).r, t);

				n = UnpackNormal(tex2D(_TexMidWarm_Norm, IN.texturePos * _TexMidWarm_Norm_ST.xy + _TexMidWarm_Norm_ST.zw));
				n.xy *= _TexMidWarm_NStr;
				float3 goalN = UnpackNormal(tex2D(_TexMidHot_Norm, IN.texturePos * _TexMidHot_Norm_ST.xy + _TexMidHot_Norm_ST.zw));
				goalN.xy *= _TexMidHot_NStr;

				n = lerp(n, goalN, t);

				mse = tex2D(_TexMidWarm_MSE, IN.texturePos * _TexMidWarm_ST.xy + _TexMidWarm_ST.zw);
				e = mse.z * _TexMidWarm_Emissive;
				float3 mseHot = tex2D(_TexMidHot_MSE, IN.texturePos * _TexMidHot_ST.xy + _TexMidHot_ST.zw);
				mse = lerp(mse, mseHot, t);
				e = lerp(e, mseHot.z * _TexMidHot_Emissive, t);
			}
			else
			{
				a = tex2D(_TexMidHot, IN.texturePos * _TexMidHot_ST.xy + _TexMidHot_ST.zw);
				a.a = tex2D(_TexMidHot_Mask, IN.texturePos * _TexMidHot_Mask_ST.xy + _TexMidHot_Mask_ST.zw).r;

				n = UnpackNormal(tex2D(_TexMidHot_Norm, IN.texturePos * _TexMidHot_Norm_ST.xy + _TexMidHot_Norm_ST.zw));
				n.xy *= _TexMidHot_NStr;

				mse = tex2D(_TexMidHot_MSE, IN.texturePos * _TexMidHot_ST.xy + _TexMidHot_ST.zw);
				e = mse.z * _TexMidHot_Emissive * _Kelvin / HOT;
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

		static const float COLD = 100;
		static const float WARM = 290;
		static const float HOT = 400;

		float _Kelvin;

		// COLD
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
		sampler2D _TexFrontCold_Norm;
		float4 _TexFrontCold_Norm_ST;

		// WARM
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
		sampler2D _TexFrontWarm_Norm;
		float4 _TexFrontWarm_Norm_ST;

		// HOT
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
		sampler2D _TexFrontHot_Norm;
		float4 _TexFrontHot_Norm_ST;

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
			float3 n = (0, 0, 1);
			float3 mse = 0;
			float3 e = 0;

			if (_Kelvin < COLD)
			{
				a = tex2D(_TexFrontCold, IN.texturePos * _TexFrontCold_ST.xy + _TexFrontCold_ST.zw);
				a.a = tex2D(_TexFrontCold_Mask, IN.texturePos * _TexFrontCold_Mask_ST.xy + _TexFrontCold_Mask_ST.zw).r;

				n = UnpackNormal(tex2D(_TexFrontCold_Norm, IN.texturePos * _TexFrontCold_Norm_ST.xy + _TexFrontCold_Norm_ST.zw));
				n.xy *= _TexFrontCold_NStr;

				mse = tex2D(_TexFrontCold_MSE, IN.texturePos * _TexFrontCold_ST.xy + _TexFrontCold_ST.zw);
				e = mse.z * _TexFrontCold_Emissive;
			}
			else if (_Kelvin < WARM)
			{
				// Calculate how warm the planet is, counteracting the cold
				float t = (_Kelvin - COLD) / (WARM - COLD);

				a = tex2D(_TexFrontCold, IN.texturePos * _TexFrontCold_ST.xy + _TexFrontCold_ST.zw);
				a = lerp(a, tex2D(_TexFrontWarm, IN.texturePos * _TexFrontWarm_ST.xy + _TexFrontWarm_ST.zw), t);
				a.a = tex2D(_TexFrontCold_Mask, IN.texturePos * _TexFrontCold_Mask_ST.xy + _TexFrontCold_Mask_ST.zw).r;
				a.a = lerp(a.a, tex2D(_TexFrontWarm_Mask, IN.texturePos * _TexFrontWarm_Mask_ST.xy + _TexFrontWarm_Mask_ST.zw).r, t);

				n = UnpackNormal(tex2D(_TexFrontCold_Norm, IN.texturePos * _TexFrontCold_Norm_ST.xy + _TexFrontCold_Norm_ST.zw));
				n.xy *= _TexFrontCold_NStr;
				float3 goalN = UnpackNormal(tex2D(_TexFrontWarm_Norm, IN.texturePos * _TexFrontWarm_Norm_ST.xy + _TexFrontWarm_Norm_ST.zw));
				goalN.xy *= _TexFrontWarm_NStr;

				n = lerp(n, goalN, t);

				mse = tex2D(_TexFrontCold_MSE, IN.texturePos * _TexFrontCold_ST.xy + _TexFrontCold_ST.zw);
				e = mse.z * _TexFrontCold_Emissive;
				float3 mseWarm = tex2D(_TexFrontWarm_MSE, IN.texturePos * _TexFrontWarm_ST.xy + _TexFrontWarm_ST.zw);
				mse = lerp(mse, mseWarm, t);
				e = lerp(e, mseWarm.z * _TexFrontWarm_Emissive, t);
			}
			else if (_Kelvin < HOT)
			{
				// Calculate how hot the planet is, counteracting the warm
				float t = (_Kelvin - WARM) / (HOT - WARM);

				a = tex2D(_TexFrontWarm, IN.texturePos * _TexFrontWarm_ST.xy + _TexFrontWarm_ST.zw);
				a = lerp(a, tex2D(_TexFrontHot, IN.texturePos * _TexFrontHot_ST.xy + _TexFrontHot_ST.zw), t);
				a.a = tex2D(_TexFrontWarm_Mask, IN.texturePos * _TexFrontWarm_Mask_ST.xy + _TexFrontWarm_Mask_ST.zw).r;
				a.a = lerp(a.a, tex2D(_TexFrontHot_Mask, IN.texturePos * _TexFrontHot_Mask_ST.xy + _TexFrontHot_Mask_ST.zw).r, t);

				n = UnpackNormal(tex2D(_TexFrontWarm_Norm, IN.texturePos * _TexFrontWarm_Norm_ST.xy + _TexFrontWarm_Norm_ST.zw));
				n.xy *= _TexFrontWarm_NStr;
				float3 goalN = UnpackNormal(tex2D(_TexFrontHot_Norm, IN.texturePos * _TexFrontHot_Norm_ST.xy + _TexFrontHot_Norm_ST.zw));
				goalN.xy *= _TexFrontHot_NStr;

				n = lerp(n, goalN, t);

				mse = tex2D(_TexFrontWarm_MSE, IN.texturePos * _TexFrontWarm_ST.xy + _TexFrontWarm_ST.zw);
				e = mse.z * _TexFrontWarm_Emissive;
				float3 mseHot = tex2D(_TexFrontHot_MSE, IN.texturePos * _TexFrontHot_ST.xy + _TexFrontHot_ST.zw);
				mse = lerp(mse, mseHot, t);
				e = lerp(e, mseHot.z * _TexFrontHot_Emissive, t);
			}
			else
			{
				a = tex2D(_TexFrontHot, IN.texturePos * _TexFrontHot_ST.xy + _TexFrontHot_ST.zw);
				a.a = tex2D(_TexFrontHot_Mask, IN.texturePos * _TexFrontHot_Mask_ST.xy + _TexFrontHot_Mask_ST.zw).r;

				n = UnpackNormal(tex2D(_TexFrontHot_Norm, IN.texturePos * _TexFrontHot_Norm_ST.xy + _TexFrontHot_Norm_ST.zw));
				n.xy *= _TexFrontHot_NStr;

				mse = tex2D(_TexFrontHot_MSE, IN.texturePos * _TexFrontHot_ST.xy + _TexFrontHot_ST.zw);
				e = mse.z * _TexFrontHot_Emissive * _Kelvin / HOT;
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
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf WrapScattering fullforwardshadows vertex:vert alpha:fade
		#include "Assets/Surfaces/Shaders/surfaceLighting.cginc"

		static const float COLD = 100;
		static const float WARM = 290;
		static const float HOT = 400;

		float _Kelvin;
		
		// COLD
		sampler2D _TexPoleCold;
		float4 _TexPoleCold_ST;
		sampler2D _TexPoleCold_Mask;
		float4 _TexPoleCold_Mask_ST;
		// RGB emissive colour
		half3 _TexPoleCold_Emissive;
		// RGB encoded to Metallic Smoothness Emissive
		sampler2D _TexPoleCold_MSE;
		// Scalar normal strength modifier;
		float _TexPoleCold_NStr;
		sampler2D _TexPoleCold_Norm;
		float4 _TexPoleCold_Norm_ST;

		// WARM
		sampler2D _TexPoleWarm;
		float4 _TexPoleWarm_ST;
		sampler2D _TexPoleWarm_Mask;
		float4 _TexPoleWarm_Mask_ST;
		// RGB emissive colour
		half3 _TexPoleWarm_Emissive;
		// RGB encoded to Metallic Smoothness Emissive
		sampler2D _TexPoleWarm_MSE;
		// Scalar normal strength modifier;
		float _TexPoleWarm_NStr;
		sampler2D _TexPoleWarm_Norm;
		float4 _TexPoleWarm_Norm_ST;

		// HOT
		sampler2D _TexPoleHot;
		float4 _TexPoleHot_ST;
		sampler2D _TexPoleHot_Mask;
		float4 _TexPoleHot_Mask_ST;
		// RGB emissive colour
		half3 _TexPoleHot_Emissive;
		// RGB encoded to Metallic Smoothness Emissive
		sampler2D _TexPoleHot_MSE;
		// Scalar normal strength modifier;
		float _TexPoleHot_NStr;
		sampler2D _TexPoleHot_Norm;
		float4 _TexPoleHot_Norm_ST;

		struct Input
		{
			float3 localPos;
			float2 texturePos;
		};

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.localPos = v.vertex.xyz;
			o.texturePos = v.vertex.xz;//v.texcoord.xy;
		}

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			float4 a = 0;
			float3 n = (0.5, 0.5, 1);
			float3 mse = 0;
			float3 e = 0;

			if (_Kelvin < COLD)
			{
				a = tex2D(_TexPoleCold, IN.texturePos * _TexPoleCold_ST.xy + _TexPoleCold_ST.zw);
				a.a = tex2D(_TexPoleCold_Mask, IN.texturePos * _TexPoleCold_Mask_ST.xy + _TexPoleCold_Mask_ST.zw).r;

				n = UnpackNormal(tex2D(_TexPoleCold_Norm, IN.texturePos * _TexPoleCold_Norm_ST.xy + _TexPoleCold_Norm_ST.zw));
				n.xy *= _TexPoleCold_NStr;

				mse = tex2D(_TexPoleCold_MSE, IN.texturePos * _TexPoleCold_ST.xy + _TexPoleCold_ST.zw);
				e = mse.z * _TexPoleCold_Emissive;
			}
			else if (_Kelvin < WARM)
			{
				// Calculate how warm the planet is, counteracting the cold
				float t = (_Kelvin - COLD) / (WARM - COLD);

				a = tex2D(_TexPoleCold, IN.texturePos * _TexPoleCold_ST.xy + _TexPoleCold_ST.zw);
				a = lerp(a, tex2D(_TexPoleWarm, IN.texturePos * _TexPoleWarm_ST.xy + _TexPoleWarm_ST.zw), t);
				a.a = tex2D(_TexPoleCold_Mask, IN.texturePos * _TexPoleCold_Mask_ST.xy + _TexPoleCold_Mask_ST.zw).r;
				a.a = lerp(a.a, tex2D(_TexPoleWarm_Mask, IN.texturePos * _TexPoleWarm_Mask_ST.xy + _TexPoleWarm_Mask_ST.zw).r, t);

				n = UnpackNormal(tex2D(_TexPoleCold_Norm, IN.texturePos * _TexPoleCold_Norm_ST.xy + _TexPoleCold_Norm_ST.zw));
				n.xy *= _TexPoleCold_NStr;
				float3 goalN = UnpackNormal(tex2D(_TexPoleWarm_Norm, IN.texturePos * _TexPoleWarm_Norm_ST.xy + _TexPoleWarm_Norm_ST.zw));
				goalN.xy *= _TexPoleWarm_NStr;

				n = lerp(n, goalN, t);

				mse = tex2D(_TexPoleCold_MSE, IN.texturePos * _TexPoleCold_ST.xy + _TexPoleCold_ST.zw);
				e = mse.z * _TexPoleCold_Emissive;
				float3 mseWarm = tex2D(_TexPoleWarm_MSE, IN.texturePos * _TexPoleWarm_ST.xy + _TexPoleWarm_ST.zw);
				mse = lerp(mse, mseWarm, t);
				e = lerp(e, mseWarm.z * _TexPoleWarm_Emissive, t);
			}
			else if (_Kelvin < HOT)
			{
				// Calculate how hot the planet is, counteracting the warm
				float t = (_Kelvin - WARM) / (HOT - WARM);

				a = tex2D(_TexPoleWarm, IN.texturePos * _TexPoleWarm_ST.xy + _TexPoleWarm_ST.zw);
				a = lerp(a, tex2D(_TexPoleHot, IN.texturePos * _TexPoleHot_ST.xy + _TexPoleHot_ST.zw), t);
				a.a = tex2D(_TexPoleWarm_Mask, IN.texturePos * _TexPoleWarm_Mask_ST.xy + _TexPoleWarm_Mask_ST.zw).r;
				a.a = lerp(a.a, tex2D(_TexPoleHot_Mask, IN.texturePos * _TexPoleHot_Mask_ST.xy + _TexPoleHot_Mask_ST.zw).r, t);

				n = UnpackNormal(tex2D(_TexPoleWarm_Norm, IN.texturePos * _TexPoleWarm_Norm_ST.xy + _TexPoleWarm_Norm_ST.zw));
				n.xy *= _TexPoleWarm_NStr;
				float3 goalN = UnpackNormal(tex2D(_TexPoleHot_Norm, IN.texturePos * _TexPoleHot_Norm_ST.xy + _TexPoleHot_Norm_ST.zw));
				goalN.xy *= _TexPoleHot_NStr;

				n = lerp(n, goalN, t);

				mse = tex2D(_TexPoleWarm_MSE, IN.texturePos * _TexPoleWarm_ST.xy + _TexPoleWarm_ST.zw);
				e = mse.z * _TexPoleWarm_Emissive;
				float3 mseHot = tex2D(_TexPoleHot_MSE, IN.texturePos * _TexPoleHot_ST.xy + _TexPoleHot_ST.zw);
				mse = lerp(mse, mseHot, t);
				e = lerp(e, mseHot.z * _TexPoleHot_Emissive, t);
			}
			else
			{
				a = tex2D(_TexPoleHot, IN.texturePos * _TexPoleHot_ST.xy + _TexPoleHot_ST.zw);
				a.a = tex2D(_TexPoleHot_Mask, IN.texturePos * _TexPoleHot_Mask_ST.xy + _TexPoleHot_Mask_ST.zw).r;

				n = UnpackNormal(tex2D(_TexPoleHot_Norm, IN.texturePos * _TexPoleHot_Norm_ST.xy + _TexPoleHot_Norm_ST.zw));
				n.xy *= _TexPoleHot_NStr;

				mse = tex2D(_TexPoleHot_MSE, IN.texturePos * _TexPoleHot_ST.xy + _TexPoleHot_ST.zw);
				e = mse.z * _TexPoleHot_Emissive * _Kelvin / HOT;
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
		#pragma surface surf WrapScatteringClouds fullforwardshadows vertex:vert alpha:fade
		#include "Assets/Surfaces/Shaders/surfaceLighting.cginc"
		#include "Assets/Surfaces/Shaders/noise.cginc"

		static const float COLD = 50;
		static const float WARM = 290;
		static const float HOT = 700;

		float _Kelvin;

		float _CloudsSpeed;
		float _CloudsTempScale;

		half3 _TexCloudsCold_Colour;
		sampler2D _TexCloudsCold;
		float4 _TexCloudsCold_ST;
		float _TexCloudsCold_NStr;
		sampler2D _TexCloudsCold_Norm;
		float4 _TexCloudsCold_Norm_ST;

		half3 _TexCloudsWarm_Colour;
		sampler2D _TexCloudsWarm;
		float4 _TexCloudsWarm_ST;
		float _TexCloudsWarm_NStr;
		sampler2D _TexCloudsWarm_Norm;
		float4 _TexCloudsWarm_Norm_ST;

		half3 _TexCloudsHot_Colour;
		sampler2D _TexCloudsHot;
		float4 _TexCloudsHot_ST;
		float _TexCloudsHot_NStr;
		sampler2D _TexCloudsHot_Norm;
		float4 _TexCloudsHot_Norm_ST;

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
			float4 a = 1;
			float3 n = (0, 0, 1);
			float3 e = 0;

			float aFade = pow(1 - abs(IN.localPos.y), 0.5);
			float2 time = float2(_Time.x * _CloudsSpeed *_CloudsTempScale * _Kelvin, 0);

			float2 samplePos = IN.texturePos.xy;// +IN.texturePos.xy * snoise4(float4(IN.localPos.xyz, time.x), 4) * 0.1;

			if (_Kelvin < COLD)
			{
				a.rgb = _TexCloudsCold_Colour;
				a.a = tex2D(_TexCloudsCold, (samplePos + time) * _TexCloudsCold_ST.xy + _TexCloudsCold_ST.zw + time) * saturate(_Kelvin / COLD) * aFade;

				n = UnpackNormal(tex2D(_TexCloudsCold_Norm, (IN.texturePos + time) * _TexCloudsCold_Norm_ST.xy + _TexCloudsCold_Norm_ST.zw + time));
				n.xy *= _TexCloudsCold_NStr;
			}
			else if (_Kelvin < WARM)
			{
				// Calculate how warm the planet is, counteracting the cold
				float t = (_Kelvin - COLD) / (WARM - COLD);

				a.rgb = _TexCloudsCold_Colour;
				a.a = tex2D(_TexCloudsCold, (samplePos + time) * _TexCloudsCold_ST.xy + _TexCloudsCold_ST.zw) * aFade;
				a.rgb = lerp(a.rgb, _TexCloudsWarm_Colour, t);
				a.a = lerp(a.a, tex2D(_TexCloudsWarm, (samplePos + time) * _TexCloudsWarm_ST.xy + _TexCloudsWarm_ST.zw) * aFade, t);

				n = UnpackNormal(tex2D(_TexCloudsCold_Norm, (IN.texturePos + time) * _TexCloudsCold_Norm_ST.xy + _TexCloudsCold_Norm_ST.zw));
				n.xy *= _TexCloudsCold_NStr;
				float3 goalN = UnpackNormal(tex2D(_TexCloudsWarm_Norm, (IN.texturePos + time) * _TexCloudsWarm_Norm_ST.xy + _TexCloudsWarm_Norm_ST.zw));
				goalN.xy *= _TexCloudsWarm_NStr;
				n = lerp(n, goalN, t);
			}
			else if (_Kelvin < HOT)
			{
				// Calculate how hot the planet is, counteracting the warm
				float t = (_Kelvin - WARM) / (HOT - WARM);

				a.rgb = _TexCloudsWarm_Colour;
				a.a = tex2D(_TexCloudsWarm, (samplePos + time) * _TexCloudsWarm_ST.xy + _TexCloudsWarm_ST.zw) * aFade;
				a.rgb = lerp(a.rgb, _TexCloudsHot_Colour, t);
				a.a = lerp(a.a, tex2D(_TexCloudsHot, (samplePos + time) * _TexCloudsHot_ST.xy + _TexCloudsHot_ST.zw) * aFade, t);

				n = UnpackNormal(tex2D(_TexCloudsWarm_Norm, (IN.texturePos + time) * _TexCloudsWarm_Norm_ST.xy + _TexCloudsWarm_Norm_ST.zw));
				n.xy *= _TexCloudsWarm_NStr;
				float3 goalN = UnpackNormal(tex2D(_TexCloudsHot_Norm, (IN.texturePos + time) * _TexCloudsHot_Norm_ST.xy + _TexCloudsHot_Norm_ST.zw));
				goalN.xy *= _TexCloudsHot_NStr;
				n = lerp(n, goalN, t);
			}
			else
			{
				a.rgb = _TexCloudsHot_Colour;
				a.a = tex2D(_TexCloudsHot, (samplePos + time) * _TexCloudsHot_ST.xy + _TexCloudsHot_ST.zw) * saturate(1 - (_Kelvin - HOT) / HOT) * aFade;

				n = UnpackNormal(tex2D(_TexCloudsHot_Norm, (IN.texturePos + time) * _TexCloudsHot_Norm_ST.xy + _TexCloudsHot_Norm_ST.zw));
				n.xy *= _TexCloudsHot_NStr;
			}

			a.a = pow(saturate(a.a + a.a * snoise4(float4(IN.localPos.xyz, time.x), 4)), 0.5);

			o.Albedo = a;
			o.Alpha = a.a;
			o.Normal = normalize(n);
			o.Metallic = 0;
			o.Smoothness = 0;
			o.Emission = e;
		}

		ENDCG
	}
	FallBack "Diffuse"
}
