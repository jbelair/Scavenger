using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{
    [System.Serializable]
    public class PlanetProfile
    {
        public string name;

        public Material[] surfaces;
        public Material[] atmospheres;

        public Material Surface()
        {
            if (surfaces.Length > 0)
                return surfaces[Random.Range(0, surfaces.Length - 1)];
            else
                return null;
        }

        public Material Surface(int index)
        {
            if (surfaces.Length > 0)
                return surfaces[index % surfaces.Length];
            else
                return null;
        }

        public Material Atmosphere()
        {
            if (atmospheres.Length > 0)
                return atmospheres[Random.Range(0, atmospheres.Length - 1)];
            else
                return null;
        }

        public Material Atmosphere(int index)
        {
            if (atmospheres.Length > 0)
                return atmospheres[index % atmospheres.Length];
            else
                return null;
        }
    }

    public PlanetProfile[] profiles;
    public Dictionary<string, PlanetProfile> profilesDictionary = new Dictionary<string, PlanetProfile>();

    [Header("Metallic Planets")]
    public string[] metallicProfiles;

    [Header("Mixed Planets")]
    public string[] mixedProfiles;

    [Header("Gasseous Planets")]
    public string[] gasseousProfiles;

    public float Cold = 273;
    public float Hot = 320;

    [Header("Special Alternatives")]
    public GameObject[] asteroidBelts;
    public GameObject[] artefacts;
    public GameObject[] gasBelts;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public PlanetProfile GetProfileByName(string name)
    {
        if (profilesDictionary.ContainsKey(name))
            return profilesDictionary[name];
        else
        {
            for(int i = 0; i < profiles.Length; i++)
            {
                if (profiles[i].name == name)
                {
                    profilesDictionary.Add(name, profiles[i]);
                    return profiles[i];
                }
            }
        }

        return profiles[0];
    }

    public void GeneratePlanet(EnvironmentBasedPlanet planet)
    {
        float kelvin = planet.environment[planet.statisticName + " Actual Kelvin"].Get<float>();
        float kelvinLow = planet.environment[planet.statisticName + " Actual Kelvin Low"].Get<float>();
        float kelvinHigh = planet.environment[planet.statisticName + " Actual Kelvin High"].Get<float>();

        EnvironmentBasedPlanet.Temperature temperature;
        EnvironmentBasedPlanet.Classification classification;

        if (planet.transform.localScale.x >= (EnvironmentRules.RadiusOfEarth * 2f) / 10000f)
        {
            if (kelvinLow >= 75 && kelvinHigh <= 200)
            {
                temperature = EnvironmentBasedPlanet.Temperature.Warm;
            }
            else
            {
                if (kelvin < 75)
                {
                    temperature = EnvironmentBasedPlanet.Temperature.Cold;
                }
                else
                {
                    temperature = EnvironmentBasedPlanet.Temperature.Hot;
                }
            }

            if (planet.transform.localScale.x >= (EnvironmentRules.RadiusOfEarth * 5f) / 10000f)
                classification = EnvironmentBasedPlanet.Classification.GasGiant;
            else
                classification = EnvironmentBasedPlanet.Classification.GasDwarf;
        }
        else
        {
            if (kelvin >= Cold && kelvin <= Hot)
            {
                temperature = EnvironmentBasedPlanet.Temperature.Warm;
            }
            else
            {
                if (kelvin < Cold)
                {
                    temperature = EnvironmentBasedPlanet.Temperature.Cold;
                }
                else
                {
                    temperature = EnvironmentBasedPlanet.Temperature.Hot;
                }
            }

            if (planet.transform.localScale.x > (EnvironmentRules.RadiusOfEarth * 0.25f) / 10000f)
            {
                classification = EnvironmentBasedPlanet.Classification.Mixed;
            }
            else
            {
                classification = EnvironmentBasedPlanet.Classification.Metallic;
            }
        }

        PlanetProfile profile = GetProfileByName(metallicProfiles[0]);
        switch (classification)
        {
            case EnvironmentBasedPlanet.Classification.GasGiant:
                profile = GetProfileByName(gasseousProfiles[planet.environment[planet.statisticName + " Profile"] % gasseousProfiles.Length]);
                break;
            case EnvironmentBasedPlanet.Classification.GasDwarf:
                profile = GetProfileByName(gasseousProfiles[planet.environment[planet.statisticName + " Profile"] % gasseousProfiles.Length]);
                break;
            case EnvironmentBasedPlanet.Classification.Mixed:
                profile = GetProfileByName(mixedProfiles[planet.environment[planet.statisticName + " Profile"] % mixedProfiles.Length]);
                break;
            case EnvironmentBasedPlanet.Classification.Metallic:
                profile = GetProfileByName(metallicProfiles[planet.environment[planet.statisticName + " Profile"] % metallicProfiles.Length]);
                break;
        }

        planet.surface.surface = profile.Surface(planet.environment[planet.statisticName + " Surface"]);
        planet.atmosphere.surface = profile.Atmosphere(planet.environment[planet.statisticName + " Atmosphere"]);

        planet.temperature = temperature;
        planet.classification = classification;

        planet.kelvin = kelvin;
        planet.kelvinLow = kelvinLow;
        planet.kelvinHigh = kelvinHigh;
    }
}
