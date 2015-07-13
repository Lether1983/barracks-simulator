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

//	internal ObjectLogicObject GetRoomObjects(UseableObjects usableObject)
//    {
//		foreach (var item in roomList)
//		{
//			if (item.type == usableObject)
//			{
//				return item;
//			}
//		}
//		return null;
//    }
}
