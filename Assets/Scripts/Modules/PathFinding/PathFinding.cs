using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathFinding
{
    private static AStarNode Vector2ToNode(Vector2 point)
    {
        AStarNode node = new AStarNode(point);
        return node;
    }
    
    public static List<Vector2> FindPath(Vector2 startPoint, Vector2 endPoint, int[,] map)
    {
        int mapSizeX = map.GetLength(0);
        int mapSizeY = map.GetLength(1);
        List<AStarNode> closedNode = new List<AStarNode>();
        List<AStarNode> openNode = new List<AStarNode>();
        AStarNode startNode = Vector2ToNode(startPoint);
        startNode.g = 0;
        startNode.h = ManhattanDistance(startPoint, endPoint);
        startNode.f = startNode.g + startNode.h;
        startNode.parent = null;

        openNode.Add(startNode);
        AStarNode currentMinNode;
        while (openNode.Count > 0)
        {
            currentMinNode = openNode[0];
            foreach (var v in openNode)
            {
                if (v.f < currentMinNode.f || v.f == currentMinNode.f && v.h < currentMinNode.h)
                {
                    currentMinNode = v;
                }
            }
            openNode.Remove(currentMinNode);
            closedNode.Add(currentMinNode);
            if ((int)currentMinNode.position.x == (int)endPoint.x && (int)currentMinNode.position.y == (int)endPoint.y)
            {
                return RetracePath(currentMinNode);
            }
            List<AStarNode> neighbourNode = GetNeighbour(currentMinNode.position, mapSizeX, mapSizeY);
            foreach (var neighbour in neighbourNode)
            {
                Vector2 neighbourIndex = neighbour.position;
                if (map[(int)neighbourIndex.x, (int)neighbourIndex.y] == -1)
                {
                    continue;
                }
                else
                {
                    if (!closedNode.Contains(neighbour))
                    {
                        if (!openNode.Contains(neighbour))
                        {
                            neighbour.g = currentMinNode.g + 1;
                            neighbour.h = ManhattanDistance(neighbour.position, endPoint);
                            neighbour.f = neighbour.g + neighbour.h;
                            neighbour.parent = currentMinNode;
                            openNode.Add(neighbour);
                        }
                        else
                        {
                            if (currentMinNode.g + 1 + ManhattanDistance(neighbour.position, endPoint) < neighbour.f)
                            {
                                neighbour.g = currentMinNode.g + 1;
                                neighbour.h = ManhattanDistance(neighbour.position, endPoint);
                                neighbour.f = neighbour.g + neighbour.h;
                                neighbour.parent = currentMinNode;
                            }
                        }
                    }
                }
            }
            neighbourNode.Clear();
        }
        return null;
    }
    public static List<AStarNode> GetNeighbour(Vector2 node,int mapSizeX,int mapSizeY)
    {
        List<AStarNode> neighbourNode = new List<AStarNode>();
        Vector2 index = node;
        if (!(index.x <= 0))
        {
            neighbourNode.Add(new AStarNode(node + Vector2.left));
        }
        if (!(index.x >= mapSizeX - 1))
        {
            neighbourNode.Add(new AStarNode(node + Vector2.right));
        }
        if (!(index.y <= 0))
        {
            neighbourNode.Add(new AStarNode(node + Vector2.down));
        }
        if (!(index.y >= mapSizeY - 1))
        {
            neighbourNode.Add(new AStarNode(node + Vector2.up));
        }
        return neighbourNode;
    }
    public static float ManhattanDistance(Vector2 startPoint, Vector2 endPoint)
    {
        int baseDistance = (int)Mathf.Abs(startPoint.x - endPoint.x) + (int)Mathf.Abs(startPoint.y - endPoint.y);
        return baseDistance;
    }
    public static float EuclideDistance(Vector2 startPoint, Vector2 endPoint)
    {
        float baseDistance = (endPoint-startPoint).magnitude;
        return baseDistance;
    }
    private static List<Vector2> RetracePath(AStarNode end)
    {
        List<Vector2> path = new List<Vector2>();
        AStarNode currentNode = end;

        while (currentNode.parent != null)
        {
            path.Add(currentNode.position);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }
}

public class AStarNode
{
    public AStarNode(Vector2 position)
    {
        this.position = position;
    }
    public AStarNode(Vector2 position, float f, float g, float h, AStarNode parent)
    {
        this.position = position;
        this.f = f;
        this.g = g;
        this.h = h;
        this.parent = parent;
    }
    public Vector2 position;
    public float f = 0;
    public float g = 0;
    public float h = 0;
    public AStarNode parent;
}