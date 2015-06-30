using UnityEngine;
using System.Collections;
using System;



public class RoomBaseClass : ICloneable
{
    public GameObject myObject;
    public RoomBaseClass roomObject;
    public Vector2 Position;
    public Sprite Texture;
    public Vector2 Size;
    public Vector2 roomStartValue;

    public System.Object Clone()
    {
        return this.MemberwiseClone();
    }
}
