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
        fillTrusterPlan();
    }

    private void changeMessage_OnMessageReceived(MessageSubscription<int> s, MessageReceivedEventArgs<int> args)
    {
        MessageBusManager.AddMessage<ChangeStateEventArgs>("ChangeState", new ChangeStateEventArgs(trusterplan[args.Message].state, kompanieObject));
    }

    public void fillTrusterPlan()
    {
        //TODO: Noch genutzt? Wenn ja, wofür :D Wird doch eigentlich alles im ScriptableObject behandelt, oder?
        for (int i = 0; i < trusterplan.Length; i++)
		{
		    if(i == 0 || i > 0 || i < 6 ||i == 23)
            {
                trusterplan[i] = Resources.Load<trusterState>("Prefabs/Scriptable Objects/States/SleepTime");
            }
            if(i == 6 || i == 18)
            {
                trusterplan[i] = Resources.Load<trusterState>("Prefabs/Scriptable Objects/States/ShowerTime");
            }
            if(i == 7 || i == 13 || i == 19)
            {
                trusterplan[i] = Resources.Load<trusterState>("Prefabs/Scriptable Objects/States/EatTime");
            }
            if(i > 7 && i < 13 || i > 13 && i < 18)
            {
                trusterplan[i] = Resources.Load<trusterState>("Prefabs/Scriptable Objects/States/WorkTime");
            }
            if(i > 19 && i < 23)
            {
                trusterplan[i] = Resources.Load<trusterState>("Prefabs/Scriptable Objects/States/FreeTime");
            }
		}
    }
}
