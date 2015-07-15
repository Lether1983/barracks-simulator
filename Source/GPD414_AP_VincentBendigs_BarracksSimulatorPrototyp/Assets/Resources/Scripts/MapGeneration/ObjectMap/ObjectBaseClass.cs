using UnityEngine;
using System.Collections;
using System;

public enum UseableObjects { Bed, Toilette, Door, Shower, SportEquipment, Phone,TV,Food,SchoolDesk,Bench,Table,EssensAusgabe,WaescheKorb,Herd,Kuehltruhe,Tischkicker,Stuhl,Schreibtisch,Muelleimer,Schrank};

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
