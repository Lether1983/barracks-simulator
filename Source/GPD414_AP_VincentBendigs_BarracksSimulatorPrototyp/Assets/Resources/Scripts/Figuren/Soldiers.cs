using UnityEngine;
using System.Collections;
using DH.Messaging.Bus;

public enum DinstgradGruppen { NormalSolider, TimeSoldier, ProfessionalSoldiers };

public enum KompaniezugehoerigkeitsGruppen { AusbildungsKompanien, KampfKompanien, VersorgungsKompanien, LehrKompanien, KampfUnterstuetzungsKompanien, SanitaetsKompanien };

public enum Kompaniezugehorigkeit
{
    JaegerKompanie, PanzerKompanie, GrenadierKompanie, PanzerGrenadierKompanie, 
    TransportKompanie, InstansetzungsKompanie, UnterstuetzungsKompanie, MilitaerPolizeiKompanie, 
    VersorgungsKompanie, EODKompanie, ABCAbwehrKompanie, PionierKompanie, HeeresfliegerKompanie, 
    FernmeldeKompanie, VersorgungsSanitaeter, KampfSanitaeter, EinsatzSanitaeter, KrankenhausSanitaeter
};

public class Soldiers : MonoBehaviour
{
    MessageSubscription<RoomLogicObject> subscribtion;
    public KompanieObject ownKompanie;
    public RoomLogicObject OwnRoom;
    public RoomLogicObject WorkPlace;
    public Job myJob;
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

    public GameManager manager;

    void Start()
    {
        subscribtion = MessageBusManager.Subscribe<RoomLogicObject>("freeStube");
        subscribtion.OnMessageReceived += changeMessage_OnMessageReceived;
        manager = roomManager.gameObject.GetComponent<GameManager>();
    }

    private void changeMessage_OnMessageReceived(MessageSubscription<RoomLogicObject> s, MessageReceivedEventArgs<RoomLogicObject> args)
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
        else if (shouldMove)
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
