using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class TileBaseClass : ICloneable
{
    public bool IsPassible;
    public bool IsIndoor = false;
    public bool IsOutdoor;
    public bool isOverridable;
    public float Duarbility;
    public int totalCost;
    public Vector2 Position;
    public GameObject myObject;
    public int movementCost;
    public Sprite Texture;
    protected List<TileBaseClass> neighbors;
    protected TileMap map = TileMap.Instance();
    int temp = 0;


    public virtual List<TileBaseClass> GetNeighbors()
    {
        return neighbors;
    }

    public void GetYourNeighbors()
    {
        neighbors = new List<TileBaseClass>();
        for (int i = (int)Position.x - 1; i <= (int)Position.x + 1; i++)
        {
            for (int j = (int)Position.y - 1; j <= (int)Position.y + 1; j++)
            {
                if (i >= 0 && i < map.MapData.GetLength(0) && j >= 0 && j < map.MapData.GetLength(1))
                {
                    if (i == Position.x && j == Position.y)
                    {
                        continue;
                    }
                    else
                    {
                        neighbors.Add(map.MapData[i, j]);
                    }
                }
            }
        }
    }
    public void GetAllValues(GroundObject groundObject)
    {
        this.IsPassible = groundObject.isPassible;
        this.isOverridable = groundObject.isOverridable;
        this.IsOutdoor = groundObject.isOutdoor;
        this.IsIndoor = groundObject.isIndoor;
        this.movementCost = groundObject.movementCost;
        this.Duarbility = groundObject.Duarbility;
        this.Texture = groundObject.texture;
        if (this.myObject != null)
        {
            myObject.GetComponent<SpriteRenderer>().sprite = groundObject.texture;
        }
    }
    public System.Object Clone()
    {
        return this.MemberwiseClone();
    }
}
