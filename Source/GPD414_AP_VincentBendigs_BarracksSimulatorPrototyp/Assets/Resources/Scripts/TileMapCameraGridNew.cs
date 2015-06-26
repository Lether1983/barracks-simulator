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
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        mainCamera = this.gameObject.GetComponent<CameraControl>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        Rect TempRect = mainCamera.fieldOfView;
        for (int i = Mathf.FloorToInt(TempRect.xMin); i < Mathf.CeilToInt(TempRect.xMax); i++)
        {
            for (int j = Mathf.FloorToInt(TempRect.yMax); j < Mathf.CeilToInt(TempRect.yMin); j++)
            {
                var temptile = map.MapData[i, j];
                SetTile(new Vector2(i, j), temptile);
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


    public void TileFactory(GameObject gameobject)
    {
        gameobject.GetComponent<PoolCheck>().pool = tilePool;
        gameobject.GetComponent<PoolCheck>().cameraControll = mainCamera;
    }
    public void ObjectFactory(GameObject gameobject)
    {

    }
    public void RoomFactory(GameObject gameobject)
    {

    }
    public void TileDisable(GameObject gameobject)
    {
        activeTiles.Remove(gameobject.transform.position);
    }
}
