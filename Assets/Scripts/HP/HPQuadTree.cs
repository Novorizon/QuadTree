//#region << 版 本 注 释 >>
//// Copyright (C) 2022 Guan Jinbiao
//// 版权所有。 
////
//// 文件名：HPQuadTree
//// 版本：V1.0.0
//// 文件功能描述：
////
//// 
//// 创建者：Guan Jinbiao
//// 时间：2022-8-17
////
//// 修改人：
//// 时间：
//// 修改说明：
////

//#endregion

//using System.Collections.Generic;
//using Unity.Burst;
//using Unity.Collections;
//using Unity.Mathematics;

//[BurstCompile]
//public struct HPQuadTree
//{
//    static int id;
//    static int depth;

//    private HPQuadTreeNode root;
//    public HPQuadTreeNode Root => root;

//    static NativeHashMap<int, HPQuadNodeInfo> datas;
//    static NativeHashMap<int, HPQuadTreeNode> nodes;


//    public HPQuadTree(AABB bounds, int count, int maxDeep = 31)
//    {
//        id = 0;
//        datas = new NativeHashMap<int, HPQuadNodeInfo>(count, Allocator.Persistent);
//        nodes = new NativeHashMap<int, HPQuadTreeNode>(count, Allocator.Persistent);

//        depth = maxDeep;

//        root = new HPQuadTreeNode(id++);
//        CreateNodes(ref root, ref bounds);
//        nodes.Add(root.id, root);
//    }


//    //Insert node of bounds to the first HPQuadTreeNode contains it totally
//    [BurstCompile]
//    public void Insert(int id, AABB bounds)
//    {
//        Insert(ref root, id, bounds);
//    }

//    //return all objects of int intersected with bounds
//    public NativeList<int> Search(AABB bounds, bool accuracy = false)
//    {
//        NativeList<int> list = new NativeList<int>();
//        Search(root, bounds, ref list, accuracy);
//        return list;
//    }

//    public void Update(int data, AABB bounds)
//    {
//        Remove(data);
//        Insert(ref root, data, bounds);
//    }



//    [BurstCompile]
//    static internal void Insert(ref HPQuadTreeNode root, int data, AABB AABB)
//    {
//        if (root.depth + 1 < depth)
//        {
//            //Create child nodes
//            if (root.hasNode == 0)
//            {
//                CreateNodes(ref root, ref root.bounds);
//            }

//            for (int i = 0; i < 4; i++)
//            {
//                if (nodes.TryGetValue(root.nodes[i], out HPQuadTreeNode value))
//                {
//                    if (value.bounds.Contains(AABB))
//                    {
//                        //set bounds to the child node if overlapped  by child bounds
//                        Insert(ref value, data, AABB);
//                        return;
//                    }
//                }
//            }
//        }
//        //set bounds to the current root node if can not contained  by any child bounds
//        //如果任何子节点都不能完全包含该AABB，则加到上一层节点上
//        HPQuadNodeInfo nodeInfo = new HPQuadNodeInfo(root.id, data, AABB);
//        root.datas.Add(nodeInfo);

//        datas[data] = nodeInfo;
//        nodes[root.id] = root;
//    }


//    internal void Search(HPQuadTreeNode node, AABB bounds, ref NativeList<int> list, bool accuracy = false)
//    {
//        int Length = node.datas.Length;

//        if (accuracy)
//        {
//            for (int i = 0; i < Length; i++)
//            {
//                if (node.datas[i].bounds.Intersect(bounds))
//                {
//                    list.Add(node.datas[i].data);
//                }
//            }
//        }
//        else
//        {
//            for (int i = 0; i < Length; i++)
//            {
//                list.Add(node.datas[i].data);
//            }
//        }

//        if (node.nodes.Length == 0)
//        {
//            return;
//        }
//        //recurse if intersect
//        for (int i = 0; i < 4; i++)
//        {
//            if (nodes.TryGetValue(node.nodes[i], out HPQuadTreeNode value))
//            {
//                if (value.bounds.Intersect(bounds))
//                {

//                    Search(value, bounds, ref list, accuracy);
//                }
//            }
//        }
//    }


//    [BurstCompile]
//    static internal void CreateNodes(ref HPQuadTreeNode root, ref AABB bounds)
//    {
//        NativeArray<float2> centers = new NativeArray<float2>(4, Allocator.Temp);
//        float2 quadSize = bounds.Size / 4;
//        centers[0] = bounds.Min + quadSize * 3;
//        float2 center = bounds.Min + quadSize;
//        centers[1] = new float2(center.x, center.y + 2 * quadSize.y);
//        centers[2] = bounds.Min + quadSize;
//        center = bounds.Min + quadSize;
//        centers[3] = new float2(centers[3].x + 2 * quadSize.x, center.y);
//        float2 Size = bounds.Size / 2;

//        for (int i = 0; i < 4; i++)
//        {
//            Random random = new Random();
//            int id = random.NextInt();
//            HPQuadTreeNode node = new HPQuadTreeNode(id);
//            node.depth = root.depth + 1;
//            node.upperId = root.id;
//            node.bounds = new AABB(centers[i], Size);

//            root.nodes.Add(node.id);
//            nodes.Add(node.id, node);

//        }
//        root.hasNode = 1;
//    }


//    internal void Remove(int data)
//    {
//        if (nodes.TryGetValue(data, out HPQuadTreeNode node))
//        {
//            for (int i = 0; i < node.datas.Length; i++)
//            {
//                if (node.datas[i].data.Equals(data))
//                {
//                    node.datas.RemoveAt(i);
//                    break;
//                }
//            }
//            datas.Remove(data);
//        }
//    }

//    public void Dispose()
//    {
//        int count = nodes.Count();
//        for (int i = 0; i < count; i++)
//        {
//            HPQuadTreeNode node = nodes[i];
//            node.datas.Clear();
//            node.datas.Dispose();
//            node.nodes.Clear();
//            node.nodes.Dispose();
//        }
//        datas.Dispose();
//        nodes.Dispose();

//        id = 0;
//    }
//    internal void Dispose(HPQuadTreeNode node)
//    {

//    }
//}
