using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;


public class GameManager : MonoBehaviour 
{
    public GameObject spriteAtlas;
    public GameObject[] images;
    TileMap map;
    ObjectTileMap objectMap;
    RoomMap roomMap;
    public GroundObject ground_Object;
    public GroundObject wall_Object;
    public GroundObject IndoorDefault;
    public GroundObject OutdoorDefault;
    public GroundObject GrassDefault;
    public ObjectsObject object_object;
    public RoomObjects room_object;

    public KompanieObject kompanie;
    public trusterState state;


    public AStarController controller;
	public AStarController controller2;


    
    #region Sprites
    public Sprite Grass;
    public Sprite Desert;
    public Sprite Wall;
    public Sprite Beton;
    public Sprite Dusche;
    public Sprite Door;
    public Sprite RoomSprite;
    #endregion
    
    #region Menü Building Bools
    public bool BuildWalls;
    public bool BuildFoundation;
    public bool InObjectBuildMode;
    public bool InRoomBuildMode;
    #endregion

    #region Menü Destroy Bools
    public bool DestroyModus;
    public bool DestroyTiles;
    public bool DestroyWalls;
    public bool DestroyObjects;
    public bool DestroyRooms;
    public bool DestroyFoundation;
    #endregion

    int bigMaxMapSize = 512;
    int smallMaxMapSize = 128;
    int middMaxMapSize = 256;
    bool smallMap = false;
    bool midMap = false;
    bool largeMap = true;
    

	// Use this for initialization
	void Awake()
    {
        map = TileMap.Instance();
        objectMap = ObjectTileMap.Instance();
        roomMap = RoomMap.Instance();
        GenerateAMap(map);
    }
    
    void GenerateAMap(TileMap map)
    {
        if(smallMap)
        {
            map.StartFieldOfViewValue = 20;
            map.GenerateStartMap(smallMaxMapSize);
            objectMap.GenerateObjectDataMap(smallMaxMapSize);
            roomMap.GenerateRoomObjects(smallMaxMapSize);
        }
        else if(midMap)
        {
            map.StartFieldOfViewValue = 40;
            map.GenerateStartMap(middMaxMapSize);
            objectMap.GenerateObjectDataMap(middMaxMapSize);
            roomMap.GenerateRoomObjects(middMaxMapSize);
        }
        else if(largeMap)
        {
            map.StartFieldOfViewValue = 80;
            map.GenerateStartMap(bigMaxMapSize);
            objectMap.GenerateObjectDataMap(bigMaxMapSize);
            roomMap.GenerateRoomObjects(bigMaxMapSize);
        }
    }

    public void ChangeTileByClick(GameObject hittedGameobject)
    {
        if (ground_Object == null)
        {
            return;
        }
        if (map.MapData[(int)hittedGameobject.transform.position.x, (int)hittedGameobject.transform.position.y].IsOutdoor)
        {
            if (map.MapData[(int)hittedGameobject.transform.position.x, (int)hittedGameobject.transform.position.y].isOverridable)
            {
                if (ground_Object.isOutdoor)
                {
                    hittedGameobject.GetComponent<SpriteRenderer>().sprite = ground_Object.texture;
                    map.MapData[(int)hittedGameobject.transform.position.x, (int)hittedGameobject.transform.position.y].GetAllValues(ground_Object);
                }
                if (ground_Object.isOverridable == false)
                {
                    hittedGameobject.GetComponent<SpriteRenderer>().sprite = ground_Object.texture;
                    map.MapData[(int)hittedGameobject.transform.position.x, (int)hittedGameobject.transform.position.y].GetAllValues(ground_Object);
                }
            }
        }
        else if (map.MapData[(int)hittedGameobject.transform.position.x, (int)hittedGameobject.transform.position.y].IsIndoor)
        {
            if (map.MapData[(int)hittedGameobject.transform.position.x, (int)hittedGameobject.transform.position.y].isOverridable)
            {
                if (ground_Object.isIndoor)
                {
                    hittedGameobject.GetComponent<SpriteRenderer>().sprite = ground_Object.texture;
                    map.MapData[(int)hittedGameobject.transform.position.x, (int)hittedGameobject.transform.position.y].GetAllValues(ground_Object);
                }
                if (ground_Object.isOverridable == false)
                {
                    hittedGameobject.GetComponent<SpriteRenderer>().sprite = ground_Object.texture;
                    map.MapData[(int)hittedGameobject.transform.position.x, (int)hittedGameobject.transform.position.y].GetAllValues(ground_Object);
                }
            }
        }
    }

    public void DestroyOneTile(GameObject hittedGameobject)
    {
        if (map.MapData[(int)hittedGameobject.transform.position.x, (int)hittedGameobject.transform.position.y].IsIndoor == false)
        {
            if (DestroyTiles)
            {
                hittedGameobject.GetComponent<SpriteRenderer>().sprite = OutdoorDefault.texture;
                map.MapData[(int)hittedGameobject.transform.position.x, (int)hittedGameobject.transform.position.y].GetAllValues(OutdoorDefault);
            }
            else if (DestroyWalls || GetComponent<ObjectManager>().DoorPlacement)
            {
                hittedGameobject.GetComponent<SpriteRenderer>().sprite = OutdoorDefault.texture;
                map.MapData[(int)hittedGameobject.transform.position.x, (int)hittedGameobject.transform.position.y].GetAllValues(OutdoorDefault);
            }
        }
        else if (map.MapData[(int)hittedGameobject.transform.position.x, (int)hittedGameobject.transform.position.y].IsIndoor)
        {
            if (DestroyTiles)
            {
                hittedGameobject.GetComponent<SpriteRenderer>().sprite = IndoorDefault.texture;
                map.MapData[(int)hittedGameobject.transform.position.x, (int)hittedGameobject.transform.position.y].GetAllValues(IndoorDefault);
            }
            else if (DestroyWalls || GetComponent<ObjectManager>().DoorPlacement)
            {
                hittedGameobject.GetComponent<SpriteRenderer>().sprite = IndoorDefault.texture;
                map.MapData[(int)hittedGameobject.transform.position.x, (int)hittedGameobject.transform.position.y].GetAllValues(IndoorDefault);
            }
        }
    }

    public void PlaceObjectByClick(GameObject hittetObject)
    {
        if(object_object == null)
        {
            return;
        }
        for (int i = 0; i < object_object.infos.Length; i++)
        {
             Vector2 Reference = objectMap.ObjectData[(int)hittetObject.transform.position.x, (int)hittetObject.transform.position.y].@object.position;
             Vector2 HittetTransform = hittetObject.transform.position;
             Vector2 delta = HittetTransform - Reference;

            if(object_object.infos[i].delta == delta)
            {
                hittetObject.GetComponent<SpriteRenderer>().sprite = object_object.infos[i].texture;
                objectMap.ObjectData[(int)hittetObject.transform.position.x, (int)hittetObject.transform.position.y].Texture = object_object.infos[i].texture;
            }
        }
    }

    public void PlaceRoomByClickOnMap(GameObject hittetObjectForRoom)
    {
        if (room_object == null) return;

        for (int i = 0; i < room_object.infos.Length; i++)
        {
            Vector2 Reference = roomMap.RoomData[(int)hittetObjectForRoom.transform.position.x, (int)hittetObjectForRoom.transform.position.y].room.Position;
            Vector2 HittetTransform = hittetObjectForRoom.transform.position;
            Vector2 delta = HittetTransform - Reference;
            if(room_object.infos[i].delta == delta)
            {
                hittetObjectForRoom.GetComponent<SpriteRenderer>().sprite = room_object.infos[i].texture;
                roomMap.RoomData[(int)hittetObjectForRoom.transform.position.x, (int)hittetObjectForRoom.transform.position.y].Texture = room_object.infos[i].texture;
                return;
            }
        }
        hittetObjectForRoom.GetComponent<SpriteRenderer>().sprite = room_object.defaultTexture;
        roomMap.RoomData[(int)hittetObjectForRoom.transform.position.x, (int)hittetObjectForRoom.transform.position.y].Texture = room_object.defaultTexture;
    }

    public void ResetAllBuildingModi()
    {
        BuildWalls = false;
        BuildFoundation = false;
        InRoomBuildMode = false;
        InObjectBuildMode = false;
        
        DestroyTiles = false;
        DestroyModus = false;
        DestroyWalls = false;
        DestroyFoundation = false;
        DestroyObjects = false;
        DestroyRooms = false;
        GetComponent<ObjectManager>().IplaceShower = false;
        GetComponent<ObjectManager>().DoorPlacement = false;
        ground_Object = null;
    }

    public void SetTruster(int hour)
    {
        if (state != null)
        {
            kompanie.truster.trusterplan[hour] = state;
            images[hour].GetComponentInChildren<Text>().text = state.name;
            images[hour].GetComponent<Image>().color = state.color;
        }
    }

    public void SetTrusterNames()
    {
        for (int i = 0; i < images.Length; i++)
        {
            images[i].GetComponentInChildren<Text>().text = kompanie.truster.trusterplan[i].name;
            images[i].GetComponent<Image>().color = kompanie.truster.trusterplan[i].color;
        }
    }

    public void SetDestinationWithAButton()
    {

        controller.getTargetPosition((GroundTile)map.MapData[256, 260]);
        controller.GetFinalPath();
    }

	public void ResetPerson()
	{
		controller.gameObject.transform.position = new Vector3 (256, 240, -2);
	}
}
