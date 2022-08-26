
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

[BurstCompile]
public struct AABB
{
    public float2 Center { get; private set; }
    public float2 BevelRadius { get; private set; }
    public float2 Size { get; private set; }

    public float2 Min { get; private set; }
    public float2 Max { get; private set; }

    [BurstCompile]
    public void Build(float2 min, float2 max)
    {
        Min = min;
        Max = max;

        Center = (Min + Max) / 2;
        Size = (Max - Min);
        BevelRadius = Size / 2;
    }

    public AABB(AABB a, AABB b)
    {
        Min = new float2(math.min(a.Min.x, b.Min.x), math.min(a.Min.y, b.Min.y));
        Max = new float2(math.max(a.Max.x, b.Max.x), math.max(a.Max.y, b.Max.y));

        Center = (Min + Max) / 2;
        Size = (Max - Min);
        BevelRadius = Size / 2;

    }


    public AABB(float2 center, float2 siye)
    {
        Center = center;
        Size = siye;
        BevelRadius = Size / 2;
        Min = Center - BevelRadius;
        Max = Center + BevelRadius;
    }


    [BurstCompile]
    public bool Contains(AABB aabb)
    {
        if (Min.x <= aabb.Min.x && Min.y <= aabb.Min.y && Max.x >= aabb.Max.x && Max.y >= aabb.Max.y)
            return true;
        //if (Min.x >= aabb.Min.x && Min.y >= aabb.Min.y && Max.x <= aabb.Max.x && Max.y <= aabb.Max.y)
        //    return true;
        return false;
    }

    [BurstCompile]
    public bool Intersect(AABB aabb)
    {
        if (Min.x <= aabb.Min.x && Min.y <= aabb.Min.y && Max.x >= aabb.Min.x && Max.y >= aabb.Min.y)
            return true;
        if (Min.x >= aabb.Min.x && Min.y >= aabb.Min.y && Min.x <= aabb.Max.x && Min.y <= aabb.Max.y)
            return true;
        return false;
    }

    [BurstCompile]
    public void ReBuild(Bounds bounds)
    {
        Center = new float2(bounds.center.x, bounds.center.z);
        Size = new float2(bounds.size.x, bounds.size.z);
        BevelRadius = Size / 2;
        Min = Center - BevelRadius;
        Max = Center + BevelRadius;
    }
}
