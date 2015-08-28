using UnityEngine;
using System.Collections;
using DH.Messaging.Bus;

public enum DinstgradGruppen { NormalSolider, TimeSoldier, ProfessionalSoldiers };

public enum KompaniezugehoerigkeitsGruppen { AusbildungsKompanien, KampfKompanien, VersorgungsKompanien, LehrKompanien, KampfUnterstuetzungsKompanien, SanitaetsKompanien };

public enum Kompaniezugehorigkeit
{
    None = 0, JaegerKompanie, PanzerKompanie, GrenadierKompanie, PanzerGrenadierKompanie,
    TransportKompanie, InstansetzungsKompanie, UnterstuetzungsKompanie, MilitaerPolizeiKompanie,
    VersorgungsKompanie, EODKompanie, ABCAbwehrKompanie, PionierKompanie, HeeresfliegerKompanie,
    FernmeldeKompanie, VersorgungsSanitaeter, KampfSanitaeter, EinsatzSanitaeter, KrankenhausSanitaeter
};
public enum TypeOfJobs { None ,Koch, WaescheWart, Civilian};

public class Soldiers : MonoBehaviour
{
    MessageSubscription<RoomLogicObject> subscribtion;
    MessageSubscription<RoomLogicObject> subscribtion1;

    public KompanieObject ownKompanie;
    public RoomLogicObject OwnRoom;
    public RoomLogicObject WorkPlace;
    public GameManager manager;
    public WorkManager workManager;
    public Job myJob;
    public RoomManager roomManager;
    public Vector3 waypoint;
    public WorkTask currentTask;


    public WorkObjects currentObjectToCarry;

    public float TrainingsLevel;
    public float isDirty;
    public float tired;
    public float diversity;
    public float hasToUseTheToilette;
    public float hungry;
    public float homeIll;
    public float needFitness;
    public bool shouldMove = false;

    float timer = 0;

    void Start()
    {
        subscribtion = MessageBusManager.Subscribe<RoomLogicObject>("freeStube");
        subscribtion.OnMessageReceived += freeRoom_OnMessageReceived;
        subscribtion1 = MessageBusManager.Subscribe<RoomLogicObject>("FreeWorkPlace");
        subscribtion1.OnMessageReceived += freeWorkMessage_OnMessagesReceived;
        manager = roomManager.gameObject.GetComponent<GameManager>();

        if (ownKompanie != null)
        {
            manager.AllSoldiers.Add(this);
        }
        else
        {
            manager.AllCivilians.Add(this);
        }
    }

    private void freeWorkMessage_OnMessagesReceived(MessageSubscription<RoomLogicObject> s, MessageReceivedEventArgs<RoomLogicObject> args)
    {
        if (ownKompanie.KompanieType == Kompaniezugehorigkeit.VersorgungsKompanie && (WorkPlace == null || WorkPlace.Workerscount > 0))
        {
            WorkPlace = args.Message;
            WorkPlace.Workerscount--;
            for (int i = 0; i < WorkPlace.workers.Length; i++)
            {
                if (WorkPlace.workers[i] == null)
                {
                    WorkPlace.workers[i] = this;
                    myJob = WorkPlace.RoomInfo.availableJobs[Random.Range(0, WorkPlace.RoomInfo.availableJobs.Length - 1)];
                    break;
                }
            }
        }
    }

    private void freeRoom_OnMessageReceived(MessageSubscription<RoomLogicObject> s, MessageReceivedEventArgs<RoomLogicObject> args)
    {
        if (OwnRoom == null && args.Message.Claim(this))
        {
            OwnRoom = args.Message;
            roomManager.AssignRoomToCompany(this.ownKompanie, args.Message);
        }
    }

    public void Move(Vector2 destination)
    {
        waypoint = destination;
        shouldMove = true;
    }

    void Update()
    {
        tired += 5 / 3600f * Time.deltaTime * manager.speed;
        isDirty += 5 / 3600f * Time.deltaTime * manager.speed;
        needFitness += 5 / 3600f * Time.deltaTime * manager.speed;
        hasToUseTheToilette += 5 / 3600f * Time.deltaTime * manager.speed;
        hungry += 5 / 3600f * Time.deltaTime * manager.speed;
        diversity += 5 / 3600f * Time.deltaTime * manager.speed;
        homeIll += 5 / 3600f * Time.deltaTime * manager.speed;

        if (hungry < 12)
        {
            hasToUseTheToilette += 2.5f / 3600f * Time.deltaTime * manager.speed;
        }

        if (this.gameObject.transform.position.x == waypoint.x && this.gameObject.transform.position.y == waypoint.y && this.gameObject.transform.position.z == -2)
        {
            shouldMove = false;
            MessageBusManager.AddMessage<int>("Reachtarget", 1);
        }

        if (shouldMove)
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
