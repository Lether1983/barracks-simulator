﻿using UnityEngine;
using System.Collections;

public class RoomMap 
{
    public static RoomMap instance;

    public RoomBaseClass[,] RoomData;

    public void GenerateRoomObjects(int MapSize)
    {
        RoomData = new RoomBaseClass[MapSize, MapSize];
        for (int i = 0; i < MapSize; i++)
        {
            for (int j = 0; j < MapSize; j++)
            {
                RoomData[i, j] = new RoomBaseClass();
            }
        }
    }

    public static RoomMap Instance()
    {
        if (instance == null)
        {
            instance = new RoomMap();
        }
        return instance;
    }
}