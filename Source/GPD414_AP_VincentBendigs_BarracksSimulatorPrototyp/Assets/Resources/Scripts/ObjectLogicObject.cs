using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectLogicObject
{
    public Vector2 position;
    public ObjectsObject info;
    public RoomLogicObject Room;
    public bool isInUse;
    public GameObject storagePlace1;
    public GameObject storagePlace2;
    public GameObject storagePlace3;
    public GameObject storagePlace4;


    public UseableObjects type
    { 
        get
        {
            return info.type;
        } 
    }

    public void Update(WorkManager manager)
    {
        if(info.name == "Herd")
        {
           if(storagePlace1 == null)
           {
               GameObject temp = GameObject.Instantiate(Resources.Load("Prefabs/New Sprite"), new Vector3(position.x, position.y, -2f),Quaternion.identity) as GameObject;
               temp.name = "Essen";
               temp.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Objects/Essen");
               storagePlace1 = temp;
               WorkObjects Food = ScriptableObject.CreateInstance<WorkObjects>();
               Food.myObject = temp;

               manager.CreateWork(TypeOfWork.Cooking, new Vector2(position.x, position.y), new Vector2(position.x, position.y), Food);
           }
           else if(storagePlace2 == null)
           {
               GameObject temp = GameObject.Instantiate(Resources.Load("Prefabs/New Sprite"), new Vector3(position.x+1, position.y, -2f), Quaternion.identity) as GameObject;
               temp.name = "Essen";
               temp.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Objects/Essen");
               storagePlace2 = temp;
               WorkObjects Food = ScriptableObject.CreateInstance<WorkObjects>();
               Food.myObject = temp;

               manager.CreateWork(TypeOfWork.Cooking, new Vector2(position.x+1, position.y), new Vector2(position.x +1, position.y), Food);
           }
        }
    }
}
