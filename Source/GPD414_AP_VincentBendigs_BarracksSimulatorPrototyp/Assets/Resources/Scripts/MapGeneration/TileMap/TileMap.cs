using UnityEngine;
using System.Collections;
using System;
//TODO: Whitespace (mehr als eine Zeile) entfernen!
public class TileMap
{
    /* TODO: Indexer implementieren.
    Es wäre eventuell angenehmer, wenn statt TileMap.Instance.MapData[x, y]
    TileMap.Instance[x, y] verwendet würde und die Logik zum Mapping von
    x >= 0 & x < Width
    y >= 0 & y < Height.
    Beispiel:
    public TileBaseClass this[int x, int y]
    {
        get
        {
            x = Mathf.Clamp(x, 0, MapData.GetLength(0));
            y = Mathf.Clamp(y, 0, MapData.GetLength(1));
            return mapData[x, y];
        }
    }
    Zudem kann die float-Prüfung ebenfalls auf den Indexer verschoben werden.
    public TileBaseClass this[float x, float y]
    {
        get
        {
            x = Mathf.FloorToInt(Mathf.Clamp(x, 0, MapData.GetLength(0)));
            y = Mathf.FloorToInt(Mathf.Clamp(x, 0, MapData.GetLength(1)));
            return mapData[x, y];
        }
    }
    Außerdem könnte in einen Indexer auch ein Vektor eingebaut werden:
    public TileBaseClass this[Vector2 v]
    {
        get
        {
            return this[v.x, v.y];
        }
    }
    mapData ist dann ein privates Feld.
    */

    #region Fields
    public TileBaseClass[,] MapData;
    public static TileMap instance; //TODO: Meine Meinung: statische Werte an den Anfang schreiben.
    public int BuyfieldSize = 44;
    public int StartFieldOfViewValue;
    public float purchasedLandWidthMax; //TODO: Die Breite ist doch eine Ganzzahl.
    public float purchasedLandWidthMin; //TODO: Die Breite ist doch eine Ganzzahl.
    public float purchasedLandHeightMax; //TODO: Die Höhe ist doch eine Ganzzahl.
    public float purchasedLandHeightMin; //TODO: Die Höhe ist doch eine Ganzzahl.
    #endregion
  
    CaveRandomShowMap ShowMap;
    int offSet = 2;
    Sprite BlackSprite;


    public void GenerateStartMap(int MapSize)
    {
        MapData = new TileBaseClass[MapSize, MapSize];
        ShowMap = new CaveRandomShowMap(StartFieldOfViewValue, StartFieldOfViewValue);
        ShowMap.MakeGrassFields();
        generateAFieldOfView(MapSize);
        FillStartField(MapSize);
        GetNeighborsTiles(MapSize);
        
    }

    void generateAFieldOfView(int maxSize)
    {
        purchasedLandHeightMin = ((maxSize / 2) - (StartFieldOfViewValue - offSet));
        purchasedLandHeightMax = ((maxSize / 2) + (StartFieldOfViewValue - offSet));
        purchasedLandWidthMin = ((maxSize / 2) - (StartFieldOfViewValue - offSet));
        purchasedLandWidthMax = ((maxSize / 2) + (StartFieldOfViewValue - offSet));
    }

    private void FillStartField(int maxSize)
    {
        BlackSprite = Resources.Load<Sprite>("Sprites/GroundTile/BlackTile");
        for (int i = 0; i < maxSize; i++)
        {
            for (int j = 0; j < maxSize; j++)
            {
                if(i > purchasedLandWidthMin && i < purchasedLandWidthMax && j > purchasedLandHeightMin && j < purchasedLandHeightMax)
                {
                    MapData[i, j] = new GroundTile(i,j);
                    MapData[i, j].GetAllValues(ShowMap.ShowMap(i, j, (int)purchasedLandWidthMin, (int)purchasedLandHeightMin));
                }
                else
                {
                    MapData[i, j] = new GroundTile(i, j,BlackSprite);
                }
            }
        }
    }

    void GetNeighborsTiles(int MapSize)
    {
        for (int i = 0; i <MapSize ; i++)
        {
            for (int j = 0; j < MapSize; j++)
            {
                MapData[i, j].GetYourNeighbors();
            }
        }
    }




    //TODO: Hieraus eine Property machen.
    /*
    public static TileMap Instance
    {
        get
        {
            if (instance == null) instance = new TileMap();
            return instance;
        }
    }
    */
    public static TileMap Instance()
    {
        if (instance == null)
        {
            instance = new TileMap();
        }
        return instance;
    }
}
