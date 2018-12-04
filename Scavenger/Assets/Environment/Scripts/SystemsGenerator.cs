using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemsGenerator : MonoBehaviour
{
    public static SystemsGenerator active;
    
    public Transform[] maps;

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
        Environment.selectedCoordinates = Environment.SystemCoordinates.ToInt();
        jumpFuel = Environment.JumpFuel;
        jumpRange = Mathf.Min(Environment.JumpFuel, Environment.JumpRadius);
        jumpRadius.transform.localScale = Vector3.one * jumpRange * scale;

        if (Environment.JumpRadius != jumpRange)
            maxJumpRadius.transform.localScale = Vector3.one * Environment.JumpRadius * scale;
        else
            maxJumpRadius.transform.localScale = Vector3.zero;

        lastPosition = position.position = Environment.SystemCoordinates.ToInt();

        StartCoroutine(Generate());

        active = this;
    }

    // Update is called once per frame
    void Update()
    {
        //if (!systems.ContainsKey(Environment.selectedCoordinates - Environment.SystemCoordinates))
        //    return;

        jumpRange = Mathf.Min(Environment.JumpFuel, Environment.JumpRadius);
        jumpRadius.transform.localScale = Vector3.one * jumpRange * scale;

        Vector3 pos = Environment.selectedCoordinates;

        foreach (Transform trans in maps)
        {
            trans.position = -Environment.SystemCoordinates * scale + Vector3.one.XYO() * (scale / 2f);//(position.position.ToInt().ToFloat().Multiply(new Vector3(-1, -1, 0)) + new Vector3(0.5f, 0.5f, 0)) * scale;
        }

        if (Environment.JumpRadius != jumpRange)
            maxJumpRadius.transform.localScale = Vector3.one * Environment.JumpRadius * scale;
        else
            maxJumpRadius.transform.localScale = Vector3.zero;

        if (environmentTime != Environment.environmentTime)
        {
            environmentTime = Environment.environmentTime;
            Regenerate();
        }

        if (SystemsFilter.active.filter.Contains("Distance"))
        {
            float distance = Mathf.Round((pos - Environment.SystemCoordinates).magnitude * 10f) / 10f;

            int value = 0;

            value = Mathf.FloorToInt((pos - Environment.SystemCoordinates).magnitude / jumpRange * 5f + 0.5f);

            if (distance <= jumpRange)
            {
                jumpLine.enabled = true;
                jumpLine.startColor = jumpRadius.color;
                jumpLine.endColor = Schemes.Scheme("Rarity " + StringHelper.RarityIntToString(value)).colour;
            }
            else
            {
                jumpLine.enabled = false;
            }
        }
        else if (SystemsFilter.active.filter.Contains("Rarity"))
        {
            if (!systems.ContainsKey(Environment.selectedCoordinates))
                return;

            if (!systems[Environment.selectedCoordinates].Has("Average Rarity"))
                return;

            float rarity = systems[Environment.selectedCoordinates]["Average Rarity"].Get<int>();
            jumpLine.endColor = Schemes.Scheme("Rarity " + StringHelper.RarityIntToString((int)rarity)).colour;
        }
        else if (SystemsFilter.active.filter.Contains("Risk"))
        {
            if(!systems.ContainsKey(Environment.selectedCoordinates))
                return;

            if (!systems[Environment.selectedCoordinates].Has("Average Risk"))
                return;

            float risk = systems[Environment.selectedCoordinates]["Average Risk"].Get<float>();
            jumpLine.endColor = Schemes.Scheme("Risk " + StringHelper.RiskIntToString(Mathf.FloorToInt(risk))).colour;
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

            Vector3 center = position.position;
            Vector3 centerWorld;
            Statistics system;
            Vector3 pos = center;

            Random.InitState(SystemGenerator.Hash(center));
            Vector3 worldPos = centerWorld = ((pos.XY() - Environment.SystemCoordinates.XY()).XYO() + Random.Range(0, -3f).OOZ()) * scale;
            Environment.systemCoordinatesDepth = worldPos.z;

            lastPosition = centerWorld;
            jumpFuel = Environment.JumpFuel;
            Environment.selectedCoordinates = worldPos;

            system = Instantiate(systemCenter, Environment.systemCoordinatesDepth.OOZ(), new Quaternion(), transform);
            system["System Coordinates"].Set(center);
            systems.Add(Vector3.zero, system);
            systemsSoFar++;

            for (int r = 1; r <= Mathf.FloorToInt(Environment.ScanRadius); r++)
            {
                for (int x = -r; x <= r; x++)
                {
                    for (int y = -r; y <= r; y++)
                    {
                        if (x == 0 && y == 0)
                            continue;

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

                        pos = new Vector3(x, y, center.z);
                        Random.InitState(SystemGenerator.Hash(worldPos + pos));
                        worldPos = ((pos.XY() - Environment.SystemCoordinates.XY()).XYO() + Random.Range(0, -3f).OOZ()) * scale;
                        float distance = pos.magnitude;
                        if (!systems.ContainsKey(pos) && distance <= r)
                        {
                            //if (distance > Environment.jumpFuel)
                            //    system = Instantiate(systemDisabled, worldPos, new Quaternion(), transform);
                            //else
                            system = Instantiate(systemDefault, pos.XYO() * scale + worldPos.OOZ(), new Quaternion(), transform);
                            system["System Coordinates"].Set(center + pos);
                            systems.Add(pos, system);
                            systemsSoFar++;
                        }
                    }
                }
            }

            CleanUp();

            isGenerating = false;
        }

        yield return null;
    }

    void CleanUp()
    {
        List<Vector3> validPoints = new List<Vector3>();
        Vector3 center = Vector3.zero;// position.position;//.RoundToNearestNoScaling(scale);
        for (int x = -Mathf.CeilToInt(Environment.ScanRadius); x <= Mathf.CeilToInt(Environment.ScanRadius); x++)
        {
            for (int y = -Mathf.CeilToInt(Environment.ScanRadius); y <= Mathf.CeilToInt(Environment.ScanRadius); y++)
            {
                Vector3 pos = new Vector3(x, y, center.z);
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
        Debug.Log("Regenerating Systems");
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
}
