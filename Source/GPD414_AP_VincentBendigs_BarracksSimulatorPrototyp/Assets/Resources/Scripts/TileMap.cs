using UnityEngine;
using System.Collections;
using System;

public class TileMap
{
    #region Fields
    public TileBaseClass[,] MapData;
    public static TileMap instance;
    public int BuyfieldSize = 44;
    public int StartFieldOfViewValue;
    public float purchasedLandWidthMax;
    public float purchasedLandWidthMin;
    public float purchasedLandHeightMax;
    public float purchasedLandHeightMin;
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
        BlackSprite = Resources.Load<Sprite>("Sprites/BlackTile");
        for (int i = 0; i < maxSize; i++)
        {
            for (int j = 0; j < maxSize; j++)
            {
                if(i > purchasedLandWidthMin && i < purchasedLandWidthMax && j > purchasedLandHeightMin && j < purchasedLandHeightMax)
                {
                    MapData[i, j] = new GroundTile(i, j, ShowMap.ShowMap(i, j,(int)purchasedLandWidthMin,(int)purchasedLandHeightMin));
                }
                else
                {
                    MapData[i, j] = new GroundTile(i, j,BlackSprite);
                }
            }
        }
    }

    public static TileMap Instance()
    {
        if (instance == null)
        {
            instance = new TileMap();
        }
        return instance;
    }
}
