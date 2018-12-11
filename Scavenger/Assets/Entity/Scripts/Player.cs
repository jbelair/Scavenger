using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    // Use this for initialization
    public override void Awake()
    {
        base.Awake();

        isCounting = true;

        if (Players.players.Find(p => p.name == name))
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            DontDestroyOnLoad(this);
            Players.players.Add(this);
        }
    }

    private ShipDefinition ship;
    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        // TODO: Fix this, why is this here, why is this like this, why did I do this? Ehk.
        //statistics["matter"].Set(int.Parse(PlayerSave.Active.Get("matter").value));
        if (!statistics.Has("matter"))
            statistics["matter"] = new Statistic("matter", Statistic.ValueType.Integer, int.Parse(PlayerSave.Active.Get("matter").value));

        if (Environment.environmentTimeSpeed > 0)
        {
            if (!statistics.Has("ship"))
            {
                ship = JsonUtility.FromJson<ShipDefinition>(PlayerSave.Active.Get("ship").value);
                statistics["ship"] = new Statistic("ship", Statistic.ValueType.Object, ship);
            }

            if (!statistics.Has("stat_hull"))
            {
                foreach (ShipDefinition.Statistic stat in ship.statistics)
                {
                    string statistic = StringHelper.RemoveTrailingSpaces(stat.name);
                    if (!statistics.Has(statistic))
                        statistics[statistic] = new Statistic(statistic, Statistic.ValueType.Float, stat.value);
                }
            }

            if (!statistics.Has("hull") && statistics.Has("stat_hull"))
                statistics["hull"] = new Statistic("hull", Statistic.ValueType.Object, new MinMaxStatistic() { value = statistics["stat_hull"], maximum = statistics["stat_hull_max"], minimum = new StatisticUEI(new Statistic("minimum", Statistic.ValueType.Float, 0f)) });

            if (!statistics.Has("armour") && statistics.Has("stat_armour"))
                statistics["armour"] = new Statistic("armour", Statistic.ValueType.Object, new MinMaxStatistic() { value = statistics["stat_armour"], maximum = statistics["stat_armour_max"], minimum = new StatisticUEI(new Statistic("minimum", Statistic.ValueType.Float, 0f)) });

            if (!statistics.Has("shield") && statistics.Has("stat_armour"))
                statistics["shield"] = new Statistic("shield", Statistic.ValueType.Object, new MinMaxStatistic() { value = statistics["stat_shield"], maximum = statistics["stat_shield_max"], minimum = new StatisticUEI(new Statistic("minimum", Statistic.ValueType.Float, 0f)) });
        }
    }

    private void OnDestroy()
    {
        Players.players.Remove(this);
    }
}
