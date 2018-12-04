using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatisticModifier : MonoBehaviour
{
    public StatisticModifier value;
    private Statistic data;

    public void Do()
    {
        if (data == null)
            data = FindObjectOfType<PlayerUEI>().statistics[value.unityStatistic.name];

        if (value.Maximum == null)
            value.Maximum = FindObjectOfType<PlayerUEI>().statistics[value.maximumStatistic];

        value.Modify(data);
    }
}
