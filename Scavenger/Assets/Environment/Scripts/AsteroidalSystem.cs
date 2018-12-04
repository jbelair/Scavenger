using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class AsteroidalSystem : MonoBehaviour, ISystemGeneratorDecorator
{
    public EnvironmentBasedAsteroid asteroidPrefab;
    public EnvironmentBasedAsteroidCloud asteroidCloudPrefab;
    public EnvironmentBasedAsteroidBelt asteroidBeltPrefab;

    [Header("Asteroids")]
    public AnimationCurve asteroidPlotNumber;
    public AnimationCurve oortPlotRadius;

    public Statistics statistics;

    public void Start()
    {
        if (!statistics)
            statistics = GetComponent<Statistics>();
    }

    private int dungeons;
    private List<string> preAsteroidTargets = new List<string>();
    public void System()
    {
        Texture2D map = Maps.Map("asteroids");
        Vector3Int coordinates = (statistics["System Coordinates"].Get<Vector3>() + new Vector3(map.width / 2f - 1, map.height / 2f - 1, 0)).ToInt();
        Color mapSample = map.GetPixel(coordinates.x, coordinates.y);
        float asteroidSample = mapSample.r;

        float systemSeed = UnityEngine.Random.Range(0, 1f);
        int numberOfAsteroids = Mathf.RoundToInt(asteroidPlotNumber.Evaluate(asteroidSample * 0.8f + systemSeed * 0.2f));
        int numberOfStars = statistics.Has("Stars") ? statistics["Stars"] : 0;
        int asteroids = Mathf.CeilToInt(numberOfAsteroids / 2f);
        int asteroid = numberOfAsteroids - asteroids;
        statistics["Metallicity"] = new Statistic("Metallicity", Statistic.ValueType.Integer, numberOfAsteroids);
        statistics["Asteroids"] = new Statistic("Asteroids", Statistic.ValueType.Integer, asteroids);
        statistics["Asteroid"] = new Statistic("Asteroid", Statistic.ValueType.Integer, asteroid);

        asteroidsGenerated = asteroidGenerated = 0;
    }

    private int asteroidsGenerated = 0;
    private int asteroidGenerated = 0;
    public void Stars()
    {
        int numberOfAsteroids = statistics["Asteroids"];
        //int numberOfAsteroid = statistics["Asteroid"];
        int numberOfStars = statistics.Has("Stars") ? statistics["Stars"] : 0;
        float stellarTemperature = statistics.Has("Temperature") ? statistics["Temperature"] : 0;
        //float stellarRadius = statistics.Has("Radius") ? statistics["Radius"] : 0;
        //Vector2 systemCenter = statistics.Has("System Center") ? statistics["System Center"] : Vector2.zero;

        for (int i = 0; i < numberOfAsteroids; i++)
        {
            // The first asteroid will always be an Oort Cloud.
            // If a system has no stars then the oort cloud in fact becomes a system wide asteroid cloud.
            if (i == 0)
            {
                float oortCloudInnerRadius = (numberOfStars > 0) ? oortPlotRadius.Evaluate(stellarTemperature) : 1;
                float oortCloudOuterRadius = (numberOfStars > 0) ? SystemsGenerator.active.scale / 2f + SystemsGenerator.active.scale * stellarTemperature : SystemsGenerator.active.scale * UnityEngine.Random.Range(0.5f, 2f);
                statistics["Oort Cloud Inner Radius"] = new Statistic("Oort Cloud Inner Radius", Statistic.ValueType.Float, oortCloudInnerRadius);
                statistics["Oort Cloud Outer Radius"] = new Statistic("Oort Cloud Outer Radius", Statistic.ValueType.Float, oortCloudOuterRadius);
                asteroidsGenerated++;
            }
        }
    }

    public void PopulateStars()
    {
        int numberOfAsteroids = statistics["Asteroids"];
        int numberOfAsteroid = statistics["Asteroid"];

        Texture2D map = Maps.Map("asteroids");
        Vector3Int coordinates = (statistics["System Coordinates"].Get<Vector3>() + new Vector3(map.width / 2f - 1, map.height / 2f - 1, 0)).ToInt();
        Color mapSample = map.GetPixel(coordinates.x, coordinates.y);

        if (numberOfAsteroids > 0)
        {
            EnvironmentBasedAsteroidCloud cloud = Instantiate(asteroidCloudPrefab, transform);
            cloud.name = "Oort Cloud";
            cloud.environment = statistics;
        }

        if (numberOfAsteroids > 1)
        {
            int numberOfStars = statistics.Has("Stars") ? statistics["Stars"] : 0;
            if (numberOfStars > 0)
            {
                Vector3 biggestStar = statistics[statistics["Most Massive Star"].Get<string>() + " GO"].Get<GameObject>().transform.localPosition;
                int systemRings = Mathf.CeilToInt(numberOfAsteroids - 1);
                float starDistance = Mathf.Max(GetComponent<StandardSystem>().starDistance / 2f, statistics.Has("Stellar Distance") ? statistics["Stellar Distance"] * 0.5f : 0);
                for (int i = 0; i < systemRings; i++)
                {
                    EnvironmentBasedAsteroidBelt belt = Instantiate(asteroidBeltPrefab, transform);
                    belt.transform.localPosition = biggestStar;
                    belt.transform.localScale = Vector3.one * starDistance;
                    starDistance += starDistance * (i + 1);
                    belt.environment = statistics;
                }
            }
        }

        if (numberOfAsteroid > 0)
        {

        }
    }

    public void Planets()
    {

    }

    public void PopulatePlanets()
    {

    }

    public void Moons()
    {

    }

    public void PopulateMoons()
    {

    }

    public void Dungeons()
    {
        List<string> dungeonTargets = statistics["Dungeon Targets"].Get<object>() as List<string>;
        preAsteroidTargets.AddRange(dungeonTargets.ToArray());
        dungeons = dungeonTargets.Count;

        int numberOfAsteroids = statistics["Asteroids"];
        int numberOfAsteroid = statistics["Asteroid"];
        int numberOfStars = statistics["Stars"];

        for (int i = 0; i < numberOfAsteroids; i++)
        {
            if (UnityEngine.Random.Range(0, 8) == 0 && dungeonTargets.Count < Environment.maximumDungeons)
            {
                if (i == 0)
                    dungeonTargets.Add("Oort Cloud");
                else if (numberOfStars > 0)
                    dungeonTargets.Add("Asteroid Belt " + StringHelper.IndexIntToChar(i - 1));
                else
                    dungeonTargets.Add("Asteroid Cloud " + StringHelper.IndexIntToChar(i - 1));
                //dungeons++;
            }
        }

        for (int i = 0; i < numberOfAsteroid; i++)
        {
            if (UnityEngine.Random.Range(0, 8) == 0 && dungeonTargets.Count < Environment.maximumDungeons)
            {
                dungeonTargets.Add("Asteroid " + StringHelper.IndexIntToChar(i));
                //dungeons++;
            }
        }

        dungeonTargets.Shuffle();
    }

    public void PopulateDungeons()
    {

    }
}
