using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Map
{
    public int Width { get; set; }
    public int Height { get; set; }

    public Boundaries Boundaries { get; set; }

    public int FreeTiles { get; set; }

    public bool[][] Obstacles { get; set; }

    public int[][] Tiles { get; set; }


    private Map() {}

    public bool IsFreeTile(GridTile tile)
    {
        return tile.x >= 0 && tile.x < Width &&
            tile.y >= 0 && tile.y < Height &&
            !Obstacles[tile.y][tile.x];
    }
    public static Map LoadMap(int[,] mapArray)
    {
        Map map = new Map();
        map.Height = mapArray.GetLength(0);
        map.Width = mapArray.GetLength(1);
        map.Boundaries = new Boundaries
        {
            Min = new GridTile(0, 0),
            Max = new GridTile(map.Width - 1, map.Height - 1)
        };
        map.Obstacles = new bool[map.Height][];
        map.FreeTiles = 0;
        map.Tiles = new int[map.Height][];
        for (int i = 0; i < map.Height; i++)
        {
            map.Obstacles[i] = new bool[map.Width];
            map.Tiles[i] = new int[map.Width];

            for (int j = 0; j < map.Width; j++)
            {
                map.Tiles[i][j] = mapArray[i, j];

                switch (mapArray[i, j])
                {
                    case -1:
                        map.Obstacles[i][j] = true;
                        break;
                    case 0:
                        map.Obstacles[i][j] = false;
                        map.FreeTiles++;
                        break;
                    default:
                        throw new Exception("Integer not recognized");
                }
            }
        }
        return map;
    }
}
