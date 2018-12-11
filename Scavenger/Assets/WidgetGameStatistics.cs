using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidgetGameStatistics : MonoBehaviour
{
    public WidgetStatistic prefab;

    public EntityRef target;
    public List<ShipDefinition.Statistic> stats = new List<ShipDefinition.Statistic>();
    public List<WidgetStatistic> statistics = new List<WidgetStatistic>();

    public bool isInitialised = false;

    private void Awake()
    {
        StartCoroutine(Co_Initialise());
    }

    private void Update()
    {
        foreach(WidgetStatistic statistic in statistics)
        {
            ShipDefinition.Statistic stat = stats.Find(s => s.name == statistic.statistic_ref);
            statistic.gameObject.SetActive(stat.value != 1);
            if (stat.value != 1)
                statistic.value.SetText(stat.value + StringHelper.Units(stat.unit));
        }
    }

    // Update is called once per frame
    IEnumerator Co_Initialise()
    {
        while (!isInitialised)
        {
            yield return new WaitForSeconds(0.1f);

            if (target && target.Entity && target.Entity.statistics.Has("ship"))
            {
                stats = ((ShipDefinition)target.Entity.statistics["ship"].Get<object>()).statistics;

                foreach (ShipDefinition.Statistic stat in stats)
                {
                    if (stat.name.Contains("mult"))
                    {
                        WidgetStatistic inst = Instantiate(prefab, transform, false);
                        inst.statistic_ref = stat.name;
                        inst.Initialise();
                        statistics.Add(inst);
                    }
                }

                isInitialised = true;
            }
        }

        yield return null;
    }
}
