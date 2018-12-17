using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WidgetStatisticBar : MonoBehaviour
{
    public EntityRef target;

    public Image background;
    public Image fill;
    public TextMeshProUGUI textCurrentMaximum;
    public TextMeshProUGUI textRegeneration;

    public string spriteName;
    public string minMaxName;
    public MinMaxStatistic value;
    public StatisticUEI regen;
    public StatisticUEI delay;

    private Statistic reg;
    private Statistic del;

    private List<float> polls = new List<float>();

    private void Awake()
    {
        StartCoroutine(Poll());
    }

    private void Update()
    {
        float average = 0;
        foreach (float f in polls)
        {
            average += f;
        }
        average /= polls.Count;
        while (polls.Count > 10)
        {
            polls.RemoveAt(0);
        }
        fill.fillAmount = average;
    }

    // Update is called once per frame
    IEnumerator Poll()
    {
        while (isActiveAndEnabled)
        {
            yield return new WaitForSeconds(0.1f);

            Initialise();

            if (isInitialised)
            {
                regen = reg;
                delay = del;
                
                polls.Add(value.Value.Get<float>() / value.Maximum.Get<float>());
                //fill.fillAmount = ;

                textCurrentMaximum.SetText(value.Value.Get<float>().RoundTo(1).ToString() + "/" + value.Maximum.Get<float>().RoundTo(1).ToString());
                if (value.Maximum.name.Contains("hull"))
                {
                    if (target.Entity.statistics["stat_hull_delay_cur"] < 0f && reg.Get<float>() > 0 && value.Percentage < 1)
                    {
                        textRegeneration.SetText(reg.Get<float>().RoundTo(2).ToString() + StringHelper.Units("persecond"));
                    }
                    else if (target.Entity.statistics["stat_hull_delay_cur"] > 0f)
                    {
                        textRegeneration.SetText(target.Entity.statistics["stat_hull_delay_cur"].Get<float>().RoundTo(2).ToString() + StringHelper.Units("seconds"));
                    }
                    else
                    {
                        textRegeneration.SetText("");
                    }
                }
                else if (value.Maximum.name.Contains("armour"))
                {
                    if (target.Entity.statistics["stat_armour_delay_cur"] < 0f && reg.Get<float>() > 0 && value.Percentage < 1)
                    {
                        textRegeneration.SetText(reg.Get<float>().RoundTo(2).ToString() + StringHelper.Units("persecond"));
                    }
                    else if (target.Entity.statistics["stat_armour_delay_cur"] > 0f)
                    {
                        textRegeneration.SetText(target.Entity.statistics["stat_armour_delay_cur"].Get<float>().RoundTo(2).ToString() + StringHelper.Units("seconds"));
                    }
                    else
                    {
                        textRegeneration.SetText("");
                    }
                }
                else if (value.Maximum.name.Contains("shield"))
                {
                    if (target.Entity.statistics["stat_shield_delay_cur"] < 0f && reg.Get<float>() > 0 && value.Percentage < 1)
                    {
                        textRegeneration.SetText(reg.Get<float>().RoundTo(2).ToString() + StringHelper.Units("persecond"));
                    }
                    else if (target.Entity.statistics["stat_shield_delay_cur"] > 0f)
                    {
                        textRegeneration.SetText(target.Entity.statistics["stat_shield_delay_cur"].Get<float>().RoundTo(2).ToString() + StringHelper.Units("seconds"));
                    }
                    else
                    {
                        textRegeneration.SetText("");
                    }
                }
            }
        }

        yield return null;
    }

    public bool isInitialised = false;
    void Initialise()
    {
        //if (!isInitialised)
        {
            if (target.Entity != null)
            {
                if (target.Entity.statistics.Has(value.value.name))
                {
                    isInitialised = true;

                    MinMaxStatistic t = new MinMaxStatistic(value);
                    if (target.Entity.statistics.Has(minMaxName))
                    {
                        if (target.Entity.statistics[minMaxName].Get<object>() == null)
                            target.Entity.statistics[minMaxName].Set(new MinMaxStatistic() { value = new StatisticUEI(new Statistic("stat_hull", Statistic.ValueType.Float, 0f)), minimum = new StatisticUEI(new Statistic("minimum", Statistic.ValueType.Float, 0f)), maximum = new StatisticUEI(new Statistic("stat_hull_maximum", Statistic.ValueType.Float, 0f)) });

                        value = target.Entity.statistics[minMaxName].Get<object>() as MinMaxStatistic;
                    }
                    else
                        value = t;

                    //value.Value = player.statistics[value.value.name];
                    //max = player.statistics[value.maximum.name];
                    reg = target.Entity.statistics[regen.name];
                    del = target.Entity.statistics[delay.name];

                    fill.sprite = background.sprite = Sprites.Get((spriteName != null && spriteName != "") ? spriteName : ((ShipDefinition)target.Entity.statistics["ship"].Get<object>()).name + "_" + minMaxName);
                    fill.color = Schemes.Scheme(value.Maximum.name.Remove(value.Maximum.name.Length - 4, 4)).colour;
                    background.color = fill.color.A(0.5f);
                }
                else
                    return;
            }
        }
    }
}
