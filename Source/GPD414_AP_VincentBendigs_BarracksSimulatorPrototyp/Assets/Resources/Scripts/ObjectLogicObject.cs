using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectLogicObject
{
    public Vector2 position;
    public ObjectsObject info;
    public RoomLogicObject Room;
    public bool isInUse;
    private WorkObjects[] storage;

    public WorkObjects[] Storage
    {
        get
        {
            if (storage == null)
            {
                storage = new WorkObjects[info.storageSize];
            }
            return storage;
        }
    }
    public UseableObjects type
    {
        get
        {
            return info.type;
        }
    }

    public void Update(WorkManager manager)
    {
        if (info.name == "Herd")
        {
            //Gesamt Objects Uses
            int ObjectUses = manager.gameObject.GetComponent<GameManager>().AllSoldiers.Count * 3 - GetOverAllUses(manager);

            //Anzahl der Vollen Teller
            int FullPlateCount = ObjectUses / 10;
            // Uses auf dem Letzten Teller der nicht voll ist 
            int LastPlateUses = ObjectUses % 10;

            for (int i = 0; i < FullPlateCount; i++)
            {
                for (int j = 0; j < Storage.Length; j++)
                {
                    if (Storage[j] == null)
                    {
                        GameObject temp = GameObject.Instantiate(Resources.Load("Prefabs/New Sprite"), new Vector3(position.x + j, position.y, -2f), Quaternion.identity) as GameObject;
                        temp.name = "Essen";
                        temp.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Objects/Essen");
                        WorkObjects Food = ScriptableObject.Instantiate(Resources.Load<WorkObjects>("Prefabs/Scriptable Objects/ObjectObjects/WorkObjects/Essen"));
                        Storage[j] = Food;
                        Food.myObject = temp;

                        manager.CreateWork(TypeOfWork.Cooking, new Vector2(position.x + j, position.y - 1), new Vector2(position.x + j, position.y), Food, 10);
                    }
                }
            }
            if (LastPlateUses != 0)
            {
                for (int j = 0; j < Storage.Length; j++)
                {
                    if (Storage[j] == null)
                    {
                        GameObject temp = GameObject.Instantiate(Resources.Load("Prefabs/New Sprite"), new Vector3(position.x + j, position.y, -2f), Quaternion.identity) as GameObject;
                        temp.name = "Essen";
                        temp.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Objects/Essen");
                        WorkObjects Food = ScriptableObject.Instantiate(Resources.Load<WorkObjects>("Prefabs/Scriptable Objects/ObjectObjects/WorkObjects/Essen"));
                        Storage[j] = Food;
                        Food.myObject = temp;

                        manager.CreateWork(TypeOfWork.Cooking, new Vector2(position.x + j, position.y - 1), new Vector2(position.x + j, position.y), Food, LastPlateUses);
                        break;
                    }
                }
            }
        }

        if (info.name == "Wäschekorb")
        {
            for (int j = 0; j < Storage.Length; j++)
            {
                if (Storage[j] == null)
                {
                    GameObject temp = GameObject.Instantiate(Resources.Load("Prefabs/New Sprite"), new Vector3(position.x + j, position.y, -2f), Quaternion.identity) as GameObject;
                    temp.name = "Clothing";
                    temp.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Objects/Klamotten");
                    WorkObjects Kleidung = ScriptableObject.Instantiate(Resources.Load<WorkObjects>("Prefabs/Scriptable Objects/ObjectObjects/WorkObjects/Kleidung"));
                    Storage[j] = Kleidung;
                    Kleidung.myObject = temp;

                    manager.CreateWork(TypeOfWork.MovingMaterial, new Vector2(position.x + j, position.y-1), manager.GetTargetPosition(Kleidung), Kleidung, 1);
                }
            }
        }
    }

    private static int GetOverAllUses(WorkManager manager)
    {
        List<WorkTask> templist = manager.GetWorkTasks(TypeOfWork.Cooking);

        int tempUses = 0;

        foreach (var item in templist)
        {
            tempUses += item.targetUses;
        }

        List<WorkTask> tempInProgress = manager.GetWorkInProgress(TypeOfWork.Cooking);

        int tempIntInProgress = 0;

        foreach (var item in tempInProgress)
        {
            tempIntInProgress += item.targetUses;
        }

        List<WorkObjects> finishedItems = manager.GetTypicalWorkObjects(TypeofWorkObjects.Food);

        int tempfinishedUsed = 0;

        foreach (var item in finishedItems)
        {
            tempfinishedUsed += item.Uses;
        }
        return tempUses + tempIntInProgress + tempfinishedUsed;
    }
}
