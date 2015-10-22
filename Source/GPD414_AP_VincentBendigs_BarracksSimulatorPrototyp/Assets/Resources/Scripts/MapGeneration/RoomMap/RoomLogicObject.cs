using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DH.Messaging.Bus;

public class RoomLogicObject
{
    public Vector2 Position;
    public Vector2 Size;
    public RoomObjects RoomInfo;
    public List<ObjectLogicObject> Objects = new List<ObjectLogicObject>();
    public Soldiers[] workers;
    public int Workerscount;
    public Soldiers soldier;
    MessageSubscription<int> subscribtion;
    public KompanieObject kompanieObject;

    public RoomLogicObject ()
	{
        subscribtion = MessageBusManager.Subscribe<int>("ChangeHour");
        subscribtion.OnMessageReceived += changeMessage_OnMessageReceived;
	}

    private void changeMessage_OnMessageReceived(MessageSubscription<int> s, MessageReceivedEventArgs<int> args)
    {
        if(workers != null && workers.Length > 0)
        {
            MessageBusManager.AddMessage<RoomLogicObject>("FreeWorkPlace", this);
        }
    }

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
