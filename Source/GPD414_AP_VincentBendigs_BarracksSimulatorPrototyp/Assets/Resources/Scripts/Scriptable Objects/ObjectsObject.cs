﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public struct SpriteInfo
{
    public Vector2 delta;
    public Sprite texture;
}

[System.Flags]
public enum UseableObjects : int
{ 
    Updateable = 1 << 0,
    Storable = 1 << 1,
    CanStore = 1 << 2,
    Useable = 1 << 3,
    ReduceHunger = 1 << 4,
    ReduceToilette = 1 << 5,
    ReduceHomeIll = 1 << 6,
    ReduceDiversity = 1 << 7,
    ReduceFitness = 1 << 8,
    ReduceDirty = 1 << 9,
    ReduceTired = 1 << 10,
    IncreaceTraining = 1 << 11, 
    CanStoreFood = 1 << 12
};


[System.Serializable]
public class ObjectsObject : ScriptableObject
{

    public SpriteInfo[] infos;
    public bool isPassible;
    public int storageSize;
    [EnumFlag]
    public UseableObjects type;
}
