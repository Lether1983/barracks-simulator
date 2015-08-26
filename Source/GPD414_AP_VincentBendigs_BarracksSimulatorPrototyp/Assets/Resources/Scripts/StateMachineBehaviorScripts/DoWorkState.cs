using UnityEngine;
using System.Collections;

public class DoWorkState : StateMachineBehaviour
{

    Soldiers me;
    WorkEvaluator evaluator;
    TileMap map = TileMap.Instance;
    ObjectTileMap ObjectMap = ObjectTileMap.Instance;
    float elapsedTime;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (me == null)
        {
            me = animator.gameObject.GetComponent<Soldiers>();
        }
        if (evaluator == null)
        {
            evaluator = animator.gameObject.GetComponent<WorkEvaluator>();
        }
        elapsedTime = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (me.currentTask == null)
        {
            animator.SetBool("DoWork", false);
            return;
        }
        if (me.currentTask.type == TypeOfWork.Cooking)
        {
            elapsedTime += 10 / 3600f * Time.deltaTime * me.manager.speed;
            if ((Vector2)me.transform.position == me.currentTask.StartPosition)
            {
                if (me.currentTask.Item.Uses < me.currentTask.targetUses)
                {
                    if (elapsedTime >= 1)
                    {
                        elapsedTime--;
                        me.currentTask.Item.Uses++;
                    }
                }
                else
                {
                    me.workManager.WorkInProgress.Remove(me.currentTask);
                    me.workManager.FinishedWorkObjects.Add(me.currentTask.Item);
                    me.workManager.CreateAdvancedWork(me.currentTask.type, me.currentTask.Item);
                    me.currentTask = null;
                    animator.SetBool("DoWork", false);
                }
            }
        }
        else if (me.currentTask.type == TypeOfWork.MovingFood)
        {
            MoveItemFromStartToEnd(animator);
        }
        else if(me.currentTask.type == TypeOfWork.MovingMaterial)
        {
            MoveItemFromStartToEnd(animator);
        }
    }

    private void MoveItemFromStartToEnd(Animator animator)
    {
        if ((Vector2)me.transform.position == me.currentTask.StartPosition)
        {
            Debug.Log("STartpos: " + me.currentTask.StartPosition);
            int delta = (int)(me.currentTask.StartPosition.x - ObjectMap.ObjectData[(int)me.currentTask.StartPosition.x, (int)me.currentTask.StartPosition.y].@object.position.x);
            ObjectMap.ObjectData[(int)me.currentTask.StartPosition.x, (int)me.currentTask.StartPosition.y].@object.Storage[delta] = null;
            me.currentObjectToCarry = me.currentTask.Item;
            me.currentTask.Item.myObject.SetActive(false);
            me.GoTo((GroundTile)map.MapData[(int)me.currentTask.EndPosition.x, (int)me.currentTask.EndPosition.y]);
        }
        else if ((Vector2)me.transform.position == me.currentTask.EndPosition)
        {
            for (int i = 0; i < ObjectMap.ObjectData[(int)me.currentTask.EndPosition.x, (int)me.currentTask.EndPosition.y+1].@object.Storage.Length; i++)
            {
                if (me.currentObjectToCarry.Uses > 0)
                {
                    if (ObjectMap.ObjectData[(int)me.currentTask.EndPosition.x, (int)me.currentTask.EndPosition.y+1].@object.Storage[i] == null)
                    {
                        me.currentObjectToCarry.myObject.SetActive(true);
                        me.currentObjectToCarry.myObject.transform.position = new Vector3(me.currentTask.EndPosition.x, me.currentTask.EndPosition.y + 1, -2f);
                        ObjectMap.ObjectData[(int)me.currentTask.EndPosition.x, (int)me.currentTask.EndPosition.y + 1].@object.Storage[i] = me.currentObjectToCarry;
                        me.workManager.WorkInProgress.Remove(me.currentTask);
                        me.workManager.FinishedWorkObjects.Add(me.currentTask.Item);
                        me.currentTask = null;
                        animator.SetBool("DoWork", false);
                        return;
                    }
                    else
                    {
                        int delta = Mathf.Clamp(10 - ObjectMap.ObjectData[(int)me.currentTask.EndPosition.x, (int)me.currentTask.EndPosition.y + 1].@object.Storage[i].Uses, 0, me.currentObjectToCarry.Uses);
                        me.currentObjectToCarry.Uses -= delta;
                        ObjectMap.ObjectData[(int)me.currentTask.EndPosition.x, (int)me.currentTask.EndPosition.y + 1].@object.Storage[i].Uses += delta;
                    }
                }
            }
            Destroy(me.currentObjectToCarry.myObject);
            me.currentObjectToCarry = null;
            me.workManager.WorkInProgress.Remove(me.currentTask);
            me.currentTask = null;
            animator.SetBool("DoWork", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (me.currentTask != null)
        {
            me.workManager.WorkInProgress.Remove(me.currentTask);
            me.workManager.AddToLookUp(me.currentTask.type, me.currentTask);
            me.currentTask = null;
            animator.SetBool("DoWork", false);
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
