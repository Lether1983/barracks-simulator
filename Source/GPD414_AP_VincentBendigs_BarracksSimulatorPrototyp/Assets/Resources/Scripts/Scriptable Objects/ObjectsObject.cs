using UnityEngine;
using System.Collections;

[System.Serializable]
public class ObjectsObject : ScriptableObject
{

    public Objectinfo[] infos;
    public bool isPassible;


    [System.Serializable]
    public struct Objectinfo
    {
        public Vector2 delta;
        public Sprite texture;
    }
}
