using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpawnNewFigure : MonoBehaviour 
{
    public GameManager gmanager;
    public Slider slider;
    public RoomManager rmanager;
    public int SpawnValue = 0;
    public GameObject FigurePrefab;
    public Job CivilJob;
    bool spawnNow;
    float timer;
    TileMap map = TileMap.Instance;
    


    public void StartSpawning()
    {
        SpawnValue = (int)slider.value;
        spawnNow = true;
    }


	void Update ()
    {
        timer += Time.deltaTime;
        if (timer > 3)
        {
            if (spawnNow && SpawnValue > 0)
            {
                SpawnCivilian();
                timer = 0;
            }
        }
	}

    private void SpawnCivilian()
    {
        GameObject tempSoldier = GameObject.Instantiate(FigurePrefab, new Vector3(200f, 250f, -2f), Quaternion.identity) as GameObject;
        tempSoldier.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Figuren/Civilian_2_Front");
        tempSoldier.GetComponent<Soldiers>().manager = gmanager;
        tempSoldier.GetComponent<Soldiers>().roomManager = gmanager.gameObject.GetComponent<RoomManager>();
        tempSoldier.GetComponent<Soldiers>().workManager = gmanager.gameObject.GetComponent<WorkManager>();
        SpawnValue--;
        SetTargetPos(tempSoldier.GetComponent<Soldiers>());
        tempSoldier.GetComponent<Soldiers>().myJob = CivilJob;
    }

    void SetTargetPos(Soldiers tempsoldier)
    {
        RoomLogicObject room = rmanager.FindRoom(TypeOfRoom.Waeschekammer);
        if(room != null)
        {
            tempsoldier.GoTo((GroundTile)map.MapData[(int)room.Position.x,(int)room.Position.y]);
        }

    }
}
