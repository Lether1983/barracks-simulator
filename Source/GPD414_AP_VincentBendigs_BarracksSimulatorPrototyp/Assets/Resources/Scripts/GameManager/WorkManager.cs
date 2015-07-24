using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class WorkManager : MonoBehaviour 
{
    public List<WorkObjects> FinishedWorkObjects = new List<WorkObjects>();

    public List<WorkTask> WorkInProgress = new List<WorkTask>();
    LookUp<TypeOfWork, WorkTask> lookUp = new LookUp<TypeOfWork, WorkTask>();


    public void CreateWork(TypeOfWork type, Vector2 StartPosition, Vector2 EndPosition, WorkObjects item,int targetUses)
    {
        WorkTask TempWork = new WorkTask(type, item, StartPosition, EndPosition,targetUses);
        lookUp.Add(type,TempWork);
    }

    public WorkTask GetWorkForMe(Soldiers soldiers)
    {
        foreach (var item in lookUp)
        {
            if((soldiers.myJob.Types & item.Key) > 0 )
            {
                WorkTask temptask = item.Value[0];

                lookUp.Remove(item.Key,temptask);

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
            if (item == null)
                Debug.Log("Bleurgh");
            if(item.type == type)
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
            if(item.Type == type)
            {
                tempList.Add(item);
            }
        }    
        return tempList;
    }

    public void AddToLookUp(TypeOfWork type,WorkTask task)
    {
        lookUp.Add(type, task);
    }

    public void CreateAdvancedWork(TypeOfWork type,WorkObjects item,TypeofWorkObjects objectType)
    {

    }
}
