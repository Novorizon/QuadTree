
//using System.Collections.Generic;
//using Unity.Burst;
//using Unity.Collections;
//using Unity.Collections.LowLevel.Unsafe;

//[BurstCompile]
//public struct HPQuadNodeInfo
//{
//    public int nodeId;//所在的节点id
//    public int data;
//    public AABB bounds;

//    public HPQuadNodeInfo(int id, int t, AABB b)
//    {
//        nodeId = id;
//        data = t;
//        bounds = b;
//    }
//}
//[BurstCompile]
//public struct HPQuadTreeNode
//{
//    public int id;
//    public int depth;//深度
//    public int upperId;
//    public AABB bounds;//包围盒

//    public UnsafeList<HPQuadNodeInfo> datas;

//    //子节点
//    public UnsafeList<int> nodes;
//    public int hasNode;

//    public HPQuadTreeNode(int i)
//    {
//        id = i;
//        depth = 0;
//        upperId = 0;
//        hasNode = 0;
//        bounds = new AABB();

//        datas = new UnsafeList<HPQuadNodeInfo>(4,Allocator.Persistent);
//        nodes = new UnsafeList<int>(4, Allocator.Persistent);
//    }
//};