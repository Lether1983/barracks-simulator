using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectLogicObject
{
    public Vector2 position;
    public ObjectsObject info;
    public RoomLogicObject Room;

    public UseableObjects type
    { 
        get
        {
         return info.type;
        } 
    }
}
