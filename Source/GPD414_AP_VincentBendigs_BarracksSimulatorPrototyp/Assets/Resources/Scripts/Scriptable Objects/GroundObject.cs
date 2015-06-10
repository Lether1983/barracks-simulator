using UnityEngine;
using System.Collections;

[System.Serializable]
public class GroundObject : ScriptableObject
{
    public bool isPassible;
    public bool isIndoor;
    public bool isOutdoor;
    public bool isOverridable;
    public int movementCost;
    public float Duarbility;
    public Sprite texture;
}
