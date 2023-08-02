using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    public int depth;
    public List<Cluster>[] C;

    public Dictionary<GridTile, Node> nodes;

    readonly int width;
    readonly int height;

    List<Node> addedNodes;

    public Graph(Map map, int depth, int clusterSize)
    {
        this.depth = depth;
        addedNodes = new List<Node>();
        nodes = CreateMapRepresentation(map);
        width = map.Width;
        height = map.Height;

        int clusterWidth, clusterHeight;

        C = new List<Cluster>[this.depth];

        for (int i = 0; i < this.depth; ++i)
        {
            if (i != 0)
                clusterSize = clusterSize*2;

            clusterHeight = Mathf.CeilToInt((float)map.Height / clusterSize);
            clusterWidth = Mathf.CeilToInt((float)map.Width / clusterSize);

            if (clusterWidth <= 1 && clusterHeight <= 1)
            {
                this.depth = i;
                break;
            }

            C[i] = BuildClusters(i, clusterSize, clusterWidth, clusterHeight);
        }
    }

    private Dictionary<GridTile, Node> CreateMapRepresentation(Map map)
    {
        Dictionary<GridTile, Node> mapnodes = new Dictionary<GridTile, Node>(map.FreeTiles);
        int i, j;
        GridTile gridTile;
        for (i = 0; i < map.Width; i++)
            for (j = 0; j < map.Height; j++)
            {
                if (!map.Obstacles[j][i])
                {
                    gridTile = new GridTile(i, j);
                    mapnodes.Add(gridTile, new Node(gridTile));
                }
            }
        foreach (Node n in mapnodes.Values)
        {
            SearchMapEdge(map, mapnodes, n, n.pos.x - 1, n.pos.y);
            SearchMapEdge(map, mapnodes, n, n.pos.x + 1, n.pos.y);
            SearchMapEdge(map, mapnodes, n, n.pos.x, n.pos.y - 1);
            SearchMapEdge(map, mapnodes, n, n.pos.x, n.pos.y + 1);
        }

        return mapnodes;
    }
    private void SearchMapEdge(Map map, Dictionary<GridTile, Node> mapNodes, Node n, int x, int y)
    {
        var weight = 1f;
        GridTile gridTile = new GridTile();

        gridTile.x = x;
        gridTile.y = y;
        if (!map.IsFreeTile(gridTile)) return;

        n.edges.Add(new Edge()
        {
            start = n,
            end = mapNodes[gridTile],
            type = EdgeType.INTER,
            weight = weight
        });
    }
    public void InsertNodes(GridTile start, GridTile dest, out Node nStart, out Node nDest)
    {
        Cluster cStart, cDest;
        Node newStart, newDest;
        nStart = nodes[start];
        nDest = nodes[dest];
        bool isConnected;
        addedNodes.Clear();

        for (int i = 0; i < depth; ++i)
        {
            cStart = null;
            cDest = null;
            isConnected = false;

            foreach (Cluster c in C[i])
            {
                if (c.Contains(start))
                    cStart = c;

                if (c.Contains(dest))
                    cDest = c;

                if (cStart != null && cDest != null)
                    break;
            }
            if (cStart == cDest)
            {
                newStart = new Node(start) { child = nStart };
                newDest = new Node(dest) { child = nDest };

                isConnected = ConnectNodes(newStart, newDest, cStart);

                if (isConnected)
                {
                    nStart = newStart;
                    nDest = newDest;
                }
            }

            if (!isConnected)
            {
                nStart = ConnectToBorder(start, cStart, nStart);
                nDest = ConnectToBorder(dest, cDest, nDest);
            }
        }
    }

    public void RemoveAddedNodes()
    {
        foreach(Node n in addedNodes)
            foreach(Edge e in n.edges)
                e.end.edges.RemoveAll((ee)=> ee.end == n);
    }

    private Node ConnectToBorder(GridTile pos, Cluster c, Node child)
    {
        Node newNode;
        if (c.Nodes.TryGetValue(pos, out newNode))
            return newNode;
        newNode = new Node(pos) { child = child };

        foreach (KeyValuePair<GridTile, Node> n in c.Nodes)
        {
            ConnectNodes(newNode, n.Value, c);
        }
        addedNodes.Add(newNode);

        return newNode;
    }
    private bool ConnectNodes(Node n1, Node n2, Cluster c)
    {
        LinkedList<Edge> path;
        LinkedListNode<Edge> iter;
        Edge e1, e2;

        float weight = 0f;

        path = Pathfinder.FindPath(n1.child, n2.child, c.Boundaries);

        if (path.Count > 0)
        {
            e1 = new Edge()
            {
                start = n1,
                end = n2,
                type = EdgeType.INTRA,
                UnderlyingPath = path
            };

            e2 = new Edge()
            {
                start = n2,
                end = n1,
                type = EdgeType.INTRA,
                UnderlyingPath = new LinkedList<Edge>()
            };
            iter = e1.UnderlyingPath.Last;
            while (iter != null)
            {
                var val = iter.Value.end.edges.Find(
                    e => e.start == iter.Value.end && e.end == iter.Value.start);

                e2.UnderlyingPath.AddLast(val);
                weight += val.weight;
                iter = iter.Previous;
            }
            e1.weight = weight;
            e2.weight = weight;

            n1.edges.Add(e1);
            n2.edges.Add(e2);

            return true;
        } else
        {
            return false;
        }
    }

    private delegate void CreateBorderNodes(Cluster c1, Cluster c2, bool x);

    private List<Cluster> BuildClusters(int level, int ClusterSize, int ClusterWidth, int ClusterHeight)
    {
        List<Cluster> clusters = new List<Cluster>();

        Cluster cluster;

        int i, j;

        for (i = 0; i < ClusterHeight; ++i)
            for (j = 0; j < ClusterWidth; ++j)
            {
                cluster = new Cluster();
                cluster.Boundaries.Min = new GridTile(j * ClusterSize, i * ClusterSize);
                cluster.Boundaries.Max = new GridTile(
                    Mathf.Min(cluster.Boundaries.Min.x + ClusterSize - 1, width - 1),
                    Mathf.Min(cluster.Boundaries.Min.y + ClusterSize - 1, height - 1));

                cluster.Width = cluster.Boundaries.Max.x - cluster.Boundaries.Min.x + 1;
                cluster.Height = cluster.Boundaries.Max.y - cluster.Boundaries.Min.y + 1;

                if (level > 0)
                {
                    cluster.Clusters = new List<Cluster>();

                    foreach (Cluster c in C[level - 1])
                        if (cluster.Contains(c))
                            cluster.Clusters.Add(c);
                }
                clusters.Add(cluster);
            }

        if (level == 0)
        {
            for (i = 0; i < clusters.Count; i++)
                for (j = i + 1; j < clusters.Count; j++)
                    DetectAdjacentClusters(clusters[i], clusters[j], CreateConcreteBorderNodes);

        } else
        {
            for (i = 0; i < clusters.Count; i++)
                for (j = i + 1; j < clusters.Count; j++)
                    DetectAdjacentClusters(clusters[i], clusters[j], CreateAbstractBorderNodes);
        }

        for (i = 0; i < clusters.Count; i++)
            GenerateIntraEdges(clusters[i]);

        return clusters;
    }

    private void DetectAdjacentClusters(Cluster c1, Cluster c2, CreateBorderNodes CreateBorderNodes)
    {
        if (c1.Boundaries.Min.x == c2.Boundaries.Min.x)
        {
            if (c1.Boundaries.Max.y + 1 == c2.Boundaries.Min.y) //dưới
                CreateBorderNodes(c1, c2, false);
            else if (c2.Boundaries.Max.y + 1 == c1.Boundaries.Min.y) //trên
                CreateBorderNodes(c2, c1, false);

        }
        else if (c1.Boundaries.Min.y == c2.Boundaries.Min.y)
        {
            if (c1.Boundaries.Max.x + 1 == c2.Boundaries.Min.x) //trái
                CreateBorderNodes(c1, c2, true);
            else if (c2.Boundaries.Max.x + 1 == c1.Boundaries.Min.x) //phải
                CreateBorderNodes(c2, c1, true);
        }
    }
    //entrance
    private void CreateConcreteBorderNodes(Cluster c1, Cluster c2, bool x)
    {
        int i;
        int iMin;
        int iMax;
        if (x)
        {
            iMin = c1.Boundaries.Min.y;
            iMax = iMin + c1.Height;
        } else
        {
            iMin = c1.Boundaries.Min.x;
            iMax = iMin + c1.Width;
        }

        int lineSize = 0;
        for (i = iMin; i < iMax; i++)
        {
            if (x && (nodes.ContainsKey(new GridTile(c1.Boundaries.Max.x, i)) && nodes.ContainsKey(new GridTile(c2.Boundaries.Min.x, i)))
                || !x && (nodes.ContainsKey(new GridTile(i, c1.Boundaries.Max.y)) && nodes.ContainsKey(new GridTile(i, c2.Boundaries.Min.y))))
            {
                lineSize++;
            } else {

                CreateConcreteInterEdges(c1, c2, x, ref lineSize, i);
            }
        }
        CreateConcreteInterEdges(c1, c2, x, ref lineSize, i);
    }

    private void CreateConcreteInterEdges(Cluster c1, Cluster c2, bool x, ref int lineSize, int i)
    {
        if (lineSize > 0)
        {
            if (lineSize <= 5)
            {
                CreateConcreteInterEdge(c1, c2, x, i - (lineSize / 2 + 1)); //lấy ô ở giữa
            }
            else
            {
                CreateConcreteInterEdge(c1, c2, x, i - lineSize); // 2 ô
                CreateConcreteInterEdge(c1, c2, x, i - 1);
            }

            lineSize = 0;
        }
    }

    private void CreateConcreteInterEdge(Cluster c1, Cluster c2, bool x, int i)
    {
        GridTile g1, g2;
        Node n1, n2;
        if (x)
        {
            g1 = new GridTile(c1.Boundaries.Max.x, i);
            g2 = new GridTile(c2.Boundaries.Min.x, i);
        }
        else
        {
            g1 = new GridTile(i, c1.Boundaries.Max.y);
            g2 = new GridTile(i, c2.Boundaries.Min.y);
        }

        if (!c1.Nodes.TryGetValue(g1, out n1))
        {
            n1 = new Node(g1);
            c1.Nodes.Add(g1, n1);
            n1.child = nodes[g1];
        }

        if (!c2.Nodes.TryGetValue(g2, out n2))
        {
            n2 = new Node(g2);
            c2.Nodes.Add(g2, n2);
            n2.child = nodes[g2];
        }

        n1.edges.Add(new Edge() { start = n1, end = n2, type = EdgeType.INTER, weight = 1 });
        n2.edges.Add(new Edge() { start = n2, end = n1, type = EdgeType.INTER, weight = 1 });
    }


    private void CreateAbstractBorderNodes(Cluster p1, Cluster p2, bool x)
    {
        foreach (Cluster c1 in p1.Clusters)
            foreach(Cluster c2 in p2.Clusters)
            {
                if ((x && c1.Boundaries.Min.y == c2.Boundaries.Min.y && c1.Boundaries.Max.x + 1 == c2.Boundaries.Min.x) || 
                    (!x && c1.Boundaries.Min.x == c2.Boundaries.Min.x && c1.Boundaries.Max.y + 1 == c2.Boundaries.Min.y))
                {
                    CreateAbstractInterEdges(p1, p2, c1, c2);
                }
            }
    }

    private void CreateAbstractInterEdges(Cluster p1, Cluster p2, Cluster c1, Cluster c2)
    {
        List<Edge> edges1 = new List<Edge>(),
            edges2 = new List<Edge>();
        Node n1, n2;

        foreach (KeyValuePair<GridTile, Node> n in c1.Nodes)
            foreach (Edge e in n.Value.edges)
            {
                if (e.type == EdgeType.INTER && c2.Contains(e.end.pos))
                    edges1.Add(e);
            }

        foreach(KeyValuePair<GridTile, Node> n in c2.Nodes)
            foreach (Edge e in n.Value.edges)
            {
                if (e.type == EdgeType.INTER && c1.Contains(e.end.pos))
                    edges2.Add(e);
            }

        foreach (Edge e1 in edges1)
            foreach (Edge e2 in edges2)
            {
                if (e1.end == e2.start)
                {
                    if (!p1.Nodes.TryGetValue(e1.start.pos, out n1))
                    {
                        n1 = new Node(e1.start.pos) { child = e1.start };
                        p1.Nodes.Add(n1.pos, n1);
                    }

                    if (!p2.Nodes.TryGetValue(e2.start.pos, out n2))
                    {
                        n2 = new Node(e2.start.pos) { child = e2.start };
                        p2.Nodes.Add(n2.pos, n2);
                    }

                    n1.edges.Add(new Edge() { start = n1, end = n2, type = EdgeType.INTER, weight = 1 });
                    n2.edges.Add(new Edge() { start = n2, end = n1, type = EdgeType.INTER, weight = 1 });

                    break;
                }
            }
    }
    //xây dựng các cạnh trong cluster - intraedge
    private void GenerateIntraEdges(Cluster c)
    {
        int i, j;
        Node n1, n2;
        var nodes = new List<Node>(c.Nodes.Values);
        
        for(i =0; i < nodes.Count; ++i)
        {
            n1 = nodes[i];
            for (j = i + 1; j < nodes.Count; ++j)
            {
                n2 = nodes[j];

                ConnectNodes(n1, n2, c);
            }
        }
    }
}