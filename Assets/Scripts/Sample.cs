using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;
using Tree;

//[ExecuteInEditMode]
public class Sample : MonoBehaviour
{
    public int number = 1000;

    BoxCollider[] cubes;
    QuadTree<GameObject> quadTree;
    List<Color> colors = new List<Color> { Color.cyan, Color.black, Color.red, Color.yellow, Color.gray };


    void Start()
    {
        cubes = new BoxCollider[number];
        for (int i = 0; i < number; i++)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);


            Vector3 position = Random.insideUnitSphere * 100;
            go.transform.position = new Vector3(position.x, 0, position.z);
            cubes[i] = go.GetComponent<BoxCollider>();
        }

        for (int i = 0; i < number * 10; i++)
        {
            colors.Add(Random.ColorHSV());
        }

    }

    void Update()
    {
        if (cubes == null)
            return;


        Profiler.BeginSample("MinMax");
        Vector3 min = new Vector3(1000, 0, 1000);
        Vector3 max = new Vector3(-1000, 0, -1000);
        for (int i = 0; i < cubes.Length; i++)
        {
            min.x = min.x < cubes[i].bounds.min.x ? min.x : cubes[i].bounds.min.x;
            min.z = min.z < cubes[i].bounds.min.z ? min.z : cubes[i].bounds.min.z;
            max.x = max.x > cubes[i].bounds.max.x ? max.x : cubes[i].bounds.max.x;
            max.z = max.z > cubes[i].bounds.max.z ? max.z : cubes[i].bounds.max.z;
        }
        Profiler.EndSample();

        Vector3 center = (max + min) / 2;
        Vector3 size = max - min;
        Bounds bounds = new Bounds(center, size);
        quadTree = new QuadTree<GameObject>(bounds);
        Profiler.BeginSample("Insert");
        for (int i = 0; i < cubes.Length; i++)
        {
            quadTree.Insert(cubes[i].gameObject, cubes[i].bounds);
        }
        Profiler.EndSample();


        List<GameObject> list = quadTree.Search(cubes[0].bounds, true);

    }

    int count = 0;
    private void OnDrawGizmos()
    {
        count = 0;
        Gizmos.color = Color.green;

        if (quadTree != null)
        {
            Traverse(quadTree.Root);
        }
    }

    void Traverse(QuadTreeNode<GameObject> node)
    {
        if (node == null)
            return;
        //Debug.LogError(node.bounds.center);

        Vector3 min = node.bounds.min;
        Vector3 max = node.bounds.max;
        Gizmos.DrawLine(min, new Vector3(min.x, 0, max.z));
        Gizmos.DrawLine(min, new Vector3(max.x, 0, min.z));
        Gizmos.DrawLine(max, new Vector3(min.x, 0, max.z));
        Gizmos.DrawLine(max, new Vector3(max.x, 0, min.z));
        if (node.nodes == null)
            return;
        Gizmos.color = colors[count++];
        for (int i = 0; i < node.nodes.Count; i++)
        {
            Traverse(node.nodes[i]);
        }
    }
    public async void TraverseAsync(QuadTreeNode<GameObject> node)
    {
        if (node == null)
            return;

        Vector3 min = node.bounds.min;
        Vector3 max = node.bounds.max;
        Gizmos.DrawLine(min, new Vector3(min.x, 0, max.z));
        Gizmos.DrawLine(min, new Vector3(max.x, 0, min.z));
        Gizmos.DrawLine(max, new Vector3(min.x, 0, max.z));
        Gizmos.DrawLine(max, new Vector3(max.x, 0, min.z));
        Thread.Sleep(100);
        if (node.nodes == null)
            return;

        Gizmos.color = colors[count++];
        await Task.Run(() =>
        {
            for (int i = 0; i < node.nodes.Count; i++)
            {
                TraverseAsync(node.nodes[i]);
            }
        });
    }
}
