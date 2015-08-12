using UnityEngine;
using System.Collections;


public enum TypeofWorkObjects { Material,SmallObjects,Food}
public class WorkObjects : ScriptableObject
{
    public int Uses;

    public TypeofWorkObjects Type;
    public ObjectsObject target;
    public ObjectsObject placeOfGeneration;
    public GameObject myObject;
    public Vector3 Position;
    public Sprite Image;
    [EnumFlag]
    public UseableObjects type;
}