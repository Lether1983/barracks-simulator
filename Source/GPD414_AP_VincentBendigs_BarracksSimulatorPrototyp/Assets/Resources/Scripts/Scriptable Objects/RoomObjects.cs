using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum TypeOfRoom { Stube,Dusche,Waeschekammer,Kueche,Kantine,Buero,Aufentallsraum}
public class RoomObjects : ScriptableObject
{
    public Sprite defaultTexture;

    public SpriteInfo[] infos;
    public TypeOfRoom type;
}
