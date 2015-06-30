using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomObjects : ScriptableObject
{
    [SerializeField]
    List<ObjectsObject> roomObjectList;
    public KompanieObject kompanieObject;
    
    public Sprite defaultTexture;

    public SpriteInfo[] infos;


    public bool GetRoomObjects(UseableObjects usableObject)
    {
        foreach (var item in roomObjectList)
        {
            if(item.type == usableObject)
            {
                return true;
            }
        }
        return false;
    }
}
