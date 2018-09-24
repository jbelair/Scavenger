using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingularitySystem : MonoBehaviour, ISystemGeneratorDecorator
{
    public EnvironmentBasedSingularity singularityPrefab;
    //public ParticleSystem atmosphereLeechPrefab;
    public LineRendererSpiral atmosphereLeechPrefab;
    public LineRendererSpiral planetLeechPrefab;

    public Statistics statistics;
    public EnvironmentBasedSingularity singularity;

    public void Start()
    {
        if (!statistics)
            statistics = GetComponent<Statistics>();
    }

    public void System()
    {

    }

    public void Stars()
    {
        // Find the biggest star
        // Convert it into a blackhole
        int numberOfStars = statistics["Stars"];
        string star = "";
        float largest = 0;
        int largestStar = 0;
        for (int i = 0; i < numberOfStars; i++)
        {
            star = "Star " + StringHelper.IndexIntToChar(i);
            float radius = statistics[star + " Radius"];
            if (radius > largest)
            {
                largest = radius;
                largestStar = i;
            }
        }
        statistics["Singularity Star"] = new Statistic("Singularity Star", Statistic.ValueType.Integer, largestStar);

        statistics["Singularities"].Set(1);

        List<string> dungeonTargets = statistics["Dungeon Targets"].Get<object>() as List<string>;
        //dungeonTargets.Add("Star " + StringHelper.IndexIntToChar(i));
        string starName = "Star " + StringHelper.IndexIntToChar(largestStar);
        if (dungeonTargets.Contains(starName))
        {
            dungeonTargets.Remove(starName);
            dungeonTargets.Add("Singularity");
        }
    }

    public void PopulateStars()
    {
        int numberOfStars = statistics["Stars"];
        int largestStar = statistics["Singularity Star"];
        string star = "Star " + StringHelper.IndexIntToChar(largestStar);
        Destroy(statistics[star + " GO"].Get<GameObject>());

        string singularityName = "Singularity";

        singularity = Instantiate(singularityPrefab, statistics[star + " Position"].Get<Vector2>(), singularityPrefab.transform.rotation, transform);
        singularity.transform.localPosition = statistics[star + " Position"].Get<Vector2>();
        singularity.name = singularityName;
        singularity.environment = statistics;

        // Foreach star in the system determine whether and how much they would be leeched by the blackhole
        // Creating the appropriate prefab to match the needs of the leeching
        float accretion = 0;

        int convertedStar = statistics["Singularity Star"].Get<int>();
        star = "Star " + StringHelper.IndexIntToChar(convertedStar);
        Vector2 position = statistics[star + " Position"];

        for (int i = 0; i < numberOfStars; i++)
        {
            string name = "Star " + StringHelper.IndexIntToChar(i);

            float distanceFromSingularity = Vector2.Distance(position, statistics[name + " Position"]);

            //accretion += statistics[name + " Radius"] * Mathf.Clamp(1f - (distanceFromSingularity / 10000f), 0f, 1f);
            if (1 == Random.Range(1, (int)distanceFromSingularity / 100f))
            {
                if (i != convertedStar)
                {
                    Destroy(statistics[name + " GO"].Get<GameObject>());
                    accretion += statistics[name + " Radius"];
                }
            }
            else if (distanceFromSingularity < singularity.transform.localScale.x * 500f && i != convertedStar)
            {
                GameObject foundStar = statistics[name + " GO"].Get<GameObject>();
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

    public void Planets()
    {

    }

    public void PopulatePlanets()
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
                Destroy(statistics[name + " GO"].Get<GameObject>());
                accretion += statistics[name + " Radius"];

                // MOONS ---------------------------------------------------------------------------------------------------------------------------------------------------------------------
                int numberOfMoons = statistics[name + " Moons"];
                for (int j = 0; j < numberOfMoons; j++)
                {
                    string moonName = name + " Moon " + StringHelper.IndexIntToChar(j);

                    Destroy(statistics[moonName + " GO"].Get<GameObject>());
                    accretion += statistics[moonName + " Radius"];
                }
            }
            else
            {
                if (distanceFromSingularity < singularity.transform.localScale.x * 1000f)
                {
                    GameObject foundPlanet = statistics[name + " GO"].Get<GameObject>();
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


            }
        }

        statistics["Singularity Accretion"].Set(statistics["Singularity Accretion"].Get<float>() + accretion);
    }

    public void Moons()
    {

    }

    public void PopulateMoons()
    {
        float accretion = 0;

        string star = "Star " + StringHelper.IndexIntToChar(statistics["Singularity Star"].Get<int>());
        Vector2 position = statistics[star + " Position"];
        int numberOfPlanets = statistics["Planets"];
        for (int i = 0; i < numberOfPlanets; i++)
        {
            string name = "Planet " + StringHelper.IndexIntToChar(i);
            // MOONS ---------------------------------------------------------------------------------------------------------------------------------------------------------------------
            int numberOfMoons = statistics[name + " Moons"];
            for (int j = 0; j < numberOfMoons; j++)
            {
                string moonName = name + " Moon " + StringHelper.IndexIntToChar(j);

                float distanceFromSingularity = Vector2.Distance(position, statistics[name + " Position"]);

                if (1 == Random.Range(1, (int)distanceFromSingularity / 100))
                {
                    Destroy(statistics[moonName + " GO"].Get<GameObject>());
                    accretion += statistics[moonName + " Radius"];
                }
            }
        }
    }

    public void Dungeons()
    {
        List<DungeonType> dungeons = new List<DungeonType>();

        statistics["Dungeons"] = new Statistic("Dungeons", Statistic.ValueType.Object, dungeons);

        //List<string> dungeonTargets = statistics["Dungeon Targets"].Get<object>() as List<string>;

        foreach (string target in DungeonType.targets)
        {
            for (int i = 0; i < 5; i++)
            {
                DungeonType dungeonType = DungeonType.SelectByChance(dungeons.FindAll(dungeon => dungeon.target == target));
                dungeonType.tags = StringHelper.TagParse(dungeonType.tags);
                dungeons.Add(dungeonType);
            }
        }
    }

    public void PopulateDungeons()
    {
        List<DungeonGenerator> dungeonables = new List<DungeonGenerator>();
        dungeonables.AddRange(GameObject.FindObjectsOfType<DungeonGenerator>());
        List<string> dungeonTargets = statistics["Dungeon Targets"].Get<object>() as List<string>;
        List<DungeonType> activeDungeons = statistics["Active Dungeons"].Get<object>() as List<DungeonType>;

        int numberOfDungeons = Mathf.Min(5, Mathf.Max(1, Random.Range(0, dungeonTargets.Count)));

        for (int i = 0; i < numberOfDungeons; i++)
        {
            DungeonGenerator dungeon = dungeonables.Find(dng => dng.name == dungeonTargets[i]);

            dungeon.hash = Random.Range(int.MinValue, int.MaxValue);

            List<DungeonType> eligibleDungeonTypes = statistics["Dungeons"].Get<object>() as List<DungeonType>;

            if (dungeon.name == "Singularity")
                dungeon.dungeonType = eligibleDungeonTypes.Find(dng => dng.target == dungeon.dungeonTarget);
            //else
            //    dungeon.dungeonType = eligibleDungeonTypes.Find(dng => dng.target == "any" || dng.target == dungeon.dungeonTarget);

            eligibleDungeonTypes.Remove(dungeon.dungeonType);
            dungeon.riskLevel = FloatHelper.RiskStringToFloat(dungeon.dungeonType.risk) * Random.Range(0.5f, 2f);
            //dungeon.dungeonType.tags = StringHelper.TagParse(dungeon.dungeonType.tags);
            dungeon.generates = true;

            activeDungeons.Add(dungeon.dungeonType);
        }
    }
}
