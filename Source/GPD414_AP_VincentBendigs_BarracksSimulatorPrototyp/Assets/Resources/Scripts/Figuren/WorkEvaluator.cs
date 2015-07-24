using UnityEngine;
using System.Collections;

public class WorkEvaluator : MonoBehaviour
{
    [SerializeField]
    Soldiers me;
    [SerializeField]
    Animator animator;
    TileMap map = TileMap.Instance();

    public void GetEvaluationForWork()
    {
        if (me.myJob == null) return;
        if(me.currentTask == null)
        {
            me.currentTask = me.workManager.GetWorkForMe(me);
        }
        if(me.currentTask != null)
        {
            me.GoTo((GroundTile)map.MapData[(int)me.currentTask.StartPosition.x, (int)me.currentTask.StartPosition.y]);
            me.workManager.WorkInProgress.Add(me.currentTask);
            animator.SetBool("DoWork",true);
        }
        else
        {
            animator.SetBool("DoWork", false);
        }
    }
}