using UnityEngine;
using System.Collections;
using System;

enum SpriteNames { Desert,Water,Grass,BlackTile}

public class GameManager : MonoBehaviour 
{
    public GameObject spriteAtlas;
    public Sprite Grass;
    public Sprite Desert;
    public bool BuildDesert;
    public bool BuildGrass;


    int bigMaxMapSize = 512;
    int smallMaxMapSize = 128;
    int middMaxMapSize = 256;
    bool smallMap = false;
    bool midMap = false;
    bool largeMap = true;
    

	// Use this for initialization
	void Awake()
    {
        TileMap map = TileMap.Instance();
        Grass = Resources.Load<Sprite>("Sprites/Grass");
        Desert = Resources.Load<Sprite>("Sprites/Desert");
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
        }
        else if(BuildGrass)
        {
            hittedGameobject.GetComponent<SpriteRenderer>().sprite = Grass;
        }
    }
}
