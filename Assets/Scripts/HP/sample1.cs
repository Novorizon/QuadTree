//using System.Collections;
//using System.Collections.Generic;
//using Unity.Burst;
//using Unity.Collections;
//using Unity.Jobs;
//using Unity.Mathematics;
//using UnityEngine;
//using UnityEngine.Profiling;
//using Random = UnityEngine.Random;

//public class sample1 : MonoBehaviour
//{
//    BoxCollider[] cubes;
//    AABB[] aabbs;
//    static HPQuadTree HPQuadTree;
//    Dictionary<int, GameObject> gameObjects;
//    List<Color> colors = new List<Color> { Color.cyan, Color.black, Color.red, Color.yellow, Color.gray };

//    Vector3 min = new Vector3(1000, 0, 1000);
//    Vector3 max = new Vector3(-1000, 0, -1000);

//    int count = 10000;
//    int id = 0;
//    void Start()
//    {
//        gameObjects = new Dictionary<int, GameObject>();
//        id = 0;
//        cubes = new BoxCollider[count];
//        aabbs = new AABB[count];
//        for (int i = 0; i < count; i++)
//        {
//            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);

//            go.transform.position = Random.insideUnitSphere * 1000;
//            cubes[i] = go.GetComponent<BoxCollider>();
//            aabbs[i].ReBuild(cubes[i].bounds);

//            min.x = min.x < cubes[i].bounds.min.x ? min.x : cubes[i].bounds.min.x;
//            min.z = min.z < cubes[i].bounds.min.z ? min.z : cubes[i].bounds.min.z;
//            max.x = max.x > cubes[i].bounds.max.x ? max.x : cubes[i].bounds.max.x;
//            max.z = max.z > cubes[i].bounds.max.z ? max.z : cubes[i].bounds.max.z;

//            gameObjects.Add(id++, go);
//        }


//        Vector3 center = (max + min) / 2;
//        Vector3 size = max - min;
//        float2 Center = new float2(center.x, center.z);
//        float2 Size = new float2(size.x, size.z);
//        AABB bounds = new AABB(Center, Size);
//        HPQuadTree = new HPQuadTree(bounds, count);
//        for (int i = 0; i < count; i++)
//        {
//            AABB aabb = new AABB();
//            aabb.ReBuild(cubes[i].bounds);
//            HPQuadTree.Insert(i, aabb);
//        }

//        for (int i = 0; i < 100; i++)
//        {
//            colors.Add(Random.ColorHSV());
//        }

//    }

//    void Update()
//    {
//        HPQuadTree.Dispose();
//        Vector3 center = (max + min) / 2;
//        Vector3 size = max - min;
//        float2 Center = new float2(center.x, center.z);
//        float2 Size = new float2(size.x, size.z);
//        AABB bounds = new AABB(Center, Size);
//        HPQuadTree = new HPQuadTree(bounds, count);

//        Profiler.BeginSample("ReBuild");
//        for (int i = 0; i < count; i++)
//        {
//            aabbs[i].ReBuild(cubes[i].bounds);
//        }
//        Profiler.EndSample();

//        Profiler.BeginSample("Insert");
//        for (int i = 0; i < count; i++)
//        {
//            HPQuadTree.Insert(i, aabbs[i]);
//        }
//        Profiler.EndSample();

//        //RunJob();
//    }

//    void RunJob()
//    {
//        BuildTreeJob job = new BuildTreeJob();
//        job.aabbs = new NativeArray<AABB>(count, Allocator.TempJob);
//        job.datas = new NativeArray<int>(count, Allocator.TempJob);
//        job.count = count;

//        for (int i = 0; i < count; i++)
//        {
//            aabbs[i].ReBuild(cubes[i].bounds);
//            job.aabbs[i] = aabbs[i];
//            job.datas[i] = i;
//        }
//        job.Execute();
//    }
//    [BurstCompile]
//    public struct BuildTreeJob : IJob
//    {
//        [ReadOnly]
//        public NativeArray<AABB> aabbs;
//        [ReadOnly]
//        public int count;
//        [ReadOnly]
//        public NativeArray<int> datas;

//        //public HPQuadTree HPQuadTree;

//        public void Execute()
//        {
//            for (int i = 0; i < aabbs.Length; i++)
//            {
//                HPQuadTree.Insert(datas[i], aabbs[i]);
//            }
//        }
//    }

//    private void OnDestroy()
//    {
//        HPQuadTree.Dispose();
//    }
//}
