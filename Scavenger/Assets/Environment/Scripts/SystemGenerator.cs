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
    public bool moonsDisplayOrbits = true;
    public AnimationCurve starPlotStars;
    public AnimationCurve starPlotRadius;
    public AnimationCurve starPlotKelvin;

    public AnimationCurve planetPlotPlanets;
    public AnimationCurve planetPlotRadius;
    public AnimationCurve planetPlotMoons;

    public int hash = 0;

    public Statistics statistics;

    // Use this for initialization
    void Start()
    {
        if (!statistics)
            statistics = GetComponent<Statistics>();

        if (statistics)
        {
            statistics.Initialise();

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

    }

    IEnumerator GenerateTheatricRandom()
    {
        while (generateRandom)
        {
            int stars = statistics["Stars"];
            int planets = statistics["Planets"];

            if ((stopForStars >= 0 && stopForStars <= stars) || (stopForPlanets >= 0 && stopForPlanets <= planets))
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

        statistics["System Coordinates"].Set((Vector3)(Random.insideUnitCircle * 1000000).Round());

        Debug.Log("RANDOM\nPosition: " + statistics["System Coordinates"].Get<Vector3>());

        Generate();
    }

    public int Hash(Vector3 c)
    {
        int h = (int)c.x * 374761393 + (int)c.y * 668265263 + (int)c.z * 1800560953; //all constants are prime
        h = (h ^ (h >> 13)) * 1274126177;
        h = h ^ (h >> 16);
        return h;
    }

    public void Generate()
    {
        System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();
        GenerateEnvironment();
        GenerateGameObjects();
        stopwatch.Stop();
        Debug.Log("System Generation -----\nTime: " + stopwatch.ElapsedMilliseconds + "ms");
    }

    public void GenerateEnvironment()
    {
        hash = Hash(statistics["System Coordinates"].Get<Vector3>());

        Random.InitState(hash);

        Debug.Log("System Generation -----\nEnvironment Data Generation Started\nPosition: " + statistics["System Coordinates"].Get<Vector2>() + "\nHash: " + hash);

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

            float kelvin = (Random.Range(0f, 1f) + radius) / 2f;
            stellarTemperature += kelvin;

            float kelvinSkewed = starPlotKelvin.Evaluate(kelvin);
            statistics[name + " Kelvin"] = new Statistic(name + " Kelvin", Statistic.ValueType.Float, kelvinSkewed);

            float kelvinRange = (Random.Range(0, kelvinSkewed / 2f) + Random.Range(0, kelvinSkewed / 2f) + Random.Range(0, kelvinSkewed / 2f)) / 3;
            statistics[name + " Kelvin Range"] = new Statistic(name + " Kelvin Range", Statistic.ValueType.Float, kelvinRange);

            starPositions.Add(Random.insideUnitCircle * 1000f);

            contributions[i] = Mathf.Pow(2, (radius + 1) * 5f) + Mathf.Pow((radius + 1f) * 2f, (kelvin + 1f) * 10f);// (radius * 10) + (kelvin * 10);
            centerOfMass += starPositions[i] * contributions[i];
            starPositionContributions += contributions[i];

            orbitDistances[i] = statistics["Star " + StringHelper.IndexIntToChar(i) + " Kelvin"] / 20f + 1000f * Mathf.Pow(statistics["Star " + StringHelper.IndexIntToChar(i) + " Kelvin"] / 30000f, 2);
        }
        centerOfMass /= starPositionContributions;

        // Get the greatest distance between stars
        for (int i = 0; i < numberOfStars; i++)
        {
            starPositions[i] -= centerOfMass;
            string name = "Star " + StringHelper.IndexIntToChar(i);
            statistics[name + " Position"] = new Statistic(name + " Position", Statistic.ValueType.Vector2, starPositions[i]);

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

        float maximumPlanetaryDistance = 0f;
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
            // Make a random point at that distance around a circle's circumference
            Vector2 position = starPositions[star] + Random.insideUnitCircle.normalized * orbitDistance;

            float[] distances = new float[numberOfStars];
            distances[star] = orbitDistance;
            for (int j = 0; j < numberOfStars; j++)
            {
                // Calculate the distance between the planet and each star
                distances[j] = (starPositions[j] - position).magnitude;
                if (distances[j] / contributions[j] < distances[star] / contributions[star])
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
                //float logKelvin = Mathf.Log10(starKelvin);
                float starRadius = EnvironmentRules.StellarRadius(statistics["Star " + StringHelper.IndexIntToChar(j) + " Radius"]);
                kelvin += EnvironmentRules.PlanetTemperature(starRadius, EnvironmentRules.StellarLuminosity(starRadius, starKelvin), EnvironmentRules.PlanetDistance(distances[j]), 0.3f);//planetPlotKelvin.Evaluate(distances[j] / (250 * Mathf.Pow(logKelvin, 2))) * (starKelvin / (logKelvin / 2));
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

                //position = (position - orbitCenter).normalized * orbitDistance + orbitCenter;
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

            statistics[name + " Failed"] = new Statistic(name + " Failed", Statistic.ValueType.Integer, ((failed) ? 1 : 0));

            Vector2 delta = position - orbitCenter;
            // I need to find the hottest, and coldest point around the orbit
            float kelvinLow = 30000;
            float kelvinHigh = 0;
            float distance = delta.magnitude;

            for (float k = 0; k < Mathf.PI * 2f; k += Mathf.PI / 32f)
            {
                float kelvinCurrent = 0;
                Vector2 pos = orbitCenter + new Vector2(Mathf.Cos(k), Mathf.Sin(k)) * distance;
                for (int j = 0; j < numberOfStars; j++)
                {
                    string starName = "Star " + StringHelper.IndexIntToChar(j);
                    float starRadius = EnvironmentRules.StellarRadius(statistics[starName + " Radius"].Get<float>());
                    float starTemperature = statistics[starName + " Kelvin"].Get<float>();
                    float stellarDistance = EnvironmentRules.PlanetDistance((statistics[starName + " Position"].Get<Vector2>() - pos).magnitude);
                    kelvinCurrent += EnvironmentRules.PlanetTemperature(starRadius, EnvironmentRules.StellarLuminosity(starRadius, starTemperature), stellarDistance, 0.3f);
                }

                if (kelvinCurrent < kelvinLow)
                    kelvinLow = kelvinCurrent;
                if (kelvinCurrent > kelvinHigh)
                    kelvinHigh = kelvinCurrent;
            }

            maximumPlanetaryDistance = Mathf.Max(distance, maximumPlanetaryDistance);

            GeneratePlanet(name, 0, kelvin, kelvinLow, kelvinHigh, orbitCenter, distance, delta);

            //statistics[name + " Radius"] =                  new Statistic(name + " Radius",                     Statistic.ValueType.Float,      0f);
            //statistics[name + " Kelvin"] =                  new Statistic(name + " Kelvin",                     Statistic.ValueType.Float,      kelvin);
            //statistics[name + " Kelvin Low"] =              new Statistic(name + " Kelvin Low",                 Statistic.ValueType.Float,      kelvinLow);
            //statistics[name + " Kelvin High"] =             new Statistic(name + " Kelvin High",                Statistic.ValueType.Float,      kelvinHigh);
            //statistics[name + " Actual Kelvin"] =           new Statistic(name + " Actual Kelvin",              Statistic.ValueType.Float,      kelvin);
            //statistics[name + " Actual Kelvin Low"] =       new Statistic(name + " Actual Kelvin Low",          Statistic.ValueType.Float,      kelvinLow);
            //statistics[name + " Actual Kelvin High"] =      new Statistic(name + " Actual Kelvin High",         Statistic.ValueType.Float,      kelvinHigh);
            //statistics[name + " Position"] =                new Statistic(name + " Position",                   Statistic.ValueType.Vector2,    delta);
            //statistics[name + " Orbit"] =                   new Statistic(name + " Orbit",                      Statistic.ValueType.Vector2,    orbitCenter);
            //statistics[name + " Orbit Distance"] =          new Statistic(name + " Orbit Distance",             Statistic.ValueType.Float,      distance);
            //statistics[name + " Profile"] =                 new Statistic(name + " Profile",                    Statistic.ValueType.Integer,    Random.Range(0, 1000000));
            //statistics[name + " Surface"] =                 new Statistic(name + " Surface",                    Statistic.ValueType.Integer,    Random.Range(0, 1000000));
            //statistics[name + " Clouds"] =                  new Statistic(name + " Clouds",                     Statistic.ValueType.Integer,    Random.Range(0, 1000000));
            //statistics[name + " Atmosphere"] =              new Statistic(name + " Atmosphere",                 Statistic.ValueType.Integer,    Random.Range(0, 1000000));
            //statistics[name + " Atmosphere Intensity"] =    new Statistic(name + " Atmosphere Intensity",       Statistic.ValueType.Float,      0);
            //statistics[name + " Atmosphere Density"] =      new Statistic(name + " Atmosphere Density",         Statistic.ValueType.Float,      0);
        }

        int gasGiants = 0;
        for (int i = 0; i < numberOfPlanets; i++)
        {
            string name = "Planet " + StringHelper.IndexIntToChar(i);

            float kelvin = statistics[name + " Kelvin"].Get<float>();
            float kelvinLow = statistics[name + " Kelvin Low"].Get<float>();
            float kelvinHigh = statistics[name + " Kelvin High"].Get<float>();
            float distance = statistics[name + " Orbit Distance"].Get<float>();

            float distanceRatio = distance / maximumPlanetaryDistance * Random.Range(0.8f,1.2f);
            float radius = planetPlotRadius.Evaluate(distanceRatio);
            if (kelvinLow < 170f)
            {
                gasGiants++;
                radius *= Mathf.Max(1, 8f / (numberOfStars * gasGiants));
            }
            //else
            //    radius *= 20f / numberOfPlanets;

            UpdatePlanet(name, radius, kelvin, kelvinLow, kelvinHigh);

            //float atmoIntensity = (kelvin / 200f) * Mathf.Max(0, 1f - (kelvin / 1500f));
            //float atmoDensity = Mathf.Max(1, (20f - kelvin / 100f) * Mathf.Max(0, 1f - (kelvin / 2000f)) * kelvin / 200f);
            //float kelvinRange = (kelvinHigh - kelvinLow) * (atmoIntensity * atmoDensity);
            //float planetKelvin = kelvin + kelvinRange;
            //float planetKelvinLow = planetKelvin - kelvinRange;
            //float planetKelvinHigh = planetKelvin + kelvinRange;

            //statistics[name + " Radius"].Set(radius);
            //statistics[name + " Actual Kelvin"].Set(planetKelvin);
            //statistics[name + " Actual Kelvin Low"].Set(planetKelvinLow);
            //statistics[name + " Actual Kelvin High"].Set(planetKelvinHigh);
            //statistics[name + " Atmosphere Intensity"].Set(atmoIntensity);
            //statistics[name + " Atmosphere Density"].Set(atmoDensity);

            //float radiusFactor = Mathf.Pow(radius / 4f, 5);
            //int minimum = Mathf.FloorToInt(radius);
            //int maximum = (int)Mathf.Min(32, Mathf.Pow(2 + minimum * 2, 2));
            //int numberOfMoons = Random.Range(minimum, maximum);
            int numberOfMoons = (int)planetPlotMoons.Evaluate(Random.Range(0, 1f) * radius);
            statistics[name + " Moons"] = new Statistic(name + " Moons", Statistic.ValueType.Integer, numberOfMoons);
            float distanceLast = radius * 1.5f * Random.Range(1f, 3f);
            for (int j = 0; j < numberOfMoons; j++)
            {
                Debug.Log("Generating moon environment data for planet " + (i + 1) + " of " + numberOfPlanets + " moon " + (j + 1) + " of " + numberOfMoons);

                string moonName = name + " Moon " + StringHelper.IndexIntToChar(j);
                float moonRadius = (radius / (numberOfMoons + 1)) * Random.Range(0.1f,1f);
                Vector2 orbit = Random.insideUnitCircle.normalized;
                float orbitDistance = distanceLast;
                distanceLast = radius * 1.5f * Random.Range(1f, 3f); //distanceLast * Random.Range(1.01f, 1.15f) + moonRadius * 2;
                
                Vector2 delta = statistics[name + " Position"].Get<Vector2>() + orbit * orbitDistance;
                // I need to find the hottest, and coldest point around the orbit

                kelvin = 0;
                for (int l = 0; l < numberOfStars; l++)
                {
                    string starName = "Star " + StringHelper.IndexIntToChar(l);
                    float starRadius = EnvironmentRules.StellarRadius(statistics[starName + " Radius"].Get<float>());
                    float starTemperature = statistics[starName + " Kelvin"].Get<float>();
                    float stellarDistance = EnvironmentRules.PlanetDistance((statistics[starName + " Position"].Get<Vector2>() - delta).magnitude);
                    kelvin += EnvironmentRules.PlanetTemperature(starRadius, EnvironmentRules.StellarLuminosity(starRadius, starTemperature), stellarDistance, 0.3f);
                }

                kelvinLow = 30000;
                kelvinHigh = 0;
                float sampleDistance = delta.magnitude;
                Vector2 orbitCenter = statistics[name + " Orbit"].Get<Vector2>();
                for (float p = 0; p < Mathf.PI * 2f; p += Mathf.PI / 8f)
                {
                    Vector2 planetPosition = orbitCenter + new Vector2(Mathf.Cos(p), Mathf.Sin(p)) * distance;

                    for (float m = 0; m < Mathf.PI * 2f; m += Mathf.PI / 8f)
                    {
                        float kelvinCurrent = 0;

                        Vector2 moonPosition = planetPosition + new Vector2(Mathf.Cos(m), Mathf.Sin(m)) * orbitDistance;
                        for (int l = 0; l < numberOfStars; l++)
                        {
                            string starName = "Star " + StringHelper.IndexIntToChar(l);
                            float starRadius = EnvironmentRules.StellarRadius(statistics[starName + " Radius"].Get<float>());
                            float starTemperature = statistics[starName + " Kelvin"].Get<float>();
                            float stellarDistance = EnvironmentRules.PlanetDistance((statistics[starName + " Position"].Get<Vector2>() - moonPosition).magnitude);
                            kelvinCurrent += EnvironmentRules.PlanetTemperature(starRadius, EnvironmentRules.StellarLuminosity(starRadius, starTemperature), stellarDistance, 0.3f);
                        }

                        if (kelvinCurrent < kelvinLow)
                            kelvinLow = kelvinCurrent;
                        if (kelvinCurrent > kelvinHigh)
                            kelvinHigh = kelvinCurrent;
                    }
                }

                Vector2 position = orbitCenter + orbit * distance;
                GeneratePlanet(moonName, moonRadius, kelvin, kelvinLow, kelvinHigh, orbit, orbitDistance, position);
            }
        }

        // Statistics Over Time
        if (statistics.Has("Statistics: Generated"))
            statistics["Statistics: Generated"].Set(statistics["Statistics: Generated"].Get<int>() + 1);
        else
            statistics["Statistics: Generated"] = new Statistic("Statistics: Generated", Statistic.ValueType.Integer, 1);

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
            if (starsDisplayOrbits && numberOfStars == 2)
            {
                Vector2 delta = star.transform.position;
                float atan = Mathf.Atan2(delta.y, delta.x);
                LineRendererCircle orbit = Instantiate(orbitPrefab, Vector3.zero, Quaternion.Euler(0, 0, atan * Mathf.Rad2Deg), transform);
                orbit.radius = star.transform.position.XY().magnitude;
                orbit.line.startColor = ((star.starMaterials[0].GetTexture("_Emissive") as Texture2D).GetPixelBilinear((statistics[name + " Kelvin"].Get<float>() + statistics[name + " Kelvin Range"].Get<float>()) / star.starMaterials[0].GetFloat("_KelvinMax"), 0) * 8).Normalize() * 0.5f;
                orbit.line.endColor = new Color(orbit.line.startColor.r, orbit.line.startColor.g, orbit.line.startColor.b, 0f);
            }
        }

        int numberOfPlanets = statistics["Planets"];
        for (int i = 0; i < numberOfPlanets; i++)
        {
            Debug.Log("Generating planet " + (i + 1) + " of " + numberOfPlanets);

            string name = "Planet " + StringHelper.IndexIntToChar(i);

            //if (statistics[name + " Failed"].Get<int>() == 1)
            //    continue;

            Vector2 orbitCenter = statistics[name + " Orbit"];
            EnvironmentBasedPlanet planet = Instantiate(planetPrefab, statistics[name + " Position"].Get<Vector2>(), planetPrefab.transform.rotation, transform);
            planet.name = name;
            planet.environment = statistics;

            // ORBIT ---------------------------------------------------------------------------------------------------------------------------------------------------------------------
            if (planetsDisplayOrbits)
            {
                Vector2 delta = planet.transform.position.XY() - orbitCenter;
                float atan = Mathf.Atan2(delta.y, delta.x);
                LineRendererCircle orbit = Instantiate(orbitPrefab, orbitCenter, Quaternion.Euler(0, 0, atan * Mathf.Rad2Deg), transform);
                orbit.radius = (delta).magnitude;
                orbit.line.startWidth = orbit.line.endWidth = statistics[name + " Radius"].Get<float>();
                orbit.line.startColor = ColourHelper.HeatMap(statistics[name + " Kelvin"], 150, 500) * 0.5f;
                orbit.line.endColor = new Color(orbit.line.startColor.r, orbit.line.startColor.g, orbit.line.startColor.b, 0f);
            }

            // MOONS ---------------------------------------------------------------------------------------------------------------------------------------------------------------------
            int numberOfMoons = statistics[name + " Moons"];
            for (int j = 0; j < numberOfMoons; j++)
            {
                Debug.Log("Generating moon of planet " + (i + 1) + " of " + numberOfPlanets + " moon " + (j + 1) + " of " + numberOfMoons);

                string moonName = name + " Moon " + StringHelper.IndexIntToChar(j);

                orbitCenter = planet.transform.position;
                Vector2 orbitPoint = statistics[moonName + " Orbit"];
                float distance = statistics[moonName + " Orbit Distance"];

                EnvironmentBasedPlanet moon = Instantiate(planetPrefab, orbitCenter + orbitPoint * distance, planetPrefab.transform.rotation, transform);
                moon.name = moonName;
                moon.environment = statistics;

                // ORBIT ---------------------------------------------------------------------------------------------------------------------------------------------------------------------
                if (moonsDisplayOrbits)
                {
                    float atan = Mathf.Atan2(orbitPoint.y, orbitPoint.x);
                    LineRendererCircle moonOrbit = Instantiate(orbitPrefab, orbitCenter, Quaternion.Euler(0, 0, atan * Mathf.Rad2Deg), transform);
                    moonOrbit.radius = distance;
                    moonOrbit.line.startWidth = moonOrbit.line.endWidth = statistics[moonName + " Radius"].Get<float>();
                    moonOrbit.line.startColor = ColourHelper.HeatMap(statistics[moonName + " Kelvin"], 150, 500) * 0.5f;
                    moonOrbit.line.endColor = new Color(moonOrbit.line.startColor.r, moonOrbit.line.startColor.g, moonOrbit.line.startColor.b, 0f);
                }
            }
        }

        Debug.Log("System Generation -----\nGame Object Generation Completed");
    }

    public void Clear()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.activeSelf)
                Destroy(child);
        }
    }

    float AtmosphereIntensity(float kelvin)
    {
        return (kelvin / 200f) * Mathf.Max(0, 1f - (kelvin / 1500f));
    }

    float AtmosphereDensity(float kelvin)
    {
        return Mathf.Max(1, (20f - kelvin / 100f) * Mathf.Max(0, 1f - (kelvin / 2000f)));// * kelvin / 200f);
    }

    float AtmosphericKelvinRange(float kelvinRange, float intensity, float density)
    {
        return (kelvinRange / intensity) * density;
    }

    float AtmosphereKelvin(float kelvin, float kelvinRange, float intensity, float density)
    {
        return kelvin + (kelvinRange * intensity) / density;
    }

    void GeneratePlanet(string name, float radius, float kelvin, float kelvinLow, float kelvinHigh, Vector2 orbit, float orbitDistance, Vector2 position)
    {
        float intensity = AtmosphereIntensity(kelvin);
        float density = AtmosphereDensity(kelvin);
        float actualKelvinRange = AtmosphericKelvinRange(kelvinHigh - kelvinLow, intensity, density);
        float actualKelvin = AtmosphereKelvin(kelvin, actualKelvinRange, intensity, density);
        float actualKelvinLow = actualKelvin - actualKelvinRange;
        float actualKelvinHigh = actualKelvin + actualKelvinRange;

        statistics[name + " Radius"] = new Statistic(name + " Radius", Statistic.ValueType.Float, radius);
        statistics[name + " Kelvin"] = new Statistic(name + " Kelvin", Statistic.ValueType.Float, kelvin);
        statistics[name + " Kelvin Low"] = new Statistic(name + " Kelvin Low", Statistic.ValueType.Float, kelvinLow);
        statistics[name + " Kelvin High"] = new Statistic(name + " Kelvin High", Statistic.ValueType.Float, kelvinHigh);
        statistics[name + " Actual Kelvin"] = new Statistic(name + " Actual Kelvin", Statistic.ValueType.Float, actualKelvin);
        statistics[name + " Actual Kelvin Low"] = new Statistic(name + " Actual Kelvin Low", Statistic.ValueType.Float, actualKelvinLow);
        statistics[name + " Actual Kelvin High"] = new Statistic(name + " Actual Kelvin High", Statistic.ValueType.Float, actualKelvinHigh);
        statistics[name + " Orbit"] = new Statistic(name + " Orbit", Statistic.ValueType.Vector2, orbit);
        statistics[name + " Orbit Distance"] = new Statistic(name + " Orbit Distance", Statistic.ValueType.Float, orbitDistance);
        statistics[name + " Position"] = new Statistic(name + " Position", Statistic.ValueType.Vector2, position);
        statistics[name + " Profile"] = new Statistic(name + " Profile", Statistic.ValueType.Integer, Random.Range(0, 1000000));
        statistics[name + " Surface"] = new Statistic(name + " Surface", Statistic.ValueType.Integer, Random.Range(0, 1000000));
        statistics[name + " Clouds"] = new Statistic(name + " Clouds", Statistic.ValueType.Integer, Random.Range(0, 1000000));
        statistics[name + " Atmosphere"] = new Statistic(name + " Atmosphere", Statistic.ValueType.Integer, Random.Range(0, 1000000));
        statistics[name + " Atmosphere Intensity"] = new Statistic(name + " Atmosphere Intensity", Statistic.ValueType.Float, intensity);
        statistics[name + " Atmosphere Density"] = new Statistic(name + " Atmosphere Density", Statistic.ValueType.Float, density);
    }

    void UpdatePlanet(string name, float radius, float kelvin, float kelvinLow, float kelvinHigh)
    {
        float intensity = AtmosphereIntensity(kelvin);
        float density = AtmosphereDensity(kelvin);
        float actualKelvinRange = AtmosphericKelvinRange(kelvinHigh - kelvinLow, intensity, density);
        float actualKelvin = AtmosphereKelvin(kelvin, actualKelvinRange, intensity, density);
        float actualKelvinLow = actualKelvin - actualKelvinRange;
        float actualKelvinHigh = actualKelvin + actualKelvinRange;

        statistics[name + " Radius"].Set(radius);
        statistics[name + " Actual Kelvin"].Set(actualKelvin);
        statistics[name + " Actual Kelvin Low"].Set(actualKelvinLow);
        statistics[name + " Actual Kelvin High"].Set(actualKelvinHigh);
        statistics[name + " Atmosphere Intensity"].Set(intensity);
        statistics[name + " Atmosphere Density"].Set(density);
    }
}
