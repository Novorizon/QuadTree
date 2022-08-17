#region << 版 本 注 释 >>
// Copyright (C) 2022 Guan Jinbiao
// 版权所有。 
//
// 文件名：QuadTree
// 版本：V1.0.0
// 文件功能描述：
//
// 
// 创建者：Guan Jinbiao
// 时间：2022-8-17
//
// 修改人：
// 时间：
// 修改说明：
//

#endregion

using System.Collections.Generic;
using UnityEngine;

public class QuadTree<T>
{
    private int depth;
    private QuadTreeNode<T> root;
    public QuadTreeNode<T> Root => root;

    public QuadTree(Bounds Bounds, int maxDeep = 31)
    {
        depth = maxDeep;
        root = new QuadTreeNode<T>(Bounds);
    }


    //Insert node of bounds to the first QuadTreeNode contains it totally
    public void Insert(T data, Bounds Bounds)
    {
        Insert(root, data, Bounds, 1);
    }

    //return all objects of T intersected with bounds
    public void Search(Bounds bounds, List<T> list, bool accuracy = false)
    {
        Search(root, bounds, list, accuracy);
    }

    public List<T> Traverse()
    {
        List<T> values = new List<T>();
        Traverse(root, values);
        return values;
    }


    internal void Insert(QuadTreeNode<T> root, T data, Bounds Bounds, int deep)
    {
        Bounds pBounds = root.bounds;
        if (deep < depth)
        {
            //Create child nodes
            if (root.children == null)
            {
                root.children = new QuadTreeNode<T>[4];
                Vector3 quadSize = pBounds.size / 4;

                Vector3 center1 = pBounds.min + quadSize * 3;

                Vector3 center2 = pBounds.min + quadSize;
                center2.z = center2.z + 2 * quadSize.z;

                Vector3 center3 = pBounds.min + quadSize;

                Vector3 center4 = pBounds.min + quadSize;
                center4.x = center4.x + 2 * quadSize.x;

                Vector3 size = pBounds.size / 2;

                root.children[(int)Quadrant.First] = new QuadTreeNode<T>(new Bounds(center1, size));
                root.children[(int)Quadrant.Second] = new QuadTreeNode<T>(new Bounds(center2, size));
                root.children[(int)Quadrant.Third] = new QuadTreeNode<T>(new Bounds(center3, size));
                root.children[(int)Quadrant.Fourth] = new QuadTreeNode<T>(new Bounds(center4, size));
            }

            for (int i = 0; i < 4; i++)
            {
                QuadTreeNode<T> child = root.children[i];
                if (child.bounds.Contains(Bounds))
                {
                    //set bounds to the child node if overlapped  by child bounds
                    Insert(child, data, Bounds, deep + 1);
                    return;
                }
            }
        }
        //set bounds to the current root node if can not contained  by any child bounds
        root.datas.Add(data);
        root.boundsList.Add(Bounds);
    }

    internal void Search(QuadTreeNode<T> node, Bounds bounds, List<T> list, bool accuracy = false)
    {
        int dataCount = node.datas.Count;
        if (dataCount > 0)
        {
            if (accuracy)
            {
                for (int i = 0; i < dataCount; i++)
                {
                    if (node.boundsList[i].Intersect(bounds))
                    {
                        list.Add(node.datas[i]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < dataCount; i++)
                {
                    list.Add(node.datas[i]);
                }
            }

        }

        if (node.children == null)
        {
            return;
        }
        //recurse if intersect
        for (int i = 0; i < 4; i++)
        {
            QuadTreeNode<T> child = node.children[i];
            if (child.bounds.Intersect(bounds))
            {
                Search(child, bounds, list, accuracy);
            }
        }
    }

    internal void Traverse(QuadTreeNode<T> node, List<T> values)
    {
        if (node == null)
            return;

        for (int i = 0; i < node.datas.Count; i++)
        {
            values.Add(node.datas[i]);
        }

        if (node.children == null)
            return;

        for (int i = 0; i < node.children.Length; i++)
        {
            Traverse(node.children[i], values);
        }
    }
}
