using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class WorkManager : MonoBehaviour
{
    public List<WorkObjects> FinishedWorkObjects = new List<WorkObjects>();

    public List<WorkTask> WorkInProgress = new List<WorkTask>();
    LookUp<TypeOfWork, WorkTask> lookUp = new LookUp<TypeOfWork, WorkTask>();
    RoomManager manager;

    public void Awake()
    {
        manager = this.gameObject.GetComponent<RoomManager>();
    }

    public void CreateWork(TypeOfWork type, Vector2 StartPosition, Vector2 EndPosition, WorkObjects item, int targetUses)
    {
        WorkTask TempWork = new WorkTask(type, item, StartPosition, EndPosition, targetUses);
        lookUp.Add(type, TempWork);
    }

    public WorkTask GetWorkForMe(Soldiers soldiers)
    {
        foreach (var item in lookUp)
        {
            if ((soldiers.myJob.Types & item.Key) > 0)
            {
                WorkTask temptask = item.Value[0];

                lookUp.Remove(item.Key, temptask);

                return temptask;
            }
        }
        return null;
    }

    public List<WorkTask> GetWorkTasks(TypeOfWork type)
    {
        return lookUp.Values(type);
    }

    public List<WorkTask> GetWorkInProgress(TypeOfWork type)
    {
        List<WorkTask> tempList = new List<WorkTask>();
        foreach (var item in WorkInProgress)
        {
            if (item.type == type)
            {
                tempList.Add(item);
            }
        }
        return tempList;
    }

    public List<WorkObjects> GetTypicalWorkObjects(TypeofWorkObjects type)
    {
        List<WorkObjects> tempList = new List<WorkObjects>();
        foreach (var item in FinishedWorkObjects)
        {
            if (item.Type == type)
            {
                tempList.Add(item);
            }
        }
        return tempList;
    }

    public void AddToLookUp(TypeOfWork type, WorkTask task)
    {
        lookUp.Add(type, task);
    }

    public void CreateAdvancedWork(TypeOfWork type, WorkObjects Workitem)
    {
        if (type == TypeOfWork.Cooking)
        {
            WorkTask tempTask = new WorkTask(TypeOfWork.MovingFood, Workitem, Workitem.myObject.transform.position, GetTargetPosition(Workitem), 0);
            AddToLookUp(TypeOfWork.MovingFood, tempTask);
            FinishedWorkObjects.Remove(Workitem);
        }
        else if(type == TypeOfWork.MovingMaterial)
        {
            WorkTask tempTask = new WorkTask(TypeOfWork.TakeCloth, Workitem, Workitem.myObject.transform.position, Workitem.myObject.transform.position, 0);
            AddToLookUp(TypeOfWork.TakeCloth, tempTask);
            FinishedWorkObjects.Remove(Workitem);
        }
    }

    public Vector2 GetTargetPosition(WorkObjects WorkItem)
    {
        RoomLogicObject TempRoom;

        for (int j = 0; j < manager.everywhereRooms.Count; j++)
        {
            TempRoom = manager.everywhereRooms[j];
            if (TempRoom.type == TypeOfRoom.Kantine)
            {
                for (int i = 0; i < TempRoom.Objects.Count; i++)
                {
                    ObjectLogicObject tempObject = TempRoom.Objects[i];

                    if (tempObject.info.name == WorkItem.target.name)
                    {
                        for (int k = 0; k < tempObject.Storage.Length; k++)
                        {
                            if (tempObject.Storage[k] == null)
                            {
                                return new Vector3(tempObject.position.x + k, tempObject.position.y - 1, -2f);
                            }
                        }
                    }
                }
            }
            if (TempRoom.type == TypeOfRoom.Waeschekammer)
            {
                for (int i = 0; i < TempRoom.Objects.Count; i++)
                {
                    ObjectLogicObject tempObject2 = TempRoom.Objects[i];
                    if (tempObject2.info.name == WorkItem.target.name)
                    {
                        for (int l = 0; l < tempObject2.Storage.Length; l++)
                        {
                            if (tempObject2.Storage[l] == null)
                            {
                                return new Vector3(tempObject2.position.x + l, tempObject2.position.y - 1, -2f);
                            }
                        }
                    }
                }
            }
        }
        return Vector2.zero;
    }
}
