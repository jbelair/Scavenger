using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingularitySystem : MonoBehaviour, ISystemGeneratorDecorator
{
    public EnvironmentBasedSingularity singularityPrefab;
    //public ParticleSystem atmosphereLeechPrefab;
    public LineRendererSpiral atmosphereLeechPrefab;
    public LineRendererSpiral planetLeechPrefab;

    public List<DungeonTypeCategory> dungeonCategories;

    public Statistics statistics;
    public EnvironmentBasedSingularity singularity;

    public void Start()
    {
        if (!statistics)
            statistics = GetComponent<Statistics>();
    }

    public void System()
    {
        // Find the biggest star
        // Convert it into a blackhole
        int numberOfStars = statistics["Stars"];
        float largest = 0;
        int largestStar = 0;
        for (int i = 0; i < numberOfStars; i++)
        {
            string star = "Star " + StringHelper.IndexIntToChar(i);
            float radius = statistics[star + " Radius"];
            if (radius > largest)
            {
                largest = radius;
                largestStar = i;
            }
        }

        string starName = "Star " + StringHelper.IndexIntToChar(largestStar);
        Destroy(GameObject.Find(starName));

        string singularityName = "Singularity";

        singularity = Instantiate(singularityPrefab, statistics[starName + " Position"].Get<Vector2>(), singularityPrefab.transform.rotation, transform);
        singularity.name = singularityName;
        singularity.environment = statistics;

        statistics["Singularities"].Set(1);
        statistics["Singularity Star"] = new Statistic("Singularity Star", Statistic.ValueType.Integer, largestStar);
    }

    public void Star()
    {
        // Foreach star in the system determine whether and how much they would be leeched by the blackhole
        // Creating the appropriate prefab to match the needs of the leeching
        float accretion = 0;

        int convertedStar = statistics["Singularity Star"].Get<int>();
        string star = "Star " + StringHelper.IndexIntToChar(convertedStar);
        Vector2 position = statistics[star + " Position"];

        int numberOfStars = statistics["Stars"];
        for (int i = 0; i < numberOfStars; i++)
        {
            string name = "Star " + StringHelper.IndexIntToChar(i);

            float distanceFromSingularity = Vector2.Distance(position, statistics[name + " Position"]);

            //accretion += statistics[name + " Radius"] * Mathf.Clamp(1f - (distanceFromSingularity / 10000f), 0f, 1f);
            if (1 == Random.Range(1, (int)distanceFromSingularity / 100f))
            {
                if (i != convertedStar)
                {
                    Destroy(GameObject.Find(name));
                    accretion += statistics[name + " Radius"];
                }
            }
            else if (distanceFromSingularity < singularity.transform.localScale.x * 500f && i != convertedStar)
            {
                GameObject foundStar = GameObject.Find(name);
                LineRendererSpiral atmosphereSpiral = Instantiate(atmosphereLeechPrefab, foundStar.transform, false);
                atmosphereSpiral.center = singularity.transform;
                //atmosphereSpiral.line.startWidth = singularity.transform.localScale.x * 10f;
                //atmosphereSpiral.line.endWidth = singularity.transform.localScale.x * 2f;
                
                //ParticleSystem atmosphereLeech = Instantiate(atmosphereLeechPrefab, foundStar.transform, false);
                //atmosphereLeech.GetComponent<ParticleOrbit>().center = singularity.transform;

                EnvironmentBasedStar foundStarEnvi = foundStar.GetComponent<EnvironmentBasedStar>();
                //ParticleSystem.TrailModule atmosphereLeechTrail = atmosphereLeech.trails;
                Texture2D kelvinRamp = (foundStarEnvi.starMaterials[0].GetTexture("_Emissive") as Texture2D);
                float radius = statistics[name + " Radius"].Get<float>();
                float kelvin = statistics[name + " Kelvin"].Get<float>();
                float kelvinMax = foundStarEnvi.starMaterials[0].GetFloat("_KelvinMax");
                float kelvinRatio = kelvin / kelvinMax;
                Gradient colourGradient = new Gradient()
                {
                    alphaKeys = new GradientAlphaKey[] 
                    {
                        new GradientAlphaKey(0, 0),
                        new GradientAlphaKey(1, 0.2f),
                        new GradientAlphaKey(1, 1)
                    },
                    colorKeys = new GradientColorKey[] 
                    {
                        new GradientColorKey(kelvinRamp.GetPixelBilinear(kelvinRatio, 0), 1),
                        new GradientColorKey(kelvinRamp.GetPixelBilinear(Mathf.Lerp(kelvinRatio, 1, 0.25f), 0), 0.75f),
                        new GradientColorKey(kelvinRamp.GetPixelBilinear(Mathf.Lerp(kelvinRatio, 1, 0.5f), 0), 0.5f),
                        new GradientColorKey(kelvinRamp.GetPixelBilinear(Mathf.Lerp(kelvinRatio, 1, 0.75f), 0), 0.25f),
                        new GradientColorKey(kelvinRamp.GetPixelBilinear(1, 0), 0f),
                    }
                };
                atmosphereSpiral.line.colorGradient = colourGradient;
                atmosphereSpiral.line.startWidth = statistics[star + " Radius"].Get<float>() * 2f;
                atmosphereSpiral.line.endWidth = radius * 2f;
                atmosphereSpiral.line.widthCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, atmosphereSpiral.line.startWidth), new Keyframe(1, atmosphereSpiral.line.endWidth) });
                //atmosphereLeechTrail.colorOverTrail = colourGradient;
                //singularity.particleAttractor.particleSystems.Add(atmosphereLeech);
                accretion += statistics[name + " Radius"] * 0.1f;
            }
        }

        statistics["Singularity Accretion"] = new Statistic("Singularity Accretion", Statistic.ValueType.Float, accretion);
    }

    public void Planet()
    {
        // Foreach planet in the system determine whether and how much they would be leeched by the blackhole
        // Creating the appropriate prefab to match the needs of the leeching
        float accretion = 0;

        string star = "Star " + StringHelper.IndexIntToChar(statistics["Singularity Star"].Get<int>());
        Vector2 position = statistics[star + " Position"];

        int numberOfPlanets = statistics["Planets"];
        for (int i = 0; i < numberOfPlanets; i++)
        {
            string name = "Planet " + StringHelper.IndexIntToChar(i);

            float distanceFromSingularity = Vector2.Distance(position, statistics[name + " Position"]);

            //accretion += statistics[name + " Radius"] * Mathf.Clamp(1f - (distanceFromSingularity / 10000f), 0f, 1f);
            if (1 == Random.Range(1, (int)distanceFromSingularity / 100))
            {
                Destroy(GameObject.Find(name));
                accretion += statistics[name + " Radius"];

                // MOONS ---------------------------------------------------------------------------------------------------------------------------------------------------------------------
                int numberOfMoons = statistics[name + " Moons"];
                for (int j = 0; j < numberOfMoons; j++)
                {
                    string moonName = name + " Moon " + StringHelper.IndexIntToChar(j);
                    
                    Destroy(GameObject.Find(moonName));
                    accretion += statistics[moonName + " Radius"];
                }
            }
            else
            {
                if (distanceFromSingularity < singularity.transform.localScale.x * 1000f)
                {
                    GameObject foundPlanet = GameObject.Find(name);
                    if (foundPlanet.transform.localScale.x >= 1)
                    {
                        LineRendererSpiral planetLeech = Instantiate(planetLeechPrefab, foundPlanet.transform, false);
                        planetLeech.center = singularity.transform;

                        EnvironmentBasedStar foundStarEnvi = GameObject.Find(star).GetComponent<EnvironmentBasedStar>();
                        //ParticleSystem.TrailModule atmosphereLeechTrail = atmosphereLeech.trails;
                        Texture2D kelvinRamp = (foundStarEnvi.starMaterials[0].GetTexture("_Emissive") as Texture2D);
                        float radius = statistics[name + " Radius"].Get<float>();
                        float kelvin = statistics[name + " Kelvin"].Get<float>();
                        float kelvinMax = foundStarEnvi.starMaterials[0].GetFloat("_KelvinMax");
                        float kelvinRatio = kelvin / kelvinMax;
                        Gradient colourGradient = new Gradient()
                        {
                            alphaKeys = new GradientAlphaKey[]
                            {
                                new GradientAlphaKey(0, 0),
                                new GradientAlphaKey(1, 0.2f),
                                new GradientAlphaKey(1, 1)
                            },
                            colorKeys = new GradientColorKey[]
                            {
                                new GradientColorKey(new Color(0.2f,0.2f,0.2f), 1),
                                new GradientColorKey(kelvinRamp.GetPixelBilinear(Mathf.Lerp(kelvinRatio, 1, 0.25f), 0), 0.75f),
                                new GradientColorKey(kelvinRamp.GetPixelBilinear(Mathf.Lerp(kelvinRatio, 1, 0.5f), 0), 0.5f),
                                new GradientColorKey(kelvinRamp.GetPixelBilinear(Mathf.Lerp(kelvinRatio, 1, 0.75f), 0), 0.25f),
                                new GradientColorKey(kelvinRamp.GetPixelBilinear(1, 0), 0f),
                            }
                        };
                        planetLeech.line.colorGradient = colourGradient;
                        planetLeech.line.startWidth = radius * 2f;
                        planetLeech.line.endWidth = statistics[star + " Radius"].Get<float>() * 2f;
                        planetLeech.line.widthCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, planetLeech.line.startWidth), new Keyframe(1, planetLeech.line.endWidth) });

                        accretion += statistics[name + " Radius"] * 0.1f;
                    }
                }

                // MOONS ---------------------------------------------------------------------------------------------------------------------------------------------------------------------
                int numberOfMoons = statistics[name + " Moons"];
                for (int j = 0; j < numberOfMoons; j++)
                {
                    string moonName = name + " Moon " + StringHelper.IndexIntToChar(j);

                    if (1 == Random.Range(1, (int)distanceFromSingularity / 100))
                    {
                        Destroy(GameObject.Find(moonName));
                        accretion += statistics[moonName + " Radius"];


                    }
                }
            }
        }

        statistics["Singularity Accretion"].Set(statistics["Singularity Accretion"].Get<float>() + accretion);
    }

    public void Dungeon()
    {
        DungeonGenerator[] dungeonables = GameObject.FindObjectsOfType<DungeonGenerator>();

        for (int i = 0; i < dungeonables.Length; i++)
        {
            DungeonTypeCategory dungeonCategory = dungeonCategories.Find(dungeon => dungeon.name == dungeonables[i].dungeonCategory);
            if (dungeonCategory != null && dungeonCategory.dungeons.Length > 0 && 1 == Random.Range(1f,2f))
            {
                dungeonables[i].dungeonType = dungeonCategory.dungeons[Random.Range(0, dungeonCategory.dungeons.Length)];
                dungeonables[i].riskLevel = (float)dungeonables[i].dungeonType.risk * Random.Range(0.5f, 2f);
            }

            dungeonables[i].riskLevel = Mathf.Clamp((dungeonables[i].riskLevel * Random.Range(1f, 2f)), 0, 5);
        }
    }
}
