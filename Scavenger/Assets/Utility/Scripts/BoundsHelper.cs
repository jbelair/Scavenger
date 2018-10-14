using UnityEngine;

public static class BoundsHelper
{
    public static bool PointWithin(Vector2 point, Vector2 bounds)
    {
        return point.x >= 0 && point.y >= 0 && point.x <= bounds.x && point.y <= bounds.y;
    }
}
