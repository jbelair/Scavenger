using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemsGenerator : MonoBehaviour
{
    public static SystemsGenerator active;

    public string mapPath;
    public Texture2D map;
    public Transform mapPlane;

    public Statistics systemDefault;
    public Statistics systemDisabled;
    public Statistics systemCenter;

    public float scale = 20000f;
    public Transform position;
    public float jumpFuel;
    public float jumpRange;
    public SpriteRenderer jumpRadius;
    public SpriteRenderer maxJumpRadius;
    public LineRenderer jumpLine;
    //public Vector2Int grid = new Vector2Int(5,3);

    public Dictionary<Vector3, Statistics> systems = new Dictionary<Vector3, Statistics>();

    public Vector3 lastPosition;

    public int environmentTime = Environment.environmentTime;

    public bool isGenerating = false;

    // Use this for initialization
    void Start()
    {
        Load();

        jumpRange = Environment.JumpRadius;
        lastPosition = position.position = Environment.SystemCoordinates.ToInt();
        mapPlane.position = -(position.position + new Vector3(0.5f, 0.5f, 0)) * scale;

        StartCoroutine(Generate());

        active = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!systems.ContainsKey(Environment.selectedCoordinates))
            return;

        jumpRange = Mathf.Min(Environment.JumpFuel, Environment.JumpRadius);

        if (lastPosition != position.position || jumpRange != Environment.JumpRadius || jumpFuel != Environment.JumpFuel)
        {
            StartCoroutine(Generate());
        }

        jumpRadius.transform.localScale = Vector3.one * jumpRange * scale;

        Vector3 pos = Environment.selectedCoordinates;

        if (Environment.JumpRadius != jumpRange)
            maxJumpRadius.transform.localScale = Vector3.one * Environment.JumpRadius * scale;
        else
            maxJumpRadius.transform.localScale = Vector3.zero;

        if (SystemsFilter.active.filter.Contains("Distance"))
        {
            float distance = Mathf.Round((pos - Environment.SystemCoordinates).magnitude * 10f) / 10f;

            int value = 0;

            if (!SystemsFilter.active.filter.Contains("Risk"))
                value = Mathf.FloorToInt(Mathf.Pow(10, (pos - Environment.SystemCoordinates).magnitude / jumpRange * 6f + 0.5f));
            else
                value = Mathf.FloorToInt((pos - Environment.SystemCoordinates).magnitude / jumpRange * 5f + 0.5f);

            if (distance <= jumpRange)
            {
                jumpLine.enabled = true;
                jumpLine.startColor = jumpRadius.color;

                if (SystemsFilter.active.filter.Contains("Rarity"))
                    jumpLine.endColor = Schemes.Scheme("Rarity " + StringHelper.RarityIntToString(value)).colour;
                else
                    jumpLine.endColor = Schemes.Scheme("Risk " + StringHelper.RiskIntToString(value)).colour;
            }
            else
            {
                jumpLine.enabled = false;
            }
        }
        else if (SystemsFilter.active.filter.Contains("Rarity") &&
            ((SystemsFilter.active.filter.Contains("Risk") && (SystemsFilter.active.filter.IndexOf("Rarity") < SystemsFilter.active.filter.IndexOf("Risk"))) || !SystemsFilter.active.filter.Contains("Risk")))
        {
            if (!systems.ContainsKey(Environment.selectedCoordinates))
                return;

            if (!systems[Environment.selectedCoordinates].Has("Average Rarity"))
                return;

            float rarity = systems[Environment.selectedCoordinates]["Average Rarity"].Get<int>();

            if (SystemsFilter.active.filter.Contains("Rarity"))
                jumpLine.endColor = Schemes.Scheme("Rarity " + StringHelper.RarityIntToString((int)rarity)).colour;
            else
                jumpLine.endColor = Schemes.Scheme("Risk " + StringHelper.RiskIntToString(Mathf.FloorToInt(Mathf.Log10(rarity)))).colour;
        }
        else if (SystemsFilter.active.filter.Contains("Risk"))
        {
            if (!systems[Environment.selectedCoordinates].Has("Average Risk"))
                return;

            float risk = systems[Environment.selectedCoordinates]["Average Risk"].Get<float>();

            if (SystemsFilter.active.filter.Contains("Rarity"))
                jumpLine.endColor = Schemes.Scheme("Rarity " + StringHelper.RarityIntToString(Mathf.FloorToInt(Mathf.Pow(10, risk)))).colour;
            else
                jumpLine.endColor = Schemes.Scheme("Risk " + StringHelper.RiskIntToString(Mathf.FloorToInt(risk))).colour;
        }

        //jumpLine.startColor.A(0.5f);
        //jumpLine.endColor.A(0.5f);
        //if (systems.ContainsKey(Environment.systemCoordinates) && systems.ContainsKey(Environment.selectedCoordinates))
        //{
        //    jumpLine.SetPosition(0, systems[Environment.systemCoordinates].transform.position);
        //    jumpLine.SetPosition(1, systems[Environment.selectedCoordinates].transform.position);
        //}

        if (environmentTime != Environment.environmentTime)
        {
            environmentTime = Environment.environmentTime;
            Regenerate();
        }
    }

    public int systemsPerFrame = 1;
    public int systemsSoFar = 0;
    public float systemsPerFrameTime = 0;

    public IEnumerator Generate()
    {
        if (!isGenerating)
        {
            isGenerating = true;

            Vector3 center = Vector3.zero;
            Vector3 centerWorld = position.position.RoundToNearestNoScaling(scale);
            Statistics system;
            Vector3 pos = center;

            Random.InitState(SystemGenerator.Hash(pos));
            Vector3 worldPos = centerWorld = ((pos.XY() - Environment.SystemCoordinates.XY()).XYO() + Random.Range(-2.5f, 2.5f).OOZ()) * scale;
            Environment.systemCoordinatesDepth = worldPos.z;
            if (!systems.ContainsKey(pos))
            {
                system = Instantiate(systemCenter, Vector3.zero, new Quaternion(), transform);
                system["System Coordinates"].Set(centerWorld);
                systems.Add(pos, system);
                systemsSoFar++;
            }
            
            for (int r = 1; r < Mathf.FloorToInt(Environment.ScanRadius); r++)
            {
                for (int x = -r; x < r; x++)
                {
                    for (int y = -r; y < r; y++)
                    {
                        if (systemsSoFar >= systemsPerFrame)
                        {
                            systemsSoFar = 0;
                            systemsPerFrameTime = Time.time;
                            yield return new WaitForEndOfFrame();
                            systemsPerFrameTime = Time.time - systemsPerFrameTime;
                            if (systemsPerFrameTime < 1f / 20)
                                systemsPerFrame++;
                            else if (systemsPerFrameTime > 1f / 10)
                                systemsPerFrame--;
                        }

                        pos = center + new Vector3(x, y, center.z);
                        Random.InitState(SystemGenerator.Hash(pos));
                        worldPos = ((pos.XY() - Environment.SystemCoordinates.XY()).XYO() + Random.Range(-2.5f, 2.5f).OOZ()) * scale;
                        float distance = (worldPos - centerWorld).magnitude / scale;
                        if (!systems.ContainsKey(pos) && distance <= r)
                        {
                            //if (distance > Environment.jumpFuel)
                            //    system = Instantiate(systemDisabled, worldPos, new Quaternion(), transform);
                            //else
                            system = Instantiate(systemDefault, pos * scale, new Quaternion(), transform);

                            system["System Coordinates"].Set(Environment.SystemCoordinates.ToInt() + pos);
                            systems.Add(pos, system);
                            systemsSoFar++;
                        }
                    }
                }
            }

            CleanUp();

            lastPosition = center;
            jumpFuel = Environment.JumpFuel;

            isGenerating = false;
        }

        yield return null;
    }

    void CleanUp()
    {
        List<Vector3> validPoints = new List<Vector3>();
        Vector3 center = position.position.RoundToNearestNoScaling(scale);
        for (int x = -Mathf.CeilToInt(Environment.ScanRadius); x <= Mathf.CeilToInt(Environment.ScanRadius); x++)
        {
            for (int y = -Mathf.CeilToInt(Environment.ScanRadius); y <= Mathf.CeilToInt(Environment.ScanRadius); y++)
            {
                Vector3 pos = center + new Vector3(x, y, center.z);
                if ((pos - center).magnitude <= Environment.ScanRadius)
                {
                    validPoints.Add(pos);
                }
            }
        }

        List<Vector3> scheduledForRemoval = new List<Vector3>();
        foreach (KeyValuePair<Vector3, Statistics> system in systems)
        {
            if (!validPoints.Contains(system.Key))
                scheduledForRemoval.Add(system.Key);
        }

        foreach (Vector3 system in scheduledForRemoval)
        {
            if (systems[system] != null)
            {
                Destroy(systems[system].GetComponent<SystemSelect>().widget.gameObject);
                Destroy(systems[system].gameObject);
            }
            systems.Remove(system);
        }
    }

    public void Regenerate()
    {
        StopAllCoroutines();
        isGenerating = false;

        List<Vector3> scheduledForRemoval = new List<Vector3>();
        foreach (KeyValuePair<Vector3, Statistics> system in systems)
        {
            scheduledForRemoval.Add(system.Key);
        }

        foreach (Vector3 system in scheduledForRemoval)
        {
            if (systems[system] != null)
            {
                Destroy(systems[system].GetComponent<SystemSelect>().widget.gameObject);
                Destroy(systems[system].gameObject);
            }
            systems.Remove(system);
        }

        StartCoroutine(Generate());
    }

    private void OnDestroy()
    {
        if (active == this)
            active = null;

        StopAllCoroutines();
    }

    [ExposeMethodInEditor]
    public void Load()
    {
        map = Resources.Load<Texture2D>(mapPath);
    }
}
