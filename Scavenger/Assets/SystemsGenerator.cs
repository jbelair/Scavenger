using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemsGenerator : MonoBehaviour
{
    public Statistics systemPrefab;
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

    // Use this for initialization
    void Start()
    {
        jumpRange = Environment.jumpRadius;
        lastPosition = position.position = Environment.systemCoordinates.XY() * 1000f;
        Generate();
    }

    // Update is called once per frame
    void Update()
    {
        jumpRange = Mathf.Min(Environment.jumpFuel, Environment.jumpRadius);

        if (lastPosition != position.position || jumpRange != Environment.jumpRadius || jumpFuel != Environment.jumpFuel)
        {
            StartCoroutine(Generate());
        }

        jumpRadius.transform.localScale = Vector3.one * jumpRange * 1000f;

        Vector3 pos = Environment.selectedCoordinates;

        if (Environment.jumpRadius != jumpRange)
            maxJumpRadius.transform.localScale = Vector3.one * Environment.jumpRadius * 1000f;
        else
            maxJumpRadius.transform.localScale = Vector3.zero;

        if (SystemsFilter.active.filter == SystemsFilter.Filter.DistanceAsRarity || SystemsFilter.active.filter == SystemsFilter.Filter.DistanceAsRisk)
        {
            float distance = Mathf.Round((pos - Environment.systemCoordinates).magnitude * 10f) / 10f;

            int value = 0;

            if (SystemsFilter.active.filter == SystemsFilter.Filter.DistanceAsRarity)
                value = Mathf.FloorToInt(Mathf.Pow(10, (pos - Environment.systemCoordinates).magnitude / jumpRange * 6f + 0.5f));
            else
                value = Mathf.FloorToInt((pos - Environment.systemCoordinates).magnitude / jumpRange * 5f + 0.5f);

            if (distance <= jumpRange)
            {
                jumpLine.enabled = true;
                jumpLine.startColor = jumpRadius.color;

                if (SystemsFilter.active.filter == SystemsFilter.Filter.DistanceAsRarity)
                    jumpLine.endColor = WidgetScheme.active.Scheme("Rarity " + StringHelper.RarityIntToString(value)).colour;
                else
                    jumpLine.endColor = WidgetScheme.active.Scheme("Risk " + StringHelper.RiskIntToString(value)).colour;
            }
            else
            {
                jumpLine.enabled = false;
            }
        }
        else if (SystemsFilter.active.filter == SystemsFilter.Filter.RarityAsRarity || SystemsFilter.active.filter == SystemsFilter.Filter.RarityAsRisk)
        {
            if (!systems.ContainsKey(Environment.selectedCoordinates))
                return;

            if (!systems[Environment.selectedCoordinates].Has("Average Rarity"))
                return;

            float rarity = systems[Environment.selectedCoordinates]["Average Rarity"].Get<int>();

            if (SystemsFilter.active.filter == SystemsFilter.Filter.RarityAsRarity)
                jumpLine.endColor = WidgetScheme.active.Scheme("Rarity " + StringHelper.RarityIntToString((int)rarity)).colour;
            else
                jumpLine.endColor = WidgetScheme.active.Scheme("Risk " + StringHelper.RiskIntToString(Mathf.FloorToInt(Mathf.Log10(rarity)))).colour;
        }
        else if (SystemsFilter.active.filter == SystemsFilter.Filter.RiskAsRarity || SystemsFilter.active.filter == SystemsFilter.Filter.RiskAsRisk)
        {
            if (!systems[Environment.selectedCoordinates].Has("Average Risk"))
                return;

            float risk = systems[Environment.selectedCoordinates]["Average Risk"].Get<float>();

            if (SystemsFilter.active.filter == SystemsFilter.Filter.RiskAsRarity)
                jumpLine.endColor = WidgetScheme.active.Scheme("Rarity " + StringHelper.RarityIntToString(Mathf.FloorToInt(Mathf.Pow(10, risk)))).colour;
            else
                jumpLine.endColor = WidgetScheme.active.Scheme("Risk " + StringHelper.RiskIntToString(Mathf.FloorToInt(risk))).colour;
        }

        jumpLine.SetPosition(0, Vector3.zero);
        jumpLine.SetPosition(1, (pos - Environment.systemCoordinates) * 1000f);

        if (environmentTime != Environment.environmentTime)
        {
            environmentTime = Environment.environmentTime;
            //Regenerate();
        }
    }

    public IEnumerator Generate()
    {
        Vector3 center = position.position.RoundToNearestNoScaling(1000f);
        for (int x = -Mathf.CeilToInt(Environment.jumpRadius); x <= Mathf.CeilToInt(Environment.jumpRadius); x++)
        {
            for (int y = -Mathf.CeilToInt(Environment.jumpRadius); y <= Mathf.CeilToInt(Environment.jumpRadius); y++)
            {
                Vector3 pos = center + new Vector3(x, y, center.z);
                if (!systems.ContainsKey(pos) && (pos - center).magnitude <= Environment.jumpRadius)
                {
                    Statistics system = Instantiate(systemPrefab, (pos.XY() - Environment.systemCoordinates.XY()) * 1000f, new Quaternion(), transform);
                    system["System Coordinates"].Set(pos);
                    systems.Add(pos, system);
                    yield return new WaitForEndOfFrame();
                }
            }
        }

        CleanUp();

        lastPosition = center;
        jumpFuel = Environment.jumpFuel;
    }

    void CleanUp()
    {
        List<Vector3> validPoints = new List<Vector3>();
        Vector3 center = position.position.RoundToNearestNoScaling(1000f);
        for (int x = -Mathf.CeilToInt(Environment.jumpRadius); x <= Mathf.CeilToInt(Environment.jumpRadius); x++)
        {
            for (int y = -Mathf.CeilToInt(Environment.jumpRadius); y <= Mathf.CeilToInt(Environment.jumpRadius); y++)
            {
                Vector3 pos = center + new Vector3(x, y, center.z);
                if ((pos - center).magnitude <= Environment.jumpRadius)
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
            Destroy(systems[system].gameObject);
            systems.Remove(system);
        }
    }

    public void Regenerate()
    {
        List<Vector3> scheduledForRemoval = new List<Vector3>();
        foreach (KeyValuePair<Vector3, Statistics> system in systems)
        {
            scheduledForRemoval.Add(system.Key);
        }

        foreach (Vector3 system in scheduledForRemoval)
        {
            if (systems[system] != null)
                Destroy(systems[system].gameObject);
            systems.Remove(system);
        }

        Generate();
    }
}
