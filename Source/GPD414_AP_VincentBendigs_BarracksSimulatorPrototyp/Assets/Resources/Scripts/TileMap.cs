using UnityEngine;
using System.Collections;
using System;

public class TileMap
{
    public TileBaseClass[,] MapData;
    public static TileMap instance;
    public int BuyfieldSize = 44;
    public int StartFieldOfViewValue;
    public float purchasedLandWidthMax;
    public float purchasedLandWidthMin;
    public float purchasedLandHeightMax;
    public float purchasedLandHeightMin;

    int offSet = 1;


    public void GenerateStartMap(int MapSize)
    {
        MapData = new TileBaseClass[MapSize, MapSize];
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
        for (int i = 0; i < maxSize; i++)
        {
            for (int j = 0; j < maxSize; j++)
            {
                if(i > purchasedLandWidthMin && i < purchasedLandWidthMax && j > purchasedLandHeightMin && j < purchasedLandHeightMax)
                {
                    MapData[i, j] = new GroundTile(i, j);
                }
                else
                {
                    MapData[i, j] = new GroundTile(i, j);
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
