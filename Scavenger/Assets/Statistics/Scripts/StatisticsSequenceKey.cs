using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class StatisticsSequenceKey
{
    public string name = "Default";
    public float duration = 0;
    public float durationCurrent = 0;

    public List<StatisticUEI> format;
}
