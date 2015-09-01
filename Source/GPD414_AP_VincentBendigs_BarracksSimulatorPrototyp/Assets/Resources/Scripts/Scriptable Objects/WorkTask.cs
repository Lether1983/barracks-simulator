using UnityEngine;
using System.Collections;

public class WorkTask 
{
    public TypeOfWork type;
    public WorkObjects Item;
    public Vector2 StartPosition;
    public Vector2 EndPosition;
    public int targetUses;

    public WorkTask(TypeOfWork Type,WorkObjects item,Vector2 start,Vector2 end,int targetUses)
    {
        this.type = Type;
        this.Item = item;
        this.StartPosition = start;
        this.EndPosition = end;
        this.targetUses = targetUses;
    }
    
}
