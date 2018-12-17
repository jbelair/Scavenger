using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Statistic
{
    public enum ValueType { Integer, Float, String, Colour, Vector2, Vector3, GameObject, GameObjectArray, /*Statistic,*/ Object };

    public string name = "";
    public ValueType type;
    public bool isDirty = false;
    private object value;
    private readonly List<StatisticModifier> modifiers = new List<StatisticModifier>();

    public Statistic(string name, ValueType type, object value)
    {
        this.name = name;
        this.type = type;
        this.value = value;
        isDirty = true;
    }

    public void Set(int i)
    {
        if (type == ValueType.Integer || type == ValueType.Float)
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
        else
        {
            type = ValueType.Float;
            value = f;
            isDirty = true;
            Debug.Log("Statistic: " + name + " has been switched to a float");
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

    public void Set(Color c)
    {
        if (type == ValueType.Colour)
        {
            value = c;
            isDirty = true;
        }
    }

    public void Set(Vector2 v2)
    {
        if (type == ValueType.Vector2)
        {
            //Debug.Log("Setting statistic " + name + " to " + v2);
            value = v2;
            isDirty = true;
        }
        else
        {
            type = ValueType.Vector2;
            value = v2;
            isDirty = true;
            Debug.Log("Statistic: " + name + " has been switched to a Vector2");
        }
    }

    public void Set(Vector3 v3)
    {
        if (type == ValueType.Vector3)
        {
            value = v3;
            isDirty = true;
        }
        else
        {
            type = ValueType.Vector3;
            value = v3;
            isDirty = true;
            Debug.Log("Statistic: " + name + " has been switched to a Vector3");
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
        else
        {
            type = ValueType.GameObject;
            value = go;
            isDirty = true;
            Debug.Log("Statistic: " + name + " has been switched to a GameObject");
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
            value = o;
            isDirty = true;
        }
        else
        {
            type = ValueType.Object;
            value = o;
            isDirty = true;
        }
    }

    //public void Set<T>(T value)
    //{
    //    Type t = typeof(T);
    //    // This is a type that the value will never be, so we will be fine
    //    Type t2 = typeof(Application);
    //    switch (type)
    //    {
    //        case ValueType.Integer:
    //            t2 = typeof(int);
    //            break;
    //        case ValueType.Float:
    //            t2 = typeof(float);
    //            break;
    //        case ValueType.String:
    //            t2 = typeof(string);
    //            break;
    //        case ValueType.Vector2:
    //            t2 = typeof(Vector2);
    //            break;
    //        case ValueType.Vector3:
    //            t2 = typeof(Vector3);
    //            break;
    //        case ValueType.GameObject:
    //            t2 = typeof(GameObject);
    //            break;
    //        case ValueType.GameObjectArray:
    //            t2 = typeof(GameObject[]);
    //            break;
    //        case ValueType.Object:
    //            t2 = typeof(T);
    //            break;
    //    }

    //    if (t == t2)
    //    {
    //        this.value = value;
    //        isDirty = true;
    //    }
    //}

    public T Get<T>()
    {
        if (value == null)
            return default(T);

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
            case ValueType.Colour:
                if (t == typeof(Color))
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

    public void Add(int i)
    {
        if (type == ValueType.Integer)
        {
            value = (int)value + i;
        }
        else if (type == ValueType.Float)
        {
            value = (float)value + i;
        }
    }

    public void Add(float f)
    {
        if (type == ValueType.Float)
        {
            value = (float)value + f;
        }
        else if (type == ValueType.Integer)
        {
            value = (int)value + (int)f;
        }
    }

    public void Max(float f)
    {
        if (type == ValueType.Float)
        {
            value = Mathf.Min(f, (float)value);
        }
    }

    public void Default()
    {
        switch(type)
        {
            case ValueType.Integer:
                value = default(int);
                break;
            case ValueType.Float:
                value = default(float);
                break;
            case ValueType.String:
                value = default(string);
                break;
            case ValueType.Colour:
                value = default(Color);
                break;
            case ValueType.Vector2:
                value = default(Vector2);
                break;
            case ValueType.Vector3:
                value = default(Vector3);
                break;
            case ValueType.GameObject:
                value = null;
                break;
            case ValueType.GameObjectArray:
                value = null;
                break;
            case ValueType.Object:
                value = null;
                break;
        }
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

    public static implicit operator Color(Statistic stat)
    {
        return stat.Get<Color>();
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
