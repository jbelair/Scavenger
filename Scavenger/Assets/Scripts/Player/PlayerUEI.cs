using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class PlayerUEI : MonoBehaviour
{
    public List<StatisticUEI> unityStatistics = new List<StatisticUEI>();
    public Dictionary<string, Statistic> statistics = new Dictionary<string, Statistic>();
    public List<SkillUEI> unitySkills = new List<SkillUEI>();
    public bool isAlive = true;

    private void Start()
    {
        foreach (StatisticUEI stat in unityStatistics)
        {
            statistics.Add(stat.name, stat.Initialise());
        }

        if (Application.isEditor)
            StartCoroutine("PollUEI");
    }

    private void Update()
    {

    }

    private void OnDestroy()
    {
        isAlive = false;
    }

    IEnumerable PollUEI()
    {
        while (isAlive)
        {
            foreach (KeyValuePair<string, Statistic> stat in statistics)
            {
                StatisticUEI statistic = unityStatistics.First(s => s.name == stat.Key);
                if (statistic == null)
                    unityStatistics.Add(new StatisticUEI(stat.Value));
                else
                {
                    if (stat.Value.isDirty)
                    {
                        statistic = new StatisticUEI(stat.Value);
                    }
                    else
                    {
                        switch(stat.Value.type)
                        {
                            case Statistic.ValueType.Integer:
                                stat.Value.Set(statistic.valueInt);
                                break;
                            case Statistic.ValueType.Float:
                                stat.Value.Set(statistic.valueFloat);
                                break;
                            case Statistic.ValueType.String:
                                stat.Value.Set(statistic.valueString);
                                break;
                            case Statistic.ValueType.Vector2:
                                stat.Value.Set(statistic.valueV2);
                                break;
                            case Statistic.ValueType.Vector3:
                                stat.Value.Set(statistic.valueV3);
                                break;
                            case Statistic.ValueType.GameObject:
                                stat.Value.Set(statistic.valueGO);
                                break;
                        }
                    }
                }
            }

            yield return new WaitForSeconds(1.0f);
        }
    }
}
