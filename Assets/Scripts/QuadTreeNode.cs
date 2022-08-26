
using System.Collections.Generic;
using UnityEngine;

namespace Tree
{
    public class QuadNodeInfo<T>
    {
        public int nodeId;//所在的节点id
        public T data;
        public Bounds bounds;

        public QuadNodeInfo(int id, T t, Bounds b)
        {
            nodeId = id;
            data = t;
            bounds = b;
        }
    }
    public class QuadTreeNode<T>
    {
        public int id;
        public int upperId;
        public int depth;
        public Bounds bounds;

        public List<QuadTreeNode<T>> nodes;
        public List<QuadNodeInfo<T>> datas;

        public bool hasNode = false;

        public QuadTreeNode(int id, int upperId, int depth, Bounds bounds)
        {
            this.id = id;
            this.upperId = upperId;
            this.depth = depth;
            this.bounds = bounds;

            datas = new List<QuadNodeInfo<T>>();
            nodes = null;
        }
    }
}