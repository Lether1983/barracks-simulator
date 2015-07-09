using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


public class RoomBaseClass : ICloneable
{
    public GameObject myObject;
    public RoomBaseClass roomObject;
    public RoomLogicObject room;
    public Vector2 Position;
    public Sprite Texture;
    public Vector2 Size;
    public Vector2 roomStartValue;

    

    public System.Object Clone()
    {
        return this.MemberwiseClone();
    }

    
}
