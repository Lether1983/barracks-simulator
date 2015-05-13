using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AStarManager : MonoBehaviour
{
    public int heuristicValue;
    public GroundTileMap tileMap;
    LinkedList<GroundTile> way;
    float finalCost;

    public int GetTotalCost(int currentTotalCost,int nextTotalCost)
    {
        int newTotalcost = currentTotalCost + nextTotalCost;
        return newTotalcost;
    }

    public LinkedList<GroundTile> GetFinalPath(GroundTile rootNode,GroundTile destination,LinkedList<GroundTile> OpenList,LinkedList<GroundTile> ClosedList,int totalCost)
    {
        GroundTile currentNode = rootNode;

        if (currentNode == destination)
        {
            way = new LinkedList<GroundTile>();
            way.AddFirst(currentNode);
            return way;
        }
        else
        {
            for (int i = 0; i < OpenList.Count; i++)
            {
                currentNode = OpenList.ElementAt(0);

                if (currentNode == destination)
                {
                    way.AddFirst(currentNode);
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
                                ResetNodeInList(destination,OpenList,currentNode,j,tempTotalCost,item);

                                totalCost = AddNewNodeToOpenList(destination,OpenList,totalCost,currentNode,j);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    ClosedList.AddFirst(currentNode);
                }
            }
        }
        return way;
    }

    private int AddNewNodeToOpenList(GroundTile destination, LinkedList<GroundTile> OpenList, int totalCost, GroundTile currentNode, int j)
    {
        GroundTile newNode;
        newNode = (GroundTile)tileMap.MapData[(int)currentNode.Position.x, (int)currentNode.Position.y].GetNeighbors()[j];

        totalCost = GetTotalCost(totalCost,currentNode.movementCost);
        newNode.finalCost = CalculateFinalCost(Vector2.Distance(newNode.Position, destination.Position), heuristicValue, totalCost);
        newNode.parentWaypoint = currentNode;

        if (OpenList.First.Value.finalCost < newNode.finalCost)
        {
            OpenList.AddAfter(OpenList.First,(GroundTile)newNode.Clone());
        }
        else
        {
            OpenList.AddFirst((GroundTile)newNode.Clone());
        }
        return totalCost;
    }

    private void ResetNodeInList(GroundTile destination, LinkedList<GroundTile> OpenList, GroundTile currentNode, int j, int tempTotalCost, GroundTile item)
    {
        item.finalCost = CalculateFinalCost(Vector2.Distance(tileMap.MapData[(int)currentNode.Position.x, (int)currentNode.Position.y].GetNeighbors()[j].Position, destination.Position), heuristicValue, tempTotalCost);

        OpenList.Remove(item);

        if (OpenList.First.Value.finalCost < item.finalCost)
        {
            OpenList.AddAfter(OpenList.First, (GroundTile)tileMap.MapData[(int)currentNode.Position.x, (int)currentNode.Position.y].GetNeighbors()[j].Clone());
        }
        else
        {
            OpenList.AddFirst((GroundTile)tileMap.MapData[(int)currentNode.Position.x, (int)currentNode.Position.y].GetNeighbors()[j].Clone());
        }
    }

    public float CalculateFinalCost(float Distanz,int heuristicValue,int totalCost)
    {
        finalCost = Distanz * heuristicValue + totalCost;

        return finalCost;   
    }

    void SortOpenListWithFinalCost(LinkedList<GroundTile> OpenList)
    {
        OpenList.OrderBy(e => e.finalCost);
    }
}
