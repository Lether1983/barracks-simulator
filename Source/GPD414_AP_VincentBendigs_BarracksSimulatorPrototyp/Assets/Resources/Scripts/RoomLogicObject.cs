using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomLogicObject
{
    public Vector2 Position;
    public Vector2 Size;
    public RoomObjects RoomInfo;
    public List<ObjectLogicObject> Objects = new List<ObjectLogicObject>();
    Soldiers soldier;

    public KompanieObject kompanieObject;

    public bool Claim(Soldiers soldier)
    {
        if(this.soldier != null)
        {
            return false;
        }
        else
        {
            this.soldier = soldier;
            return true;
        }
    }

    public TypeOfRoom type
    {
        get
        {
            return RoomInfo.type;
        }
    }
    public ObjectLogicObject GetRoomObjects(UseableObjects usableObject)
    {
        foreach (var item in Objects)
        {
            if (item.type == usableObject)
            {
                return item;
            }
        }
        return null;
    }
}
