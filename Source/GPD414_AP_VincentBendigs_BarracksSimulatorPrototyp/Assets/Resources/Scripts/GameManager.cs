using UnityEngine;
using System.Collections;
using System;

enum SpriteNames { Desert,Water,Grass,BlackTile}

public class GameManager : MonoBehaviour 
{
    public GameObject spriteAtlas;
    TileMap map;
    public Sprite Grass;
    public Sprite Desert;
    public Sprite Wall;
    public bool BuildDesert;
    public bool BuildGrass;
    public bool BuildWalls;


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
        Grass = Resources.Load<Sprite>("Sprites/Grass");
        Desert = Resources.Load<Sprite>("Sprites/Desert");
        Wall = Resources.Load<Sprite>("Sprites/Wall");
        GenerateAMap(map);
    }
    
    void GenerateAMap(TileMap map)
    {
        if(smallMap)
        {
            map.StartFieldOfViewValue = 20;
            map.GenerateStartMap(smallMaxMapSize);
        }
        else if(midMap)
        {
            map.StartFieldOfViewValue = 40;
            map.GenerateStartMap(middMaxMapSize);
        }
        else if(largeMap)
        {
            map.StartFieldOfViewValue = 80;
            map.GenerateStartMap(bigMaxMapSize);
        }
    }
    public void ChangeTileByClick(GameObject hittedGameobject)
    {
        if(BuildDesert)
        {
            hittedGameobject.GetComponent<SpriteRenderer>().sprite = Desert;
            map.MapData[(int)hittedGameobject.transform.position.x,(int)hittedGameobject.transform.position.y].Texture = Desert;
        }
        else if(BuildGrass)
        {
            hittedGameobject.GetComponent<SpriteRenderer>().sprite = Grass;
            map.MapData[(int)hittedGameobject.transform.position.x, (int)hittedGameobject.transform.position.y].Texture = Grass;
        }
        else if(BuildWalls)
        {
            hittedGameobject.GetComponent<SpriteRenderer>().sprite = Wall;
            map.MapData[(int)hittedGameobject.transform.position.x, (int)hittedGameobject.transform.position.y].Texture = Wall;
        }
    }
}
