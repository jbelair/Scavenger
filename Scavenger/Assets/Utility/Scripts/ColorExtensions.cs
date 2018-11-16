using UnityEngine;

public static class ColorExtensions
{
    public static Color A(this Color colour, float a)
    {
        return new Color(colour.r, colour.g, colour.b, a);
    }

    public static string Hexidecimal(this Color colour)
    {
        return ColorUtility.ToHtmlStringRGB(colour);
    }
}
