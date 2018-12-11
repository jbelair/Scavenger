using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatisticModifier : MonoBehaviour
{
    public StatisticModifier modifier;

    public void Do()
    {
        if (modifier.maximum == null)
            modifier.maximum = FindObjectOfType<Player>().statistics[modifier.unityStatistic.maximum.name];

        modifier.Modify(modifier.unityStatistic.Value);
    }

    public void Undo()
    {
        if (modifier.maximum == null)
            modifier.maximum = FindObjectOfType<Player>().statistics[modifier.unityStatistic.maximum.name];

        modifier.Unmodify(modifier.unityStatistic.Value);
    }
}
