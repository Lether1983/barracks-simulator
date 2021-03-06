﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class AStarManager : MonoBehaviour
{
    public TileMap tileMap;
    public int heuristicValue;



    void Awake()
    {
        tileMap = TileMap.Instance;
    }

    public int GetTotalCost(int currentTotalCost,int nextTotalCost)
    {
        int newTotalcost = currentTotalCost + nextTotalCost;
        return newTotalcost;
    }

    public IEnumerator GetFinalPath(GroundTile rootNode,GroundTile destination,List<GroundTile> OpenList,List<GroundTile> ClosedList,int totalCost, Action<GroundTile> result)
    {
        GroundTile currentNode = rootNode;
        int counter = 0;

        if (currentNode.Position == destination.Position)
        {
            result(destination);
            yield break;
        }
        else
        {
            while (OpenList.Count > 0)
            {
                currentNode = OpenList[0];

                if (currentNode.Position == destination.Position)
                {
                    result(currentNode);
                    yield break;
                }
                else
                {
                    if (currentNode.IsPassible)
                    {
                        for (int j = 0; j < tileMap.MapData[(int)currentNode.Position.x, (int)currentNode.Position.y].GetNeighbors().Count; j++)
                        {
                            GroundTile TempGroundTile = (GroundTile)tileMap.MapData[(int)currentNode.Position.x, (int)currentNode.Position.y].GetNeighbors()[j];
                            int tempTotalCost = GetTotalCost(currentNode.totalCost, TempGroundTile.movementCost);

                            GroundTile ResultOpenTile;
                            GroundTile ResultCloseTile;
                            ResultOpenTile = OpenList.Find(OpenTile => OpenTile.Position == TempGroundTile.Position);
                            ResultCloseTile = ClosedList.Find(ClosedTile => ClosedTile.Position == TempGroundTile.Position);
                            if (ResultOpenTile != null)
                            {
                                if (currentNode.totalCost > tempTotalCost)
                                {
                                    TempGroundTile.totalCost = ResetNodeInList(destination, OpenList, currentNode, j, tempTotalCost, ResultOpenTile);
                                }
                                continue;
                            }
                            else if (ResultCloseTile != null)
                            {
                                if (totalCost > tempTotalCost)
                                {
                                    ClosedList.Remove(ResultCloseTile);
                                    TempGroundTile.totalCost = ResetNodeInList(destination, OpenList, currentNode, j, tempTotalCost, ResultCloseTile);
                                }
                                continue;
                            }
                            AddNewNodeToOpenList(destination, OpenList, currentNode, j);
                        }
                    }
                    OpenList.Remove(currentNode);
                    ClosedList.Insert(0,currentNode);
                }
                if(counter++ > 20)
                {
                    yield return currentNode;
                    counter = 0;
                }
            }
        }
    }

    private void AddNewNodeToOpenList(GroundTile destination, List<GroundTile> OpenList, GroundTile currentNode, int j)
    {
        Vector2 CalculateWaypoint = new Vector2(0.5f, 0.5f);
        GroundTile newNode;
        newNode = (GroundTile)tileMap.MapData[(int)currentNode.Position.x, (int)currentNode.Position.y].GetNeighbors()[j].Clone();

        newNode.totalCost = GetTotalCost(currentNode.totalCost,newNode.movementCost);
        newNode.finalCost = CalculateFinalCost(Vector2.Distance(newNode.Position + CalculateWaypoint, destination.Position + CalculateWaypoint), heuristicValue, newNode.totalCost);
        newNode.parentWaypoint = currentNode;

        OpenList.Add(newNode);
        
        SortOpenListWithFinalCost(OpenList);
    }

    private int ResetNodeInList(GroundTile destination, List<GroundTile> OpenList, GroundTile currentNode, int j, int tempTotalCost, GroundTile item)
    {
        int totalCost = GetTotalCost(currentNode.totalCost, item.movementCost);
        item.finalCost = CalculateFinalCost(Vector2.Distance(tileMap.MapData[(int)currentNode.Position.x, (int)currentNode.Position.y].GetNeighbors()[j].Position, destination.Position), heuristicValue, tempTotalCost);

        SortOpenListWithFinalCost(OpenList);

        return totalCost;
    }

    public float CalculateFinalCost(float Distanz,int heuristicValue,int totalCost)
    {
        return Distanz * heuristicValue + totalCost;  
    }

    void SortOpenListWithFinalCost(List<GroundTile> OpenList)
    {
        QuickSort quickSort = new QuickSort();
        quickSort.Sort(OpenList,0,OpenList.Count-1);
    }
}
