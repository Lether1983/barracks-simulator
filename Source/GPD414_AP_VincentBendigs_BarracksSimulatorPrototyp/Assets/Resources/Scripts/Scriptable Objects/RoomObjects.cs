using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomObjects : ScriptableObject
{
    [SerializeField]
    List<ObjectsObject> roomObjectList;
    public KompanieObject kompanieObject;
    
    public Sprite texture;
}
