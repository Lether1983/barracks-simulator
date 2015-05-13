using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour 
{
    public GameObject spriteAtlas;
    public GameObject[] spritePrefab;
    public Sprite Grass;
    public Sprite Desert;
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

}
