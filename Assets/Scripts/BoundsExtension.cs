using UnityEngine;
public static class BoundsExtension
{
    public static bool Contains(this Bounds a, Bounds b)
    {
        if (a.min.x <= b.min.x && a.min.z <= b.min.z && a.max.x >= b.max.x && a.max.z >= b.max.z)
            return true;
        if (a.min.x >= b.min.x && a.min.z >= b.min.z && a.max.x <= b.max.x && a.max.z <= b.max.z)
            return true;
        return false;
    }

    public static bool Intersect(this Bounds a, Bounds b)
    {
        if (a.min.x <= b.min.x && a.min.z <= b.min.z && a.max.x >= b.min.x && a.max.z >= b.min.z)
            return true;
        if (a.min.x >= b.min.x && a.min.z >= b.min.z && a.min.x <= b.max.x && a.min.z <= b.max.z)
            return true;
        return false;
    }
}