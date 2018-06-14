using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class ColourHelper
{
    #region C# Vector Swizzles RGBA
    // Vector 2 --------------------------------------------------------------------------------
    //public static float r(this Vector2 v)
    //{
    //    return v.x;
    //}

    //public static float g(this Vector2 v)
    //{
    //    return v.y;
    //}

    // Vector 3 --------------------------------------------------------------------------------
    public static float r(this Vector3 v)
    {
        return v.x;
    }

    public static float g(this Vector3 v)
    {
        return v.y;
    }

    public static float b(this Vector3 v)
    {
        return v.z;
    }

    //public static Vector2 rr(this Vector3 v)
    //{
    //    return new Vector2(v.x, v.x);
    //}

    //public static Vector2 rg(this Vector3 v)
    //{
    //    return new Vector2(v.x, v.y);
    //}

    //public static Vector2 rb(this Vector3 v)
    //{
    //    return new Vector2(v.x, v.z);
    //}

    //public static Vector2 gr(this Vector3 v)
    //{
    //    return new Vector2(v.y, v.x);
    //}

    //public static Vector2 gg(this Vector3 v)
    //{
    //    return new Vector2(v.y, v.y);
    //}

    //public static Vector2 gb(this Vector3 v)
    //{
    //    return new Vector2(v.y, v.z);
    //}

    //public static Vector2 br(this Vector3 v)
    //{
    //    return new Vector2(v.z, v.x);
    //}

    //public static Vector2 bg(this Vector3 v)
    //{
    //    return new Vector2(v.z, v.y);
    //}

    //public static Vector2 bb(this Vector3 v)
    //{
    //    return new Vector2(v.z, v.z);
    //}

    public static Color rgb(this Vector3 v)
    {
        return new Color(v.x, v.y, v.z, 1);
    }

    // Vector 4 --------------------------------------------------------------------------------
    #endregion

    public static Color Normalize(this Color colour)
    {
        Vector3 normal = new Vector3(colour.r, colour.g, colour.b).normalized;

        return normal.rgb();
    }
}
