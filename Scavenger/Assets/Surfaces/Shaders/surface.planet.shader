// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Surfaces/Planet" {
	Properties
	{
		_Phase("Atmosphere Phase", Vector) = (0.75,0.825,1,1)
		_TexMain("Main Texture", 2D) = "white" {}
		_NorMain("Main Normal Map", 2D) = "bump" {}
		_NorStrength("Normal Strength", Range(0,10)) = 1
		_MSMain("Main Metallic & Smoothness Map", 2D) = "black" {}
		[HDR]_EmiMainColour("Main Emission Colour", Color) = (1,1,1,1)
		_EmiMain("Main Emission Map", 2D) = "black" {}
		_PoleScale("Poles Scale", Range(0,1)) = 0.5
		_TexPoleN("North Pole Texture", 2D) = "white" {}
		_NorPoleN("North Pole Normal Map", 2D) = "bump" {}
		_MSPoleN("North Pole Metallic, Smoothness & Alpha Map", 2D) = "black" {}
		[HDR]_EmiPoleNColour("North Pole Emission Colour", Color) = (1,1,1,1)
		_EmiPoleN("North Pole Emission Map", 2D) = "black" {}
		_TexPoleS("South Pole Texture", 2D) = "white" {}
		_NorPoleS("South Pole Normal Map", 2D) = "bump" {}
		_MSPoleS("South Pole Metallic, Smoothness & Alpha Map", 2D) = "black" {}
		[HDR]_EmiPoleSColour("South Pole Emission Colour", Color) = (1,1,1,1)
		_EmiPoleS("South Pole Emission Map", 2D) = "black" {}
	}
		SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 400

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf WrapScattering fullforwardshadows vertex:vert
		#include "UnityPBSLighting.cginc"

		// Primary Texture
		sampler2D _TexMain;
		float4 _TexMain_ST;
		// Primary Normal Map
		sampler2D _NorMain;
		float4 _NorMain_ST;
		float _NorStrength;
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

		inline fixed4 LightingWrapScattering(SurfaceOutputStandard s, fixed3 viewDir, UnityGI gi)
		{
			half NdotL = dot(s.Normal, gi.light.dir);
			half diff = NdotL * 0.5 + 0.5;
			half4 c = LightingStandard(s, viewDir, gi);

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
			float3 ms = 0;
			if (IN.localPos.y < 0.8 - _PoleScale && IN.localPos.y > -0.8 + _PoleScale)
			{
				c = tex2D(_TexMain, IN.texturePos * _TexMain_ST.xy + _TexMain_ST.zw);
				e = tex2D(_EmiMain, IN.texturePos * _EmiMain_ST.xy + _EmiMain_ST.zw) * _EmiMainColour;
				n = UnpackNormal(tex2D(_NorMain, IN.texturePos * _NorMain_ST.xy + _NorMain_ST.zw));
				ms = tex2D(_MSMain, IN.texturePos * _MSMain_ST.xy + _MSMain_ST.zw).rgb;
			}
			else
			{
				float scale = _PoleScale * 1;
				if (IN.localPos.y >= 0.8 - _PoleScale)
				{
					c = tex2D(_TexPoleN, (IN.localPos.xz / 2 + 0.5) * _TexPoleN_ST.xy + _TexPoleN_ST.zw);
					e = tex2D(_EmiPoleN, (IN.localPos.xz / 2 + 0.5) * _EmiPoleN_ST.xy + _EmiPoleN_ST.zw) * _EmiPoleNColour;
					n = UnpackNormal(tex2D(_NorPoleN, IN.localPos.xz * _NorPoleN_ST.xy + _NorPoleN_ST.zw));
					ms = tex2D(_MSPoleN, (IN.localPos.xz / 2 + 0.5) * _MSPoleN_ST.xy + _MSPoleN_ST.zw).rgb;

					e = lerp(tex2D(_EmiMain, IN.texturePos * _EmiMain_ST.xy + _EmiMain_ST.zw) * _EmiMainColour, e, ms.b);
					n = lerp(UnpackNormal(tex2D(_NorMain, IN.texturePos * _NorMain_ST.xy + _NorMain_ST.zw)), n, ms.b);
					c = lerp(tex2D(_TexMain, IN.texturePos * _TexMain_ST.xy + _TexMain_ST.zw), c, ms.b);
					ms = lerp(tex2D(_MSMain, IN.texturePos * _MSMain_ST.xy + _MSMain_ST.zw).rgb, ms, ms.b);
				}
				else if (IN.localPos.y <= -0.8 + _PoleScale)
				{
					c = tex2D(_TexPoleS, (IN.localPos.xz / 2 + 0.5) * _TexPoleS_ST.xy + _TexPoleS_ST.zw);
					e = tex2D(_EmiPoleS, (IN.localPos.xz / 2 + 0.5) * _EmiPoleS_ST.xy + _EmiPoleS_ST.zw) * _EmiPoleSColour;
					n = UnpackNormal(tex2D(_NorPoleS, IN.localPos.xz * _NorPoleS_ST.xy + _NorPoleS_ST.zw));
					ms = tex2D(_MSPoleS, (IN.localPos.xz / 2 + 0.5) * _MSPoleS_ST.xy + _MSPoleS_ST.zw).rgb;
					ms = tex2D(_MSPoleS, (IN.localPos.xz / 2 + 0.5) * _MSPoleS_ST.xy + _MSPoleS_ST.zw).rgb;

					e = lerp(tex2D(_EmiMain, IN.texturePos * _EmiMain_ST.xy + _EmiMain_ST.zw) * _EmiMainColour, e, ms.b);
					n = lerp(UnpackNormal(tex2D(_NorMain, IN.texturePos * _NorMain_ST.xy + _NorMain_ST.zw)), n, ms.b);
					c = lerp(tex2D(_TexMain, IN.texturePos * _TexMain_ST.xy + _TexMain_ST.zw), c, ms.b);
					ms = lerp(tex2D(_MSMain, IN.texturePos * _MSMain_ST.xy + _MSMain_ST.zw).rgb, ms, ms.b);
				}
			}

			o.Albedo = c.rgb;
			o.Emission = lerp(float3(0,0,0),e.rgb,e.a);
			n = float3(n.x * _NorStrength, n.y * _NorStrength, n.z);
			n = normalize(n);
			o.Normal = n;
			o.Metallic = ms.x;
			o.Smoothness = ms.y;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
