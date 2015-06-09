using UnityEngine;
using System.Collections;

public class WallTile : TileBaseClass 
{
    public float Duarbility;


    public WallTile(TileBaseClass copy)
    {
        this.Position = copy.Position;
        this.myObject = copy.myObject;
        this.IsIndoor = copy.IsIndoor;
    }
}
