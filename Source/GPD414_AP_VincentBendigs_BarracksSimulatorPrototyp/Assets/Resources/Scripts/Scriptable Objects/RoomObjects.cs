using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum TypeOfRoom { Stube,Dusche,Waeschekammer}
public class RoomObjects : ScriptableObject
{
    public Sprite defaultTexture;

    public SpriteInfo[] infos;
    public TypeOfRoom type;
}
