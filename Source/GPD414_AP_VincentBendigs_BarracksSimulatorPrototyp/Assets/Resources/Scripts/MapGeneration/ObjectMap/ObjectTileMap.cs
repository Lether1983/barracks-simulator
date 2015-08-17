using UnityEngine;
using System.Collections;

public class ObjectTileMap
{
    public ObjectBaseClass[,] ObjectData;

    public static ObjectTileMap instance;

    public void GenerateObjectDataMap(int MapSize)
    {
        ObjectData = new ObjectBaseClass[MapSize, MapSize];
        for (int i = 0; i < MapSize; i++)
        {
            for (int j = 0; j < MapSize; j++)
            {
                ObjectData[i, j] = new ObjectBaseClass() { Position = new Vector2(i, j) };
            }
        }
    }

    //TODO: Property draus machen. Siehe TileMap.
    public static ObjectTileMap Instance()
    {
        if (instance == null)
        {
            instance = new ObjectTileMap();
        }
        return instance;
    }
}
