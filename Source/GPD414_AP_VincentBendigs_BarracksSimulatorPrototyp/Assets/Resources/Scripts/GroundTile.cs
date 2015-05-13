using UnityEngine;
using System.Collections;

public class GroundTile : TileBaseClass
{
    public GroundTile parentWaypoint;
    //private TileBaseClass[] neighbors;
    public float finalCost { get; set; }

    public GroundTile(int x, int y)
    {
        this.Position.x = x;
        this.Position.y = y;
    }

    //public override TileBaseClass[] GetNeighbors()
    //{
    //    return neighbors;
    //}

}
