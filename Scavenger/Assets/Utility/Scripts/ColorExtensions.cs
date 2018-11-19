using UnityEngine;

public static class ColorExtensions
{
    public static Vector3 Vector3(this Color colour)
    {
        return new Vector3(colour.r, colour.g, colour.b);
    }

    public static Vector4 Vector4(this Color colour)
    {
        return new Vector4(colour.r, colour.g, colour.b, colour.a);
    }

    public static Color A(this Color colour, float a)
    {
        return new Color(colour.r, colour.g, colour.b, a);
    }

    public static string Hexidecimal(this Color colour)
    {
        return ColorUtility.ToHtmlStringRGB(colour);
    }
}
