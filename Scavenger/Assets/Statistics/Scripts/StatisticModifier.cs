using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class StatisticModifier
{
    public enum ModifierType { Additive, Subtractive, Percentage, Multiplicative, Override };
    public enum ValueType { Number, Percentage };
    public ModifierType type = ModifierType.Additive;
    public ValueType value = ValueType.Number;
    public StatisticUEI unityStatistic;
    public string maximumStatistic;

    private Statistic statisticInst;
    public Statistic Statistic
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

    public Statistic Maximum {get;set;}

    public void Modify(Statistic original)
    {
        if (original.type == Statistic.type)
        {
            switch (original.type)
            {
                case Statistic.ValueType.Integer:
                    switch (type)
                    {
                        case ModifierType.Additive:
                            if (value == ValueType.Number)
                                original.Set(original.Get<int>() + Statistic.Get<int>());
                            else
                                original.Set(original.Get<int>() + Statistic.Get<int>() / 100f * Maximum.Get<int>());
                            break;
                        case ModifierType.Subtractive:
                            if (value == ValueType.Number)
                                original.Set(original.Get<int>() - Statistic.Get<int>());
                            else
                                original.Set(original.Get<int>() - Statistic.Get<int>() / 100f * Maximum.Get<int>());
                            break;
                        case ModifierType.Percentage:
                            original.Set((int)(original.Get<int>() * (Statistic.Get<int>() / 100.0f)));
                            break;
                        case ModifierType.Multiplicative:
                            if (value == ValueType.Number)
                                original.Set(original.Get<int>() * Statistic.Get<int>());
                            else
                                original.Set(original.Get<int>() * Statistic.Get<int>() / 100f * Maximum.Get<int>());
                            break;
                        case ModifierType.Override:
                            if (value == ValueType.Number)
                                original.Set(Statistic.Get<int>());
                            else
                                original.Set(Statistic.Get<int>() / 100f * Maximum.Get<int>());
                            break;
                    }
                    break;
                case Statistic.ValueType.Float:
                    switch (type)
                    {
                        case ModifierType.Additive:
                            if (value == ValueType.Number)
                                original.Set(original.Get<float>() + Statistic.Get<float>());
                            else
                                original.Set(original.Get<float>() + Statistic.Get<float>() * Maximum.Get<float>());
                            break;
                        case ModifierType.Subtractive:
                            if (value == ValueType.Number)
                                original.Set(original.Get<float>() - Statistic.Get<float>());
                            else
                                original.Set(original.Get<float>() - Statistic.Get<float>() * Maximum.Get<float>());
                            break;
                        case ModifierType.Percentage:
                            original.Set(original.Get<float>() * (Statistic.Get<float>() / 100.0f));
                            break;
                        case ModifierType.Multiplicative:
                            if (value == ValueType.Number)
                                original.Set(original.Get<float>() * Statistic.Get<float>());
                            else
                                original.Set(original.Get<float>() * Statistic.Get<float>() * Maximum.Get<float>());
                            break;
                        case ModifierType.Override:
                            if (value == ValueType.Number)
                                original.Set(Statistic.Get<float>());
                            else
                                original.Set(Statistic.Get<float>() * Maximum.Get<float>());
                            break;
                    }
                    break;
                case Statistic.ValueType.String:
                    switch (type)
                    {
                        case ModifierType.Additive:
                            original.Set(original.Get<string>() + Statistic.Get<string>());
                            break;
                        case ModifierType.Subtractive:
                            string s = original.Get<string>();
                            string s2 = Statistic.Get<string>();
                            original.Set(s.Remove(s.IndexOf(s2), s2.Length));
                            break;
                        case ModifierType.Override:
                            original.Set(Statistic.Get<string>());
                            break;
                    }
                    break;
                case Statistic.ValueType.Vector2:
                    Vector2 v2;
                    Vector2 m2;
                    switch (type)
                    {
                        case ModifierType.Additive:
                            original.Set(original.Get<Vector2>() + Statistic.Get<Vector2>());
                            break;
                        case ModifierType.Subtractive:
                            original.Set(original.Get<Vector2>() - Statistic.Get<Vector2>());
                            break;
                        case ModifierType.Percentage:
                            v2 = original.Get<Vector2>();
                            m2 = Statistic.Get<Vector2>() / 100.0f;
                            original.Set(new Vector2(v2.x * m2.x, v2.y * m2.y));
                            break;
                        case ModifierType.Multiplicative:
                            v2 = original.Get<Vector2>();
                            m2 = Statistic.Get<Vector2>();
                            original.Set(new Vector2(v2.x * m2.x, v2.y * m2.y));
                            break;
                        case ModifierType.Override:
                            original.Set(Statistic.Get<Vector2>());
                            break;
                    }
                    break;
                case Statistic.ValueType.Vector3:
                    Vector3 v3;
                    Vector3 m3;
                    switch (type)
                    {
                        case ModifierType.Additive:
                            original.Set(original.Get<Vector3>() + Statistic.Get<Vector3>());
                            break;
                        case ModifierType.Subtractive:
                            original.Set(original.Get<Vector3>() - Statistic.Get<Vector3>());
                            break;
                        case ModifierType.Percentage:
                            v3 = original.Get<Vector3>();
                            m3 = Statistic.Get<Vector3>() / 100.0f;
                            original.Set(new Vector3(v3.x * m3.x, v3.y * m3.y));
                            break;
                        case ModifierType.Multiplicative:
                            v3 = original.Get<Vector3>();
                            m3 = Statistic.Get<Vector3>();
                            original.Set(new Vector3(v3.x * m3.x, v3.y * m3.y));
                            break;
                        case ModifierType.Override:
                            original.Set(Statistic.Get<Vector3>());
                            break;
                    }
                    break;
                case Statistic.ValueType.GameObject:
                    switch (type)
                    {
                        case ModifierType.Override:
                            original.Set(Statistic.Get<GameObject>());
                            break;
                    }
                    break;
                case Statistic.ValueType.GameObjectArray:
                    List<GameObject> list = new List<GameObject>();
                    switch (type)
                    {
                        case ModifierType.Additive:
                            list.AddRange(original.Get<GameObject[]>());
                            list.AddRange(Statistic.Get<GameObject[]>());
                            break;
                        case ModifierType.Subtractive:
                            list.AddRange(original.Get<GameObject[]>());
                            foreach (GameObject go in Statistic.Get<GameObject[]>())
                            {
                                if (list.Contains(go))
                                    list.Remove(go);
                            }
                            break;
                        case ModifierType.Override:
                            list.AddRange(Statistic.Get<GameObject[]>());
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
            if (original.type == Statistic.ValueType.GameObject && Statistic.type == Statistic.ValueType.GameObjectArray)
            {

            }
            else if (original.type == Statistic.ValueType.GameObjectArray && Statistic.type == Statistic.ValueType.GameObject)
            {

            }
        }

        //return original;
    }
}
