﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DH.Messaging.Bus;

public class AStarController : MonoBehaviour 
{
    AStarManager controller;
    MessageSubscription<int> subscribtion;
    public Stack<GroundTile> finalPath;
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
        finalPath = new Stack<GroundTile>();
        OpenList = new List<GroundTile>();
        ClosedList = new List<GroundTile>();
        subscribtion = MessageBusManager.Subscribe<int>("Reachtarget");
        subscribtion.OnMessageReceived += changeMessage_OnMessageReceived;
    }

    private void changeMessage_OnMessageReceived(MessageSubscription<int> s, MessageReceivedEventArgs<int> args)
    {
        if (args.Message == 1 && finalPath.Count > 0)
        {
             this.GetComponent<Soldiers>().Move(finalPath.Pop().Position);
        }
    }

    public int GetTotalCost(int currentTotalCost, int nextTotalCost)
    {
        int newTotalcost = currentTotalCost + nextTotalCost;
        return newTotalcost;
    }

    void getOwnPosition()
    {
        rootNode = (GroundTile)controller.tileMap.MapData[(int)this.gameObject.transform.position.x, (int)this.gameObject.transform.position.y];
    }

    public void getTargetPosition(GroundTile target)
    {
        destination = target;
    }

    public float CalculateFinalCost(float Distanz, int heuristicValue, int totalCost)
    {
         float finalCost = Distanz * heuristicValue + totalCost;

        return finalCost;
    }

    public void GetFinalPath()
    {
        ClosedList.Clear();
        OpenList.Clear();
        getOwnPosition();
        rootNode.totalCost = GetTotalCost(rootNode.totalCost, rootNode.movementCost);
        rootNode.finalCost = CalculateFinalCost(CalulateDistanz(),controller.heuristicValue, rootNode.totalCost);
        AddRootToOpenList();
        finalPath.Push(controller.GetFinalPath(rootNode, destination, OpenList, ClosedList, totalCost));
        SetWay();
        this.GetComponent<Soldiers>().Move(finalPath.Pop().Position);
    }

    void SetWay()
    {
        while (finalPath.Peek().parentWaypoint != null)
        {
            if (finalPath.Peek().parentWaypoint == null) break;
            finalPath.Push(finalPath.Peek().parentWaypoint);
        }
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