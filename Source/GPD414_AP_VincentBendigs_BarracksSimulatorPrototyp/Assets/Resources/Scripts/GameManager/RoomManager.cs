using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DH.Messaging.Bus;

public class RoomManager : MonoBehaviour
{
    #region RoomBools

    #endregion

    public List<RoomLogicObject> everywhereRooms;
    // Use this for initialization
	void Start ()
    {
        everywhereRooms = new List<RoomLogicObject>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void addNewRoom(RoomLogicObject room)
    {
        everywhereRooms.Add(room);
        //TODO: Message mit "newroom" und dann den Raum reinhauen?
        // Liegt wahrscheinlich näher als für alles n neue Message rauszuhauen.
        if(room.type == TypeOfRoom.Stube)
        {
            MessageBusManager.AddMessage<RoomLogicObject>("freeStube", room);
        }
        //TODO: Du hast room.RoomInfo.RoomUsability was habitable, useable oder laborFactor ist.
        // Nutz das doch für die Entscheidung, welche Message geworfen wird.
        if(room.type == TypeOfRoom.Kueche)
        {
            MessageBusManager.AddMessage<RoomLogicObject>("FreeWorkPlace", room);
        }
        if(room.type == TypeOfRoom.Waeschekammer)
        {
            MessageBusManager.AddMessage<RoomLogicObject>("FreeWorkPlace", room);
        }
    }

    public void AssignRoomToCompany(KompanieObject company,RoomLogicObject room)
    {
        company.roomList.Add(room);
        room.kompanieObject = company;
        everywhereRooms.Remove(room);
    }

    public RoomLogicObject FindRoom(TypeOfRoom room)
    {
        foreach (var item in everywhereRooms)
        {
            if(item.type == room)
            {
                return item;
            }
        }
        return null;
    }
    public RoomLogicObject FindFreeRoom(TypeOfRoom room)
    {
        foreach (var item in everywhereRooms)
        {
            if(item.type == room)
            {
                if(item.soldier == null)
                {
                    return item;
                }
            }
        }
        return null;
    }

    internal ObjectLogicObject GetRoomObjects(UseableObjects usableObject)
    {
        foreach (var item in everywhereRooms)
        {
            ObjectLogicObject tempObject = item.GetRoomObjects(usableObject);

            if (tempObject != null)
            {
                return tempObject;
            }
        }
        return null;
    }
}
