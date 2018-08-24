using System.Collections.Generic;
using UnityEngine;

public class EnvironmentBasedPlanet : MonoBehaviour
{
    [System.Serializable]
    public class Sphere
    {
        public Material surface;
        public MeshRenderer[] sphere;
    }

    public enum Temperature { Cold, Warm, Hot };
    public enum Classification { GasGiant, GasDwarf, Mixed, Metallic };

    public Statistics environment;

    public bool isMoon = false;
    public string statisticName;

    public Temperature temperature;
    public Classification classification;
    public float kelvin = 0;
    public float kelvinLow = 0;
    public float kelvinHigh = 0;

    public Statistic statPosition;
    public Statistic[] statPositionStars;
    public Statistic statKelvin;
    public Statistic[] statKelvinStar;
    public Statistic[] statRadiusStar;

    public Sphere surface;
    public Sphere atmosphere;

    public bool isInitialised = false;

    // Use this for initialization
    void Start()
    {
        statisticName = name;

        if (!isMoon)
        {
            int star = environment[statisticName + " Star"];
            string starName = "Star " + StringHelper.IndexIntToChar(star);
            GameObject starGO = environment[starName + " GO"];
            name = starGO.name + "-" + statisticName.Remove(0, "Planet ".Length);
        }
        else
        {
            GameObject planetGO = environment[statisticName + " Planet GO"];
            name = planetGO.name + "-" + statisticName.Remove(0, statisticName.Length - 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInitialised)
        {//transform.localScale = Vector3.one * (Mathf.Log(environment[name + " Radius"] * 100f, 10) + 0.01f);
            isInitialised = true;
            Generate();
        }
    }

    public void UpdateStatistics()
    {
        Vector2 orbitCenter = environment[statisticName + " Orbit"];
        Vector2 delta = transform.position.XY() - orbitCenter;
        // I need to find the hottest, and coldest point around the orbit
        float kelvinLow = 30000;
        float kelvinHigh = 0;
        float distance = delta.magnitude;

        int numberOfStars = environment["Stars"];
        for (float k = 0; k < Mathf.PI * 2f; k += Mathf.PI / 32f)
        {
            float kelvinCurrent = 0;
            Vector2 pos = orbitCenter + new Vector2(Mathf.Cos(k), Mathf.Sin(k)) * distance;
            for (int j = 0; j < numberOfStars; j++)
            {
                string starName = "Star " + StringHelper.IndexIntToChar(j);
                float starRadius = EnvironmentRules.StellarRadius(statRadiusStar[j].Get<float>());
                float starTemperature = statKelvinStar[j].Get<float>();
                float stellarDistance = EnvironmentRules.PlanetDistance((statPositionStars[j].Get<Vector2>() - pos).magnitude);
                kelvinCurrent += EnvironmentRules.PlanetTemperature(starRadius, EnvironmentRules.StellarLuminosity(starRadius, starTemperature), stellarDistance, 0.3f);
            }

            if (kelvinCurrent < kelvinLow)
                kelvinLow = kelvinCurrent;
            if (kelvinCurrent > kelvinHigh)
                kelvinHigh = kelvinCurrent;
        }

        UpdateMaterials();
    }
    
    public void Generate()
    {
        transform.localScale = Vector3.one * environment[statisticName + " Radius"];

        environment.gameObject.GetComponent<PlanetGenerator>().GeneratePlanet(this);

        statKelvin = environment[statisticName + " Actual Kelvin"];
        statPosition = environment[statisticName + " Position"];

        int numberOfStars = environment["Stars"];

        statKelvinStar = new Statistic[numberOfStars];
        statRadiusStar = new Statistic[numberOfStars];
        statPositionStars = new Statistic[numberOfStars];

        for (int j = 0; j < numberOfStars; j++)
        {
            string starName = "Star " + StringHelper.IndexIntToChar(j);

            statKelvinStar[j] = environment[starName + " Kelvin"];
            statRadiusStar[j] = environment[starName + " Radius"];
            statPositionStars[j] = environment[starName + " Position"];
        }

        UpdateMaterials();
    }

    public void UpdateMaterials()
    {
        kelvin = environment[statisticName + " Actual Kelvin"];
        kelvinLow = environment[statisticName + " Actual Kelvin Low"];
        kelvinHigh = environment[statisticName + " Actual Kelvin High"];
        float kelvinRange = kelvinHigh - kelvinLow;
        float intensity = environment[statisticName + " Atmosphere Intensity"];
        float density = environment[statisticName + " Atmosphere Density"];
        float elevation = environment[statisticName + " Water Level"];

        if (classification == Classification.Metallic)
            elevation *= 0.1f;

        if (surface.surface)
        {
            foreach (MeshRenderer renderer in surface.sphere)
            {
                renderer.sharedMaterial = new Material(surface.surface);
                renderer.sharedMaterial.SetFloat("_Kelvin", kelvin);
                renderer.sharedMaterial.SetFloat("_KelvinRange", kelvinRange);
                renderer.sharedMaterial.SetFloat("_Elevation", elevation);
            }
        }
        else
        {
            foreach (MeshRenderer renderer in surface.sphere)
            {
                renderer.gameObject.SetActive(false);
            }
        }

        if (atmosphere.surface)
        {
            foreach (MeshRenderer renderer in atmosphere.sphere)
            {
                renderer.sharedMaterial = new Material(atmosphere.surface);
                renderer.sharedMaterial.SetFloat("_AtmosphereIntensity", intensity);
                renderer.sharedMaterial.SetFloat("_AtmosphereDensity", density);
                renderer.sharedMaterial.SetFloat("_Kelvin", kelvin);
                renderer.sharedMaterial.SetFloat("_KelvinRange", kelvinRange);
            }
        }
        else
        {
            foreach (MeshRenderer renderer in atmosphere.sphere)
            {
                renderer.gameObject.SetActive(false);
            }
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        List<ParticleCollisionEvent> particleCollisions = new List<ParticleCollisionEvent>();
        ParticlePhysicsExtensions.GetCollisionEvents(other.GetComponent<ParticleSystem>(), gameObject, particleCollisions);
    }
}
