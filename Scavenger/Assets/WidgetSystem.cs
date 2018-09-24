using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WidgetSystem : MonoBehaviour
{
    public Statistics environment;
    public TextMeshProUGUI label;
    public TextMeshProUGUI distanceLabel;
    public TextMeshProUGUI rarityLabel;
    public TextMeshProUGUI riskLabel;
    public TextMeshProUGUI permanentLabel;
    public Image[] images;
    public Image tag;
    public int cycleIndex = 0;
    public float cycle = 1;
    public float cycleCurrent = 1;
    public WidgetSystemTags tags;
    public SystemsFilter.Filter lastFilter = SystemsFilter.Filter.None;

    public float distance;

    public bool initialised = false;

    public bool polling = true;
    public float pollingUpdate;

    public List<DungeonType> activeDungeons = new List<DungeonType>();

    public void Set(Statistics environment)
    {
        this.environment = environment;

        //if (environment["System Coordinates"].Get<Vector3>() == Environment.systemCoordinates)
        //    transform.localScale = Vector3.one * 2;

        if (environment["Stars"].Get<int>() < 1)
            Destroy(gameObject);
        else
        {
            StartCoroutine(Poll());
        }
    }

    public void Hover()
    {
        Environment.jumpDistance = distance;

        Vector3 position = environment["System Coordinates"].Get<Vector3>();

        //if (position == Environment.systemCoordinates)
        //    transform.localScale = Vector3.one * 3;
        //else
        //    transform.localScale = Vector3.one * 2;

        Environment.selectedCoordinates = position;
    }

    public void HoverEnd()
    {
        //if (environment["System Coordinates"].Get<Vector3>() == Environment.systemCoordinates)
        //    transform.localScale = Vector3.one * 2;
        //else
        //    transform.localScale = Vector3.one;
    }

    IEnumerator Poll()
    {
        while (polling)
        {
            Vector3 position = environment["System Coordinates"];

            distance = Mathf.Round((position - Environment.systemCoordinates).magnitude * 10f) / 10f;

            if (!initialised)
            {
                if (!environment.Has("Dungeons"))
                {
                    yield return new WaitForSeconds(0.5f);
                    continue;
                }

                activeDungeons = new List<DungeonType>();

                List<DungeonType> dungeons = new List<DungeonType>();
                dungeons.AddRange(environment["Dungeons"].Get<object>() as List<DungeonType>);

                for (int i = 0; i < dungeons.Count; i++)
                {
                    dungeons[i] = new DungeonType(dungeons[i]);
                }

                List<string> targets = environment["Dungeon Targets"].Get<object>() as List<string>;

                int rarity = 0;
                float risk = -1;

                foreach (string target in targets)
                {
                    DungeonType dungeonType;
                    if (target.Contains("Star"))
                    {
                        dungeonType = dungeons.Find(dungeon => dungeon.target.Contains("Any") || dungeon.target.Contains("Star"));
                        //if (dungeonType.name == null)
                        //    dungeonType = dungeons.Find(dungeon => dungeon.target.Contains("Any"));

                        activeDungeons.Add(dungeonType);
                    }
                    else if (target.Contains("Planet"))
                    {
                        if (target.Contains("Moon"))
                        {
                            dungeonType = dungeons.Find(dungeon => dungeon.target.Contains("Any") || dungeon.target.Contains("Moon"));
                            //if (dungeonType.name == null)
                            //    dungeonType = dungeons.Find(dungeon => dungeon.target.Contains("Any"));
                            activeDungeons.Add(dungeonType);
                        }
                        else
                        {
                            dungeonType = dungeons.Find(dungeon => dungeon.target.Contains("Any") || dungeon.target.Contains("Planet"));
                            //if (dungeonType.name == null)
                            //    dungeonType = dungeons.Find(dungeon => dungeon.target.Contains("Any"));
                            activeDungeons.Add(dungeonType);
                        }

                    }
                    else if (target.Contains("Singularity"))
                    {
                        dungeonType = dungeons.Find(dungeon => dungeon.target.Contains("Singularity"));
                        activeDungeons.Add(dungeonType);
                    }
                    else
                    {
                        dungeonType = dungeons.Find(dungeon => dungeon.target.Contains("Any"));
                        activeDungeons.Add(dungeonType);
                    }

                    rarity = Mathf.Max(rarity, activeDungeons[activeDungeons.Count - 1].oneIn);

                    risk = Mathf.Max(risk, FloatHelper.RiskStringToFloat(activeDungeons[activeDungeons.Count - 1].risk));
                    dungeons.Remove(dungeonType);
                }

                if (!environment.Has("Average Rarity"))
                    environment["Average Rarity"] = new Statistic("Average Rarity", Statistic.ValueType.Integer, rarity);
                else
                    environment["Average Rarity"].Set(rarity);

                if (!environment.Has("Average Risk"))
                    environment["Average Risk"] = new Statistic("Average Risk", Statistic.ValueType.Float, risk);
                else
                    environment["Average Risk"].Set(risk);

                tags.Set(activeDungeons);

                label.text = StringHelper.CoordinateName(position);
                distanceLabel.text = "Distance " + distance;
                rarityLabel.text = "Rarity " + StringHelper.RarityIntToString(rarity);
                riskLabel.text = "Risk " + StringHelper.RiskIntToString(Mathf.FloorToInt(risk));

                initialised = true;
            }

            float jumpRange = Mathf.Min(Environment.jumpFuel, Environment.jumpRadius);

            if (lastFilter != SystemsFilter.active.filter)
            {
                WidgetScheme.SchemeContainer scheme = WidgetScheme.active.Scheme("Default");
                if (SystemsFilter.active.filter == SystemsFilter.Filter.DistanceAsRarity || SystemsFilter.active.filter == SystemsFilter.Filter.DistanceAsRisk)
                {
                    int value = 0;

                    if (SystemsFilter.active.filter == SystemsFilter.Filter.DistanceAsRarity)
                        value = Mathf.FloorToInt(Mathf.Pow(10, (position - Environment.systemCoordinates).magnitude / jumpRange * 6f + 0.5f));
                    else
                        value = Mathf.FloorToInt((position - Environment.systemCoordinates).magnitude / jumpRange * 5f + 0.5f);

                    if (distance <= jumpRange)
                    {
                        if (SystemsFilter.active.filter == SystemsFilter.Filter.DistanceAsRarity)
                            scheme = WidgetScheme.active.Scheme("Rarity " + StringHelper.RarityIntToString(value));
                        else
                            scheme = WidgetScheme.active.Scheme("Risk " + StringHelper.RiskIntToString(value));
                    }
                    else
                    {
                        if (SystemsFilter.active.filter == SystemsFilter.Filter.DistanceAsRarity)
                            scheme = WidgetScheme.active.Scheme("Rarity Abundant");
                        else
                            scheme = WidgetScheme.active.Scheme("Risk None");
                    }
                }
                else if (SystemsFilter.active.filter == SystemsFilter.Filter.RarityAsRarity || SystemsFilter.active.filter == SystemsFilter.Filter.RarityAsRisk)
                {
                    int rarity = environment["Average Rarity"];

                    if (SystemsFilter.active.filter == SystemsFilter.Filter.RarityAsRarity)
                        scheme = WidgetScheme.active.Scheme("Rarity " + StringHelper.RarityIntToString(rarity));
                    else
                        scheme = WidgetScheme.active.Scheme("Risk " + StringHelper.RiskIntToString(Mathf.FloorToInt(Mathf.Log10(rarity))));
                }
                else if (SystemsFilter.active.filter == SystemsFilter.Filter.RiskAsRarity || SystemsFilter.active.filter == SystemsFilter.Filter.RiskAsRisk)
                {
                    float risk = environment["Average Risk"];

                    if (SystemsFilter.active.filter == SystemsFilter.Filter.RiskAsRarity)
                        scheme = WidgetScheme.active.Scheme("Rarity " + StringHelper.RarityIntToString(Mathf.FloorToInt(Mathf.Pow(10, risk))));
                    else
                        scheme = WidgetScheme.active.Scheme("Risk " + StringHelper.RiskIntToString(Mathf.FloorToInt(risk)));
                }

                foreach (Image image in images)
                {
                    image.color = scheme.colour;
                }

                lastFilter = SystemsFilter.active.filter;
            }

            bool active = false;
            if (SystemsFilter.active.filterTags != "")
            {
                string[] tagSplit = SystemsFilter.active.filterTags.Split(' ');
                foreach (DungeonType dungeon in activeDungeons)
                {
                    int contains = 0;
                    foreach (string tag in tagSplit)
                    {
                        if (dungeon.tags.Contains(tag))
                            contains++;
                    }

                    if (contains == tagSplit.Length)
                        active = true;
                }
            }
            else
            {
                active = true;
            }

            if (tags.tags.Count > 0)
            {
                if (cycleCurrent >= cycle)
                {
                    cycleCurrent -= cycle;
                    cycleIndex = (cycleIndex + 1) % tags.tags.Count;
                    
                    WidgetScheme.SchemeContainer scheme = WidgetScheme.active.Scheme(tags.tags[cycleIndex]);
                    tag.sprite = scheme.symbol;
                    if (active)
                        tag.color = scheme.colour;
                    else
                        tag.color = WidgetScheme.active.Scheme("Disabled").colour;
                }
                cycleCurrent += Time.deltaTime;
            }
            else
            {
                WidgetScheme.SchemeContainer scheme = WidgetScheme.active.Scheme("Transparent");
                tag.sprite = scheme.symbol;
                tag.color = scheme.colour;
            }

            foreach (Image image in images)
            {
                if (active)
                    image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
                else
                    image.color = WidgetScheme.active.Scheme("Disabled").colour;
            }

            yield return new WaitForSeconds(pollingUpdate);
        }
    }
}
