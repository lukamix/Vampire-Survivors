﻿using System.Collections.Generic;

public class Cluster
{
    public Boundaries Boundaries;
    public Dictionary<GridTile, Node> Nodes;

    public List<Cluster> Clusters;

    public int Width;
    public int Height;

    public Cluster()
    {
        Boundaries = new Boundaries();
        Nodes = new Dictionary<GridTile, Node>();
    }

    public bool Contains(Cluster other)
    {
        return other.Boundaries.Min.x >= Boundaries.Min.x &&
                other.Boundaries.Min.y >= Boundaries.Min.y &&
                other.Boundaries.Max.x <= Boundaries.Max.x &&
                other.Boundaries.Max.y <= Boundaries.Max.y;
    }

    public bool Contains(GridTile pos)
    {
        return pos.x >= Boundaries.Min.x &&
            pos.x <= Boundaries.Max.x &&
            pos.y >= Boundaries.Min.y &&
            pos.y <= Boundaries.Max.y;
    }

}

