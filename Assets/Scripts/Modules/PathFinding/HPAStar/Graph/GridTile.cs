using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Diagnostics;

[Serializable()]
[DebuggerDisplay("({x}, {y})")]
public class GridTile {
    public int x;
    public int y;
    public GridTile() { }

    public GridTile(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public GridTile(Vector3 position)
    {
        SetGridTile(position);
    }
    public void SetGridTile(Vector3 position)
    {
        x = Mathf.FloorToInt(position.x);
        y = Mathf.FloorToInt(position.y);
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        GridTile other = obj as GridTile;
        return x == other.x && y == other.y;
    }
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 29 + x.GetHashCode();
            hash = hash * 29 + y.GetHashCode();
            return hash;
        }
    }

    public static bool operator !=(GridTile o1, GridTile o2)
    {
        if (ReferenceEquals(o1, null)) return !ReferenceEquals(o2, null);
        else return !o1.Equals(o2);
    }

    public static bool operator ==(GridTile o1, GridTile o2)
    {
        if (ReferenceEquals(o1, null)) return ReferenceEquals(o2, null);
        else return o1.Equals(o2);
    }

}
