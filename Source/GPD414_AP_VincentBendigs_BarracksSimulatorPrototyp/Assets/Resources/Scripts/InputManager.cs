using UnityEngine;
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
                if (objectMap.ObjectData[(int)hit.transform.position.x, (int)hit.transform.position.y].myObject == null &&
                    map.MapData[(int)hit.transform.position.x,(int)hit.transform.position.y].GetType() != typeof(WallTile))
                {
                    ObjectPlacementOnMap(eventData, hit);
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
        if(manager.DestroyModus)
        {
            SetDestroyModus();
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
                    roomMap.RoomData[i, j].roomObject = roomMap.RoomData[minX,minY].myObject;
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
            mainCamera.GetComponent<TileMapCameraGrid>().inactiveRoomObjects.Peek().transform.position = new Vector3(minX, minY, -0.5f);
            roomMap.RoomData[minX, minY].Position = new Vector2(minX, minY);
            mainCamera.GetComponent<TileMapCameraGrid>().inactiveRoomObjects.Peek().transform.localScale = new Vector3(maxX - minX, maxY - minY, 1);
            mainCamera.GetComponent<TileMapCameraGrid>().inactiveRoomObjects.Peek().SetActive(true);
            roomMap.RoomData[minX, minY].myObject = mainCamera.GetComponent<TileMapCameraGrid>().inactiveRoomObjects.Peek();
            gmanager.GetComponent<GameManager>().PlaceRoomByClickOnMap(roomMap.RoomData[minX, minY].myObject);
            mainCamera.GetComponent<TileMapCameraGrid>().inactiveRoomObjects.Pop();
        }
        
    }

    private void ObjectPlacementOnMap(PointerEventData eventData,RaycastHit2D hit)
    {
        if (hit.collider != null)
        {
            mainCamera.GetComponent<TileMapCameraGrid>().inactiveObjects.Peek().transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y, -1);
            objectMap.ObjectData[(int)hit.transform.position.x, (int)hit.transform.position.y].Position = hit.transform.position;
            mainCamera.GetComponent<TileMapCameraGrid>().inactiveObjects.Peek().SetActive(true);
            objectMap.ObjectData[(int)hit.transform.position.x, (int)hit.transform.position.y].myObject = mainCamera.GetComponent<TileMapCameraGrid>().inactiveObjects.Peek();
            gmanager.GetComponent<GameManager>().PlaceObjectByClick(objectMap.ObjectData[(int)hit.transform.position.x, (int)hit.transform.position.y].myObject);
            mainCamera.GetComponent<TileMapCameraGrid>().inactiveObjects.Pop();
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

        if (manager.BuildDesert)
        {
            Sprite texture = manager.Desert;

            ChangeSpriteOnMap((int)startX, (int)startY, (int)endX, (int)endY, texture);
        }
        else if (manager.BuildGrass)
        {
            Sprite texture = manager.Grass;

            ChangeSpriteOnMap((int)startX, (int)startY, (int)endX, (int)endY, texture);
        }
        else if (manager.BuildBeton)
        {
            Sprite texture = manager.Beton;

            ChangeSpriteOnMap((int)startX, (int)startY, (int)endX, (int)endY, texture);
        }
        else if (manager.BuildWalls)
        {
            Sprite texture = manager.Wall;

            DrawWallLogicOnMap((int)startX, (int)startY, (int)endX, (int)endY, texture);
        }
        else if (manager.BuildFoundation)
        {
            Sprite texture = manager.Wall;
            Sprite texture2 = manager.Beton;

            DrawFoundationLogicOnMap((int)startX,(int)startY,(int)endX,(int)endY, texture, texture2);
        }
    }

    private void ChangeSpriteOnMap(int startX,int startY, int endX,int endY,Sprite Texture)
    {
        int minX = Mathf.Min(startX, endX);
        int minY = Mathf.Min(startY, endY);
        int maxX = Mathf.Max(startX, endX);
        int maxY = Mathf.Max(startY, endY);

        for (int i = minX; i <= maxX; i++)
        {
            for (int j = minY; j <= maxY; j++)
            {
                if (CheckIndoorSet() == false && CheckTextureForIndoor(i,j,Texture) == false)
                {
                    if (map.MapData[i, j].Texture != Texture)
                    {
                        map.MapData[i, j].Texture = Texture;
                        map.MapData[i, j].myObject.GetComponent<SpriteRenderer>().sprite = Texture;
                    }
                }
                else if(CheckIndoorSet() && CheckTextureForIndoor(i,j,Texture))
                {
                    if (map.MapData[i, j].Texture != Texture)
                    {
                        map.MapData[i, j].Texture = Texture;
                        map.MapData[i, j].myObject.GetComponent<SpriteRenderer>().sprite = Texture;
                    }
                }
            }
        }
    }

    private void DrawWallLogicOnMap(int startX, int startY, int endX, int endY, Sprite Texture)
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
                    if (map.MapData[i, j].Texture != Texture)
                    {
                        map.MapData[i, j] = new WallTile(map.MapData[i, j]);
                        map.MapData[i, j].Texture = Texture;
                        map.MapData[i, j].myObject.GetComponent<SpriteRenderer>().sprite = Texture;
                    }
                }
            }
        }
    }

    private void DrawFoundationLogicOnMap(int startX, int startY, int endX, int endY, Sprite WallTexture, Sprite GroundTexture)
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
                    if (map.MapData[i, j].Texture != WallTexture)
                    {
                        map.MapData[i, j] = new WallTile(map.MapData[i, j]);
                        map.MapData[i, j].Texture = WallTexture;
                        map.MapData[i, j].myObject.GetComponent<SpriteRenderer>().sprite = WallTexture;
                        map.MapData[i, j].IsIndoor = true;
                    }
                }
                else
                {
                    if (map.MapData[i, j].Texture != GroundTexture)
                    {
                        map.MapData[i, j].Texture = GroundTexture;
                        map.MapData[i, j].myObject.GetComponent<SpriteRenderer>().sprite = GroundTexture;
                        map.MapData[i, j].IsIndoor = true;
                    }
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
                if(roomMap.RoomData[i,j].roomObject != null && map.MapData[i,j].GetType() == typeof(WallTile))
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

    private bool CheckTextureForIndoor(int i, int j,Sprite Texture)
    {
        // Indoor is True Outdoor is False
        if (Texture == manager.Desert)
        {
            return false;
        }
        else if (Texture == manager.Grass)
        {
            return false;
        }
        else if (Texture == manager.Wall)
        {
            return true;
        }
        else if (Texture == manager.Beton)
        {
            return true;
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
                    map.MapData[i, j].Texture = manager.Desert;
                    map.MapData[i, j].myObject.GetComponent<SpriteRenderer>().sprite = manager.Desert;
                }
                else if (CheckIndoorSet())
                {
                        map.MapData[i, j].Texture = manager.Beton;
                        map.MapData[i, j].myObject.GetComponent<SpriteRenderer>().sprite = manager.Beton;
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
                        map.MapData[i, j] = new GroundTile(map.MapData[i, j]);
                        map.MapData[i, j].Texture = manager.Desert;
                        map.MapData[i, j].myObject.GetComponent<SpriteRenderer>().sprite = manager.Desert;
                    }
                    else if (CheckIndoorSet())
                    {
                        map.MapData[i, j] = new GroundTile(map.MapData[i, j]);
                        map.MapData[i, j].Texture = manager.Beton;
                        map.MapData[i, j].myObject.GetComponent<SpriteRenderer>().sprite = manager.Beton;
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
                map.MapData[i, j] = new GroundTile(map.MapData[i, j]);
                map.MapData[i, j].Texture = manager.Desert;
                map.MapData[i, j].myObject.GetComponent<SpriteRenderer>().sprite = manager.Desert;
                map.MapData[i, j].IsIndoor = false;
            }
        }
    }

    private void RoomDestroymentOnMap(RaycastHit2D hit)
    {
        endX = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(endPos).x;
        endY = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(endPos).y;
        startX = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(startPos).x;
        startY = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(startPos).y;
        int minX = Mathf.Min((int)startX, (int)endX);
        int minY = Mathf.Min((int)startY, (int)endY);
        int maxX = Mathf.CeilToInt(Mathf.Max(startX, endX));
        int maxY = Mathf.CeilToInt(Mathf.Max(startY, endY));

        
        if (hit.collider != null)
        {
            GameObject TempRoomObject = roomMap.RoomData[(int)hit.transform.position.x,(int)hit.transform.position.y].roomObject;

            for (int i = (int)TempRoomObject.transform.position.x; i < (int)TempRoomObject.transform.position.x + (int)TempRoomObject.transform.localScale.x; i++)
            {
                for (int j = (int)TempRoomObject.transform.position.y; j < (int)TempRoomObject.transform.position.y + (int)TempRoomObject.transform.localScale.y; j++)
                {
                    roomMap.RoomData[i,j].roomObject = null;
                }
            }
            TempRoomObject.SetActive(false);
            mainCamera.GetComponent<TileMapCameraGrid>().inactiveRoomObjects.Push(TempRoomObject);
        }

    }

    private void ObjectDestroymentOnMap(RaycastHit2D hit)
    {
        if(hit.collider != null)
        {
            objectMap.ObjectData[(int)hit.transform.position.x,(int)hit.transform.position.y].myObject.SetActive(false);
            mainCamera.GetComponent<TileMapCameraGrid>().inactiveObjects.Push(objectMap.ObjectData[(int)hit.transform.position.x, (int)hit.transform.position.y].myObject);
            objectMap.ObjectData[(int)hit.transform.position.x, (int)hit.transform.position.y].myObject = null;
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