using UnityEngine;
using System.Collections;
using DH.Messaging.Bus;

#region EnumsFlags
public enum trusterStates
{
    SleepTime = 1 << 0,
    EatTime = 1 << 1,
    FreeTime = 1 << 2,
    ShowerTime = 1 << 3,
    WorkTime = 1 << 4,
    SportsTime = 1 << 5
}

public enum CheckScope
{
    OwnRoom = 1 << 0,
    Company = 1 << 1,
    Everywhere = 1 << 2,
    WorkPlace = 1 << 3
}

public enum AttributeCheck
{
    IsDirty = 1 << 0,
    NeedFitness = 1 << 1,
    Tired = 1 << 2,
    Hungry = 1 << 3,
    Diversity = 1 << 4,
    HomeIll = 1 << 5,
    TrainLevel = 1 << 6,
    HasToUseTheToilette = 1 << 7
}
#endregion

public class Evaluator : MonoBehaviour
{
    #region Variablen
    public trusterStates currentTrusterState;
    public ObjectLogicObject tempObject;
    MessageSubscription<ChangeStateEventArgs> subscribtion;
    TileMap map = TileMap.Instance;
    [SerializeField]
    Soldiers me;
    [SerializeField]
    Animator animator;
    int checkValue = 50;
    #endregion

    void Awake()
    {
        subscribtion = MessageBusManager.Subscribe<ChangeStateEventArgs>("ChangeState");
        subscribtion.OnMessageReceived += changeMessage_OnMessageReceived;
    }

    void changeMessage_OnMessageReceived(MessageSubscription<ChangeStateEventArgs> s, MessageReceivedEventArgs<ChangeStateEventArgs> args)
    {
        if (me.myJob == null || me.myJob.jobs != TypeOfJobs.Civilian)
        {
            if (args.Message.targetKompanie != me.ownKompanie)
            {
                return;
            }

            if (args.Message.NewState == currentTrusterState)
            {
                return;
            }
        }
        currentTrusterState = args.Message.NewState;

        switch (args.Message.NewState)
        {
            case trusterStates.SleepTime:
            case trusterStates.EatTime:
            case trusterStates.FreeTime:
            case trusterStates.ShowerTime:
            case trusterStates.SportsTime:
                if ( me.myJob == null || me.myJob.jobs != TypeOfJobs.Civilian)
                {
                    animator.SetTrigger("IsNotWorkTime");
                    break;
                }
                animator.SetTrigger("IsWorkTime");

                break;
            case trusterStates.WorkTime:
                if (me.WorkPlace == null)
                {
                    if (me.myJob.jobs != TypeOfJobs.Civilian)
                    {
                        animator.SetTrigger("IsNotWorkTime");
                        break;
                    }
                    animator.SetTrigger("IsWorkTime");
                    break;
                }
                animator.SetTrigger("IsWorkTime");
                break;
            default:
                break;
        }

    }

    bool CanSetBoolForUse(CheckScope checkScope, AttributeCheck attributes, AttributeCheck targetAttribute, float value, UseableObjects @object)
    {
        if ((attributes & targetAttribute) == 0) return false;
        if (value < checkValue) return false;
        if (me.roomManager.GetRoomObjects(@object) == null) return false;
        if ((checkScope & CheckScope.Everywhere) > 0)
        {
            for (int i = 0; i < me.roomManager.GetRoomObjects(@object).Storage.Length; i++)
            {
                if ((tempObject = me.roomManager.GetRoomObjects(@object)).Storage[i] != null)
                {
                    me.GoTo((GroundTile)map.MapData[(int)tempObject.Storage[i].myObject.transform.position.x, (int)tempObject.Storage[i].myObject.transform.position.y]);
                    if (me.roomManager.GetRoomObjects(@object).Storage[i] != null)
                    {
                        me.roomManager.GetRoomObjects(@object).Storage[i].Uses -= 3;
                        if (me.roomManager.GetRoomObjects(@object).Storage[i].Uses <= 0)
                        {
                            Destroy(me.roomManager.GetRoomObjects(@object).Storage[i].myObject);
                            me.workManager.FinishedWorkObjects.Remove(me.roomManager.GetRoomObjects(@object).Storage[i]);
                            me.roomManager.GetRoomObjects(@object).Storage[i] = null;
                        }
                    }
                    return true;
                }
            }
        }
        return false;
    }

    bool CanSetBool(CheckScope checkScope, AttributeCheck attributes, AttributeCheck targetAttribute, float value, UseableObjects @object)
    {
        if ((attributes & targetAttribute) == 0) return false;
        if (value < checkValue) return false;

        if ((checkScope & CheckScope.OwnRoom) > 0 && me.OwnRoom != null && (tempObject = me.OwnRoom.GetRoomObjects(@object)) != null)
        {
            me.GoTo((GroundTile)map.MapData[(int)tempObject.position.x, (int)tempObject.position.y]);
            return true;
        }
        else if ((checkScope & CheckScope.Company) > 0 && (tempObject = me.ownKompanie.GetRoomObjects(@object)) != null)
        {
            me.GoTo((GroundTile)map.MapData[(int)tempObject.position.x, (int)tempObject.position.y]);
            return true;
        }
        else if ((checkScope & CheckScope.WorkPlace) > 0 && me.WorkPlace != null && (tempObject = me.WorkPlace.GetRoomObjects(@object)) != null)
        {
            return true;
        }
        else if ((checkScope & CheckScope.Everywhere) > 0 && (tempObject = me.roomManager.GetRoomObjects(@object)) != null)
        {
            return true;
        }

        return false;
    }

    public void GetEvaluationForStates(trusterStates states, CheckScope checki, AttributeCheck attri)
    {
        if ((currentTrusterState & states) == 0)
        {
            return;
        }

        if (CanSetBool(checki, attri, AttributeCheck.HasToUseTheToilette, me.hasToUseTheToilette, UseableObjects.ReduceToilette))
        {
            animator.SetBool("UseToilette", true);
        }
        else if (CanSetBool(checki, attri, AttributeCheck.Tired, me.tired, UseableObjects.ReduceTired))
        {
            animator.SetBool("UseBed", true);
        }
        else if (CanSetBool(checki, attri, AttributeCheck.Diversity, me.diversity, UseableObjects.ReduceDiversity))
        {
            animator.SetBool("UseTV", true);
        }
        else if (CanSetBool(checki, attri, AttributeCheck.HomeIll, me.homeIll, UseableObjects.ReduceHomeIll))
        {
            animator.SetBool("UsePhone", true);
        }
        else if (CanSetBool(checki, attri, AttributeCheck.IsDirty, me.isDirty, UseableObjects.ReduceDirty))
        {
            animator.SetBool("UseShower", true);
        }
        else if (CanSetBoolForUse(checki, attri, AttributeCheck.Hungry, me.hungry, UseableObjects.CanStoreFood))
        {
            animator.SetBool("EatFood", true);
        }
        else if (CanSetBool(checki, attri, AttributeCheck.NeedFitness, me.needFitness, UseableObjects.ReduceFitness))
        {
            animator.SetBool("UseSportsEquipment", true);
        }
        else if (CanSetBool(checki, attri, AttributeCheck.TrainLevel, me.TrainingsLevel, UseableObjects.IncreaceTraining))
        {
            animator.SetBool("TrainBasics", true);
        }
    }
}
