using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AStarManager : MonoBehaviour
{
    public int heuristicValue;
    public TileMap tileMap;
    List<GroundTile> way;
    float finalCost;

    public int GetTotalCost(int currentTotalCost,int nextTotalCost)
    {
        int newTotalcost = currentTotalCost + nextTotalCost;
        return newTotalcost;
    }

    public List<GroundTile> GetFinalPath(GroundTile rootNode,GroundTile destination,List<GroundTile> OpenList,List<GroundTile> ClosedList,int totalCost)
    {
        GroundTile currentNode = rootNode;

        if (currentNode == destination)
        {
            way = new List<GroundTile>();
            way.Insert(0,currentNode);
            return way;
        }
        else
        {
            while (OpenList.Count > 0)
            {
                currentNode = OpenList.First();

                if (currentNode == destination)
                {
                    way.Insert(0,currentNode);
                    return way;
                }
                else
                {

                    for (int j = 0; j < tileMap.MapData[(int)currentNode.Position.x, (int)currentNode.Position.y].GetNeighbors().Length; j++)
                    {
                        int tempTotalCost = GetTotalCost(totalCost, tileMap.MapData[(int)currentNode.Position.x, (int)currentNode.Position.y].GetNeighbors()[j].movementCost);

                        foreach (var item in OpenList)
                        {
                            if (OpenList.Contains((GroundTile)tileMap.MapData[(int)currentNode.Position.x, (int)currentNode.Position.y].GetNeighbors()[j]) && totalCost > tempTotalCost)
                            {
                                ResetNodeInList(destination, OpenList, currentNode, j, tempTotalCost, item);

                                totalCost = AddNewNodeToOpenList(destination, OpenList, totalCost, currentNode, j);
                            }
                            else
                            {
                                break;
                            }
                        }

                        foreach (var item in ClosedList)
                        {
                            if (ClosedList.Contains((GroundTile)tileMap.MapData[(int)currentNode.Position.x, (int)currentNode.Position.y].GetNeighbors()[j]) && totalCost > tempTotalCost)
                            {
                                ResetNodeInList(destination, OpenList, currentNode, j, tempTotalCost, item);

                                totalCost = AddNewNodeToOpenList(destination, OpenList, totalCost, currentNode, j);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    ClosedList.Insert(0,currentNode);
                }
            }
        }
        return way;
    }

    private int AddNewNodeToOpenList(GroundTile destination, List<GroundTile> OpenList, int totalCost, GroundTile currentNode, int j)
    {
        GroundTile newNode;
        newNode = (GroundTile)tileMap.MapData[(int)currentNode.Position.x, (int)currentNode.Position.y].GetNeighbors()[j];

        totalCost = GetTotalCost(totalCost,currentNode.movementCost);
        newNode.finalCost = CalculateFinalCost(Vector2.Distance(newNode.Position, destination.Position), heuristicValue, totalCost);
        newNode.parentWaypoint = currentNode;

        if (OpenList[0].finalCost < newNode.finalCost)
        {
            OpenList.Add((GroundTile)newNode.Clone());
        }
        else
        {
            OpenList.Insert(0,(GroundTile)newNode.Clone());
        }
        return totalCost;
    }

    private void ResetNodeInList(GroundTile destination, List<GroundTile> OpenList, GroundTile currentNode, int j, int tempTotalCost, GroundTile item)
    {
        item.finalCost = CalculateFinalCost(Vector2.Distance(tileMap.MapData[(int)currentNode.Position.x, (int)currentNode.Position.y].GetNeighbors()[j].Position, destination.Position), heuristicValue, tempTotalCost);

        OpenList.Remove(item);

        if (OpenList[0].finalCost < item.finalCost)
        {
            OpenList.Add((GroundTile)tileMap.MapData[(int)currentNode.Position.x, (int)currentNode.Position.y].GetNeighbors()[j].Clone());
        }
        else
        {
            OpenList.Insert(0,(GroundTile)tileMap.MapData[(int)currentNode.Position.x, (int)currentNode.Position.y].GetNeighbors()[j].Clone());
        }
    }

    public float CalculateFinalCost(float Distanz,int heuristicValue,int totalCost)
    {
        finalCost = Distanz * heuristicValue + totalCost;

        return finalCost;   
    }

    void SortOpenListWithFinalCost(List<GroundTile> OpenList)
    {
        QuickSort quickSort = new QuickSort();
        quickSort.Sort(OpenList,0,OpenList.Count-1);
    }
}
