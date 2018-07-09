using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class EnvironmentRules
{
    public const float StefanBoltzmannConstant = 5.670373f * 10e-8f;
    /// <summary>
    /// The Planet Distance Ratio defines what a single unit of system distance denotes in proper km
    /// This is established based on the distance between the sun and mercury
    /// </summary>
    public const float PlanetDistanceRatio = 57910000 * 2f;// / 1f;

    /// <summary>
    /// Converts stellar radius from sun radii to km
    /// </summary>
    /// <param name="stellarRadius">The stellar radius in sun radii</param>
    /// <returns>The stellar radius in km</returns>
    public static float StellarRadius(float stellarRadius)
    {
        return stellarRadius * 695508f;
    }

    /// <summary>
    /// Converts stellar radius from km to sun radii
    /// </summary>
    /// <param name="stellarRadius">The stellar radius in km</param>
    /// <returns>The stellar radius in sun radii</returns>
    public static float InverseStellarRadius(float stellarRadius)
    {
        return stellarRadius / 695508f;
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
}
