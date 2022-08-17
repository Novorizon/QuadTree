using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

//[ExecuteInEditMode]
public class Sample : MonoBehaviour
{
    QuadTree<GameObject> quadTree;
  List<  Color> colors = new List<Color> { Color.cyan, Color.black, Color.red, Color.yellow, Color.gray };

    Vector3 min = new Vector3(1000, 0, 1000);
    Vector3 max = new Vector3(-1000, 0, -1000);
    void Start()
    {

        BoxCollider[] cubes = FindObjectsOfType<BoxCollider>();
        if (cubes != null)
        {
            for (int i = 0; i < cubes.Length; i++)
            {
                min.x = min.x < cubes[i].bounds.min.x ? min.x : cubes[i].bounds.min.x;
                min.z = min.z < cubes[i].bounds.min.z ? min.z : cubes[i].bounds.min.z;
                max.x = max.x > cubes[i].bounds.max.x ? max.x : cubes[i].bounds.max.x;
                max.z = max.z > cubes[i].bounds.max.z ? max.z : cubes[i].bounds.max.z;
            }
            Vector3 center = (max + min) / 2;
            Vector3 size = max - min;
            Bounds bounds = new Bounds(center, size);
            quadTree = new QuadTree<GameObject>(bounds);
            for (int i = 0; i < cubes.Length; i++)
            {
                quadTree.Insert(cubes[i].gameObject, cubes[i].bounds);
            }

        }
        for (int i = 0; i < 100; i++)
        {
            colors.Add(Random.ColorHSV());
        }

        List<GameObject> list = new List<GameObject>();
        quadTree.Search(quadTree.Root.bounds, list);
    }

    void Update()
    {

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
        if (node.children == null)
            return;
        Gizmos.color = colors[count++];
        for (int i = 0; i < node.children.Length; i++)
        {
            Traverse(node.children[i]);
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
        if (node.children == null)
            return;

        Gizmos.color = colors[count++];
        await Task.Run(() =>
        {
            for (int i = 0; i < node.children.Length; i++)
            {
                TraverseAsync(node.children[i]);
            }
        });
    }
}
