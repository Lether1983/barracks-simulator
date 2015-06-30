using UnityEngine;
using System;
using System.Collections;
using DH.Messaging.Bus;

public class GameClock : MonoBehaviour 
{
    

    public Transform hour, minute;

    public float Speed;
    bool ifcanRaise;
    TimeSpan time = new TimeSpan();
    

    private const float
        hoursToDegrees = 360f / 12f,
        minutesToDegrees = 360f / 60f;

    void Update()
    {
        time = time.Add(TimeSpan.FromSeconds(Time.deltaTime * Speed));

        if (time.Minutes == 0)
        {
            if (ifcanRaise)
            {
                MessageBusManager.AddMessage<int>("ChangeHour", time.Hours);
                ifcanRaise = false;
            }
        }
        else if(time.Minutes > 0)
        {
            ifcanRaise = true;
        }

        hour.localRotation = Quaternion.Euler(0f, 0f, (float)time.TotalHours * -hoursToDegrees);
        minute.localRotation = Quaternion.Euler(0f, 0f,(float)time.TotalMinutes * -minutesToDegrees);
    }
}
