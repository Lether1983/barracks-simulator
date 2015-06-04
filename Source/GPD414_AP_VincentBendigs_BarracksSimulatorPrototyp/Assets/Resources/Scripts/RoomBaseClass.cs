using UnityEngine;
using System.Collections;
using System;

public class RoomBaseClass : ICloneable
{
    public GameObject myObject;
    public Vector2 Position;
    public Sprite Texture;
    public Vector3 ScaleValue;
    public bool IsInRoomRange;

    public System.Object Clone()
    {
        return this.MemberwiseClone();
    }
}
