using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class EnvironmentRules
{
    public const float G = 6.67408f * 10e-11f;

    public const float MassOfEarth = 5.972f * 10e24f;
    public const float MassOfSun = 1.989f * 10e30f;

    public const float RadiusOfSun = 695508;
    public const float RadiusOfMercury = 2440;
    public const float RadiusOfVenus = 6052;
    public const float RadiusOfEarth = 6371;
    public const float RadiusOfMoon = 1737;
    public const float RadiusOfMars = 3390;
    public const float RadiusOfJupiter = 69911;
    public const float RadiusOfSaturn = 58232;
    public const float RadiusOfUranus = 25362;
    public const float RadiusOfNeptune = 24622;
    public const float RadiusOfPluto = 1188;

    public const float StefanBoltzmannConstant = 5.670373f * 10e-8f;
    /// <summary>
    /// The Planet Distance Ratio defines what a single unit of system distance denotes in proper km
    /// This is established based on the distance between the sun and mercury
    /// </summary>
    public const float PlanetDistanceRatio = 57910000;// * 2f;// / 1f;

    public const float PlanetMassRatio = MassOfEarth * 0.1f;

    public enum MassFormat { Suns, Earths, Kilograms };

    public static float Mass(float mass, MassFormat format)
    {
        switch(format)
        {
            case MassFormat.Suns:
                return mass * MassOfSun;
            case MassFormat.Earths:
                return mass * MassOfEarth;
            default:
                return mass;
        }
    }

    /// <summary>
    /// Converts stellar radius from sun radii to km
    /// </summary>
    /// <param name="stellarRadius">The stellar radius in sun radii</param>
    /// <returns>The stellar radius in km</returns>
    public static float StellarRadius(float stellarRadius)
    {
        return stellarRadius * RadiusOfSun;
    }

    /// <summary>
    /// Converts stellar radius from km to sun radii
    /// </summary>
    /// <param name="stellarRadius">The stellar radius in km</param>
    /// <returns>The stellar radius in sun radii</returns>
    public static float InverseStellarRadius(float stellarRadius)
    {
        return stellarRadius / RadiusOfSun;
    }

    public static float StellarLuminosity(float stellarRadius, float stellarTemperature)
    {
        return 4f * Mathf.PI * stellarRadius * stellarRadius * StefanBoltzmannConstant * stellarTemperature * stellarTemperature * stellarTemperature * stellarTemperature;
    }

    public static float PlanetDistance(float distance)
    {
        return distance * PlanetDistanceRatio;
    }

    public static float PlanetTemperature(float stellarRadius, float stellarLuminosity, float stellarDistance, float albedo)
    {
        return Mathf.Pow((stellarLuminosity * (1 - albedo)) / (16 * Mathf.PI * stellarDistance * stellarDistance * StefanBoltzmannConstant), 1/4f);
    }

    public static float AtmosphereIntensity(float radius, float kelvin)
    {
        float intensity = (kelvin / 200f) * Mathf.Max(0, 1f - (kelvin / 1500f)) * radius;
        return Mathf.Max(0.01f, intensity);
    }

    public static float AtmosphereDensity(float radius, float kelvin)
    {
        float density = Mathf.Max(1, (20f - kelvin / 100f) * Mathf.Max(0, 1f - (kelvin / 2000f)) * (1f / radius));
        return density;
    }

    public static float AtmosphericKelvinRange(float kelvinRange, float intensity, float density)
    {
        return kelvinRange * (Mathf.Lerp(0, 10, 1 / density) / intensity);
    }

    public static float AtmosphereKelvin(float kelvin, float kelvinRange, float intensity, float density)
    {
        return kelvin + (kelvinRange * intensity) / density;
    }
}
