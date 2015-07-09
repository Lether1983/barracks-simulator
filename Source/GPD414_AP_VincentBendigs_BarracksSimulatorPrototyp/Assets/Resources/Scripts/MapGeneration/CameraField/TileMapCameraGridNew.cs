using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileMapCameraGridNew : MonoBehaviour
{
    public float timer = 0;
    public GameObject TileSpawn;
    public GameObject ObjectSpawn;
    public GameObject RoomSpawn;
    public Pool tilePool;
    public Pool objectPool;
    public Pool roomPool;
    GameManager manager;
    CameraControl mainCamera;
    TileMap map = TileMap.Instance();
    ObjectTileMap objectMap = ObjectTileMap.Instance();
    RoomMap roomMap = RoomMap.Instance();

    Dictionary<Vector2, GameObject> activeTiles;
    Dictionary<Vector2, GameObject> activeObjects;
    Dictionary<Vector2, GameObject> activeRooms;

	// Use this for initialization
	void Start () 
    {
        activeTiles = new Dictionary<Vector2, GameObject>();
        activeObjects = new Dictionary<Vector2, GameObject>();
        activeRooms = new Dictionary<Vector2, GameObject>();
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        mainCamera = this.gameObject.GetComponent<CameraControl>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        Rect TempRect = mainCamera.fieldOfView;
        for (int i = Mathf.FloorToInt(TempRect.xMin)-1; i < Mathf.CeilToInt(TempRect.xMax)+1; i++)
        {
            for (int j = Mathf.FloorToInt(TempRect.yMax)-1; j < Mathf.CeilToInt(TempRect.yMin)+1; j++)
            {
                if(i >= 0 && j >= 0 && i < map.MapData.GetLength(0)&& j < map.MapData.GetLength(1))
                {
                    var temptile = map.MapData[i, j];
                    var tempObject = objectMap.ObjectData[i,j];
                    var tempRoom = roomMap.RoomData[i, j];
                    SetTile(new Vector2(i, j), temptile);
                    SetObject(new Vector2(i, j), tempObject);
                    SetRoom(new Vector2(i, j), tempRoom);
                }
            }
        }
	}

    void SetTile(Vector2 index,TileBaseClass tile)
    {
        if (activeTiles.ContainsKey(index)) return;
        GameObject TempGameObject;
        if(!activeTiles.TryGetValue(index, out TempGameObject))
        {
            TempGameObject = tilePool.Get();
            activeTiles.Add(index, TempGameObject);
        }
        TempGameObject.transform.position = tile.Position;
        TempGameObject.GetComponent<SpriteRenderer>().sprite = tile.Texture;
        map.MapData[(int)index.x, (int)index.y].myObject = TempGameObject;
    }

    public void SetObject(Vector2 index,ObjectBaseClass @object)
    {
        if (activeObjects.ContainsKey(index)) return;
        GameObject TempGamobject;
        if(!activeObjects.TryGetValue(index,out TempGamobject))
        {
            TempGamobject = objectPool.Get();
            activeObjects.Add(index, TempGamobject);
        }
        TempGamobject.transform.position = new Vector3(@object.Position.x, @object.Position.y, -1);
        TempGamobject.GetComponent<SpriteRenderer>().sprite = @object.Texture;
        objectMap.ObjectData[(int)index.x, (int)index.y].myObject = TempGamobject;
    }

    void SetRoom (Vector2 index,RoomBaseClass room)
    {
        if (activeRooms.ContainsKey(index)) return;
        GameObject TempGamobject;
        if (!activeRooms.TryGetValue(index, out TempGamobject))
        {
            TempGamobject = roomPool.Get();
            activeRooms.Add(index, TempGamobject);
        }
        TempGamobject.transform.position = new Vector3(room.Position.x, room.Position.y, -0.5f);
        TempGamobject.GetComponent<SpriteRenderer>().sprite = room.Texture;
        roomMap.RoomData[(int)index.x, (int)index.y].myObject = TempGamobject;
    }


    public void TileFactory(GameObject gameobject)
    {
        gameobject.GetComponent<PoolCheck>().pool = tilePool;
        gameobject.GetComponent<PoolCheck>().cameraControll = mainCamera;
    }

    public void ObjectFactory(GameObject gameobject)
    {
        gameobject.GetComponent<PoolCheck>().pool = objectPool;
        gameobject.GetComponent<PoolCheck>().cameraControll = mainCamera;
    }

    public void RoomFactory(GameObject gameobject)
    {
        gameobject.GetComponent<PoolCheck>().pool = roomPool;
        gameobject.GetComponent<PoolCheck>().cameraControll = mainCamera;
    }

    public void TileDisable(GameObject gameobject)
    {
        activeTiles.Remove(gameobject.transform.position);
    }

    public void ObjectDisable(GameObject gameobject)
    {
        activeObjects.Remove(gameobject.transform.position);
    }

    public void RoomDisable(GameObject gameobject)
    {
        activeRooms.Remove(gameobject.transform.position);
    }
}
