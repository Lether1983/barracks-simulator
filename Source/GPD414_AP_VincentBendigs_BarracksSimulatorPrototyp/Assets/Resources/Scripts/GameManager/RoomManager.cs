using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DH.Messaging.Bus;

public class RoomManager : MonoBehaviour
{
    #region RoomBools


    public bool MeIsAStube;

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
    }
    internal object GetRoomObjects(UseableObjects @object)
    {
        throw new System.NotImplementedException();
    }
}
