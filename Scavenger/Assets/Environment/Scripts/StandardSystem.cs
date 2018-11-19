using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardSystem : MonoBehaviour, ISystemGeneratorDecorator
{
    public EnvironmentBasedStar starPrefab;
    public EnvironmentBasedPlanet planetPrefab;
    public LineRendererCircle orbitPrefab;

    public bool starsDisplayOrbits = true;
    public bool planetsDisplayOrbits = true;
    public bool moonsDisplayOrbits = true;
    public float starDistance = 5000;
    public AnimationCurve starPlotStars;
    public AnimationCurve starPlotRadius;
    public AnimationCurve starPlotKelvin;

    [Header("Planets")]
    public float pDistance = 1000;
    public float pDStellarTempFraction = 20;
    public float pDStellarTempExponent = 2;
    public AnimationCurve planetPlotPlanets;
    public AnimationCurve planetPlotRadiusDistance;
    public AnimationCurve planetPlotRadiusKelvin;
    public AnimationCurve planetPlotMoons;

    public Statistics statistics;

    public void Start()
    {
        if (!statistics)
            statistics = GetComponent<Statistics>();
    }

    public void System()
    {
        List<string> dungeonTargets = new List<string>();
        statistics["Dungeon Targets"] = new Statistic("Dungeon Targets", Statistic.ValueType.Object, dungeonTargets);
        Vector3Int coordinates = (statistics["System Coordinates"].Get<Vector3>() + new Vector3(SystemsGenerator.active.map.width / 2f - 1, SystemsGenerator.active.map.height / 2f - 1, 0)).Int();
        Debug.Log(coordinates);

        if (SystemsGenerator.active.map.GetPixel(coordinates.x, coordinates.y).Vector3().magnitude > 0)
        {
            float stars = UnityEngine.Random.Range(0f, 1f);
            int numberOfStars = Mathf.RoundToInt(starPlotStars.Evaluate(stars));
            statistics["Stars"] = new Statistic("Stars", Statistic.ValueType.Integer, numberOfStars);
            //Statistic dungeonables = statistics["Dungeonables"] = new Statistic("Dungeonables", Statistic.ValueType.Integer, numberOfStars);

            float planets = UnityEngine.Random.Range(0, 1f) / numberOfStars;
            int numberOfPlanets = Mathf.RoundToInt(planetPlotPlanets.Evaluate(planets));
            statistics["Planets"] = new Statistic("Planets", Statistic.ValueType.Integer, numberOfPlanets);
            //dungeonables.Set(dungeonables.Get<int>() + numberOfPlanets);

            for (int i = 0; i < numberOfPlanets; i++)
            {
                string planetName = "Planet " + StringHelper.IndexIntToChar(i);
                int numberOfMoons = (int)planetPlotMoons.Evaluate(UnityEngine.Random.Range(0, 1f));
                statistics[planetName + " Moons"] = new Statistic(planetName + " Moons", Statistic.ValueType.Integer, numberOfMoons);
                //dungeonables.Set(dungeonables.Get<int>() + numberOfMoons);
            }

            int dungeons = 0;
            for (int i = 0; i < numberOfStars; i++)
            {
                if (UnityEngine.Random.Range(1f, numberOfStars) <= 2 && dungeons < Environment.maximumDungeons)
                {
                    dungeonTargets.Add("Star " + StringHelper.IndexIntToChar(i));
                    dungeons++;
                }
            }

            for (int i = 0; i < numberOfPlanets; i++)
            {
                string planetName = "Planet " + StringHelper.IndexIntToChar(i);
                if ((UnityEngine.Random.Range(1f, numberOfPlanets) <= 2 || dungeons <= 3) && dungeons < Environment.maximumDungeons)
                {
                    dungeonTargets.Add(planetName);
                    dungeons++;
                }

                int numberOfMoons = statistics[planetName + " Moons"];
                for (int j = 0; j < numberOfMoons; j++)
                {
                    if ((UnityEngine.Random.Range(1f, 10) == 1 || dungeons < 5) && dungeons < Environment.maximumDungeons)
                    {
                        dungeonTargets.Add(planetName + " Moon " + StringHelper.IndexIntToChar(i));
                        dungeons++;
                    }
                }
            }

            dungeonTargets.Shuffle();
        }
        else
        {
            statistics["Stars"] = new Statistic("Stars", Statistic.ValueType.Integer, 0);
            statistics["Planets"] = new Statistic("Planets", Statistic.ValueType.Integer, 0);
        }
    }

    public void Stars()
    {
        // Stellar Temperature tracks the average temperature of the stellar core (used for approximations of system temperature)
        float stellarTemperature = 0;
        float stellarRadius = 0;

        int numberOfStars = statistics["Stars"];

        // Each star generated has a position that must be offset from the center of mass, this stores those positions
        List<Vector2> starPositions = new List<Vector2>();
        float[] contributions = new float[numberOfStars];
        float starPositionContributions = 0;
        Vector2 centerOfMass = Vector2.zero;
        float greatestDistanceBetweenStars = 0;
        float[] orbitDistances = new float[numberOfStars];
        for (int i = 0; i < numberOfStars; i++)
        {
            string name = "Star " + StringHelper.IndexIntToChar(i);
            float radius = UnityEngine.Random.Range(0f, 1f);
            stellarRadius += radius;

            float radiusSkewed = starPlotRadius.Evaluate(radius) * EnvironmentRules.RadiusOfSun / 10000f;
            statistics[name + " Radius"] = new Statistic(name + " Radius", Statistic.ValueType.Float, radiusSkewed);

            float kelvin = UnityEngine.Random.Range(0, 1f);
            stellarTemperature += kelvin;

            float kelvinSkewed = starPlotKelvin.Evaluate(kelvin);
            statistics[name + " Kelvin"] = new Statistic(name + " Kelvin", Statistic.ValueType.Float, kelvinSkewed);

            float kelvinRange = (UnityEngine.Random.Range(0, kelvinSkewed / 2f) + UnityEngine.Random.Range(0, kelvinSkewed / 2f) + UnityEngine.Random.Range(0, kelvinSkewed / 2f)) / 3;
            statistics[name + " Kelvin Range"] = new Statistic(name + " Kelvin Range", Statistic.ValueType.Float, kelvinRange);

            starPositions.Add(UnityEngine.Random.insideUnitCircle * starDistance);

            contributions[i] = Mathf.Pow(2, (radius + 1) * 5f) + Mathf.Pow((radius + 1f) * 2f, (kelvin + 1f) * 10f);// (radius * 10) + (kelvin * 10);
            centerOfMass += starPositions[i] * contributions[i];
            starPositionContributions += contributions[i];
            statistics[name + " Contribution"] = new Statistic(name + " Contribution", Statistic.ValueType.Float, contributions[i]);

            orbitDistances[i] = radiusSkewed + statistics["Star " + StringHelper.IndexIntToChar(i) + " Kelvin"] / pDStellarTempFraction + pDistance * Mathf.Pow(statistics["Star " + StringHelper.IndexIntToChar(i) + " Kelvin"] / 30000f, pDStellarTempExponent);
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
        statistics["Radius"] = new Statistic("Radius", Statistic.ValueType.Float, stellarRadius);
    }

    public void PopulateStars()
    {
        int numberOfStars = statistics["Stars"];
        for (int i = 0; i < numberOfStars; i++)
        {
            string name = "Star " + StringHelper.IndexIntToChar(i);
            EnvironmentBasedStar star = Instantiate(starPrefab, statistics[name + " Position"].Get<Vector2>(), starPrefab.transform.rotation, transform);
            star.transform.localPosition = statistics[name + " Position"].Get<Vector2>();
            star.name = name;
            star.environment = statistics;
            statistics[name + " GO"] = new Statistic(name + " GO", Statistic.ValueType.GameObject, star.gameObject);

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
    }

    public void Planets()
    {
        int numberOfStars = statistics["Stars"];
        int numberOfPlanets = statistics["Planets"];
        float stellarTemperature = statistics["Temperature"];
        float stellarRadius = statistics["Radius"];

        float[] orbitDistances = new float[numberOfStars];
        Vector2[] starPositions = new Vector2[numberOfStars];
        float[] contributions = new float[numberOfStars];

        for (int i = 0; i < numberOfStars; i++)
        {
            string starName = "Star " + StringHelper.IndexIntToChar(i);
            float radiusSkewed = statistics[starName + " Radius"];
            orbitDistances[i] = radiusSkewed + statistics[starName + " Kelvin"] / pDStellarTempFraction + pDistance * Mathf.Pow(statistics[starName + " Kelvin"] / 30000f, pDStellarTempExponent);
            starPositions[i] = statistics[starName + " Position"];
            contributions[i] = statistics[starName + " Contribution"];
        }

        // Get the greatest distance between stars
        float greatestDistanceBetweenStars = 0;
        Vector2 centerOfMass = statistics["System Center"];
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

        float maximumPlanetaryDistance = 0f;
        for (int i = 0; i < numberOfPlanets; i++)
        {
            string name = "Planet " + StringHelper.IndexIntToChar(i);

            // Stars can generate quite far apart
            // Planets can generate around 1 or more stars
            // Pick a star at random
            // Pick an orbit position around that star based on its temperature
            // Then determine the center of mass that position would orbit around
            // Then orbit

            // Pick a star
            int star = UnityEngine.Random.Range(0, numberOfStars);
            // Define a distance from it
            float orbitDistance = orbitDistances[star] * UnityEngine.Random.Range(1f, 2f);
            orbitDistances[star] = orbitDistance;
            // Make a random point at that distance around a circle's circumference
            Vector2 position = starPositions[star] + UnityEngine.Random.insideUnitCircle.normalized * orbitDistance;

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
                string starName = "Star " + StringHelper.IndexIntToChar(j);
                float starKelvin = statistics[starName + " Kelvin"];
                float starRadius = EnvironmentRules.StellarRadius(statistics[starName + " Radius"]);
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
            }
            else
            {
                orbitCenter = starPositions[star];
            }

            bool failed = false;
            for (int j = 0; j < numberOfStars; j++)
            {
                float orbitOverlap = Mathf.Abs((starPositions[j] - orbitCenter).magnitude - orbitDistance);
                string starName = "Star " + StringHelper.IndexIntToChar(j);
                float minimumDistance = statistics[starName + " Kelvin"] / pDStellarTempFraction + pDistance * Mathf.Pow(statistics[starName + " Kelvin"] / 30000f, pDStellarTempExponent);
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

            statistics[name + " Star"] = new Statistic(name + " Star", Statistic.ValueType.Integer, star);

            GeneratePlanet(name, 0, kelvin, kelvinLow, kelvinHigh, orbitCenter, distance, delta);
        }

        for (int i = 0; i < numberOfPlanets; i++)
        {
            string name = "Planet " + StringHelper.IndexIntToChar(i);

            float kelvin = statistics[name + " Kelvin"].Get<float>();
            float kelvinLow = statistics[name + " Kelvin Low"].Get<float>();
            float kelvinHigh = statistics[name + " Kelvin High"].Get<float>();
            float distance = statistics[name + " Orbit Distance"].Get<float>();

            float distanceRatio = distance / maximumPlanetaryDistance * UnityEngine.Random.Range(0.8f, 1.2f);
            float radius = planetPlotRadiusDistance.Evaluate(distanceRatio);
            radius += planetPlotRadiusKelvin.Evaluate(kelvin / 240f);
            radius *= EnvironmentRules.RadiusOfJupiter / 10000f;

            UpdatePlanet(name, radius, kelvin, kelvinLow, kelvinHigh);
        }
    }

    public void PopulatePlanets()
    {
        int numberOfPlanets = statistics["Planets"];
        for (int i = 0; i < numberOfPlanets; i++)
        {
            string name = "Planet " + StringHelper.IndexIntToChar(i);

            Vector2 orbitCenter = statistics[name + " Orbit"];
            EnvironmentBasedPlanet planet = Instantiate(planetPrefab, statistics[name + " Position"].Get<Vector2>(), planetPrefab.transform.rotation, transform);
            planet.transform.localPosition = statistics[name + " Position"].Get<Vector2>();
            planet.name = name;
            planet.environment = statistics;
            statistics[name + " GO"] = new Statistic(name + " GO", Statistic.ValueType.GameObject, planet.gameObject);

            // ORBIT ---------------------------------------------------------------------------------------------------------------------------------------------------------------------
            if (planetsDisplayOrbits)
            {
                LineRendererCircle orbit = Instantiate(orbitPrefab, orbitCenter, new Quaternion(), transform);//Quaternion.Euler(0, 0, atan * Mathf.Rad2Deg), transform);
                orbit.transform.localPosition = orbitCenter;
                Vector2 delta = planet.transform.position.XY() - orbit.transform.localPosition.XY();
                orbit.radius = (delta).magnitude;
                orbit.line.startWidth = orbit.line.endWidth = statistics[name + " Radius"].Get<float>();
            }
        }
    }

    public void Moons()
    {
        int numberOfStars = statistics["Stars"];
        int numberOfPlanets = statistics["Planets"];
        for (int i = 0; i < numberOfPlanets; i++)
        {
            string planetName = "Planet " + StringHelper.IndexIntToChar(i);
            float radius = statistics[planetName + " Radius"];
            int numberOfMoons = statistics[planetName + " Moons"];
            float distanceLast = radius * 1.5f * UnityEngine.Random.Range(1f, 3f);
            for (int j = 0; j < numberOfMoons; j++)
            {
                string moonName = planetName + " Moon " + StringHelper.IndexIntToChar(j);
                float moonRadius = Mathf.Max(0.1f, ((radius * 0.3f) / (numberOfMoons + 1)) * UnityEngine.Random.Range(0.1f, 1f));
                Vector2 orbit = UnityEngine.Random.insideUnitCircle.normalized;
                float orbitDistance = distanceLast;
                distanceLast = radius * 1.5f * UnityEngine.Random.Range(1f, 3f);

                Vector2 delta = statistics[planetName + " Position"].Get<Vector2>() + orbit * orbitDistance;

                // I need to find the hottest, and coldest point around the orbit
                float kelvin = 0;
                for (int l = 0; l < numberOfStars; l++)
                {
                    string starName = "Star " + StringHelper.IndexIntToChar(l);
                    float starRadius = EnvironmentRules.StellarRadius(statistics[starName + " Radius"].Get<float>());
                    float starTemperature = statistics[starName + " Kelvin"].Get<float>();
                    float stellarDistance = EnvironmentRules.PlanetDistance((statistics[starName + " Position"].Get<Vector2>() - delta).magnitude);
                    kelvin += EnvironmentRules.PlanetTemperature(starRadius, EnvironmentRules.StellarLuminosity(starRadius, starTemperature), stellarDistance, 0.3f);
                }
                // I need to find the hottest, and coldest point around the orbit
                float kelvinLow = 30000;
                float kelvinHigh = 0;
                float distance = delta.magnitude;
                float sampleDistance = delta.magnitude;
                Vector2 orbitCenter = statistics[planetName + " Orbit"].Get<Vector2>();
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
                UpdatePlanet(moonName, moonRadius, kelvin, kelvinLow, kelvinHigh);
            }
        }
    }

    public void PopulateMoons()
    {
        int numberOfPlanets = statistics["Planets"];
        for (int i = 0; i < numberOfPlanets; i++)
        {
            string planetName = "Planet " + StringHelper.IndexIntToChar(i);
            GameObject planet = statistics[planetName + " GO"];
            // MOONS ---------------------------------------------------------------------------------------------------------------------------------------------------------------------
            int numberOfMoons = statistics[planetName + " Moons"];
            for (int j = 0; j < numberOfMoons; j++)
            {
                string moonName = planetName + " Moon " + StringHelper.IndexIntToChar(j);

                Vector2 orbitCenter = statistics[planetName + " Position"];
                Vector2 orbitPoint = statistics[moonName + " Orbit"];
                float distance = statistics[moonName + " Orbit Distance"];

                EnvironmentBasedPlanet moon = Instantiate(planetPrefab, orbitCenter + orbitPoint * distance, planetPrefab.transform.rotation, transform);
                moon.transform.localPosition = orbitCenter + orbitPoint * distance;
                moon.isMoon = true;
                moon.name = moonName;
                moon.environment = statistics;
                statistics[moonName + " GO"] = new Statistic(moonName + " GO", Statistic.ValueType.GameObject, moon.gameObject);
                statistics[moonName + " Planet GO"] = new Statistic(moonName + " Planet GO", Statistic.ValueType.GameObject, planet.gameObject);

                // ORBIT ---------------------------------------------------------------------------------------------------------------------------------------------------------------------
                if (moonsDisplayOrbits)
                {
                    float atan = Mathf.Atan2(orbitPoint.y, orbitPoint.x);
                    LineRendererCircle moonOrbit = Instantiate(orbitPrefab, orbitCenter, Quaternion.Euler(0, 0, atan * Mathf.Rad2Deg), transform);
                    moonOrbit.transform.localPosition = orbitCenter;
                    moonOrbit.radius = distance;
                    moonOrbit.line.startWidth = moonOrbit.line.endWidth = statistics[moonName + " Radius"].Get<float>();
                }
            }
        }
    }

    void GeneratePlanet(string name, float radius, float kelvin, float kelvinLow, float kelvinHigh, Vector2 orbit, float orbitDistance, Vector2 position)
    {
        float intensity = EnvironmentRules.AtmosphereIntensity(radius, kelvin);
        float density = EnvironmentRules.AtmosphereDensity(radius, kelvin);
        float actualKelvinRange = EnvironmentRules.AtmosphericKelvinRange(kelvinHigh - kelvinLow, intensity, density);
        float actualKelvin = EnvironmentRules.AtmosphereKelvin(kelvin, actualKelvinRange, intensity, density);
        float actualKelvinLow = actualKelvin - actualKelvinRange;
        float actualKelvinHigh = actualKelvin + actualKelvinRange;

        statistics[name + " Radius"] = new Statistic(name + " Radius", Statistic.ValueType.Float, radius);
        statistics[name + " Kelvin"] = new Statistic(name + " Kelvin", Statistic.ValueType.Float, kelvin);
        statistics[name + " Kelvin Low"] = new Statistic(name + " Kelvin Low", Statistic.ValueType.Float, kelvinLow);
        statistics[name + " Kelvin High"] = new Statistic(name + " Kelvin High", Statistic.ValueType.Float, kelvinHigh);
        statistics[name + " Actual Kelvin"] = new Statistic(name + " Actual Kelvin", Statistic.ValueType.Float, actualKelvin);
        statistics[name + " Actual Kelvin Low"] = new Statistic(name + " Actual Kelvin Low", Statistic.ValueType.Float, actualKelvinLow);
        statistics[name + " Actual Kelvin High"] = new Statistic(name + " Actual Kelvin High", Statistic.ValueType.Float, actualKelvinHigh);
        statistics[name + " Water Level"] = new Statistic(name + " Water Level", Statistic.ValueType.Float, 0f);
        statistics[name + " Orbit"] = new Statistic(name + " Orbit", Statistic.ValueType.Vector2, orbit);
        statistics[name + " Orbit Distance"] = new Statistic(name + " Orbit Distance", Statistic.ValueType.Float, orbitDistance);
        statistics[name + " Position"] = new Statistic(name + " Position", Statistic.ValueType.Vector2, position);
        statistics[name + " Profile"] = new Statistic(name + " Profile", Statistic.ValueType.Integer, UnityEngine.Random.Range(0, 1000000));
        statistics[name + " Surface"] = new Statistic(name + " Surface", Statistic.ValueType.Integer, UnityEngine.Random.Range(0, 1000000));
        statistics[name + " Atmosphere"] = new Statistic(name + " Atmosphere", Statistic.ValueType.Integer, UnityEngine.Random.Range(0, 1000000));
        statistics[name + " Atmosphere Intensity"] = new Statistic(name + " Atmosphere Intensity", Statistic.ValueType.Float, intensity);
        statistics[name + " Atmosphere Density"] = new Statistic(name + " Atmosphere Density", Statistic.ValueType.Float, density);
    }

    public void UpdatePlanet(string name, float radius, float kelvin, float kelvinLow, float kelvinHigh)
    {
        float intensity = EnvironmentRules.AtmosphereIntensity(radius, kelvin);
        float density = EnvironmentRules.AtmosphereDensity(radius, kelvin);
        float actualKelvinRange = EnvironmentRules.AtmosphericKelvinRange(kelvinHigh - kelvinLow, intensity, density);
        float actualKelvin = EnvironmentRules.AtmosphereKelvin(kelvin, actualKelvinRange, intensity, density);
        float actualKelvinLow = actualKelvin - actualKelvinRange;
        float actualKelvinHigh = actualKelvin + actualKelvinRange;
        float waterLevel = Mathf.Clamp01((radius / (EnvironmentRules.RadiusOfEarth / 5000f)) * UnityEngine.Random.Range(0.5f, 2f));
        waterLevel = Mathf.Clamp01(waterLevel + ((kelvin / 85f) * (1f - kelvin / 240f)) * UnityEngine.Random.Range(0.5f, 2f));

        statistics[name + " Radius"].Set(radius);
        statistics[name + " Actual Kelvin"].Set(actualKelvin);
        statistics[name + " Actual Kelvin Low"].Set(actualKelvinLow);
        statistics[name + " Actual Kelvin High"].Set(actualKelvinHigh);
        statistics[name + " Atmosphere Intensity"].Set(intensity);
        statistics[name + " Atmosphere Density"].Set(density);
        statistics[name + " Water Level"].Set(waterLevel);
    }

    public void Dungeons()
    {
        List<DungeonType> dungeons = new List<DungeonType>();

        statistics["Dungeons"] = new Statistic("Dungeons", Statistic.ValueType.Object, dungeons);

        foreach (string target in DungeonType.targets)
        {
            for (int i = 0; i < ((target == "Any") ? 5 : 1); i++)
            {
                DungeonType dungeonType = DungeonType.SelectByChance(DungeonLoader.dungeons.signals.FindAll(dungeon => dungeon.target.Contains(target)));
                dungeonType.tags = StringHelper.TagParse(dungeonType.tags);
                dungeons.Add(dungeonType);
            }
        }

        dungeons.Shuffle();
    }

    public void PopulateDungeons()
    {
        List<DungeonGenerator> dungeonables = new List<DungeonGenerator>();
        dungeonables.AddRange(GameObject.FindObjectsOfType<DungeonGenerator>());
        List<string> dungeonTargets = statistics["Dungeon Targets"].Get<object>() as List<string>;
        List<DungeonType> activeDungeons = new List<DungeonType>();
        statistics["Active Dungeons"] = new Statistic("Active Dungeons", Statistic.ValueType.Object, activeDungeons);
        int numberOfDungeons = dungeonTargets.Count;
        List<DungeonType> eligibleDungeonTypes = statistics["Dungeons"].Get<object>() as List<DungeonType>;

        for (int i = 0; i < numberOfDungeons; i++)
        {
            DungeonGenerator dungeon = dungeonables.Find(dng => dng.name == dungeonTargets[i]);

            if (!dungeon)
                continue;

            dungeon.hash = UnityEngine.Random.Range(int.MinValue, int.MaxValue);

            dungeon.dungeonType = eligibleDungeonTypes.Find(dng => dng.target.Contains("Any") || Contains(dng.target, dungeon.dungeonTarget));
            //if (dungeon.dungeonType.name == null)
            //    dungeon.dungeonType = eligibleDungeonTypes.Find(dng => dng.target.Contains("Any"));
            eligibleDungeonTypes.Remove(dungeon.dungeonType);
            dungeon.riskLevel = FloatHelper.RiskStringToFloat(dungeon.dungeonType.risk) * UnityEngine.Random.Range(0.5f, 2f);
            dungeon.generates = true;

            activeDungeons.Add(dungeon.dungeonType);
        }
    }

    bool Contains(string target, string targets)
    {
        int contains = 0;
        string[] split = targets.Split(new string[] { " ", ", ", "," }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string str in split)
        {
            if (target.Contains(str))
            {
                contains++;
            }
        }

        return contains == split.Length;
    }
}
