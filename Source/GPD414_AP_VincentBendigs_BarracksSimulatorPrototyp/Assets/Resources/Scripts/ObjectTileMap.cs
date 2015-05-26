using UnityEngine;
using System.Collections;

public class ObjectTileMap 
{
    public ObjectBaseClass[,] ObjectData;
    public static ObjectTileMap instance;

    public void GenerateObjectDataMap(int MapSize)
    {
        ObjectData = new ObjectBaseClass[MapSize, MapSize];
    }

    public static ObjectTileMap Instance()
    {
        if (instance == null)
        {
            instance = new ObjectTileMap();
        }
        return instance;
    }
}
