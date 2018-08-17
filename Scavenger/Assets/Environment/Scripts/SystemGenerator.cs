using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemGenerator : MonoBehaviour
{
    public bool generateRandom = false;
    public bool generateRandomLoop = false;
    public float generateTiming = 1.0f;
    public float stopTiming = 2.0f;
    public int stopForSingularities = -1;
    public int stopForStars = -1;
    public int stopForPlanets = -1;

    public int hash = 0;

    public SystemGeneratorDecorator[] decorators;

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
            int singularities = statistics["Singularities"];
            int stars = statistics["Stars"];
            int planets = statistics["Planets"];

            if ((stopForStars >= 0 && stopForStars <= stars) || (stopForPlanets >= 0 && stopForPlanets <= planets) || (stopForSingularities >= 0 && stopForSingularities <= singularities))
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

        hash = Hash(statistics["System Coordinates"].Get<Vector3>());
        Random.InitState(hash);
        
        foreach (SystemGeneratorDecorator decorator in decorators)
        {
            bool happens = decorator.Happens();

            if (happens)
            {
                decorator.system.Invoke();
                decorator.star.Invoke();
                decorator.planet.Invoke();
                decorator.dungeon.Invoke();
            }
        }

        stopwatch.Stop();
        Debug.Log("System Generation -----\nTime: " + stopwatch.ElapsedMilliseconds + "ms");
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
}
