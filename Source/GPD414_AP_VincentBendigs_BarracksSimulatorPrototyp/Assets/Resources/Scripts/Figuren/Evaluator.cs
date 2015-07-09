using UnityEngine;
using System.Collections;
using DH.Messaging.Bus;

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

public class Evaluator : MonoBehaviour 
{
    public trusterStates currentTrusterState;
    public ObjectLogicObject tempObject;
    MessageSubscription<ChangeStateEventArgs> subscribtion;
    TileMap map = TileMap.Instance();
    [SerializeField]
    Soldiers me;
    [SerializeField]
    Animator animator;
    int checkValue = 75;

    void Awake()
    {
        subscribtion = MessageBusManager.Subscribe<ChangeStateEventArgs>("ChangeState");
        subscribtion.OnMessageReceived += changeMessage_OnMessageReceived;
    }

    void changeMessage_OnMessageReceived(MessageSubscription<ChangeStateEventArgs> s, MessageReceivedEventArgs<ChangeStateEventArgs> args)
    {
        if(args.Message.targetKompanie != me.ownKompanie)
        {
            return;
        }
        if(args.Message.NewState == currentTrusterState)
        {
            return;
        }
        currentTrusterState = args.Message.NewState;
        switch (args.Message.NewState)
        {
            case trusterStates.SleepTime:
            case trusterStates.EatTime:
            case trusterStates.FreeTime:
            case trusterStates.ShowerTime:
            case trusterStates.SportsTime:
                animator.SetTrigger("IsNotWorkTime");
                break;
            case trusterStates.WorkTime:
                animator.SetTrigger("IsWorkTime");
                break;
            default:
                break;
        }

    }

    bool CanSetBool(CheckScope checkScope,AttributeCheck attributes,AttributeCheck targetAttribute,float value, UseableObjects @object)
    {
        if ((attributes & targetAttribute) == 0) return false;
        if (value < checkValue) return false;
        
        if ((checkScope & CheckScope.OwnRoom) > 0 && (tempObject = me.OwnRoom.GetRoomObjects(@object)) != null)
        {
            me.GoTo((GroundTile)map.MapData[(int)tempObject.position.x, (int)tempObject.position.y]);
            return true;
        }
        //else if ((checkScope & CheckScope.Company) > 0/*&& me.OwnKompanie.GetRoomObjects(@object)*/)
        //{
        //    return true;
        //}
        //else if ((checkScope & CheckScope.WorkPlace) > 0/*&& me.WorkPlace.GetRoomObjects(@object)*/)
        //{
        //    return true;
        //}
        //else if ((checkScope & CheckScope.Everywhere) > 0 /*&& me.roomManager.GetRoomObjects(@object)*/)
        //{
        //    return true;
        //}
        return false;
    }

    public void GetEvaluationForStates(trusterStates states, CheckScope checki,AttributeCheck attri)
    {
        if((currentTrusterState & states) == 0)
        {
            return;
        }

        if(CanSetBool(checki,attri,AttributeCheck.HasToUseTheToilette,me.hasToUseTheToilette,UseableObjects.Toilette))
        {
            animator.SetBool("UseToilette", true);
        }
        else if (CanSetBool(checki, attri, AttributeCheck.Tired, me.tired, UseableObjects.Bed))
        {
            animator.SetBool("UseBed", true);
        }
        else if (CanSetBool(checki, attri, AttributeCheck.Diversity, me.diversity, UseableObjects.TV))
        {
            animator.SetBool("UseTV", true);
        }
        else if(CanSetBool(checki, attri, AttributeCheck.HomeIll, me.homeIll, UseableObjects.Phone))
        {
            animator.SetBool("UsePhone", true);
        }
        else if (CanSetBool(checki, attri, AttributeCheck.IsDirty, me.isDirty, UseableObjects.Shower))
        {
            animator.SetBool("UseShower", true);
        }
        else if(CanSetBool(checki, attri, AttributeCheck.Hungry, me.hungry, UseableObjects.Food))
        {
            animator.SetBool("EatFood", true);
        }
        else if(CanSetBool(checki, attri, AttributeCheck.NeedFitness, me.needFitness, UseableObjects.SportEquipment))
        {
            animator.SetBool("UseSportsEquipment", true);
        }
        else if(CanSetBool(checki, attri, AttributeCheck.TrainLevel, me.TrainingsLevel, UseableObjects.SchoolDesk))
        {
            animator.SetBool("TrainBasics", true);
        }
    }
}
