using UnityEngine;

public static class ColorExtensions
{
    public static void A(this Color colour, float a)
    {
        colour = new Color(colour.r, colour.g, colour.b, a);
    }
}
