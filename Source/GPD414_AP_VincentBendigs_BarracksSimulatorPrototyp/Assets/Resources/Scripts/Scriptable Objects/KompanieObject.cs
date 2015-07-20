using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class KompanieObject :ScriptableObject
{
    [SerializeField]
    public List<RoomLogicObject> roomList;
    [SerializeField]
    public List<Soldiers> soldierList;

    public KompaniezugehoerigkeitsGruppen OberGruppe;
    public Truster truster;

    void OnEnable()
    {
        if (roomList == null)
        {
            roomList = new List<RoomLogicObject>();
        }
    }

    internal ObjectLogicObject GetRoomObjects(UseableObjects usableObject)
    {
        foreach (var item in roomList)
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
