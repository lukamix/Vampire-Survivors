using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPathObject : MonoBehaviour
{
    private void Start()
    {
        int[,] map = { { -1, 0 },{ 0, 0 } };
        /*List<Vector2> path = PathFinding.FindPath(new Vector2(-10, -10), new Vector2(-9, -9), map);
        Debug.Log(path);*/
    }
}
