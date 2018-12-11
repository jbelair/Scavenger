using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class MinMaxStatistic
{
    public StatisticUEI value;
    public StatisticUEI minimum;
    public StatisticUEI maximum;

    private Statistic v;
    public Statistic Value
    {
        get
        {
            if (v == null)
                v = UnityEngine.Object.FindObjectOfType<Player>().statistics[value.name];

            switch (v.type)
            {
                case Statistic.ValueType.Integer:
                    v.Set((int)Mathf.Min(Maximum.Get<float>(), Mathf.Max(v.Get<int>(), Minimum.Get<float>())));
                    return v;
                case Statistic.ValueType.Float:
                    v.Set(Mathf.Min(Maximum.Get<float>(), Mathf.Max(v.Get<float>(), Minimum.Get<float>())));
                    return v;
                case Statistic.ValueType.Vector2:
                    Vector2 v2 = v.Get<Vector2>();
                    if (v2.magnitude < Minimum.Get<float>())
                        v2 = v2.normalized * Minimum.Get<float>();
                    else if (v2.magnitude > Maximum.Get<float>())
                        v2 = v2.normalized * Maximum.Get<float>();
                    v.Set(v2);
                    return v;
                case Statistic.ValueType.Vector3:
                    Vector3 v3 = v.Get<Vector3>();
                    if (v3.magnitude < Minimum.Get<float>())
                        v3 = v3.normalized * Minimum.Get<float>();
                    else if (v3.magnitude > Maximum.Get<float>())
                        v3 = v3.normalized * Maximum.Get<float>();
                    v.Set(v3);
                    return v;
            }
            return v;
        }
    }

    private Statistic mi;
    public Statistic Minimum
    {
        get
        {
            if (mi == null)
                mi = minimum.Initialise();//UnityEngine.Object.FindObjectOfType<Player>().statistics[minimum.name];
            return mi;
        }
    }

    private Statistic ma;
    public Statistic Maximum
    {
        get
        {
            if (ma == null)
                ma = UnityEngine.Object.FindObjectOfType<Player>().statistics[maximum.name];
            return ma;
        }
    }

    public float Percentage
    {
        get
        {
            return Value.Get<float>() / Maximum.Get<float>();
        }
    }

    public MinMaxStatistic()
    {

    }

    public MinMaxStatistic(MinMaxStatistic original)
    {
        value = new StatisticUEI(original.value.Initialise());
        minimum = new StatisticUEI(original.value.Initialise());
        maximum = new StatisticUEI(original.value.Initialise());
    }
}
