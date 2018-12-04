using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class VectorExtensions
{
    public static Vector2 XY(this Vector3 v)
    {
        return new Vector2(v.x, v.y);
    }

    public static Vector2 XZ(this Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }

    public static Vector3 XOO(this float f)
    {
        return new Vector3(f, 0, 0);
    }

    public static Vector3 XOO(this Vector3 v)
    {
        return new Vector3(v.x, 0, 0);
    }

    public static Vector3 OYO(this float f)
    {
        return new Vector3(0, f, 0);
    }

    public static Vector3 OYO(this Vector3 v)
    {
        return new Vector3(0, v.y, 0);
    }

    public static Vector3 OOZ(this float f)
    {
        return new Vector3(0, 0, f);
    }

    public static Vector3 OOZ(this Vector3 v)
    {
        return new Vector3(0, 0, v.z);
    }

    public static Vector3 XYO(this Vector2 v)
    {
        return new Vector3(v.x, v.y, 0);
    }

    public static Vector3 XYO(this Vector3 v)
    {
        return new Vector3(v.x, v.y, 0);
    }

    public static Vector3 XOY(this Vector3 v)
    {
        return new Vector3(v.x, 0, v.y);
    }

    public static Vector3 XOZ(this Vector3 v)
    {
        return new Vector3(v.x, 0, v.z);
    }

    public static Vector3 OYZ(this Vector3 v)
    {
        return new Vector3(0, v.y, v.z);
    }

    public static Vector3 OYX(this Vector3 v)
    {
        return new Vector3(0, v.y, v.x);
    }

    public static Vector3 XZY(this Vector3 v)
    {
        return new Vector3(v.x, v.z, v.y);
    }

    public static Vector2 ToFloat(this Vector2Int v)
    {
        return new Vector2(v.x, v.y);
    }

    public static Vector2Int ToInt(this Vector2 v)
    {
        return new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
    }

    public static Vector2 Round(this Vector2 v)
    {
        return new Vector2(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
    }

    public static Vector3 Round(this Vector3 v)
    {
        return new Vector3(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y), Mathf.RoundToInt(v.z));
    }

    public static Vector2 Multiply(this Vector2 v, Vector2 v2)
    {
        return new Vector3(v.x * v2.x, v.y * v2.y);
    }

    public static Vector3 Multiply(this Vector3 v, Vector3 v2)
    {
        return new Vector3(v.x * v2.x, v.y * v2.y, v.z * v2.z);
    }

    public static Vector3 RoundToNearestNoScaling(this Vector3 v, float nearest)
    {
        return new Vector3(Mathf.Round(v.x / nearest), Mathf.Round(v.y / nearest), Mathf.Round(v.z / nearest));
    }

    public static Vector3 RoundToNearest(this Vector3 v, float nearest)
    {
        return new Vector3(Mathf.Round(v.x / nearest) * nearest, Mathf.Round(v.y / nearest) * nearest, Mathf.Round(v.z / nearest) * nearest);
    }

    public static Vector3 ToFloat(this Vector3Int v)
    {
        return new Vector3(v.x, v.y, v.z);
    }

    public static Vector3Int ToInt(this Vector3 v)
    {
        return new Vector3Int((int)v.x, (int)v.y, (int)v.z);
    }

    public static float Greatest(this Vector3 v)
    {
        return Mathf.Max(Mathf.Max(v.x, v.y), v.z);
    }
}