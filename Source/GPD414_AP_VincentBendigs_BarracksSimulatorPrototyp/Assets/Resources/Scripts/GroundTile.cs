using UnityEngine;
using System.Collections;

public class GroundTile : TileBaseClass
{
    public GroundTile parentWaypoint;
    //private TileBaseClass[] neighbors;
    public float finalCost { get; set; }

    public GroundTile(int x, int y,Sprite texture)
    {
        this.Position.x = x;
        this.Position.y = y;
        this.Texture = texture;
    }
    public GroundTile(TileBaseClass copy)
    {
        this.Position = copy.Position;
        this.myObject = copy.myObject;
        this.IsIndoor = copy.IsIndoor;
    }

    //public override TileBaseClass[] GetNeighbors()
    //{
    //    return neighbors;
    //}

}
