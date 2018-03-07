using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class StatisticModifier
{
    public enum ModifierType { Additive, Subtractive, Percentage, Multiplicative, Override };
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

    public Statistic Modify(Statistic original)
    {
        if (original.type == statistic.type)
        {
            switch (original.type)
            {
                case Statistic.ValueType.Integer:
                    switch (type)
                    {
                        case ModifierType.Additive:
                            original.Set(original.Get<int>() + statistic.Get<int>());
                            break;
                        case ModifierType.Subtractive:
                            original.Set(original.Get<int>() - statistic.Get<int>());
                            break;
                        case ModifierType.Percentage:
                            original.Set((int)(original.Get<int>() * (statistic.Get<int>() / 100.0f)));
                            break;
                        case ModifierType.Multiplicative:
                            original.Set(original.Get<int>() * statistic.Get<int>());
                            break;
                        case ModifierType.Override:
                            original.Set(statistic.Get<int>());
                            break;
                    }
                    break;
                case Statistic.ValueType.Float:
                    switch (type)
                    {
                        case ModifierType.Additive:
                            original.Set(original.Get<float>() + statistic.Get<float>());
                            break;
                        case ModifierType.Subtractive:
                            original.Set(original.Get<float>() - statistic.Get<float>());
                            break;
                        case ModifierType.Percentage:
                            original.Set(original.Get<float>() * (statistic.Get<float>() / 100.0f));
                            break;
                        case ModifierType.Multiplicative:
                            original.Set(original.Get<float>() * statistic.Get<float>());
                            break;
                        case ModifierType.Override:
                            original.Set(statistic.Get<float>());
                            break;
                    }
                    break;
                case Statistic.ValueType.String:
                    switch (type)
                    {
                        case ModifierType.Additive:
                            original.Set(original.Get<string>() + statistic.Get<string>());
                            break;
                        case ModifierType.Subtractive:
                            string s = original.Get<string>();
                            string s2 = statistic.Get<string>();
                            original.Set(s.Remove(s.IndexOf(s2), s2.Length));
                            break;
                        case ModifierType.Override:
                            original.Set(statistic.Get<string>());
                            break;
                    }
                    break;
                case Statistic.ValueType.Vector2:
                    Vector2 v2;
                    Vector2 m2;
                    switch (type)
                    {
                        case ModifierType.Additive:
                            original.Set(original.Get<Vector2>() + statistic.Get<Vector2>());
                            break;
                        case ModifierType.Subtractive:
                            original.Set(original.Get<Vector2>() - statistic.Get<Vector2>());
                            break;
                        case ModifierType.Percentage:
                            v2 = original.Get<Vector2>();
                            m2 = statistic.Get<Vector2>() / 100.0f;
                            original.Set(new Vector2(v2.x * m2.x, v2.y * m2.y));
                            break;
                        case ModifierType.Multiplicative:
                            v2 = original.Get<Vector2>();
                            m2 = statistic.Get<Vector2>();
                            original.Set(new Vector2(v2.x * m2.x, v2.y * m2.y));
                            break;
                        case ModifierType.Override:
                            original.Set(statistic.Get<Vector2>());
                            break;
                    }
                    break;
                case Statistic.ValueType.Vector3:
                    Vector3 v3;
                    Vector3 m3;
                    switch (type)
                    {
                        case ModifierType.Additive:
                            original.Set(original.Get<Vector3>() + statistic.Get<Vector3>());
                            break;
                        case ModifierType.Subtractive:
                            original.Set(original.Get<Vector3>() - statistic.Get<Vector3>());
                            break;
                        case ModifierType.Percentage:
                            v3 = original.Get<Vector3>();
                            m3 = statistic.Get<Vector3>() / 100.0f;
                            original.Set(new Vector3(v3.x * m3.x, v3.y * m3.y));
                            break;
                        case ModifierType.Multiplicative:
                            v3 = original.Get<Vector3>();
                            m3 = statistic.Get<Vector3>();
                            original.Set(new Vector3(v3.x * m3.x, v3.y * m3.y));
                            break;
                        case ModifierType.Override:
                            original.Set(statistic.Get<Vector3>());
                            break;
                    }
                    break;
                case Statistic.ValueType.GameObject:
                    switch (type)
                    {
                        case ModifierType.Override:
                            original.Set(statistic.Get<GameObject>());
                            break;
                    }
                    break;
                case Statistic.ValueType.GameObjectArray:
                    List<GameObject> list = new List<GameObject>();
                    switch (type)
                    {
                        case ModifierType.Additive:
                            list.AddRange(original.Get<GameObject[]>());
                            list.AddRange(statistic.Get<GameObject[]>());
                            break;
                        case ModifierType.Subtractive:
                            list.AddRange(original.Get<GameObject[]>());
                            foreach(GameObject go in statistic.Get<GameObject[]>())
                            {
                                if (list.Contains(go))
                                    list.Remove(go);
                            }
                            break;
                        case ModifierType.Override:
                            list.AddRange(statistic.Get<GameObject[]>());
                            break;
                        default:
                            list.AddRange(original.Get<GameObject[]>());
                            break;
                    }
                    original.Set(list.ToArray());
                    break;
                //case Statistic.ValueType.Statistic:
                //    switch (type)
                //    {
                //        case ModifierType.Additive:
                //            break;
                //        case ModifierType.Subtractive:
                //            break;
                //        case ModifierType.Override:
                //            break;
                //    }
                //    break;
            }
        }
        else
        {
            if (original.type == Statistic.ValueType.GameObject && statistic.type == Statistic.ValueType.GameObjectArray)
            {

            }
            else if (original.type == Statistic.ValueType.GameObjectArray && statistic.type == Statistic.ValueType.GameObject)
            {

            }
        }

        return original;
    }
}
