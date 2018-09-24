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
}