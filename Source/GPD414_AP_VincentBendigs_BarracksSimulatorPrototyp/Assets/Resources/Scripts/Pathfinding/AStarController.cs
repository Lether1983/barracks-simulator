using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarController : MonoBehaviour 
{
    AStarManager controller;
    List<GroundTile> finalPath;
    public List<GroundTile> OpenList;
    List<GroundTile> ClosedList;
    GroundTile rootNode;
    GroundTile destination;
    float Distanz = 0;
    int rootTotalCost = 0;
    public int totalCost = 0;

    void Awake()
    {
        controller = GameObject.Find("GameManager").GetComponent<AStarManager>();
        OpenList = new List<GroundTile>();
        ClosedList = new List<GroundTile>();
    }

    void Start()
    {
        
        getFinalPath();
    }

    public int GetTotalCost(int currentTotalCost, int nextTotalCost)
    {
        int newTotalcost = currentTotalCost + nextTotalCost;
        return newTotalcost;
    }

    void getOwnPosition()
    {
        rootNode.Position = gameObject.transform.position;
    }

    public float CalculateFinalCost(float Distanz, int heuristicValue, int totalCost)
    {
         float finalCost = Distanz * heuristicValue + totalCost;

        return finalCost;
    }
    public void getFinalPath()
    {
        TileMap map = controller.tileMap;
        rootNode = (GroundTile)controller.tileMap.MapData[250, 250];
        destination = (GroundTile)controller.tileMap.MapData[255, 255];
        rootNode.totalCost = GetTotalCost(rootNode.totalCost, rootNode.movementCost);
        rootNode.finalCost = CalculateFinalCost(CalulateDistanz(),controller.heuristicValue, rootNode.totalCost);
        AddRootToOpenList();
        finalPath = controller.GetFinalPath(rootNode,destination,OpenList,ClosedList,totalCost);
    }

    void AddRootToOpenList()
    {
        OpenList.Insert(0,(GroundTile)rootNode.Clone());
    }

    float CalulateDistanz()
    {
        Vector2 CalculateWaypoint = new Vector2(0.5f, 0.5f);
        return Vector2.Distance(rootNode.Position + CalculateWaypoint, destination.Position + CalculateWaypoint);
    }
}
