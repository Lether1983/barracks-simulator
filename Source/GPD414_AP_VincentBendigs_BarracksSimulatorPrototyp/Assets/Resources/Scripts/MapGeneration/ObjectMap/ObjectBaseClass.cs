using UnityEngine;
using System.Collections;
using System;


public class ObjectBaseClass : ICloneable
{
    public Vector2 Position;
    public Vector2 ParentPosition;
    public ObjectLogicObject @object;
    public Sprite Texture;
    public GameObject myObject;
    public ObjectsObject startobject;
    public bool isPassible;

    public System.Object Clone()
    {
        return this.MemberwiseClone();
    }
}
