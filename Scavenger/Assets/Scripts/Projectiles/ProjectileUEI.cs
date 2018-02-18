using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ProjectileUEI : MonoBehaviour, IStatistics
{
    public List<StatisticUEI> unityStatistics = new List<StatisticUEI>();
    public Dictionary<string, Statistic> statistics = new Dictionary<string, Statistic>();
    public bool initialised = false;

    public void Initialise()
    {
        if (!initialised)
        {
            foreach (StatisticUEI stat in unityStatistics)
            {
                statistics.Add(stat.name, stat.Initialise());
            }

            unityStatistics.Clear();
            unityStatistics = null;
            initialised = true;
        }
    }

    public void Start()
    {
        
    }

    public bool Has(string parameter)
    {
        return statistics.ContainsKey(parameter);
    }

    public Statistic this[string index]
    {
        get
        {
            Initialise();
            //Debug.Log(index + " : " + statistics.ContainsKey(index));
            return statistics[index];
        }
        set
        {
            Initialise();
            //Debug.Log(index + " : " + statistics.ContainsKey(index));
            if (statistics.ContainsKey(index))
                statistics[index] = value;
            else
                statistics.Add(index, value);
        }
    }
}
