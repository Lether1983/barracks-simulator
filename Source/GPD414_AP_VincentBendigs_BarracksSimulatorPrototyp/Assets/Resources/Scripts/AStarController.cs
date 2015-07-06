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
    }

    void getOwnPosition()
    {
        rootNode.Position = gameObject.transform.position;
    }

    public void getFinalPath()
    {
        finalPath = controller.GetFinalPath(rootNode,destination,OpenList,ClosedList,totalCost);
    }

    void AddRootToOpenList()
    {
        OpenList.Insert(0,(GroundTile)rootNode.Clone());
    }

    float CalulateDistanz()
    {
        return Vector2.Distance(rootNode.Position, destination.Position);
    }
}
