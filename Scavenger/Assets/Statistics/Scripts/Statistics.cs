using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Statistics : MonoBehaviour, IStatistics, IEnumerable<Statistic>
{
    public List<StatisticUEI> unityStatistics;
    public bool polling = false;
    public bool isPolling = false;
    public bool isInitialised = false;

    public Dictionary<string, Statistic> statistics = new Dictionary<string, Statistic>();

    public Statistic this[string index]
    {
        get
        {
            Initialise();
            if (!statistics.ContainsKey(index))
                return null;
            else
                return statistics[index];
        }
        set
        {
            Initialise();
            if (!statistics.ContainsKey(index))
                statistics.Add(index, value);
            else
                statistics[index] = value;
        }
    }

    public IEnumerator<Statistic> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator PollUEI()
    {
        //Debug.Log("PollUEI started");
        while (polling)
        {
            //Debug.Log("PollUEI polling");
            isPolling = true;

            foreach (KeyValuePair<string, Statistic> stat in statistics)
            {
                StatisticUEI statistic = null;// unityStatistics.First(s => s.name == stat.Key);

                foreach(StatisticUEI statUEI in unityStatistics)
                {
                    if (statUEI.name == stat.Key)
                        statistic = statUEI;
                }

                if (statistic == null)
                    unityStatistics.Add(new StatisticUEI(stat.Value));
                else
                {
                    if (stat.Value.isDirty)
                    {
                        statistic = new StatisticUEI(stat.Value);
                        stat.Value.isDirty = false;
                    }
                    else
                    {
                        switch (stat.Value.type)
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

        //Debug.Log("PollUEI stopping");
        isPolling = false;
    }

    public bool Has(string parameter)
    {
        return statistics.ContainsKey(parameter);
    }

    public void Initialise()
    {
        if (!isInitialised)
        {
            foreach (StatisticUEI stat in unityStatistics)
            {
                statistics.Add(stat.name, stat.Initialise());
                stat.statistic = statistics[stat.name];
            }

            if (!polling)
            {
                unityStatistics.Clear();
                //unityStatistics = null;
            }

            isInitialised = true;
        }
    }

    public void Start()
    {
        Initialise();
    }

    public void Update()
    {
        if (Application.isEditor)
        {
            if (polling)
            {
                if (!isPolling)
                {
                    //Debug.Log("Starting PollUEI");
                    StartCoroutine("PollUEI");
                }
            }
            else
            {
                if (isPolling)
                {
                    //Debug.Log("Stopping PollUEI");
                    StopCoroutine("PollUEI");
                }
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return statistics.GetEnumerator();
    }
}
