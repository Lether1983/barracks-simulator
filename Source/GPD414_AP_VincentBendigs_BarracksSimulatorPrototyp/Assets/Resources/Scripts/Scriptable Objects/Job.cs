using UnityEngine;
using System.Collections;
using System;

[Flags]
public enum TypeOfWork 
{
    Cooking  = 1 << 0,
    MovingFood  = 1 << 1,
    MovingMaterial = 1 << 2,
    Building = 1 << 3,
    Cleaning = 1 << 4,
    Repairing = 1 << 5,
    Teaching = 1 << 6
};

public class Job : ScriptableObject
{
    [EnumFlag,SerializeField]
    public TypeOfWork Types;
}
