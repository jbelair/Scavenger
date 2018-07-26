// CREATED BY AIDAN DEARING
// Contains an array of specialised surface lighting models
#include "UnityPBSLighting.cginc"

//sampler2D _Phase;
//sampler2D _PhaseClouds;
float4 _Phase;
float4 _PhaseClouds;
float4 _PhaseWater;

inline fixed4 LightingWrapScattering(SurfaceOutputStandard s, fixed3 viewDir, UnityGI gi)
{
	half NdotL = dot(s.Normal, gi.light.dir);
	half diff = NdotL * 0.5 + 0.5;
	half4 c = LightingStandard(s, viewDir, gi);

	if (NdotL > 0)
		diff = 1;
	else
		diff = saturate(1 - abs(NdotL) / _Phase.w);

	c.rgb = (c + s.Albedo * gi.light.color * float3(pow(diff, _Phase.r), pow(diff, _Phase.g), pow(diff, _Phase.b))) / 2;
	//c.rgb = (c * tex2D(_Phase, float2(1, 1 - diff)));
	c.a = s.Alpha;

	return c;
}

void LightingWrapScattering_GI(SurfaceOutputStandard s, UnityGIInput data, inout UnityGI gi)
{
	LightingStandard_GI(s, data, gi);
}

inline fixed4 LightingWrapScatteringClouds(SurfaceOutputStandard s, fixed3 viewDir, UnityGI gi)
{
	half NdotL = dot(s.Normal, gi.light.dir);
	half diff = NdotL * 0.5 + 0.5;
	half4 c = LightingStandard(s, viewDir, gi);

	if (NdotL > 0)
		diff = 1;
	else
		diff = saturate(1 - abs(NdotL) / _PhaseClouds.w);

	c.rgb = (c + s.Albedo * gi.light.color * float3(pow(diff, _PhaseClouds.r), pow(diff, _PhaseClouds.g), pow(diff, _PhaseClouds.b))) / 2;
	//c.rgb = (c * tex2D(_PhaseClouds, float2(1, 1 - diff)));
	c.a = s.Alpha;

	return c;
}

void LightingWrapScatteringClouds_GI(SurfaceOutputStandard s, UnityGIInput data, inout UnityGI gi)
{
	LightingStandard_GI(s, data, gi);
}

inline fixed4 LightingWrapScatteringWater(SurfaceOutputStandard s, fixed3 viewDir, UnityGI gi)
{
	half NdotL = dot(s.Normal, gi.light.dir);
	half diff = NdotL * 0.5 + 0.5;
	half4 c = LightingStandard(s, viewDir, gi);

	if (NdotL > 0)
		diff = 1;
	else
		diff = saturate(1 - abs(NdotL) / _PhaseWater.w);

	c.rgb = (c + s.Albedo * gi.light.color * float3(pow(diff, _PhaseWater.r), pow(diff, _PhaseWater.g), pow(diff, _PhaseWater.b))) / 2;
	//c.rgb = (c * tex2D(_PhaseClouds, float2(1, 1 - diff)));
	c.a = s.Alpha;

	return c;
}

void LightingWrapScatteringWater_GI(SurfaceOutputStandard s, UnityGIInput data, inout UnityGI gi)
{
	LightingStandard_GI(s, data, gi);
}
