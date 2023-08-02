using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private BoxCollider2D wallPrefab;
    [SerializeField] private HPAMap hpaMap;
    public int mapSizeX;
    public int mapSizeY;
    public float wallPrefabScaleX;
    public float wallPrefabScaleY;

    public string seed;
    public bool useRandomSeed;
    [Range(0, 50)]
    public int randomFillPercent;

    public int kingZoneSize;

    public int[,] map;

    private int wallValue = -1;

    public bool isCircleMap;
    public int circleRadius;
    public void GenerateMap()
    {
        map = new int[mapSizeX, mapSizeY];
        GenerateArrayMap(map);
        if (isCircleMap)
        {
            TurnToCircle(map);
        }
        GenerateRealMap(map);
        hpaMap.LoadMap(map);
    }

    private void GenerateArrayMap(int[,] map)
    {
        
        if (useRandomSeed)
        {
            seed = DateTime.Now.ToString();
        }
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeX; y++)
            {
                map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? wallValue : 0;
            }
        }
        SmoothMap(map);
    }
    public void ClearMap()
    {
        GameObject lastGO = GameObject.Find("Map");
        if (lastGO != null)
        {
            DestroyImmediate(lastGO);
        }
    }
    void GenerateRealMap(int[,] map)
    {
        ClearMap();
        GameObject mapGO = new GameObject("Map");
        wallPrefabScaleX = wallPrefab.transform.localScale.x;
        wallPrefabScaleY = wallPrefab.transform.localScale.y;
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                if (map[x, y] == wallValue)
                {
                    BoxCollider2D bc2d = Instantiate(wallPrefab, new Vector2(x * wallPrefabScaleX, y * wallPrefabScaleY), Quaternion.identity, mapGO.transform);
                }
            }
        }
    }
    void TurnToCircle(int[,] map)
    {
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                float distanceToMidPoint = Mathf.Sqrt(Mathf.Pow(x , 2) + Mathf.Pow(y , 2));
                if (distanceToMidPoint >= circleRadius)
                {
                    map[x, y] = 0;
                    if (Mathf.Abs(distanceToMidPoint - circleRadius) < 0.9f) map[x, y] = wallValue;
                }
            }
        }
    }
    void SmoothMap(int[,] map)
    {
        RemoveInsideObstacle(map);
        GenerateKingZone(map);
        SurroundMap(map);
        ConnectMap(map);
    }
    void GenerateKingZone(int[,] map)
    {
        for(int i = 0; i < kingZoneSize; i++)
        {
            for(int x = -i; x <= i; x++)
            {
                int maxY = (int)Mathf.Sqrt(i * i - x * x);
                for(int y=-maxY; y <= maxY; y++)
                {
                    map[x+(int)mapSizeX/2, y+(int)mapSizeY/2] = 0;
                }
            }
        }
    }
    void RemoveInsideObstacle(int[,] map)
    {
        for(int x = 1; x < mapSizeX - 1; x++)
        {
            for(int y = 1; y < mapSizeY - 1; y++)
            {
                if (GetCountNeighbourObstacle(x, y, map)>=3)
                {
                    map[x, y] = 0;
                }
            }
        }
    }
    int GetCountNeighbourObstacle(int x,int y, int[,] map)
    {
        return -(map[x - 1, y] + map[x + 1, y]+ map[x,y-1]+map[x,y+1]);
    }
    void SurroundMap(int[,] map)
    {
        for (int x = 0; x < mapSizeX; x++)
        {
            map[x, 0] = wallValue;
            map[x, mapSizeY-1] = wallValue;
        }
        for (int y = 0; y < mapSizeY; y++)
        {
            map[0, y] = wallValue;
            map[mapSizeX-1,y] = wallValue;
        }
    }

    void ConnectMap(int[,] map)
    {
        InitTileMapAndClusterMap(map);
        /*GetBreakList();
        Break(map);*/
    }

    TileMap[,] groundGridTiles;
    List<ClusterMap> clusterList = new List<ClusterMap>();
    void InitTileMapAndClusterMap(int[,] map)
    {
        groundGridTiles = new TileMap[mapSizeX, mapSizeY];
        for (int i = 0; i < mapSizeX; i++)
        {
            for (int j = 0; j < mapSizeY; j++)
            {
                groundGridTiles[i, j] = new TileMap(i, j, map[i, j]);
                if (map[i, j] == 0)
                {
                    if (i > 0 && map[i - 1, j] == 0 && groundGridTiles[i - 1, j] != null && groundGridTiles[i - 1, j].parent != null)
                    {
                        ClusterMap cluster = groundGridTiles[i - 1, j].parent;
                        groundGridTiles[i, j].parent = cluster;
                        cluster.tiles.Add(groundGridTiles[i, j]);
                    }
                    else if (i < mapSizeX - 1 && map[i + 1, j] == 0 && groundGridTiles[i + 1, j] != null && groundGridTiles[i + 1, j].parent != null)
                    {
                        ClusterMap cluster = groundGridTiles[i + 1, j].parent;
                        groundGridTiles[i, j].parent = cluster;
                        cluster.tiles.Add(groundGridTiles[i, j]);
                    }
                    else if (j > 0 && map[i, j - 1] == 0 && groundGridTiles[i, j - 1] != null && groundGridTiles[i, j - 1].parent != null)
                    {
                        ClusterMap cluster = groundGridTiles[i, j - 1].parent;
                        groundGridTiles[i, j].parent = cluster;
                        cluster.tiles.Add(groundGridTiles[i, j]);
                    }
                    else if (j < mapSizeY - 1 && map[i, j + 1] == 0 && groundGridTiles[i, j + 1] != null && groundGridTiles[i, j + 1].parent != null)
                    {
                        ClusterMap cluster = groundGridTiles[i, j - 1].parent;
                        groundGridTiles[i, j].parent = cluster;
                        cluster.tiles.Add(groundGridTiles[i, j]);
                    }
                    else
                    {
                        ClusterMap cluster = new ClusterMap();
                        cluster.id = $"{clusterList.Count}";
                        clusterList.Add(cluster);
                        cluster.tiles.Add(groundGridTiles[i, j]);
                        groundGridTiles[i, j].parent = cluster;
                    }
                }
            }
        }
        for (int i=0;i<mapSizeX;i++ ) {

            for(int j=0;j<mapSizeY;j++)
            {
                if (map[i, j] == wallValue)
                {
                    ConnectNearbyCluster(i, j, map);
                }
            }
        }
    }
    void ConnectNearbyCluster(int x,int y, int[,] map)
    {
        List<ClusterMap> clusters = new List<ClusterMap>();
        if (x > 0 && map[x - 1, y] == 0)
        {
            if(!clusters.Contains(groundGridTiles[x - 1, y].parent) && groundGridTiles[x - 1, y].parent!=null)
            clusters.Add(groundGridTiles[x - 1, y].parent);
        }
        if (x < mapSizeX - 1 && map[x + 1, y] == 0)
        {
            if (!clusters.Contains(groundGridTiles[x + 1, y].parent) && groundGridTiles[x + 1, y].parent != null)
                clusters.Add(groundGridTiles[x + 1, y].parent);
        }
        if (y > 0 && map[x, y - 1] == 0)
        {
            if (!clusters.Contains(groundGridTiles[x, y - 1].parent) && groundGridTiles[x, y - 1].parent != null)
                clusters.Add(groundGridTiles[x, y - 1].parent);
        }
        if (y < mapSizeY - 1 && map[x, y + 1] == 0)
        {
            if (!clusters.Contains(groundGridTiles[x, y + 1].parent) && groundGridTiles[x, y + 1].parent != null)
                clusters.Add(groundGridTiles[x, y + 1].parent);
        }
        if (clusters.Count > 1)
        {
            map[x, y] = 0;
            for (int i = 1; i < clusters.Count; i++)
            {
                MergeCluster(clusters[0], clusters[i]);
            }
        }

    }
    void MergeCluster(ClusterMap a, ClusterMap b)
    {
        for (int i = 0; i < b.tiles.Count; i++)
        {
            b.tiles[i].parent = a;
        }
        a.tiles.AddRange(b.tiles);
        b.tiles.Clear();
        clusterList.Remove(b);
    }
    /*void GetBreakList()
    {
        foreach (ClusterMap cluster in clusterList)
        {
            cluster.tiles.Sort((x, y) => x.x.CompareTo(y.x));
            if (cluster.tiles[0].x > 1)
            {
                if (!breakList.Contains(groundGridTiles[cluster.tiles[0].x - 1, cluster.tiles[0].y]))
                    breakList.Add(groundGridTiles[cluster.tiles[0].x - 1, cluster.tiles[0].y]);
            }
            cluster.tiles.Sort((x, y) => x.y.CompareTo(y.y));
            if (cluster.tiles[0].y > 1)
            {
                if (!breakList.Contains(groundGridTiles[cluster.tiles[0].x, cluster.tiles[0].y - 1]))
                    breakList.Add(groundGridTiles[cluster.tiles[0].x, cluster.tiles[0].y - 1]);
            }
        }
    }
    void Break(int[,] map)
    {
        foreach (TileMap tile in breakList)
        {
            map[tile.x, tile.y] = 0;
        }
    }*/
}
public class ClusterMap
{
    public string id;
    public List<TileMap> tiles = new List<TileMap>();
}
public class TileMap
{
    public TileMap(int x, int y, int value)
    {
        this.x = x; this.y = y; this.value = value;
    }
    public int x;
    public int y;
    public int value;
    public TileMap connectedTile;
    public ClusterMap parent;
}