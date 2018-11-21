using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WidgetSystem : MonoBehaviour
{
    public Statistics environment;
    public WidgetLabel labelWidget;
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
    public WidgetSystemTags[] tags;
    public string lastFilter = "";
    public bool lastActive = false;
    public float radius = 256f;

    public float distance;
    public float distanceFromCursor;

    public bool initialised = false;

    public bool polling = true;
    public float pollingUpdate;

    public List<DungeonType> activeDungeons = new List<DungeonType>();
    public Color colour;

    public void Set(Statistics environment)
    {
        this.environment = environment;

        //if (environment["System Coordinates"].Get<Vector3>() == Environment.systemCoordinates)
        //    transform.localScale = Vector3.one * 2;

        if (!environment.Has("Dungeons"))
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

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    void Die()
    {
        polling = false;
        StopAllCoroutines();
    }

    IEnumerator Poll()
    {
        while (polling)
        {
            if (!environment.gameObject)// || SystemsGenerator.active.systems.ContainsKey(environment["System Coordinates"]))
            {
                polling = false;
                break;
            }

            Vector3 position = environment["System Coordinates"];
            Vector3 pos;
            if (SystemsGenerator.active.systems.ContainsKey(position))
                pos = Camera.main.WorldToScreenPoint(SystemsGenerator.active.systems[position].transform.position);

            distanceFromCursor = 1;// - Mathf.Clamp01(Vector2.Distance(pos, Input.mousePosition) / radius);
            distance = Mathf.Round((position - Environment.SystemCoordinates).magnitude * 10f) / 10f;

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

                List<DungeonType> activeTargets = new List<DungeonType>();
                if (environment.Has("Active Dungeons"))
                    activeTargets = environment["Active Dungeons"].Get<object>() as List<DungeonType>;

                int rarity = 0;
                float risk = -1;

                if (activeTargets.Count > 0)
                {
                    foreach(DungeonType dungeon in activeTargets)
                    {
                        rarity = Mathf.Max(rarity, dungeon.oneIn);

                        risk = Mathf.Max(risk, FloatHelper.RiskStringToFloat(dungeon.risk));

                        labelWidget.Add(dungeon);
                    }
                }
                else
                {
                    List<string> targets = environment["Dungeon Targets"].Get<object>() as List<string>;

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

                        labelWidget.Add(dungeonType);
                        dungeons.Remove(dungeonType);
                    }
                }

                if (!environment.Has("Average Rarity"))
                    environment["Average Rarity"] = new Statistic("Average Rarity", Statistic.ValueType.Integer, rarity);
                else
                    environment["Average Rarity"].Set(rarity);

                if (!environment.Has("Average Risk"))
                    environment["Average Risk"] = new Statistic("Average Risk", Statistic.ValueType.Float, risk);
                else
                    environment["Average Risk"].Set(risk);

                foreach (WidgetSystemTags tag in tags)
                    tag.Set(activeDungeons);

                label.text = StringHelper.CoordinateName(environment["System Coordinates"]);
                if (distance <= Environment.JumpRadius)
                {
                    distanceLabel.text = Literals.active["distance"] + " " + distance;
                    distanceLabel.color = Schemes.Scheme(StringHelper.RiskIntToString(Mathf.FloorToInt((distance / Environment.JumpRadius) * 5)).Replace("risk_", "distance_")).colour;
                }
                else
                {
                    distanceLabel.text = Literals.active["unreachable"];
                    distanceLabel.color = Schemes.Scheme("disabled").colour;//WidgetScheme.Scheme("Distance " + StringHelper.RiskIntToString(Mathf.FloorToInt((distance / Environment.jumpRadius) * 5)).Replace("risk_", "")).colour;
                }
                rarityLabel.text = Literals.active[StringHelper.RarityIntToString(rarity)];
                rarityLabel.color = Schemes.Scheme(StringHelper.RarityIntToString(rarity)).colour;
                riskLabel.text = Literals.active[StringHelper.RiskIntToString(Mathf.FloorToInt(risk))];
                riskLabel.color = Schemes.Scheme(StringHelper.RiskIntToString(Mathf.FloorToInt(risk))).colour;

                initialised = true;
            }

            bool active = false;
            if (SystemsFilter.active.filterTags != "")
            {
                string[] tagSplit = SystemsFilter.active.filterTags.Split(new string[] { ", " }, System.StringSplitOptions.RemoveEmptyEntries);
                foreach (DungeonType dungeon in activeDungeons)
                {
                    if (dungeon.name == null || dungeon.description == null || dungeon.category == null || dungeon.risk == null || dungeon.target == null || dungeon.generator == null || dungeon.tags == null)
                        continue;

                    int contains = 0;
                    foreach (string tag in tagSplit)
                    {
                        if (Literals.iLiterals[PlayerPrefs.GetString("language")].ContainsKey(tag))
                            if (dungeon.tags.Contains(Literals.iLiterals[PlayerPrefs.GetString("language")][tag]))
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

            float jumpRange = Mathf.Min(Environment.JumpFuel, Environment.JumpRadius);

            if (lastFilter != SystemsFilter.active.filter || lastActive != active)
            {
                Scheme scheme = Schemes.Scheme("default");
                if (SystemsFilter.active.filter.Contains("Distance"))
                {
                    int value = 0;

                    if (SystemsFilter.active.filter.Contains("Rarity"))
                        value = Mathf.FloorToInt(Mathf.Pow(10, (position - Environment.SystemCoordinates).magnitude / jumpRange * 6f + 0.5f));
                    else
                        value = Mathf.FloorToInt((position - Environment.SystemCoordinates).magnitude / jumpRange * 5f + 0.5f);

                    if (distance <= jumpRange)
                    {
                        if (SystemsFilter.active.filter.Contains("Rarity"))
                            scheme = Schemes.Scheme(StringHelper.RarityIntToString(value));
                        else
                            scheme = Schemes.Scheme(StringHelper.RiskIntToString(value));
                    }
                    else
                    {
                        if (SystemsFilter.active.filter.Contains("Rarity"))
                            scheme = Schemes.Scheme("rarity_abundant");
                        else
                            scheme = Schemes.Scheme("risk_none");
                    }
                }
                else if (SystemsFilter.active.filter.Contains("Rarity"))
                {
                    int rarity = environment["Average Rarity"];

                    if (!SystemsFilter.active.filter.Contains("Risk"))
                        scheme = Schemes.Scheme(StringHelper.RarityIntToString(rarity));
                    else
                        scheme = Schemes.Scheme(StringHelper.RiskIntToString(Mathf.FloorToInt(Mathf.Log10(rarity))));
                }
                else if (SystemsFilter.active.filter.Contains("Risk"))
                {
                    float risk = environment["Average Risk"];

                    if (SystemsFilter.active.filter.Contains("Rarity"))
                        scheme = Schemes.Scheme(StringHelper.RarityIntToString(Mathf.FloorToInt(Mathf.Pow(10, risk))));
                    else
                        scheme = Schemes.Scheme(StringHelper.RiskIntToString(Mathf.FloorToInt(risk)));
                }

                foreach (Image image in images)
                {
                    image.color = scheme.colour;
                    if (!active)
                        image.color = (image.color + Schemes.Scheme("disabled").colour) / 2;
                }

                lastFilter = SystemsFilter.active.filter;
            }

            //if (tag)
            //{
            //    if (tags.tags.Count > 0)
            //    {
            //        if (cycleCurrent >= cycle)
            //        {
            //            cycleCurrent -= cycle;
            //            cycleIndex = (cycleIndex + 1) % tags.tags.Count;

            //            WidgetScheme.SchemeContainer tagScheme = WidgetScheme.Scheme(tags.tags[cycleIndex]);
            //            tag.sprite = tagScheme.symbol;
            //            if (active)
            //                tag.color = tagScheme.colour;
            //            else
            //                tag.color = (tagScheme.colour + WidgetScheme.Scheme("Disabled").colour) / 2;
            //        }
            //        cycleCurrent += Time.deltaTime;
            //    }
            //    else
            //    {
            //        WidgetScheme.SchemeContainer tagScheme = WidgetScheme.Scheme("Transparent");
            //        tag.sprite = tagScheme.symbol;
            //        tag.color = tagScheme.colour;
            //    }
            //}

            //foreach (Image image in images)
            //{
            //    if (active)
            //        image.color = new Color(image.color.r, image.color.g, image.color.b, distanceFromCursor);
            //    else
            //    {
            //        image.color = WidgetScheme.Scheme("Disabled").colour;
            //        image.color = new Color(image.color.r, image.color.g, image.color.b, distanceFromCursor);
            //    }

            //    colour = image.color;
            //}

            lastActive = active;

            yield return new WaitForSeconds(pollingUpdate);
        }
    }
}
