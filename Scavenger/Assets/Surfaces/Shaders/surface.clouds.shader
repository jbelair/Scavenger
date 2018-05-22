// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Surfaces/Clouds" 
{
	Properties
	{
		_Phase("Atmosphere Phase", Vector) = (0.75, 0.825, 1, 1)
		_Color("Color", Color) = (1, 1, 1, 1)
		_Texture("Cloud Density", 2D) = "black" {}
		_Cloudiness("Cloudiness", Range(0.01, 0.99)) = 0.25
		_CloudTurbulence("Cloud Turbulence", Range(0.001, 100)) = 1
		_CloudTurbulenceScale("Turbulence Scale", Vector) = (1, 1, 1, 1)
		_CloudTurbulenceOctaves("Turbulence Octaves", Range(0, 8)) = 2
		_Speed("Speed", Range(0., 2.)) = 0.2
	}
	SubShader
	{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		LOD 400
		Cull Back
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf WrapScattering fullforwardshadows vertex:vert alpha:fade
		#include "UnityPBSLighting.cginc"
		#include "Assets/Surfaces/Shaders/noise.cginc"

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		struct Input 
		{
			float3 localPos;
			float3 texturePos;
			float3 viewDir;
			float3 normal;
		};

		sampler2D _Texture;
		float4 _Texture_ST;

		float _Cloudiness;
		float _CloudTurbulence;
		float4 _CloudTurbulenceScale;
		int _CloudTurbulenceOctaves;
		fixed4 _Color;
		fixed4 _Phase;
		float _Speed;

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
			o.texturePos.xy = v.texcoord.xy;
			o.texturePos.z = 1 - pow(abs(v.vertex.y), 5);//dot(normalize(o.viewDir), v.normal);
			o.normal = v.normal;
		}

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			float freznel = pow(saturate(dot(normalize(ObjSpaceViewDir(float4(IN.viewDir, 1))), IN.normal)), 0.5);
			//float freznel = pow(saturate(dot(normalize(IN.viewDir), IN.normal)), 0.5);
			// Cloud Pass
			float2 cloudTurbulence = 0;//
			if (_CloudTurbulence > 0)
			{
				cloudTurbulence = abs(snoise4(float4(IN.localPos.xyz * _CloudTurbulenceScale, _Time.x * _Speed), _CloudTurbulenceOctaves));// * 0.5 + 0.5;
				cloudTurbulence = float2(cos(cloudTurbulence.x * 3.1415), sin(cloudTurbulence.x  * 3.1415)) * cos(cloudTurbulence.x * 3.1415 * 2) * _CloudTurbulence;
				cloudTurbulence = cloudTurbulence * 0.5 + 0.5;
			}
			float2 cloud = tex2D(_Texture, IN.texturePos.xy * _Texture_ST.xy + _Texture_ST.zw + cloudTurbulence);
			cloud = lerp(cloud, cloud * cloudTurbulence, ((1 - _Cloudiness) - cloud.r / (1 - _Cloudiness)));
			cloud = saturate(cloud);
			o.Albedo = _Color;
			o.Alpha = cloud.r * freznel * IN.texturePos.z;
		}
	ENDCG
	}
	FallBack "Diffuse"
}
