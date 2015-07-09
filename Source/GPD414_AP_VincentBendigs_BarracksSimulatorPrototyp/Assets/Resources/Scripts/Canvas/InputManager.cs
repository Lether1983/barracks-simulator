﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class InputManager : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    #region Fileds
    public static InputManager Instance { get; private set; }
    public GameObject mainCamera;
    public GameObject gmanager;
    public RectTransform rect;
    public bool DrawRect;
    public bool DestroyDrawRect;
    public bool SetRoomOnMap;

    TileMap map = TileMap.Instance();
    ObjectTileMap objectMap = ObjectTileMap.Instance();
    RoomMap roomMap = RoomMap.Instance();
    GameManager manager;
    Vector2 startPos;
    Vector2 endPos;
    float yScaler;
    float xScaler;
    private float endX;
    private float endY;
    private float startX;
    private float startY;
    #endregion#

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        manager = gmanager.GetComponent<GameManager>();
    }

    void Update()
    {
        xScaler = GetComponent<CanvasScaler>().referenceResolution.x / Screen.width;
        yScaler = GetComponent<CanvasScaler>().referenceResolution.y / Screen.height;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            startPos = eventData.position;

            Vector2 worldpoint = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(eventData.position);
            RaycastHit2D hit = Physics2D.Raycast(worldpoint, Vector2.zero);

            if (manager.InObjectBuildMode)
            {
                if (objectMap.ObjectData[(int)hit.transform.position.x, (int)hit.transform.position.y].startobject == null &&
                    map.MapData[(int)hit.transform.position.x,(int)hit.transform.position.y].isOverridable)
                {
                    ObjectPlacementOnMap(eventData, hit);
                }
                else if (objectMap.ObjectData[(int)hit.transform.position.x, (int)hit.transform.position.y].startobject == null &&
                        gmanager.GetComponent<ObjectManager>().DoorPlacement)
                {
                    DoorPlacementOnMap(eventData, hit);
                }
            }
            else
            {
                if (hit.collider != null)
                {
                    manager.ChangeTileByClick(map.MapData[(int)hit.transform.position.x, (int)hit.transform.position.y].myObject);
                }
            }
            if(manager.DestroyModus)
            {
                if(manager.DestroyRooms)
                {
                    RoomDestroymentOnMap(hit);
                }
                if(manager.DestroyObjects)
                {
                    ObjectDestroymentOnMap(hit);
                }
                if(hit.collider != null)
                {
                    manager.DestroyOneTile(map.MapData[(int)hit.transform.position.x, (int)hit.transform.position.y].myObject);
                }
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            manager.ResetAllBuildingModi();
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {

        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Vector2 worldpoint = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(eventData.position);
            RaycastHit2D hit = Physics2D.Raycast(worldpoint, Vector2.zero);
            endPos = eventData.position;
       
            if (hit.collider != null)
            {
                DrawRect = true;
                DestroyDrawRect = true;
                if ((int)mainCamera.GetComponent<Camera>().ScreenToWorldPoint(endPos).x == (int)mainCamera.GetComponent<Camera>().ScreenToWorldPoint(startPos).x ||
                    (int)mainCamera.GetComponent<Camera>().ScreenToWorldPoint(endPos).y == (int)mainCamera.GetComponent<Camera>().ScreenToWorldPoint(startPos).y)
                {
                    manager.ChangeTileByClick(hit.transform.gameObject);
                    if(manager.DestroyModus)
                    {
                        manager.DestroyOneTile(hit.transform.gameObject);
                    }
                }
            }
            DrawSelectionBox(eventData);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {

        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        rect.position = Vector2.zero;
        rect.sizeDelta = Vector2.zero;

        if (DrawRect)
        {
            ChangeTileUndertheRect();
            DrawRect = false;
        }
        if (manager.InRoomBuildMode)
        {
            RoomPlaceMentOnMap();
            BlockadeRoomSpaceOnMap();
        }
        if (DestroyDrawRect)
        {
            if (manager.DestroyModus)
            {
                SetDestroyModus();
            }
            DestroyDrawRect = false;
        }
    }

    private void BlockadeRoomSpaceOnMap()
    {
        endX = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(endPos).x;
        endY = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(endPos).y;
        startX = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(startPos).x;
        startY = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(startPos).y;
        int minX = Mathf.Min((int)startX, (int)endX);
        int minY = Mathf.Min((int)startY, (int)endY);
        int maxX = Mathf.CeilToInt(Mathf.Max(startX, endX));
        int maxY = Mathf.CeilToInt(Mathf.Max(startY, endY));

        if (RommIsBlocked() == false)
        {
            for (int i = minX; i < maxX; i++)
            {
                for (int j = minY; j < maxY; j++)
                {
                    roomMap.RoomData[i, j].roomObject = roomMap.RoomData[minX,minY];
                }
            }
        }

    }

    private void RoomPlaceMentOnMap()
    {
        endX = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(endPos).x;
        endY = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(endPos).y;
        startX = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(startPos).x;
        startY = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(startPos).y;
        int minX = Mathf.Min((int)startX,(int)endX);
        int minY = Mathf.Min((int)startY,(int)endY);
        int maxX = Mathf.CeilToInt(Mathf.Max(startX,endX));
        int maxY = Mathf.CeilToInt(Mathf.Max(startY,endY));

        
        if (RommIsBlocked() == false)
        {
            RoomLogicObject room = new RoomLogicObject();
            room.Position = new Vector2(minX, minY);
            room.Size = new Vector2(maxX - minX, maxY - minY);
            room.RoomInfo = manager.room_object;
            manager.gameObject.GetComponent<RoomManager>().addNewRoom(room);

            for (int i = minX; i < maxX; i++)
            {
                for (int j = minY; j < maxY; j++)
                {
                    roomMap.RoomData[i, j].room = room;
                    gmanager.GetComponent<GameManager>().PlaceRoomByClickOnMap(roomMap.RoomData[i, j].myObject);
                }
            }
        }
    }

    private void ObjectPlacementOnMap(PointerEventData eventData,RaycastHit2D hit)
    {
        if (hit.collider != null)
        {
            ObjectLogicObject @object = new ObjectLogicObject();
            @object.position = hit.transform.position;
            @object.info = manager.object_object;
            @object.Room = GetRoomByClick(hit.transform.position);
            if(@object.Room != null)
            {
                @object.Room.Objects.Add(@object);
            }
            for (int i = 0; i < manager.object_object.infos.Length; i++)
            {
                var info = manager.object_object.infos[i];

                Vector2 hitinfo = (Vector2)hit.transform.position + info.delta;
                objectMap.ObjectData[(int)hitinfo.x, (int)hitinfo.y].@object = @object;
                gmanager.GetComponent<GameManager>().PlaceObjectByClick(objectMap.ObjectData[(int)hitinfo.x, (int)hitinfo.y].myObject);
            }
        }
    }

    private RoomLogicObject GetRoomByClick(Vector2 hit)
    {
       return roomMap.RoomData[(int)hit.x, (int)hit.y].room;
    }

    private void DoorPlacementOnMap(PointerEventData eventData, RaycastHit2D hit)
    {
        if (hit.collider != null)
        {
            manager.DestroyOneTile(map.MapData[(int)hit.transform.position.x,(int)hit.transform.position.y].myObject);
            ObjectPlacementOnMap(eventData, hit);
        }
    }
    
    private void DrawSelectionBox(PointerEventData eventData)
    {
        float xDif;
        float yDif;
        float xPos;
        float yPos;

        if (endPos.x < startPos.x)
        {
            xDif = startPos.x - endPos.x;
            xPos = endPos.x;
        }
        else
        {
            xDif = endPos.x - startPos.x;
            xPos = startPos.x;
        }

        if (endPos.y < startPos.y)
        {
            yDif = startPos.y - endPos.y;
            yPos = endPos.y;
        }
        else
        {
            yDif = endPos.y - startPos.y;
            yPos = startPos.y;
        }

        rect.position = new Vector2(xPos, yPos);
        rect.sizeDelta = new Vector2(xDif * xScaler, yDif * yScaler);
    }

    private void ChangeTileUndertheRect()
    {
        endX = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(endPos).x;
        endY = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(endPos).y;
        startX = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(startPos).x;
        startY = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(startPos).y;

        if (manager.ground_Object != null && !manager.BuildWalls)
        {
            ChangeSpriteOnMap((int)startX, (int)startY, (int)endX, (int)endY);
        }
        if (manager.BuildWalls)
        {

            DrawWallLogicOnMap((int)startX, (int)startY, (int)endX, (int)endY);
        }
        else if (manager.BuildFoundation)
        {
            DrawFoundationLogicOnMap((int)startX,(int)startY,(int)endX,(int)endY);
        }
    }

    private void ChangeSpriteOnMap(int startX,int startY, int endX,int endY)
    {
        int minX = Mathf.Min(startX, endX);
        int minY = Mathf.Min(startY, endY);
        int maxX = Mathf.Max(startX, endX);
        int maxY = Mathf.Max(startY, endY);

        for (int i = minX; i <= maxX; i++)
        {
            for (int j = minY; j <= maxY; j++)
            {
                if (CheckIndoorSet() == false && manager.ground_Object.isOutdoor && map.MapData[i,j].isOverridable)
                {
                        map.MapData[i, j].Texture = manager.ground_Object.texture;
                        map.MapData[i, j].myObject.GetComponent<SpriteRenderer>().sprite = manager.ground_Object.texture;
                }
                else if (CheckIndoorSet() && manager.ground_Object.isIndoor && map.MapData[i, j].isOverridable)
                {
                    map.MapData[i, j].Texture = manager.ground_Object.texture;
                    map.MapData[i, j].myObject.GetComponent<SpriteRenderer>().sprite = manager.ground_Object.texture;
                }
            }
        }
    }

    private void DrawWallLogicOnMap(int startX, int startY, int endX, int endY)
    {
        int minX = Mathf.Min(startX, endX);
        int minY = Mathf.Min(startY, endY);
        int maxX = Mathf.Max(startX, endX);
        int maxY = Mathf.Max(startY, endY);

        for (int i= minX; i <= maxX; i++)
        {
            for (int j = minY; j <= maxY; j++)
            {
                if(i == minX || i == maxX || j == minY || j == maxY)
                {
                    map.MapData[i, j].GetAllValues(manager.ground_Object);
                }
            }
        }
    }

    private void DrawFoundationLogicOnMap(int startX, int startY, int endX, int endY)
    {
        int minX = Mathf.Min(startX, endX);
        int minY = Mathf.Min(startY, endY);
        int maxX = Mathf.Max(startX, endX);
        int maxY = Mathf.Max(startY, endY);

        for (int i = minX; i <= maxX; i++)
        {
            for (int j = minY; j <= maxY; j++)
            {
                if(i == minX || i == maxX || j == minY || j == maxY)
                {
                    map.MapData[i, j].GetAllValues(manager.wall_Object);
                }
                else
                {
                    map.MapData[i, j].GetAllValues(manager.ground_Object);
                }
            }
        }
    }

    private bool RommIsBlocked()
    {
        endX = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(endPos).x;
        endY = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(endPos).y;
        startX = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(startPos).x;
        startY = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(startPos).y;

        int minX = Mathf.Min((int)startX, (int)endX);
        int minY = Mathf.Min((int)startY, (int)endY);
        int maxX = Mathf.CeilToInt(Mathf.Max(startX, endX));
        int maxY = Mathf.CeilToInt(Mathf.Max(startY, endY));

        for (int i = minX; i < maxX; i++)
        {
            for (int j = minY; j < maxY; j++)
            {
                if(roomMap.RoomData[i,j].room != null || map.MapData[i,j].isOverridable == false)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool CheckIndoorSet()
    {
        endX = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(endPos).x;
        endY = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(endPos).y;
        startX = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(startPos).x;
        startY = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(startPos).y;

        int minX = Mathf.Min((int)startX, (int)endX);
        int minY = Mathf.Min((int)startY, (int)endY);
        int maxX = Mathf.CeilToInt(Mathf.Max(startX, endX));
        int maxY = Mathf.CeilToInt(Mathf.Max(startY, endY));

        for (int i = minX; i < maxX; i++)
        {
            for (int j = minY; j < maxY; j++)
            {
                if (map.MapData[i,j].IsIndoor)
                {
                    return true;
                }
            } 
        }
        return false;
    }

    private void DestroySpriteOnMap(int startX, int startY, int endX, int endY)
    {
        int minX = Mathf.Min(startX, endX);
        int minY = Mathf.Min(startY, endY);
        int maxX = Mathf.Max(startX, endX);
        int maxY = Mathf.Max(startY, endY);

        for (int i = minX; i <= maxX; i++)
        {
            for (int j = minY; j <= maxY; j++)
            {
                if (CheckIndoorSet() == false)
                {
                    map.MapData[i, j].GetAllValues(manager.OutdoorDefault);
                }
                else if (CheckIndoorSet())
                {
                    map.MapData[i, j].GetAllValues(manager.IndoorDefault);
                }
            }
        }
    }

    private void DestroyWallLogicOnMap(int startX, int startY, int endX, int endY)
    {
        int minX = Mathf.Min(startX, endX);
        int minY = Mathf.Min(startY, endY);
        int maxX = Mathf.Max(startX, endX);
        int maxY = Mathf.Max(startY, endY);

        for (int i = minX; i <= maxX; i++)
        {
            for (int j = minY; j <= maxY; j++)
            {
                if (i == minX || i == maxX || j == minY || j == maxY)
                {
                   if (CheckIndoorSet() == false)
                    {
                        map.MapData[i, j].GetAllValues(manager.OutdoorDefault);
                    }
                    else if (CheckIndoorSet())
                    {
                        map.MapData[i, j].GetAllValues(manager.IndoorDefault);
                    }
                }
            }
        }
    }

    private void DestroyFoundationLogicOnMap(int startX, int startY, int endX, int endY)
    {
        int minX = Mathf.Min(startX, endX);
        int minY = Mathf.Min(startY, endY);
        int maxX = Mathf.Max(startX, endX);
        int maxY = Mathf.Max(startY, endY);

        for (int i = minX; i <= maxX; i++)
        {
            for (int j = minY; j <= maxY; j++)
            {
                map.MapData[i, j].Texture = manager.OutdoorDefault.texture;
                map.MapData[i, j].myObject.GetComponent<SpriteRenderer>().sprite = manager.OutdoorDefault.texture;
                map.MapData[i, j].IsIndoor = false;
                map.MapData[i, j].isOverridable = true;
                map.MapData[i, j].IsOutdoor = true;
            }
        }
    }

    private void RoomDestroymentOnMap(RaycastHit2D hit)
    {
        if (hit.collider != null)
        {
            RoomLogicObject TempRoomObject = roomMap.RoomData[(int)hit.transform.position.x,(int)hit.transform.position.y].room;

            for (int i = (int)TempRoomObject.Position.x; i < (int)TempRoomObject.Position.x + (int)TempRoomObject.Size.x; i++)
            {
                for (int j = (int)TempRoomObject.Position.y; j < (int)TempRoomObject.Position.y + (int)TempRoomObject.Size.y; j++)
                {
                    roomMap.RoomData[i,j].room = null;
                    roomMap.RoomData[i,j].Texture = null;
                    roomMap.RoomData[i, j].myObject.GetComponent<SpriteRenderer>().sprite = null;
                }
            }
        }
    }

    private void ObjectDestroymentOnMap(RaycastHit2D hit)
    {
        if(hit.collider != null)
        {
            if (objectMap.ObjectData[(int)hit.transform.position.x, (int)hit.transform.position.y].startobject == null) return;
            var info = objectMap.ObjectData[(int)hit.transform.position.x, (int)hit.transform.position.y].startobject.infos;

            Vector2 Reference = objectMap.ObjectData[(int)hit.transform.position.x, (int)hit.transform.position.y].ParentPosition;

            for (int i = 0; i < info.Length; i++)
            {
                var target = Reference + info[i].delta;
                objectMap.ObjectData[(int)target.x, (int)target.y].startobject = null;
                objectMap.ObjectData[(int)target.x, (int)target.y].Texture = null;
                objectMap.ObjectData[(int)target.x, (int)target.y].myObject.GetComponent<SpriteRenderer>().sprite = null;
            }
        }
    }

    private void SetDestroyModus()
    {
        endX = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(endPos).x;
        endY = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(endPos).y;
        startX = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(startPos).x;
        startY = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(startPos).y;

        if (manager.DestroyTiles)
        {
            DestroySpriteOnMap((int)startX,(int)startY,(int)endX,(int)endY);
        }
        else if(manager.DestroyWalls)
        {
            DestroyWallLogicOnMap((int)startX, (int)startY, (int)endX, (int)endY);
        }
        else if(manager.DestroyFoundation)
        {
            DestroyFoundationLogicOnMap((int)startX, (int)startY, (int)endX, (int)endY);
        }
    }
}