using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemGenerator : MonoBehaviour
{
    public int hash = 0;
    public bool loadFromEnvironment = true;

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

            if (loadFromEnvironment)
                statistics["System Coordinates"].Set(Environment.systemCoordinates);

            Generate();
        }
    }

    // Update is called once per frame
    void Update()
    {

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

        foreach (SystemGeneratorDecorator decorator in decorators)
        {
            bool happens = decorator.Happens();

            if (happens)
            {
                hash = Hash(statistics["System Coordinates"].Get<Vector3>());
                Random.InitState(hash);

                decorator.system.Invoke();
                decorator.star.Invoke();
                decorator.planet.Invoke();

                Random.InitState(EnvironmentTime.active.time);

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
