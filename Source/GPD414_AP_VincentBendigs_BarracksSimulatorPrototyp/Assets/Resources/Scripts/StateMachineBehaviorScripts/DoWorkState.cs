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
        else if(me.currentTask.type == TypeOfWork.TakeCloth)
        {
            if (me.myJob != null)
            {
                if ((Vector2)me.transform.position == me.currentTask.StartPosition)
                {
                    int delta = (int)(me.currentTask.StartPosition.x - ObjectMap.ObjectData[(int)me.currentTask.StartPosition.x, (int)me.currentTask.StartPosition.y].@object.position.x);
                    var thingInArray = ObjectMap.ObjectData[(int)me.currentTask.StartPosition.x, (int)me.currentTask.StartPosition.y];
                    GameObject storage = thingInArray.@object.Storage[delta].myObject;
                    ObjectMap.ObjectData[(int)me.currentTask.StartPosition.x, (int)me.currentTask.StartPosition.y].@object.Storage[delta] = null;
                    me.myJob = (Job)Resources.Load<Job>("Prefabs/Scriptable Objects/JobObjects/None");
                    me.manager.AllCivilians.Remove(me);
                    me.manager.AllSoldiers.Add(me);
                    me.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Figuren/Military_Normal_Front");
                    me.ownKompanie = (KompanieObject)Resources.Load<ScriptableObject>("Prefabs/Scriptable Objects/KompanieObjects/VersorgungsKompanie");
                    me.currentTask = null;
                    Destroy(storage);
                    RoomLogicObject temp = me.roomManager.FindFreeRoom(TypeOfRoom.Stube);
                    if(temp != null && temp.Claim(me))
                    {
                        me.OwnRoom = temp;
                        me.roomManager.AssignRoomToCompany(me.ownKompanie, temp);
                    }
                    
                }
            }
        }
    }

    private void MoveItemFromStartToEnd(Animator animator)
    {
        if ((Vector2)me.transform.position == me.currentTask.StartPosition)
        {
            int delta = (int)(me.currentTask.StartPosition.x - ObjectMap.ObjectData[(int)me.currentTask.StartPosition.x, (int)me.currentTask.StartPosition.y].@object.position.x);
            ObjectMap.ObjectData[(int)me.currentTask.StartPosition.x, (int)me.currentTask.StartPosition.y].@object.Storage[delta] = null;
            me.currentObjectToCarry = me.currentTask.Item;
            var task = me.currentTask;
            var item = task.Item;
            var obj = item.myObject;
            obj.SetActive(false);
            me.GoTo((GroundTile)map.MapData[(int)me.currentTask.EndPosition.x, (int)me.currentTask.EndPosition.y]);
        }
        else if ((Vector2)me.transform.position == me.currentTask.EndPosition)
        {
            for (int i = 0; i < ObjectMap.ObjectData[(int)me.currentTask.EndPosition.x, (int)me.currentTask.EndPosition.y + 1].@object.Storage.Length; i++)
            {
                if (me.currentObjectToCarry.Uses > 0)
                {
                    if (ObjectMap.ObjectData[(int)me.currentTask.EndPosition.x, (int)me.currentTask.EndPosition.y+1].@object.Storage[i] == null)
                    {
                        me.currentObjectToCarry.myObject.SetActive(true);

                        me.currentObjectToCarry.myObject.transform.position = new Vector3(ObjectMap.ObjectData[(int)me.currentTask.EndPosition.x, (int)me.currentTask.EndPosition.y + 1].@object.position.x + i, me.currentTask.EndPosition.y + 1, -2f);
                        ObjectMap.ObjectData[(int)me.currentTask.EndPosition.x, (int)me.currentTask.EndPosition.y + 1].@object.Storage[i] = me.currentObjectToCarry;
                        me.workManager.WorkInProgress.Remove(me.currentTask);
                        me.workManager.FinishedWorkObjects.Add(me.currentTask.Item);
                        if(me.currentTask.type == TypeOfWork.MovingMaterial)
                        {
                            me.workManager.CreateAdvancedWork(me.currentTask.type, me.currentTask.Item);
                        }
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
