using UnityEngine;
using System.Collections;

public class CaveRandomShowMap
{
    GameManager manager;
    int[,] Map;
    public int MapWidth;
    public int MapHeight;
    public int PercentAreWalls;

	public CaveRandomShowMap(int mapWidth,int mapHeight)
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        MapWidth = mapWidth * 2;
        MapHeight = mapHeight * 2;
        PercentAreWalls = 45;

        RandomFillMap();
    }

    public void MakeGrassFields()
    {
        for (int i = 0; i <= MapHeight-1; i++)
        {
            for (int j = 0; j <= MapWidth-1; j++)
            {
                Map[i, j] = PlaceDirtLogic(i, j);
            }
        }
    }

    private int PlaceDirtLogic(int i, int j)
    {
        int numOfDirt = GeAdjacentDirt(i, j, 1, 1);

        if(Map[i,j] == 1)
        {
            if(numOfDirt >= 4)
            {
                return 1;
            }
            if(numOfDirt < 2)
            {
                return 0;
            }
        }
        else
        {
            if(numOfDirt >= 5)
            {
                return 1;
            }
        }
        return 0;
    }

    private int GeAdjacentDirt(int i, int j, int p1, int p2)
    {
        int startX = i - p1;
        int startY = j - p2;
        int endX = i + p1;
        int endY = j + p2;

        int dirtCounter = 0;

        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                if(!(x == i && y == j))
                {
                    if(IsWall(x,y))
                    {
                        dirtCounter++;
                    }
                }
            }
        }
        return dirtCounter;
    }

    private bool IsWall(int x, int y)
    {
        if(IsOutOfBounds(x,y))
        {
            return true;
        }
        if(Map[x,y] == 1)
        {
            return true;
        }
        if(Map[x,y] == 0)
        {
            return false;
        }
        return false;
    }

    private bool IsOutOfBounds(int x, int y)
    {
        if(x < 0 || y < 0)
        {
            return true;
        }
        else if(x > MapWidth - 1 || y > MapHeight -1)
        {
            return true;
        }
        return false;
    }

    private void RandomFillMap()
    {
        Map = new int[MapWidth, MapHeight];
        int mapMiddle = 0;
        for (int i = 0; i < MapWidth; i++)
        {
            for (int j = 0; j < MapHeight; j++)
            {
                if(i == 0)
                {
                    Map[i, j] = 1;
                }
                else if(j == 0)
                {
                    Map[i, j] = 1;
                }
                else if(i == MapHeight - 1)
                {
                    Map[i, j] = 1;
                }
                else if(j == MapWidth - 1)
                {
                    Map[i, j] = 1;
                }
                else
                {
                    mapMiddle = (MapHeight / 2);
                    if(j == mapMiddle)
                    {
                        Map[i, j] = 0;
                    }
                    else
                    {
                        Map[i, j] = RandomPercent(PercentAreWalls);
                    }
                }
            }
        }
    }

    public Sprite ShowMap(int x,int y,int OffSetX,int OffsetY)
    {
        x = x - OffSetX;
        y = y - OffsetY;
        return Map[x, y] == 1 ? manager.Grass : manager.Desert;
    }

    private int RandomPercent(int PercentAreWalls)
    {
        if(PercentAreWalls >= Random.Range(1,100))
        {
            return 1;
        }
        return 0;
    }
}
