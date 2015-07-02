using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class KompanieObject :ScriptableObject
{
    [SerializeField]
    public List<RoomObjects> roomList;
    [SerializeField]
    public List<Soldiers> soldierList;
    
    public Truster truster;

    internal bool GetRoomObjects(UseableObjects useableObjects)
    {
        throw new System.NotImplementedException();
    }
}
