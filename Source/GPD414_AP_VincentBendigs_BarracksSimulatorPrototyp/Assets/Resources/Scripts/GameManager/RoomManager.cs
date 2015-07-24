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

        if(room.type == TypeOfRoom.Stube)
        {
            MessageBusManager.AddMessage<RoomLogicObject>("freeStube", room);
        }
        if(room.type == TypeOfRoom.Kueche)
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
