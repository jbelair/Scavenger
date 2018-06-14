using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Statistics : MonoBehaviour, IStatistics, IEnumerable<Statistic>
{
    public bool polling = false;
    public float pollingTime = 0.5f;
    public bool isPolling = false;
    public bool isInitialised = false;

    public List<StatisticUEI> unityStatistics;
    
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
            {
                statistics.Add(index, value);
                statistics[index].isDirty = true;
            }
            else
            {
                statistics[index] = value;
                statistics[index].isDirty = true;
            }
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

                foreach (StatisticUEI statUEI in unityStatistics)
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
                        int index = unityStatistics.IndexOf(statistic);
                        unityStatistics.Remove(statistic);
                        unityStatistics.Insert(index, new StatisticUEI(stat.Value));
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
                            case Statistic.ValueType.Colour:
                                stat.Value.Set(statistic.valueColour);
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

            yield return new WaitForSeconds(pollingTime);
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

            if (Has("Rigidbody"))
            {
                //statistics["Rigidbody"].type = Statistic.ValueType.Object;
                statistics["Rigidbody"].Set(statistics["Rigidbody"].Get<GameObject>().GetComponentInChildren<Rigidbody2D>());
            }

            if (Has("Player"))
            {
                statistics["Player"].Set(statistics["Player"].Get<GameObject>().GetComponentInChildren<PlayerUEI>());
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
        Initialise();

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
