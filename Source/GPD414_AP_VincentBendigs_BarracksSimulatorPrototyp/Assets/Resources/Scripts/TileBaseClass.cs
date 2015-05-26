using UnityEngine;
using System.Collections;
using System;

public class TileBaseClass : ICloneable
{
    public bool IsPassible;
    public Vector2 Position;
    public GameObject myObject;
    public int movementCost;
    public Sprite Texture;


    public virtual TileBaseClass[] GetNeighbors()
    {
        return null;
    }

    public System.Object Clone()
    {
        return this.MemberwiseClone();
    }
}
