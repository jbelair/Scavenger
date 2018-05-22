Shader "Surfaces/Star"
{
	Properties
	{
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
		_Normal("Gas Giant (Normal)", 2D) = "bump" {}
		_NormalStrength("Normal Strength", Range(0,10)) = 1
		//_Occlusion("Gas Giant (Occlusion)", 2D) = "white" {}
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
		sampler2D _Normal;
		fixed4 _Normal_ST;
		float _NormalStrength;
		//sampler2D _Occlusion;
		//fixed4 _Occlusion_ST;
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

		#include "Assets/Surfaces/Shaders/noise.cginc"

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

			half4 c = saturate(LightingStandard(s, viewDir, gi));

			c.rgb = saturate((c + s.Albedo * gi.light.color * float3(pow(diff, _Phase.r), pow(diff, _Phase.g), pow(diff, _Phase.b))) / 2);
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
			o.texturePos = float3(v.texcoord.xy, 1 - pow(abs(v.vertex.y), 2));
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

			float3 n = lerp(UnpackNormal(tex2D(_Normal, sampleAt.xy * _Normal_ST.xy + _Normal_ST.zw)), UnpackNormal(float4(0.5, 0.5, 1, 1)), max(abs(IN.localPos.y), temperature));
			n = float3(n.x * _NormalStrength, n.y * _NormalStrength, n.z);
			n = normalize(n);
			o.Normal = n;
			//o.Occlusion = tex2D(_Occlusion, sampleAt.xy * _Occlusion_ST.xy + _Occlusion_ST.zw);

			o.Alpha = 1;
		}
		ENDCG
	}
	FallBack "Diffuse"
}