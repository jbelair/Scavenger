using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NebulousSystem : MonoBehaviour, ISystemGeneratorDecorator
{
    public EnvironmentBasedNebulousCloud nebulousCloudPrefab;

    [Header("Nebula")]
    public AnimationCurve nebulaPlotDensity;

    public Statistics statistics;

    public void Start()
    {
        if (!statistics)
            statistics = GetComponent<Statistics>();
    }

    public void System()
    {
        Texture2D map = Maps.Map("nebulae");
        Vector3Int coordinates = (statistics["System Coordinates"].Get<Vector3>() + new Vector3(map.width / 2f - 1, map.height / 2f - 1, 0)).ToInt();
        Color mapSample = map.GetPixel(coordinates.x, coordinates.y);

        if (mapSample.Vector3().Greatest() > 0)
        {
            float density = mapSample.Vector3().Greatest();
            float densitySkewed = nebulaPlotDensity.Evaluate(density);
            statistics["Nebula Density"] = new Statistic("Nebula Density", Statistic.ValueType.Float, densitySkewed);
            EnvironmentBasedNebulousCloud cloud = Instantiate(nebulousCloudPrefab, transform);
            cloud.name = "Nebula";
            cloud.environment = statistics;
        }
    }

    public void Stars()
    {

    }

    public void PopulateStars()
    {

    }

    public void Planets()
    {

    }

    public void PopulatePlanets()
    {

    }

    public void Moons()
    {

    }

    public void PopulateMoons()
    {

    }

    public void Dungeons()
    {
        Texture2D map = Maps.Map("nebulae");
        Vector3Int coordinates = (statistics["System Coordinates"].Get<Vector3>() + new Vector3(map.width / 2f - 1, map.height / 2f - 1, 0)).ToInt();
        Color mapSample = map.GetPixel(coordinates.x, coordinates.y);

        if (mapSample.Vector3().Greatest() > 0)
        {
            List<string> dungeonTargets = statistics["Dungeon Targets"].Get<object>() as List<string>;
            int dungeons = dungeonTargets.Count;

            if (Random.Range(0, 8) == 1f && dungeons < Environment.maximumDungeons)
            {
                dungeonTargets.Add("Nebula");
            }
        }
    }

    public void PopulateDungeons()
    {

    }
}
