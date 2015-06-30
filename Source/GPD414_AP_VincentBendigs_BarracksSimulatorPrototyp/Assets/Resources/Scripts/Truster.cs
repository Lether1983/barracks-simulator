using UnityEngine;
using System.Collections;
using DH.Messaging.Bus;

public class Truster : ScriptableObject
{
    public trusterState[] trusterplan;
    MessageSubscription<int> subscribtion;
    
    public KompanieObject kompanieObject;

    public void OnEnable()
    {
        subscribtion = MessageBusManager.Subscribe<int>("ChangeHour");
        subscribtion.OnMessageReceived += changeMessage_OnMessageReceived;
        trusterplan = new trusterState[24];
        for (int i = 0; i < trusterplan.Length; i++)
        {
            trusterplan[i] = new trusterState();
        }
        fillTrusterPlan();
    }

    private void changeMessage_OnMessageReceived(MessageSubscription<int> s, MessageReceivedEventArgs<int> args)
    {
        MessageBusManager.AddMessage<ChangeStateEventArgs>("ChangeState", new ChangeStateEventArgs(trusterplan[args.Message].state, kompanieObject));
    }

    public void fillTrusterPlan()
    {
        for (int i = 0; i < trusterplan.Length; i++)
		{
		    if(i == 0 || i > 0 || i < 6 ||i == 23)
            {
                trusterplan[i].state = trusterStates.SleepTime;
            }
            if(i == 6 || i == 18)
            {
                trusterplan[i].state = trusterStates.ShowerTime;
            }
            if(i == 7 || i == 13 || i == 19)
            {
                trusterplan[i].state = trusterStates.EatTime;
            }
            if(i > 7 && i < 13 || i > 13 && i < 18)
            {
                trusterplan[i].state = trusterStates.WorkTime;
            }
            if(i > 19 && i < 23)
            {
                trusterplan[i].state = trusterStates.FreeTime;
            }
		}
    }
}
