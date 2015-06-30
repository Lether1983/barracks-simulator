using UnityEngine;
using System.Collections;

[System.Serializable]
    public struct SpriteInfo
    {
        public Vector2 delta;
        public Sprite texture;
    }

[System.Serializable]
public class ObjectsObject : ScriptableObject
{

    public SpriteInfo[] infos;
    public bool isPassible;
    public UseableObjects type;
}
