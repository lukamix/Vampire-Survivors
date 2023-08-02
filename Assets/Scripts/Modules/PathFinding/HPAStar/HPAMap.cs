using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;


public class HPAMap : MonoBehaviour
{
    public int clusterSize = 10;
    public int layers = 1;
    private Map map;
    private Graph graph;
    public void OnLoadMap(int[,] mapArray)
    {
        map = Map.LoadMap(mapArray);
        graph = new Graph(map, layers, clusterSize);
    }

    public LinkedList<Edge> GetPath(GridTile start, GridTile dest)
    {
        LinkedList<Edge> path = HierarchicalPathfinder.FindPath(graph, start, dest);
        return HierarchicalPathfinder.GetLayerPathFromHPA(path, layers);
    }

    public void LoadMap(int[,] mapArray)
    {
        Clear();
        OnLoadMap(mapArray);
    }
    private void Clear()
    {
        graph = null;
        map = null;
        GC.Collect();
    }
}
