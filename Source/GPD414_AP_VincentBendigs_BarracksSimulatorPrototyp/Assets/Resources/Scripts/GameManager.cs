using UnityEngine;
using System.Collections;
using System;

enum SpriteNames { Desert,Water,Grass,BlackTile}

public class GameManager : MonoBehaviour 
{
    public GameObject spriteAtlas;
    TileMap map;
    ObjectTileMap objectMap;
    RoomMap roomMap;
    
    #region Sprites
    public Sprite Grass;
    public Sprite Desert;
    public Sprite Wall;
    public Sprite Beton;
    public Sprite Dusche;
    public Sprite RoomSprite;
    #endregion
    
    #region Menü Building Bools
    public bool BuildDesert;
    public bool BuildGrass;
    public bool BuildBeton;
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
        if (map.MapData[(int)hittedGameobject.transform.position.x, (int)hittedGameobject.transform.position.y].IsIndoor == false)
        {
            if (BuildDesert)
            {
                hittedGameobject.GetComponent<SpriteRenderer>().sprite = Desert;
                map.MapData[(int)hittedGameobject.transform.position.x, (int)hittedGameobject.transform.position.y].Texture = Desert;
            }
            else if (BuildGrass)
            {
                hittedGameobject.GetComponent<SpriteRenderer>().sprite = Grass;
                map.MapData[(int)hittedGameobject.transform.position.x, (int)hittedGameobject.transform.position.y].Texture = Grass;
            }
        }
        else if (map.MapData[(int)hittedGameobject.transform.position.x, (int)hittedGameobject.transform.position.y].IsIndoor)
        {
            if (BuildBeton)
            {
                hittedGameobject.GetComponent<SpriteRenderer>().sprite = Beton;
                map.MapData[(int)hittedGameobject.transform.position.x, (int)hittedGameobject.transform.position.y].Texture = Beton;
            }
        }
        if (BuildWalls)
        {
            map.MapData[(int)hittedGameobject.transform.position.x, (int)hittedGameobject.transform.position.y] = new WallTile(map.MapData[(int)hittedGameobject.transform.position.x, (int)hittedGameobject.transform.position.y]);
            hittedGameobject.GetComponent<SpriteRenderer>().sprite = Wall;
            map.MapData[(int)hittedGameobject.transform.position.x, (int)hittedGameobject.transform.position.y].Texture = Wall;
        }
    }

    public void DestroyOneTile(GameObject hittedGameobject)
    {
        if (map.MapData[(int)hittedGameobject.transform.position.x, (int)hittedGameobject.transform.position.y].IsIndoor == false)
        {
            if (DestroyTiles)
            {
                hittedGameobject.GetComponent<SpriteRenderer>().sprite = Desert;
                map.MapData[(int)hittedGameobject.transform.position.x, (int)hittedGameobject.transform.position.y].Texture = Desert;
            }
            else if (DestroyWalls)
            {
                map.MapData[(int)hittedGameobject.transform.position.x, (int)hittedGameobject.transform.position.y] = new GroundTile(map.MapData[(int)hittedGameobject.transform.position.x, (int)hittedGameobject.transform.position.y]);
                hittedGameobject.GetComponent<SpriteRenderer>().sprite = Desert;
                map.MapData[(int)hittedGameobject.transform.position.x, (int)hittedGameobject.transform.position.y].Texture = Desert;
            }
        }
        else if (map.MapData[(int)hittedGameobject.transform.position.x, (int)hittedGameobject.transform.position.y].IsIndoor)
        {
            if (DestroyTiles)
            {
                hittedGameobject.GetComponent<SpriteRenderer>().sprite = Beton;
                map.MapData[(int)hittedGameobject.transform.position.x, (int)hittedGameobject.transform.position.y].Texture = Beton;
            }
            else if (DestroyWalls)
            {
                map.MapData[(int)hittedGameobject.transform.position.x, (int)hittedGameobject.transform.position.y] = new GroundTile(map.MapData[(int)hittedGameobject.transform.position.x, (int)hittedGameobject.transform.position.y]);
                hittedGameobject.GetComponent<SpriteRenderer>().sprite = Beton;
                map.MapData[(int)hittedGameobject.transform.position.x, (int)hittedGameobject.transform.position.y].Texture = Beton;
            }
        }
    }

    public void PlaceObjectByClick(GameObject hittetObject)
    {
        if(this.GetComponent<ObjectManager>().IplaceShower)
        {
            hittetObject.GetComponent<SpriteRenderer>().sprite = Dusche;
            objectMap.ObjectData[(int)hittetObject.transform.position.x, (int)hittetObject.transform.position.y].Texture = Dusche;
        }
    }

    public void PlaceRoomByClickOnMap(GameObject hittetObjectForRoom)
    {
        hittetObjectForRoom.GetComponent<SpriteRenderer>().sprite = RoomSprite;
        roomMap.RoomData[(int)hittetObjectForRoom.transform.position.x, (int)hittetObjectForRoom.transform.position.y].Texture = RoomSprite;
    }

    public void ResetAllBuildingModi()
    {
        BuildDesert = false;
        BuildGrass = false;
        BuildWalls = false;
        BuildBeton = false;
        BuildFoundation = false;
        InRoomBuildMode = false;
        InObjectBuildMode = false;
        DestroyTiles = false;
        DestroyModus = false;
        DestroyWalls = false;
        DestroyFoundation = false;
        GetComponent<ObjectManager>().IplaceShower = false;
    }
}
