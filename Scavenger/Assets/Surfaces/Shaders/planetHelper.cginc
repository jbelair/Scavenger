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

	//if (e < _Elevation)
	//{
	//	e = saturate(lerp(e, 0, (_Elevation - e) / _ElevationSlope));
	//}

	return e;
}

//sampler2D _TexPole_North, _TexPole_South;
//float4 _TexPole_North_ST, _TexPole_South_ST;

float polefade(float y, float2 texturePos, float a)
{
	//float f = 0;
	//if (y > 0)
	//{
	//	// North Pole
	//	f = tex2D(_TexPole_North, texturePos * _TexPole_North_ST.xy + _TexPole_North_ST.zw).r;
	//}
	//else
	//{
	//	// South Pole
	//	f = tex2D(_TexPole_South, texturePos * _TexPole_South_ST.xy + _TexPole_South_ST.zw).r;
	//}

	//return 1 - f;

	float f = a;//abs(y);
	//f = pow(f, 7.5);// *a;
	
	//if (f < 0.7)
	//{
	//	f = a;//saturate(lerp(0, 1, (_Elevation - f) / _ElevationSlope));
	//}
	//else
	//{
	//	f = a * saturate(lerp(1, 0, (f - 0.7) / _ElevationSlope));
	//}

	return 1 - f;
}

float4 _KelvinMinMax;
float _Kelvin;
float _KelvinRange;

float coldToWarm(float kelvin)
{
	return pow((kelvin - _KelvinMinMax.x) / (_KelvinMinMax.y - _KelvinMinMax.x), 0.75);
}

float warmToHot(float kelvin)
{
	return pow((kelvin - _KelvinMinMax.y) / (_KelvinMinMax.z - _KelvinMinMax.y), 0.75);
}
