using UnityEngine;
using System.Collections;
using System;

public enum UseableObjects { Bed, Toilette, Door, Shower, SportEquipment, Phone,TV,Food,SchoolDesk};

public class ObjectBaseClass : ICloneable
{
    public Vector2 Position;
    public Vector2 ParentPosition;
    public Sprite Texture;
    public GameObject myObject;
    public ObjectsObject startobject;
    public bool isPassible;

    public System.Object Clone()
    {
        return this.MemberwiseClone();
    }
}
