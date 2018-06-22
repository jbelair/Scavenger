using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemGenerator : MonoBehaviour
{
    public EnvironmentBasedStar starPrefab;
    public EnvironmentBasedPlanet planetPrefab;
    public LineRendererCircle orbitPrefab;

    public bool generateRandom = false;
    public bool generateRandomLoop = false;
    public float generateTiming = 1.0f;
    public float stopTiming = 2.0f;
    public int stopForStars = -1;
    public int stopForPlanets = -1;

    public bool starsDisplayOrbits = true;
    public bool planetsDisplayOrbits = true;
    public AnimationCurve starPlotStars;
    public AnimationCurve starPlotRadius;
    //public AnimationCurve starPlotMass;
    public AnimationCurve starPlotKelvin;

    public AnimationCurve planetPlotPlanets;
    public AnimationCurve planetPlotRadius;
    public AnimationCurve planetPlotKelvin;

    public int hash = 0;

    public Statistics statistics;

    // Use this for initialization
    void Start()
    {
        if (!statistics)
            statistics = GetComponent<Statistics>();

        if (statistics)
        {
            if (!generateRandom)
                Generate();
            else
            {
                if (generateRandomLoop)
                    StartCoroutine(GenerateTheatricRandom());
                else
                    GenerateRandom();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //hash = Hash(statistics["System Coordinates"].Get<Vector2>());
    }

    IEnumerator GenerateTheatricRandom()
    {
        while (generateRandom)
        {
            int stars = statistics["Stars"];
            int planets = statistics["Planets"];
            if ((stopForStars > 0 && stopForStars <= stars) || (stopForPlanets > 0 && stopForPlanets <= planets))
            {
                yield return new WaitForSeconds(stopTiming);

                GenerateRandom();

                yield return new WaitForSeconds(generateTiming);
            }
            else
            {
                GenerateRandom();

                yield return new WaitForSeconds(generateTiming);
            }
        }

        yield return null;
    }

    public void GenerateRandom()
    {
        Clear();

        statistics["System Coordinates"].Set(Random.insideUnitCircle * 100000000);

        Generate();
    }

    public int Hash(Vector2 c)
    {
        int h = (int)c.x * 374761393 + (int)c.y * 668265263; //all constants are prime
        h = (h ^ (h >> 13)) * 1274126177;
        h = h ^ (h >> 16);
        return h;
    }

    public void Generate()
    {
        GenerateEnvironment();
        GenerateGameObjects();
    }

    public void GenerateEnvironment()
    {
        hash = Hash(statistics["System Coordinates"].Get<Vector2>());

        Random.InitState(hash);

        Debug.Log("System Generation -----\nEnvironment Data Generation Started\nPosition: " + statistics["System Coordinates"] + "\nHash: " + hash);

        // Stellar Temperature tracks the average temperature of the stellar core (used for approximations of system temperature)
        float stellarTemperature = 0;
        float stellarRadius = 0;

        // Stars generates with a heavy weight towards 0.5, this 0-1 value is used in an animation curve to generate the system with correct skewing
        float stars = Random.Range(0f, 1f);
        int numberOfStars = Mathf.RoundToInt(starPlotStars.Evaluate(stars));
        statistics["Stars"] = new Statistic("Stars", Statistic.ValueType.Integer, numberOfStars);

        // Each star generated has a position that must be offset from the center of mass, this stores those positions
        List<Vector2> starPositions = new List<Vector2>();
        float[] contributions = new float[numberOfStars];
        float starPositionContributions = 0;
        Vector2 centerOfMass = Vector2.zero;
        float greatestDistanceBetweenStars = 0;
        float[] orbitDistances = new float[numberOfStars];
        for (int i = 0; i < numberOfStars; i++)
        {
            Debug.Log("Generating star environment data for " + (i + 1) + " of " + numberOfStars);

            string name = "Star " + StringHelper.IndexIntToChar(i);
            // Radius is highly likely to be around 0.5
            float radius = Random.Range(0f, 1f);
            stellarRadius += radius;

            float radiusSkewed = starPlotRadius.Evaluate(radius);
            statistics[name + " Radius"] = new Statistic(name + " Radius", Statistic.ValueType.Float, radiusSkewed);

            float kelvin = Random.Range(0f, 1f);
            stellarTemperature += kelvin;

            float kelvinSkewed = starPlotKelvin.Evaluate(kelvin);
            statistics[name + " Kelvin"] = new Statistic(name + " Kelvin", Statistic.ValueType.Float, kelvinSkewed);

            float kelvinRange = (Random.Range(0, kelvinSkewed) + Random.Range(0, kelvinSkewed) + Random.Range(0, kelvinSkewed)) / 3;
            statistics[name + " Kelvin Range"] = new Statistic(name + " Kelvin Range", Statistic.ValueType.Float, kelvinRange);

            starPositions.Add(Random.insideUnitCircle * 1000f);
            statistics[name + " Position"] = new Statistic(name + " Position", Statistic.ValueType.Vector2, starPositions[i]);

            contributions[i] = (radius * 10) + (kelvin * 10);
            centerOfMass += starPositions[i] * contributions[i];
            starPositionContributions += contributions[i];

            starPositions[i] -= centerOfMass;
            orbitDistances[i] = statistics["Star " + StringHelper.IndexIntToChar(i) + " Kelvin"] / 20f + 1000f * Mathf.Pow(statistics["Star " + StringHelper.IndexIntToChar(i) + " Kelvin"] / 30000f, 2);
        }
        centerOfMass /= starPositionContributions;

        // Get the greatest distance between stars
        for (int i = 0; i < numberOfStars; i++)
        {
            for (int j = i + 1; j < numberOfStars; j++)
            {
                float distanceBetweenStars = (starPositions[i] - starPositions[j]).magnitude;
                if (distanceBetweenStars > greatestDistanceBetweenStars)
                    greatestDistanceBetweenStars = distanceBetweenStars;
            }
        }

        statistics["System Center"] = new Statistic("System Center", Statistic.ValueType.Vector2, centerOfMass);

        stellarRadius /= numberOfStars;
        stellarTemperature /= numberOfStars;
        statistics["Temperature"] = new Statistic("Temperature", Statistic.ValueType.Float, stellarTemperature);

        float planets = Random.Range(0, 1f) / numberOfStars * (1f - Mathf.Pow(Mathf.Abs((stellarTemperature - 0.5f) * 2), 2)) * (1f - Mathf.Pow(Mathf.Abs((stellarRadius - 0.5f) * 2), 2));
        int numberOfPlanets = Mathf.RoundToInt(planetPlotPlanets.Evaluate(planets));
        statistics["Planets"] = new Statistic("Planets", Statistic.ValueType.Integer, numberOfPlanets);

        for (int i = 0; i < numberOfPlanets; i++)
        {
            Debug.Log("Generating planet environment data for " + (i + 1) + " of " + numberOfPlanets);

            string name = "Planet " + StringHelper.IndexIntToChar(i);

            // Stars can generate quite far apart
            // Planets can generate around 1 or more stars
            // Pick a star at random
            // Pick an orbit position around that star based on its temperature
            // Then determine the center of mass that position would orbit around
            // Then orbit

            // Pick a star
            int star = Random.Range(0, numberOfStars);
            // Define a distance from it
            float orbitDistance = orbitDistances[star] * Random.Range(1f, 2f);// + 100f * Mathf.Pow(statistics["Star " + StringHelper.IndexIntToChar(star) + " Kelvin"] / 30000f, 2);
            orbitDistances[star] = orbitDistance;
            // Make a random point along that distance
            Vector2 position = starPositions[star] + Random.insideUnitCircle.normalized * orbitDistance;

            float[] distances = new float[numberOfStars];
            // Iterate until the planet is not too close to another star

            for (int j = 0; j < numberOfStars; j++)
            {
                // Calculate the distance between the planet and each star
                distances[j] = (starPositions[j] - position).magnitude;
            }

            for (int j = 0; j < numberOfStars; j++)
            {
                if ((starPositions[j] - position).magnitude < (starPositions[star] - position).magnitude)
                    star = j;
            }

            float kelvin = 0;
            // Define the center of the orbit
            Vector2 orbitCenter = Vector2.zero;
            float total = 0;
            float greatestDistance = 0;
            float lowestDistance = 10000f;
            for (int j = 0; j < numberOfStars; j++)
            {
                float starKelvin = statistics["Star " + StringHelper.IndexIntToChar(j) + " Kelvin"];
                float logKelvin = Mathf.Log10(starKelvin);
                kelvin += planetPlotKelvin.Evaluate(distances[j] / (250 * Mathf.Pow(logKelvin, 2))) * (starKelvin / (logKelvin / 2));
                // Weight distances against the greatest distance between stars
                distances[j] /= greatestDistanceBetweenStars;
                distances[j] *= distances[j];
                total += distances[j];
                if (distances[j] > greatestDistance)
                    greatestDistance = distances[j];
                if (distances[j] < lowestDistance)
                    lowestDistance = distances[j];
            }

            if (numberOfStars > 1 && lowestDistance > 0.5f)
            {
                for (int j = 0; j < numberOfStars; j++)
                {
                    orbitCenter += starPositions[j] * (greatestDistance - distances[j]);
                }

                orbitCenter /= total;

                position = (position - orbitCenter).normalized * orbitDistance + orbitCenter;
            }
            else
            {
                orbitCenter = starPositions[star];
            }

            bool failed = false;
            for (int j = 0; j < numberOfStars; j++)
            {
                float orbitOverlap = Mathf.Abs((starPositions[j] - orbitCenter).magnitude - orbitDistance);
                float minimumDistance = statistics["Star " + StringHelper.IndexIntToChar(j) + " Kelvin"] / 20f + 1000f * Mathf.Pow(statistics["Star " + StringHelper.IndexIntToChar(j) + " Kelvin"] / 30000f, 2);
                if (orbitOverlap < minimumDistance)
                {
                    failed = true;
                }
            }

            if (failed)
                continue;

            float radius = Random.Range(0f, 1f);
            float radiusSkewed = planetPlotRadius.Evaluate(radius);
            statistics[name + " Radius"] = new Statistic(name + " Radius", Statistic.ValueType.Float, radiusSkewed);
            statistics[name + " Kelvin"] = new Statistic(name + " Kelvin", Statistic.ValueType.Float, kelvin);
            statistics[name + " Position"] = new Statistic(name + " Position", Statistic.ValueType.Vector2, position);
            statistics[name + " Orbit"] = new Statistic(name + " Orbit", Statistic.ValueType.Vector2, orbitCenter);
        }

        // Statistics Over Time
        if (statistics.Has("Statistics: Generated"))
            statistics["Statistics: Generated"].Set(statistics["Statistics: Generated"].Get<int>() + 1);

        int generated = statistics["Statistics: Generated"].Get<int>();

        if (statistics.Has("Statistics: Stars"))
            statistics["Statistics: Stars"].Set((statistics["Statistics: Stars"].Get<float>() * (generated - 1) + statistics["Stars"].Get<int>()) / generated);
        else
            statistics["Statistics: Stars"] = new Statistic("Statistics: Stars", Statistic.ValueType.Float, statistics["Stars"].Get<int>());

        Debug.Log("System Generation -----\nEnvironment Data Generation Complete");
    }

    public void GenerateGameObjects()
    {
        Debug.Log("System Generation -----\nGame Object Generation Complete\nPosition: " + statistics["System Coordinates"] + "\nHash: " + hash);

        int numberOfStars = statistics["Stars"];
        for (int i = 0; i < numberOfStars; i++)
        {
            Debug.Log("Generating star " + (i + 1) + " of " + numberOfStars);

            string name = "Star " + StringHelper.IndexIntToChar(i);
            EnvironmentBasedStar star = Instantiate(starPrefab, statistics[name + " Position"].Get<Vector2>(), starPrefab.transform.rotation, transform);
            star.name = name;
            star.environment = statistics;

            // ORBIT ---------------------------------------------------------------------------------------------------------------------------------------------------------------------
            if (starsDisplayOrbits && numberOfStars > 1)
            {
                if (numberOfStars < 3)
                {
                    Vector2 delta = star.transform.position;
                    float atan = Mathf.Atan2(delta.y, delta.x);
                    LineRendererCircle orbit = Instantiate(orbitPrefab, Vector3.zero, Quaternion.Euler(0, 0, atan * Mathf.Rad2Deg), transform);
                    orbit.radius = star.transform.position.xy().magnitude;
                    orbit.line.startColor = ((star.starMaterials[0].GetTexture("_Emissive") as Texture2D).GetPixelBilinear((statistics[name + " Kelvin"].Get<float>() + statistics[name + " Kelvin Range"].Get<float>()) / star.starMaterials[0].GetFloat("_KelvinMax"), 0) * 8).Normalize() * 0.5f;
                    orbit.line.endColor = Color.clear;
                }
            }
        }

        int numberOfPlanets = statistics["Planets"];
        for (int i = 0; i < numberOfPlanets; i++)
        {
            Debug.Log("Generating planet " + (i + 1) + " of " + numberOfPlanets);

            string name = "Planet " + StringHelper.IndexIntToChar(i);
            Vector2 orbitCenter = statistics[name + " Orbit"];
            EnvironmentBasedPlanet planet = Instantiate(planetPrefab, statistics[name + " Position"].Get<Vector2>(), planetPrefab.transform.rotation, transform);
            planet.name = name;
            planet.environment = statistics;

            // ORBIT ---------------------------------------------------------------------------------------------------------------------------------------------------------------------
            if (planetsDisplayOrbits)
            {
                Vector2 delta = planet.transform.position.xy() - orbitCenter;
                float atan = Mathf.Atan2(delta.y, delta.x);
                LineRendererCircle orbit = Instantiate(orbitPrefab, orbitCenter, Quaternion.Euler(0, 0, atan * Mathf.Rad2Deg), transform);
                orbit.radius = (delta).magnitude;
                orbit.line.startColor = Color.green * 0.5f;
                orbit.line.endColor = Color.clear;
            }
        }

        Debug.Log("System Generation\nGame Object Generation Completed");
    }

    public void Clear()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
