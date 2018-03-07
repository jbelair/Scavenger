using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Statistic
{
    public enum ValueType { Integer, Float, String, Vector2, Vector3, GameObject, GameObjectArray, /*Statistic,*/ Object };

    public string name = "";
    public ValueType type;
    public bool isDirty = false;
    private object value;
    private List<StatisticModifier> modifiers = new List<StatisticModifier>();

    public Statistic(string name, ValueType type, object value)
    {
        this.name = name;
        this.type = type;
        this.value = value;
        isDirty = true;
    }

    public void Set(int i)
    {
        if (type == ValueType.Integer)
        {
            value = i;
            isDirty = true;
        }
    }

    public void Set(float f)
    {
        if (type == ValueType.Float)
        {
            value = f;
            isDirty = true;
        }
    }

    public void Set(string s)
    {
        if (type == ValueType.String)
        {
            value = s;
            isDirty = true;
        }
    }

    public void Set(Vector2 v2)
    {
        if (type == ValueType.Vector2)
        {
            value = v2;
            isDirty = true;
        }
    }

    public void Set(Vector3 v3)
    {
        if (type == ValueType.Vector3)
        {
            value = v3;
            isDirty = true;
        }
    }

    public void Set(GameObject go)
    {
        if (type == ValueType.GameObject)
        {
            value = go;
            isDirty = true;
        }
        else if (type == ValueType.GameObjectArray)
        {
            value = new GameObject[] { go };
            isDirty = true;
        }
    }

    public void Set(GameObject[] goArray)
    {
        if (type == ValueType.GameObjectArray)
        {
            value = goArray;
            isDirty = true;
        }
        else if (type == ValueType.GameObject)
        {
            type = ValueType.GameObjectArray;
            value = goArray;
            isDirty = true;
        }
    }

    //public void Set(Statistic s)
    //{
    //    if (type == ValueType.Statistic)
    //    {
    //        value = s;
    //        isDirty = true;
    //    }
    //}

    public void Set(object o)
    {
        if (type == ValueType.Object)
        {
            value = 0;
            isDirty = true;
        }
    }

    public void Set<T>(T value)
    {
        Type t = typeof(T);
        // This is a type that the value will never be, so we will be fine
        Type t2 = typeof(Application);
        switch (type)
        {
            case ValueType.Integer:
                t2 = typeof(int);
                break;
            case ValueType.Float:
                t2 = typeof(float);
                break;
            case ValueType.String:
                t2 = typeof(string);
                break;
            case ValueType.Vector2:
                t2 = typeof(Vector2);
                break;
            case ValueType.Vector3:
                t2 = typeof(Vector3);
                break;
            case ValueType.GameObject:
                t2 = typeof(GameObject);
                break;
            case ValueType.GameObjectArray:
                t2 = typeof(GameObject[]);
                break;
            case ValueType.Object:
                t2 = typeof(T);
                break;
        }

        if (t == t2)
        {
            this.value = value;
            isDirty = true;
        }
    }

    public T Get<T>()
    {
        Type t = typeof(T);
        switch (type)
        {
            case ValueType.Integer:
                if (t == typeof(int))
                    return (T)value;
                break;
            case ValueType.Float:
                if (t == typeof(float))
                    return (T)value;
                break;
            case ValueType.String:
                if (t == typeof(string))
                    return (T)value;
                break;
            case ValueType.Vector2:
                if (t == typeof(Vector2))
                    return (T)value;
                break;
            case ValueType.Vector3:
                if (t == typeof(Vector3))
                    return (T)value;
                break;
            case ValueType.GameObject:
                if (t == typeof(GameObject))
                    return (T)value;
                break;
            case ValueType.GameObjectArray:
                if (t == typeof(GameObject[]))
                    return (T)value;
                break;
            case ValueType.Object:
                return (T)value;
        }

        return default(T);
    }

    public static implicit operator int(Statistic stat)
    {
        return stat.Get<int>();
    }

    public static implicit operator float(Statistic stat)
    {
        return stat.Get<float>();
    }

    public static implicit operator string(Statistic stat)
    {
        return stat.Get<string>();
    }

    public static implicit operator Vector2(Statistic stat)
    {
        return stat.Get<Vector2>();
    }

    public static implicit operator Vector3(Statistic stat)
    {
        return stat.Get<Vector3>();
    }

    public static implicit operator GameObject(Statistic stat)
    {
        return stat.Get<GameObject>();
    }

    public static implicit operator GameObject[](Statistic stat)
    {
        return stat.Get<GameObject[]>();
    }

    public static implicit operator StatisticUEI(Statistic stat)
    {
        return new StatisticUEI(stat);
    }


}
