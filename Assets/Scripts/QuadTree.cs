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

namespace Tree
{

    public class QuadTree<T>
    {
        int id = 0;
        int depth;
        QuadTreeNode<T> root;
        public QuadTreeNode<T> Root => root;

        Dictionary<T, QuadTreeNode<T>> nodes;
        Dictionary<int, T> datas;//数据 id 和 数据

        public QuadTree(Bounds bounds, int maxDeep = 31)
        {
            id = 0;
            depth = maxDeep;

            nodes = new Dictionary<T, QuadTreeNode<T>>();
            datas = new Dictionary<int, T>();

            root = new QuadTreeNode<T>(id++, 0, 0, bounds);
            CreateNodes(root, bounds);
        }


        //Insert node of bounds to the first QuadTreeNode contains it totally
        public void Insert(T data, Bounds bounds)
        {
            Insert(root, data, bounds);
        }

        //return all objects of T intersected with bounds
        public List<T> Search(Bounds bounds, bool accuracy = false)
        {
            List<T> list = new List<T>();
            Search(root, bounds, list, accuracy);
            return list;
        }

        public void Update(T data, Bounds bounds)
        {
            Remove(data);
            Insert(root, data, bounds);
        }


        public List<T> Traverse()
        {
            List<T> values = new List<T>();
            Traverse(root, values);
            return values;
        }


        internal void Insert(QuadTreeNode<T> root, T data, Bounds Bounds)
        {
            if (root.depth + 1 < depth)
            {
                if (!root.hasNode)
                {
                    CreateNodes(root, root.bounds);
                }

                for (int i = 0; i < 4; i++)
                {
                    QuadTreeNode<T> child = root.nodes[i];
                    if (child.bounds.Contains(Bounds))
                    {
                        Insert(child, data, Bounds);
                        return;
                    }
                }
            }
            //set bounds to the current root node if can not contained  by any child bounds
            //如果任何子节点都不能完全包含该Bounds，则加到上一层节点上
            QuadNodeInfo<T> nodeInfo = new QuadNodeInfo<T>(root.id, data, Bounds);
            root.datas.Add(nodeInfo);

            //datas[data] = nodeInfo;
            nodes[data] = root;
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
                        if (node.datas[i].bounds.Intersect(bounds))
                        {
                            list.Add(node.datas[i].data);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < dataCount; i++)
                    {
                        list.Add(node.datas[i].data);
                    }
                }

            }

            if (node.nodes == null)
            {
                return;
            }
            //recurse if intersect
            for (int i = 0; i < 4; i++)
            {
                QuadTreeNode<T> child = node.nodes[i];
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
                values.Add(node.datas[i].data);
            }

            if (node.nodes == null)
                return;

            for (int i = 0; i < node.nodes.Count; i++)
            {
                Traverse(node.nodes[i], values);
            }
        }

        internal void CreateNodes(QuadTreeNode<T> root, Bounds bounds)
        {
            Vector3[] center = new Vector3[4];

            Vector3 quadSize = bounds.size / 4;

            center[0] = bounds.min + quadSize * 3;

            center[1] = bounds.min + quadSize;
            center[1].z = center[1].z + 2 * quadSize.z;

            center[2] = bounds.min + quadSize;

            center[3] = bounds.min + quadSize;
            center[3].x = center[3].x + 2 * quadSize.x;

            Vector3 size = bounds.size / 2;

            root.nodes = new List<QuadTreeNode<T>>(4);
            for (int i = 0; i < 4; i++)
            {
                QuadTreeNode<T> node = new QuadTreeNode<T>(id++, root.id, root.depth + 1, new Bounds(center[i], size));
                root.nodes.Add(node);
            }
            root.hasNode = true;
        }


        internal void Remove(T data)
        {
            if (nodes.TryGetValue(data, out QuadTreeNode<T> node))
            {
                for (int i = 0; i < node.datas.Count; i++)
                {
                    if (node.datas[i].data.Equals(data))
                    {
                        node.datas.RemoveAt(i);
                        break;
                    }
                }
                //datas.Remove(data);
            }
        }
    }

}