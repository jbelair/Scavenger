using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemGenerator : MonoBehaviour
{
    public EnvironmentSetter setter;

    public int hash = 0;
    public bool generatesFromEnvironment = true;

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

            if (generatesFromEnvironment)
                statistics["System Coordinates"].Set(JsonUtility.FromJson<Vector3>(PlayerSave.Active.Get("system coordinates").value));

            StartCoroutine(Generate());
        }

        name = StringHelper.CoordinateName(statistics["System Coordinates"]);

        setter = GetComponent<EnvironmentSetter>();
    }

    public static int Hash(Vector3 c)
    {
        int h = (int)c.x * 374761393 + (int)c.y * 668265263 + (int)c.z * 1800560953; //all constants are prime
        h = (h ^ (h >> 13)) * 1274126177;
        h = h ^ (h >> 16);
        return h;
    }

    public IEnumerator Generate()
    {
        System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();

        if (setter)
            setter.Set();

        foreach (SystemGeneratorDecorator decorator in decorators)
        {
            bool happens = decorator.Happens();

            if (happens)
            {
                hash = Hash(statistics["System Coordinates"].Get<Vector3>());
                Random.InitState(hash);

                decorator.system.Invoke();
            }
        }

        foreach (SystemGeneratorDecorator decorator in decorators)
        {
            bool happens = decorator.Happens();

            if (happens)
            {
                if (Environment.generateStars)
                    decorator.stars.Invoke();

                //yield return new WaitForEndOfFrame();
            }
        }

        foreach (SystemGeneratorDecorator decorator in decorators)
        {
            bool happens = decorator.Happens();

            if (happens)
            {
                if (Environment.populateStars)
                {
                    decorator.populateStars.Invoke();

                    //yield return new WaitForSeconds(0.5f);
                }
            }
        }

        foreach (SystemGeneratorDecorator decorator in decorators)
        {
            bool happens = decorator.Happens();

            if (happens)
            {
                if (Environment.generatePlanets)
                    decorator.planets.Invoke();

                //yield return new WaitForEndOfFrame();
            }
        }

        foreach (SystemGeneratorDecorator decorator in decorators)
        {
            bool happens = decorator.Happens();

            if (happens)
            {
                if (Environment.populatePlanets)
                {
                    decorator.populatePlanets.Invoke();

                    //yield return new WaitForSeconds(0.5f);
                }
            }
        }

        foreach (SystemGeneratorDecorator decorator in decorators)
        {
            bool happens = decorator.Happens();

            if (happens)
            {
                if (Environment.generateMoons)
                    decorator.moons.Invoke();

                //yield return new WaitForEndOfFrame();
            }
        }

        foreach (SystemGeneratorDecorator decorator in decorators)
        {
            bool happens = decorator.Happens();

            if (happens)
            {
                if (Environment.populateMoons)
                {
                    decorator.populateMoons.Invoke();

                    //yield return new WaitForSeconds(0.5f);
                }
            }
        }

        foreach (SystemGeneratorDecorator decorator in decorators)
        {
            bool happens = decorator.Happens();

            if (happens)
            {
                //hash = Hash(statistics["System Coordinates"].Get<Vector3>() + new Vector3(0, 0, EnvironmentTime.active.time));
                Random.InitState(hash);
                if (Environment.generateDungeons)
                    decorator.dungeons.Invoke();
            }
        }

        foreach (SystemGeneratorDecorator decorator in decorators)
        {
            bool happens = decorator.Happens();

            if (happens)
            {
                if (Environment.populateDungeons)
                    decorator.populateDungeons.Invoke();
            }
        }

        stopwatch.Stop();
        Debug.Log("System Generation -----\nTime: " + stopwatch.ElapsedMilliseconds + "ms");

        yield return null;
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
