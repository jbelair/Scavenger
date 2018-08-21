// CREATED BY AIDAN DEARING
// Contains methods to aid in planet shaders

sampler2D _Elevation_Mask;
float4 _Elevation_Mask_ST;
float _Elevation;
float _ElevationSlope;

float waterElevation(float elevation, float2 texturePos)
{
	float e = (elevation + tex2D(_Elevation_Mask, texturePos * _Elevation_Mask_ST.xy + _Elevation_Mask_ST.zw)) / 2;

	if (e < _Elevation)
	{
		e = saturate(lerp(e, 1, (_Elevation - e) / _ElevationSlope));
	}
	else
	{
		e = 0;
	}

	return e;
}

float surfaceElevation(float elevation, float2 texturePos)
{
	float e = tex2D(_Elevation_Mask, texturePos * _Elevation_Mask_ST.xy + _Elevation_Mask_ST.zw);

	return e;
}

float polefade(float y, float2 texturePos, float a)
{
	float f = a;

	return 1 - f;
}

float4 _KelvinMinMax;
float _Kelvin;
float _KelvinRange;

float coldToWarm(float kelvin)
{
	return (kelvin - _KelvinMinMax.x) / (_KelvinMinMax.y - _KelvinMinMax.x);
}

float warmToHot(float kelvin)
{
	return (kelvin - _KelvinMinMax.y) / (_KelvinMinMax.z - _KelvinMinMax.y);
}
