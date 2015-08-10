using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomLogicObject
{
    public Vector2 Position;
    public Vector2 Size;
    public RoomObjects RoomInfo;
    public List<ObjectLogicObject> Objects = new List<ObjectLogicObject>();
    public Soldiers[] workers;
    public int Workerscount;
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
            if ((item.type & usableObject) > 0)
            {
                if(item.isInUse == false)
                {
                    return item;
                }
            }
        }
        return null;
    }
    public void GetWorkerSpaces()
    {
        workers = new Soldiers[(((int)Size.x * (int)Size.y) / 10)];
        Workerscount = workers.Length;
    }
}
