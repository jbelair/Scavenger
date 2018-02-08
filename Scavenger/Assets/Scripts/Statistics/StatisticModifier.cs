using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class StatisticModifier
{
    public enum ModifierType { Additive, Percentage, Multiplicative, Override };
    public ModifierType type = ModifierType.Additive;
    public StatisticUEI unityStatistic;
    private Statistic statisticInst;
    public Statistic statistic
    {
        get
        {
            if (statisticInst == null)
                statisticInst = unityStatistic.Initialise();
            return statisticInst;
        }
        set
        {
            statisticInst = value;
        }
    }
}
