﻿using UnityEngine;
using System.Collections;
using DH.Messaging.Bus;

enum DinstgradGruppen { NormalSolider, TimeSoldier, ProfessionalSoldiers };

enum KompaniezugehoerigkeitsGruppen { AusbildungsKompanien, KampfKompanien, VersorgungsKompanien, LehrKompanien, KampfUnterstuetzungsKompanien, SanitaetsKompanien };

enum KampfkompanienZugehörigkeit { JaegerKompanie,PanzerKompanie,GrenadierKompanie,PanzerGrenadierKompanie};
enum VersorgungsKompanieZugehörigkeit { TransportKompanie,InstansetzungsKompanie,UnterstuetzungsKompanie,MilitaerPolizeiKompanie,VersorgungsKompanie};
enum KampfUnterstuetzungsKomapnieZugehörigkeit { EODKompanie,ABCAbwehrKompanie,PionierKompanie,HeeresfliegerKompanie,FernmeldeKompanie};
enum SanitaetsZugehörigkeit { VersorgungsSanitaeter,KampfSanitaeter,EinsatzSanitaeter,KrankenhausSanitaeter};

public class Soldiers : MonoBehaviour
{
    MessageSubscription<RoomLogicObject> subscribtion;
    public KompanieObject ownKompanie;
    public RoomLogicObject OwnRoom;
    public RoomLogicObject WorkPlace;
    public RoomManager roomManager;
    public Vector3 waypoint;

    public float TrainingsLevel;
    public float isDirty;
    public float tired;
    public float diversity;
    public float hasToUseTheToilette;
    public float hungry;
    public float homeIll;
    public float needFitness;
    public bool shouldMove = false;

    void Start()
    {
        subscribtion = MessageBusManager.Subscribe<RoomLogicObject>("freeStube");
        subscribtion.OnMessageReceived += changeMessage_OnMessageReceived;
        tired = 76;
        isDirty = 76;
        needFitness = 76;
        hasToUseTheToilette = 76;
        hungry = 76;
        diversity = 76;
        homeIll = 76;
        TrainingsLevel = 0;
    }

    private void changeMessage_OnMessageReceived(MessageSubscription<RoomLogicObject> s, MessageReceivedEventArgs<RoomLogicObject> args)
    {
        if (OwnRoom == null && args.Message.Claim(this))
        {
             OwnRoom = args.Message;
             OwnRoom.kompanieObject = this.ownKompanie;
        }
    }

    public void Move(Vector2 destination)
    {
        waypoint = destination;
        shouldMove = true;
    }

    void Update()
    {
        if (this.gameObject.transform.position.x == waypoint.x && this.gameObject.transform.position.y == waypoint.y && this.gameObject.transform.position.z == -2)
        {
            shouldMove = false;
            MessageBusManager.AddMessage<int>("Reachtarget", 1);
        }
        else if(shouldMove)
        {
            this.transform.Translate(waypoint.x - transform.position.x, waypoint.y - transform.position.y, 0);
        }
    }
    public void GoTo(GroundTile target)
    {
        this.gameObject.GetComponent<AStarController>().getTargetPosition(target);
        this.gameObject.GetComponent<AStarController>().GetFinalPath();
    }

}
