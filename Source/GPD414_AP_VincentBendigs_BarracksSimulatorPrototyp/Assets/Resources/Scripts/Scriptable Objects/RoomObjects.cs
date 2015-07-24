using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum TypeOfRoom { Stube,Dusche,Waeschekammer,Kueche,Kantine,Buero,Aufentallsraum};
public enum RoomUsability : int
{
    none = 1 << 0,
    habitable = 1 << 1,
    Useable = 1 << 2,
    laborFactor = 1 << 3
};
public class RoomObjects : ScriptableObject
{
    public Sprite defaultTexture;

    public SpriteInfo[] infos;
    public Job[] availableJobs;
    public TypeOfRoom type;
    [EnumFlag]
    public RoomUsability checkRoom;
}
