using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemGenerator : MonoBehaviour
{
    public EnvironmentBasedStar starPrefab;
    public EnvironmentBasedPlanet planetPrefab;
    public LineRendererCircle orbitPrefab;

    public bool generateRandom = false;
    public float generateTiming = 1.0f;
    public int stopStarCount = 5;
    public int stopPlanetCount = 0;
    public float stopTiming = 2.0f;

    public bool starsDisplayOrbits = false;

    public AnimationCurve starPlotStars;
    public AnimationCurve starPlotRadius;
    //public AnimationCurve starPlotMass;
    public AnimationCurve starPlotKelvin;

    public AnimationCurve planetPlotRadius;

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
                StartCoroutine(GenerateTheatricRandom());
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
            if ((statistics["Stars"] < stopStarCount || stopStarCount == 0) && (statistics["Planets"] < stopPlanetCount || stopPlanetCount == 0))
            {
                Clear();

                yield return new WaitForEndOfFrame();

                statistics["System Coordinates"].Set(Random.insideUnitCircle * 1000000);

                Generate();

                yield return new WaitForSeconds(generateTiming);
            }
            else
            {
                yield return new WaitForSeconds(stopTiming);

                Clear();

                yield return new WaitForEndOfFrame();

                statistics["System Coordinates"].Set(Random.insideUnitCircle * 1000000);

                Generate();

                yield return new WaitForSeconds(generateTiming);
            }
        }
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
        hash = Hash(statistics["System Coordinates"].Get<Vector2>());

        Random.InitState(hash);

        // The hotter the 'stellar core' is the farther away the protoplanetary disc forms
        // Such that very hot stellar cores push the protoplanetary disc so far away it doesn't form at all
        // The more massive the 'stellar core' is the closer the protoplanetary disc forms
        // Such that very massive stellar cores consume the entire protoplanetary disc

        // The protoplanetary disc itself is thinest at both edges, with increased density at some function of the middle
        // Such that the region with the highest density is balanced between stellar temperature and stellar mass

        // Each planetary formation disrupts the possibility of nearby planets forming out of the protoplanetary disc
        // If planets form far enough apart that this disruption is too great for a planet to form between the 2, this increases the likelihood of an asteroid belt
        // Gasseous belts should not form, as gasses rapidly get blown away by stellar heat
        // Such that low heat stars could have a gasseous belt

        // So with all these complex factors, time to construct a model that works acceptably to create a playable game
        // This means I have massive room to fudge the details (so it seems from how inaccurate other space games are)

        // How to start:
        // First, stars are primarily within a band of 1 stellar radius
        // With very low likelihood of stars forming very much smaller than that (though the 1 stellar radius range is 0.1 to 10)
        // And with a slightly higher likelihood of forming much larger (10 to 1000 stellar radii)
        // As well stars often form in binary pairs
        // Stellar cores can form with higher that 2 stars, but the third is often very far away
        // Systems with higher stars are often unstable and will stabilize to a smaller number (1 - 3)
        // And finally stars have a much higher likelihood of being an M class (~83%-86%)

        // So:
        // 1.   Most stars should be radius 1 with a low of 0.01 and high of 1000 (this is a heavily skewed probability scheme)
        // 2.   Most stars should be 3000 Kelvin with a low of 0 and a high of 30000 (again heavily skewed probability scheme)
        // 3.   Most stellar cores should have 2 stars with a low of 1 and a high of n (again heavily skewed probability scheme)

        // Base Gasseousness and Metallicity
        // Gasseousness has a highest value of 3, and a probable value of 1.5
        float gasseousness = Random.Range(0, 1f) + Random.Range(0, 1f) + Random.Range(0, 1f);
        // Metallicity has a highest value of 2, and a probable value of 1
        float metallicity = Random.Range(0, 1f) + Random.Range(0, 1f);
        // Stellar Temperature tracks the average temperature of the stellar core (used for approximations of system temperature)
        float stellarTemperature = 0;

        // Update the environment statistics
        statistics["Gasseousness"] = new Statistic("Gasseousness", Statistic.ValueType.Float, gasseousness);
        statistics["Metallicity"] = new Statistic("Metallicity", Statistic.ValueType.Float, metallicity);

        // Stars generates with a heavy weight towards 0.5, this 0-1 value is used in an animation curve to generate the system with correct skewing
        float stars = (Random.Range(0f, 1f) + Random.Range(0f, 1f) + Random.Range(0f, 1f)) / 3f;// * (gasseousness / 3f);
        int numberOfStars = Mathf.RoundToInt(starPlotStars.Evaluate(stars));
        statistics["Stars"] = new Statistic("Stars", Statistic.ValueType.Integer, numberOfStars);

        // Each star generated has a position that must be offset from the center of mass, this stores those positions
        List<Vector2> starPositions = new List<Vector2>();
        float starPositionContributions = 0;
        Vector2 centerOfMass = Vector2.zero;

        for (int i = 0; i < numberOfStars; i++)
        {
            string name = "Star " + StringHelper.IndexIntToChar(i);
            // Radius is highly likely to be around 0.5, with 
            float radius = ((Random.Range(0f, 1f) + Random.Range(0f, 1f) + Random.Range(0f, 1f)) / 3f) * (gasseousness /3f);

            gasseousness += radius / 2;
            metallicity += radius / 5;

            float radiusSkewed = starPlotRadius.Evaluate(radius);
            statistics[name + " Radius"] = new Statistic(name + " Radius", Statistic.ValueType.Float, radiusSkewed);

            //float mass = (Random.Range(0f, 1f) + Random.Range(0f, 1f) + Random.Range(0f, 1f)) / 3f;
            //float massSkewed = starPlotMass.Evaluate(mass);
            //statistics[name + " Mass"] = new Statistic(name + " Mass", Statistic.ValueType.Float, massSkewed);

            //float kelvin = radius * mass * (1 - metallicity);
            float kelvin = ((Random.Range(0f, 1f) + Random.Range(0f, 1f) + Random.Range(0f, 1f)) / 3f) * (2f - metallicity) * (gasseousness / 3f) + radius / 4f;
            stellarTemperature += kelvin;

            gasseousness -= kelvin * 2;
            metallicity -= kelvin / 5;

            float kelvinSkewed = starPlotKelvin.Evaluate(kelvin);
            statistics[name + " Kelvin"] = new Statistic(name + " Kelvin", Statistic.ValueType.Float, kelvinSkewed);

            float kelvinRange = (Random.Range(0, kelvinSkewed) + Random.Range(0, kelvinSkewed) + Random.Range(0, kelvinSkewed)) / 3;
            statistics[name + " Kelvin Range"] = new Statistic(name + " Kelvin Range", Statistic.ValueType.Float, kelvinRange);

            starPositions.Add(Random.insideUnitCircle * 500f * (1f - radius) * (1f - kelvin));// * Mathf.Log(radius * 10000, 5) * Mathf.Log(kelvin, 5));

            centerOfMass += starPositions[i] * ((radius * 10) + (kelvin * 10));// * (radius / 10f) * (kelvin / 10f);
            starPositionContributions += (radius * 10) + (kelvin * 10);

            gasseousness = Mathf.Clamp(gasseousness, 0, 3);
            metallicity = Mathf.Clamp(metallicity, 0, 2);
        }

        centerOfMass /= starPositionContributions;

        for (int i = 0; i < numberOfStars; i++)
        {
            string name = "Star " + StringHelper.IndexIntToChar(i);
            starPositions[i] = starPositions[i] - centerOfMass;
            EnvironmentBasedStar star = Instantiate(starPrefab, starPositions[i], starPrefab.transform.rotation, transform);
            star.name = name;
            star.environment = statistics;

            if (starsDisplayOrbits && numberOfStars > 1)
            {
                LineRendererCircle orbit = Instantiate(orbitPrefab, Vector3.zero, orbitPrefab.transform.rotation, transform);
                orbit.radius = star.transform.position.magnitude;
                LineRenderer render = orbit.GetComponent<LineRenderer>();
                render.startColor = render.endColor = ((star.starMaterials[0].GetTexture("_Emissive") as Texture2D).GetPixelBilinear((statistics[name + " Kelvin"].Get<float>() + statistics[name + " Kelvin Range"].Get<float>()) / star.starMaterials[0].GetFloat("_KelvinMax"), 0) * 8).Normalize();
            }
        }
        stellarTemperature /= numberOfStars;
        statistics["Temperature"] = new Statistic("Temperature", Statistic.ValueType.Float, stellarTemperature);

        int numberOfPlanets = Mathf.RoundToInt(((Random.Range(0, 20) + Random.Range(0, 20) + Random.Range(0, 20)) / 3) * (1f - stellarTemperature));//(int)((Random.Range(0, 20) + Random.Range(0, 20) + Random.Range(0, 20)) / 3 * ((6 - numberOfStars) / 6f) * Mathf.Max(0, ((kelvinTotal - kelvinSum) / kelvinTotal)));
        statistics["Planets"] = new Statistic("Planets", Statistic.ValueType.Integer, numberOfPlanets);

        statistics["Planetary Gasseousness"] = new Statistic("Planetary Gasseousness", Statistic.ValueType.Float, gasseousness);
        statistics["Planetary Metallicity"] = new Statistic("Planetary Metallicity", Statistic.ValueType.Float, metallicity * numberOfPlanets);

        for (int i = 0; i < numberOfPlanets; i++)
        {
            string name = "Planet " + StringHelper.IndexIntToChar(i);

            float maxDistance = 0;
            for (int j = 0; j < starPositions.Count; j++)
            {
                for (int k = j; k < starPositions.Count; k++)
                {
                    float distanceFromStar = (starPositions[j] - starPositions[k]).magnitude;
                    if (distanceFromStar > maxDistance)
                        maxDistance = distanceFromStar;
                }
            }

            float orbitDistance = ((Random.Range(0, 100f) + Random.Range(0, 100f)) / 2 * i) + maxDistance + 100f * stellarTemperature;
            Vector2 position = Random.insideUnitCircle.normalized * orbitDistance;

            int star = 0;
            float distance = 10000f;
            float[] distanceWeights = new float[numberOfStars];
            for (int j = 0; j < numberOfStars; j++)
            {
                float stellarDistance = (starPositions[j] - position).magnitude;
                distanceWeights[j] = stellarDistance;
                if (stellarDistance < distance)
                {
                    star = j;
                    distance = stellarDistance;
                }
            }

            Vector2 orbitCenter = Vector2.zero;
            for (int j = 0; j < numberOfStars; j++)
            {
                distanceWeights[j] /= distance;
                orbitCenter += starPositions[j] * distanceWeights[j];
            }

            orbitDistance = ((Random.Range(0, 100f) + Random.Range(0, 100f)) / 2 * i) + maxDistance + 100f * statistics["Star " + StringHelper.IndexIntToChar(star) + " Kelvin"] / 30000f;
            orbitDistance += (position.normalized * orbitDistance - orbitCenter).magnitude;
            position = orbitCenter + position.normalized * orbitDistance;

            EnvironmentBasedPlanet planet = Instantiate(planetPrefab, position, planetPrefab.transform.rotation, transform);
            planet.name = name;
            planet.environment = statistics;

            LineRendererCircle orbit = Instantiate(orbitPrefab, orbitCenter, orbitPrefab.transform.rotation, transform);
            //LineRendererCircle orbit = Instantiate(orbitPrefab, Vector3.zero, orbitPrefab.transform.rotation, transform);
            orbit.radius = orbitDistance;
            LineRenderer render = orbit.GetComponent<LineRenderer>();
            render.startColor = render.endColor = Color.green * 0.5f;

            float planetMetallicity = metallicity * (stellarTemperature * (1 - (maxDistance / orbitDistance)));
            metallicity -= planetMetallicity / numberOfPlanets;
            statistics[name + " Metallicity"] = new Statistic(name + " Metallicity", Statistic.ValueType.Float, planetMetallicity);

            float planetGasseousness = gasseousness * (stellarTemperature * (1 - (maxDistance / orbitDistance)));
            statistics[name + " Gasseousness"] = new Statistic(name + " Gasseousness", Statistic.ValueType.Float, planetGasseousness);

            float radius = ((Random.Range(0f, 1f) + Random.Range(0f, 1f) + Random.Range(0f, 1f)) / 3) * Mathf.Max(planetMetallicity, planetGasseousness);
            float radiusSkewed = planetPlotRadius.Evaluate(radius);
            statistics[name + " Radius"] = new Statistic(name + " Radius", Statistic.ValueType.Float, radiusSkewed);
        }
    }

    public void Clear()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
