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
        public Material[] clouds;
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

        public Material Cloud()
        {
            if (clouds.Length > 0)
                return clouds[Random.Range(0, clouds.Length - 1)];
            else
                return null;
        }

        public Material Cloud(int index)
        {
            if (clouds.Length > 0)
                return clouds[index % clouds.Length];
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
    public string[] coldMetallicProfiles;
    public string[] warmMetallicProfiles;
    public string[] hotMetallicProfiles;

    [Header("Mixed Planets")]
    public string[] coldMixedProfiles;
    public string[] warmMixedProfiles;
    public string[] hotMixedProfiles;

    [Header("Gasseous Planets")]
    public string[] coldGasseousProfiles;
    public string[] warmGasseousProfiles;
    public string[] hotGasseousProfiles;

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
        // Given the parameters for this planet we need to decide if it is
        // A.   Metallic
        // B.   Mixed
        // C.   Gasseous

        float kelvin = planet.environment[planet.name + " Kelvin"].Get<float>();
        float kelvinLow = planet.environment[planet.name + " Kelvin Low"].Get<float>();
        float kelvinHigh = planet.environment[planet.name + " Kelvin High"].Get<float>();

        EnvironmentBasedPlanet.Temperature temperature;
        EnvironmentBasedPlanet.Metallicity metallicity;

        if (planet.transform.localScale.x >= 2f)
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

            metallicity = EnvironmentBasedPlanet.Metallicity.Gasseous;
        }
        else
        {
            if (kelvinLow >= 280 && kelvinHigh <= 320)
            {
                temperature = EnvironmentBasedPlanet.Temperature.Warm;
            }
            else
            {
                if (kelvin < 280)
                {
                    temperature = EnvironmentBasedPlanet.Temperature.Cold;
                }
                else
                {
                    temperature = EnvironmentBasedPlanet.Temperature.Hot;
                }
            }

            if (planet.transform.localScale.x > 0.1f)
            {
                metallicity = EnvironmentBasedPlanet.Metallicity.Mixed;
            }
            else
            {
                metallicity = EnvironmentBasedPlanet.Metallicity.Metallic;
            }
        }

        PlanetProfile profile = GetProfileByName(coldMetallicProfiles[0]);
        switch (metallicity)
        {
            case EnvironmentBasedPlanet.Metallicity.Gasseous:
                switch (temperature)
                {
                    case EnvironmentBasedPlanet.Temperature.Cold:
                        profile = GetProfileByName(coldGasseousProfiles[planet.environment[planet.name + " Profile"] % coldGasseousProfiles.Length]);
                        break;
                    case EnvironmentBasedPlanet.Temperature.Warm:
                        profile = GetProfileByName(warmGasseousProfiles[planet.environment[planet.name + " Profile"] % warmGasseousProfiles.Length]);
                        break;
                    case EnvironmentBasedPlanet.Temperature.Hot:
                        profile = GetProfileByName(hotGasseousProfiles[planet.environment[planet.name + " Profile"] % hotGasseousProfiles.Length]);
                        break;
                }
                break;
            case EnvironmentBasedPlanet.Metallicity.Mixed:
                switch (temperature)
                {
                    case EnvironmentBasedPlanet.Temperature.Cold:
                        profile = GetProfileByName(coldMixedProfiles[planet.environment[planet.name + " Profile"] % coldMixedProfiles.Length]);
                        break;
                    case EnvironmentBasedPlanet.Temperature.Warm:
                        profile = GetProfileByName(warmMixedProfiles[planet.environment[planet.name + " Profile"] % warmMixedProfiles.Length]);
                        break;
                    case EnvironmentBasedPlanet.Temperature.Hot:
                        profile = GetProfileByName(hotMixedProfiles[planet.environment[planet.name + " Profile"] % hotMixedProfiles.Length]);
                        break;
                }
                break;
            case EnvironmentBasedPlanet.Metallicity.Metallic:
                switch (temperature)
                {
                    case EnvironmentBasedPlanet.Temperature.Cold:
                        profile = GetProfileByName(coldMetallicProfiles[planet.environment[planet.name + " Profile"] % coldMetallicProfiles.Length]);
                        break;
                    case EnvironmentBasedPlanet.Temperature.Warm:
                        profile = GetProfileByName(warmMetallicProfiles[planet.environment[planet.name + " Profile"] % warmMetallicProfiles.Length]);
                        break;
                    case EnvironmentBasedPlanet.Temperature.Hot:
                        profile = GetProfileByName(hotMetallicProfiles[planet.environment[planet.name + " Profile"] % hotMetallicProfiles.Length]);
                        break;
                }
                break;
        }

        planet.surface.surface = profile.Surface(planet.environment[planet.name + " Surface"]);
        planet.clouds.surface = profile.Cloud(planet.environment[planet.name + " Clouds"]);
        planet.atmosphere.surface = profile.Atmosphere(planet.environment[planet.name + " Atmosphere"]);

        planet.temperature = temperature;
        planet.metallicity = metallicity;

        planet.kelvin = kelvin;
        planet.kelvinLow = kelvinLow;
        planet.kelvinHigh = kelvinHigh;
    }
}
