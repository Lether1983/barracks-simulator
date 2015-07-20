using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class WorkManager : MonoBehaviour 
{
    List<WorkTask> workDoTo;

    public void Start()
    {
        workDoTo = new List<WorkTask>();
    }

    public void CreateWork(TypeOfWork type, Vector2 StartPosition, Vector2 EndPosition, WorkObjects item)
    {
        WorkTask TempWork = new WorkTask(type, item, StartPosition, EndPosition);
        workDoTo.Add(TempWork);
    }

    public void DoWork()
    {

    }
}
