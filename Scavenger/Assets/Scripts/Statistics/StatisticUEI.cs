using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class StatisticUEI
{
    public string name = "";
    public Statistic.ValueType type;
    public int valueInt;
    public float valueFloat;
    public string valueString;
    public Vector2 valueV2;
    public Vector3 valueV3;
    public GameObject valueGO;

    public StatisticUEI(Statistic stat)
    {
        name = stat.name;
        type = stat.type;
        switch(type)
        {
            case Statistic.ValueType.Integer:
                valueInt = stat.Get<int>();
                break;
            case Statistic.ValueType.Float:
                valueFloat = stat.Get<float>();
                break;
            case Statistic.ValueType.String:
                valueString = stat.Get<string>();
                break;
            case Statistic.ValueType.Vector2:
                valueV2 = stat.Get<Vector2>();
                break;
            case Statistic.ValueType.Vector3:
                valueV3 = stat.Get<Vector3>();
                break;
            case Statistic.ValueType.GameObject:
                valueGO = stat.Get<GameObject>();
                break;
        }
    }

    public Statistic Initialise()
    {
        switch (type)
        {
            case Statistic.ValueType.Integer:
                return new Statistic(name, type, valueInt);
            case Statistic.ValueType.Float:
                return new Statistic(name, type, valueFloat);
            case Statistic.ValueType.String:
                return new Statistic(name, type, valueString);
            case Statistic.ValueType.Vector2:
                return new Statistic(name, type, valueV2);
            case Statistic.ValueType.Vector3:
                return new Statistic(name, type, valueV3);
            case Statistic.ValueType.GameObject:
                return new Statistic(name, type, valueGO);
        }

        return null;
    }
}
