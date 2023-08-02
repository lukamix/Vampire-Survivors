using System;
using System.Collections.Generic;

public static class HierarchicalPathfinder
{
    public static LinkedList<Edge> FindPath(Graph graph, GridTile start, GridTile dest)
    {
        Node startNode, destinationNode;
        graph.InsertNodes(start, dest, out startNode, out destinationNode); // Thêm nốt vào đồ thị trừu tượng
        LinkedList<Edge> path;
        path = Pathfinder.FindPath(startNode, destinationNode); //Tìm kiếm đường đi
        graph.RemoveAddedNodes();
        return path; //Đường đi trả về là danh sách đường đi trừu tượng bao gồm các cạnh
    }
    //Từ đường đi trừu tượng chuyển thành đường đi cụ thể
    public static LinkedList<Edge> GetLayerPathFromHPA(LinkedList<Edge> hpa, int layer)
    {
        LinkedList<Edge> res = new LinkedList<Edge>();
        Queue<ValueTuple<int, Edge>> queue = new Queue<ValueTuple<int, Edge>>();
        foreach (Edge e in hpa)
            queue.Enqueue(new ValueTuple<int, Edge>(layer, e));

        ValueTuple<int, Edge> current;
        while (queue.Count > 0)
        {
            current = queue.Dequeue();

            if (current.Item1 == 0)
            {
                res.AddLast(current.Item2);
            }
            else if (current.Item2.type == EdgeType.INTER)
            {
                queue.Enqueue(new ValueTuple<int, Edge>(current.Item1 - 1, current.Item2));
            }
            else
            {
                foreach (Edge e in current.Item2.UnderlyingPath)
                    queue.Enqueue(new ValueTuple<int, Edge>(current.Item1 - 1, e));
            }
        }
        return res;
    }
}