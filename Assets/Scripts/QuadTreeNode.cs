
using System.Collections.Generic;
using UnityEngine;

public enum Quadrant
{
    First = 0,
    Second = 1,
    Third = 2,
    Fourth = 3,
}

public class QuadTreeNode<T>
{
    public List<T> datas;
    public List<Bounds> boundsList;
    public Bounds bounds;
    public QuadTreeNode<T>[] children;

    public QuadTreeNode(Bounds bounds)
    {
        children = null;
        datas = new List<T>();
        boundsList = new List<Bounds>();
        this.bounds = bounds;
    }
};