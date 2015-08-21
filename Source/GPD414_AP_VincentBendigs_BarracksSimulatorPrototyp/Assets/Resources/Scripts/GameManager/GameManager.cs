using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.UI;
using DH.Messaging.Bus;

enum MapSize { small = 128,medium = 256,large = 512};
public class GameManager : MonoBehaviour
{
    public float speed;

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

    MessageSubscription<int> subscribtion;

    public KompanieObject kompanie;
    public trusterState state;

    public List<ObjectLogicObject> AllObjects;
    public List<Soldiers> AllSoldiers;
    public List<Soldiers> AllCivilians;


    public AStarController controller;
    public AStarController controller2;


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

    //TODO: Hieraus ein Enum machen! Mit {Small, Medium, Large}
    bool smallMap = false;
    bool midMap = false;
    bool largeMap = true;


    // Use this for initialization
    void Awake()
    {
        subscribtion = MessageBusManager.Subscribe<int>("ChangeHour");
        subscribtion.OnMessageReceived += changeMessage_OnMessageReceived;
        AllObjects = new List<ObjectLogicObject>();
        AllCivilians = new List<Soldiers>();
        AllSoldiers = new List<Soldiers>();
        map = TileMap.Instance;
        objectMap = ObjectTileMap.Instance;
        roomMap = RoomMap.Instance;
        GenerateAMap(map);
    }

    private void changeMessage_OnMessageReceived(MessageSubscription<int> s, MessageReceivedEventArgs<int> args)
    {
        foreach (var item in AllObjects)
        {
            item.Update(this.gameObject.GetComponent<WorkManager>());
        }
    }

    void GenerateAMap(TileMap map)
    {
        //TODO: Enum
        if (smallMap)
        {
            map.StartFieldOfViewValue = 20;
            map.GenerateStartMap((int)MapSize.small);
            objectMap.GenerateObjectDataMap((int)MapSize.small);
            roomMap.GenerateRoomObjects((int)MapSize.small);
        }
        else if (midMap)
        {
            map.StartFieldOfViewValue = 40;
            map.GenerateStartMap((int)MapSize.medium);
            objectMap.GenerateObjectDataMap((int)MapSize.medium);
            roomMap.GenerateRoomObjects((int)MapSize.medium);
        }
        else if (largeMap)
        {
            map.StartFieldOfViewValue = 80;
            map.GenerateStartMap((int)MapSize.large);
            objectMap.GenerateObjectDataMap((int)MapSize.large);
            roomMap.GenerateRoomObjects((int)MapSize.large);
        }
    }

    public void ChangeTileByClick(GameObject hittedGameobject)
    {
        if (ground_Object == null)
        {
            return;
        }
        //TODO: Indexer in Map (vereinfacht vieles)
        TileBaseClass tile = map.MapData[(int)hittedGameobject.transform.position.x, (int)hittedGameobject.transform.position.y];
        if (tile.IsOutdoor)
        {
            if (tile.isOverridable)
            {
                if (ground_Object.isOutdoor)
                {
                    hittedGameobject.GetComponent<SpriteRenderer>().sprite = ground_Object.texture;
                    tile.GetAllValues(ground_Object);
                }
                if (ground_Object.isOverridable == false)
                {
                    hittedGameobject.GetComponent<SpriteRenderer>().sprite = ground_Object.texture;
                    tile.GetAllValues(ground_Object);
                }
            }
        }
        else if (tile.IsIndoor)
        {
            if (tile.isOverridable)
            {
                if (ground_Object.isIndoor)
                {
                    hittedGameobject.GetComponent<SpriteRenderer>().sprite = ground_Object.texture;
                    tile.GetAllValues(ground_Object);
                }
                if (ground_Object.isOverridable == false)
                {
                    hittedGameobject.GetComponent<SpriteRenderer>().sprite = ground_Object.texture;
                    tile.GetAllValues(ground_Object);
                }
            }
        }
    }

    public void DestroyOneTile(GameObject hittedGameobject)
    {
        TileBaseClass tile = map.MapData[(int)hittedGameobject.transform.position.x, (int)hittedGameobject.transform.position.y];
        if (tile.IsIndoor == false)
        {
            if (DestroyTiles)
            {
                hittedGameobject.GetComponent<SpriteRenderer>().sprite = OutdoorDefault.texture;
                tile.GetAllValues(OutdoorDefault);
            }
            else if (DestroyWalls || GetComponent<ObjectManager>().DoorPlacement)
            {
                hittedGameobject.GetComponent<SpriteRenderer>().sprite = OutdoorDefault.texture;
                tile.GetAllValues(OutdoorDefault);
            }
        }
        else if (tile.IsIndoor)
        {
            if (DestroyTiles)
            {
                hittedGameobject.GetComponent<SpriteRenderer>().sprite = IndoorDefault.texture;
                tile.GetAllValues(IndoorDefault);
            }
            else if (DestroyWalls || GetComponent<ObjectManager>().DoorPlacement)
            {
                hittedGameobject.GetComponent<SpriteRenderer>().sprite = IndoorDefault.texture;
                tile.GetAllValues(IndoorDefault);
            }
        }
    }

    public void PlaceObjectByClick(GameObject hittetObject)
    {
        if (object_object == null)
        {
            return;
        }
        for (int i = 0; i < object_object.infos.Length; i++)
        {
            ObjectBaseClass baseObject = objectMap.ObjectData[(int)hittetObject.transform.position.x, (int)hittetObject.transform.position.y];
            Vector2 Reference = baseObject.@object.position;
            Vector2 HittetTransform = hittetObject.transform.position;
            Vector2 delta = HittetTransform - Reference;

            if (object_object.infos[i].delta == delta)
            {
                hittetObject.GetComponent<SpriteRenderer>().sprite = object_object.infos[i].texture;
                baseObject.Texture = object_object.infos[i].texture;
            }
        }
    }

    public void PlaceRoomByClickOnMap(GameObject hittetObjectForRoom)
    {
        if (room_object == null) return;
        RoomBaseClass baseRoom = roomMap.RoomData[(int)hittetObjectForRoom.transform.position.x, (int)hittetObjectForRoom.transform.position.y];

        for (int i = 0; i < room_object.infos.Length; i++)
        {
            Vector2 Reference = baseRoom.room.Position;
            Vector2 HittetTransform = hittetObjectForRoom.transform.position;
            Vector2 delta = HittetTransform - Reference;
            if (room_object.infos[i].delta == delta)
            {
                hittetObjectForRoom.GetComponent<SpriteRenderer>().sprite = room_object.infos[i].texture;
                baseRoom.Texture = room_object.infos[i].texture;
                return;
            }
        }
        hittetObjectForRoom.GetComponent<SpriteRenderer>().sprite = room_object.defaultTexture;
        baseRoom.Texture = room_object.defaultTexture;
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
}
