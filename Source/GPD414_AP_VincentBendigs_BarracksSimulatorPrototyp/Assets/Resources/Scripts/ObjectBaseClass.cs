using UnityEngine;
using System.Collections;
using System;

public class ObjectBaseClass : ICloneable
{
    public Vector2 Position;
    public Sprite Texture;
    public GameObject myObject;

    public System.Object Clone()
    {
        return this.MemberwiseClone();
    }
}
