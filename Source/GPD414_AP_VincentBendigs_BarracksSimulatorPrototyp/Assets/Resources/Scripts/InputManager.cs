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
    Vector2 startPos;
    Vector2 endPos;
    float yScaler;
    float xScaler;
    private float endX;
    private float endY;
    private float startX;
    private float startY;
    #endregion

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
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

            if(gmanager.GetComponent<GameManager>().InObjectBuildMode)
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
                    gmanager.GetComponent<GameManager>().ChangeTileByClick(map.MapData[(int)hit.transform.position.x,(int)hit.transform.position.y].myObject);
                }
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            gmanager.GetComponent<GameManager>().BuildDesert = false;
            gmanager.GetComponent<GameManager>().BuildGrass = false;
            gmanager.GetComponent<GameManager>().BuildWalls = false;
            gmanager.GetComponent<GameManager>().BuildFoundation = false;
            gmanager.GetComponent<GameManager>().InObjectBuildMode = false;
            gmanager.GetComponent<ObjectManager>().IplaceShower = false;
            gmanager.GetComponent<GameManager>().InRoomBuildMode = false;
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
                    gmanager.GetComponent<GameManager>().ChangeTileByClick(hit.transform.gameObject);
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
        if(gmanager.GetComponent<GameManager>().InRoomBuildMode)
        {
            RoomPlaceMentOnMap();
            BlockadeRoomSpaceOnMap();
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
                    roomMap.RoomData[i, j].IsInRoomRange = true;
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

        if (gmanager.GetComponent<GameManager>().BuildDesert)
        {
            Sprite texture = gmanager.GetComponent<GameManager>().Desert;

            ChangeSpriteOnMap((int)startX, (int)startY, (int)endX, (int)endY, texture);
        }
        else if (gmanager.GetComponent<GameManager>().BuildGrass)
        {
            Sprite texture = gmanager.GetComponent<GameManager>().Grass;

            ChangeSpriteOnMap((int)startX, (int)startY, (int)endX, (int)endY, texture);
        }
        else if(gmanager.GetComponent<GameManager>().BuildWalls)
        {
            Sprite texture = gmanager.GetComponent<GameManager>().Wall;

            DrawWallLogicOnMap((int)startX, (int)startY, (int)endX, (int)endY, texture);
        }
        else if(gmanager.GetComponent<GameManager>().BuildFoundation)
        {
            Sprite texture = gmanager.GetComponent<GameManager>().Wall;
            Sprite texture2 = gmanager.GetComponent<GameManager>().Beton;

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
                if (map.MapData[i, j].Texture != Texture)
                {
                    map.MapData[i, j].Texture = Texture;
                    map.MapData[i, j].myObject.GetComponent<SpriteRenderer>().sprite = Texture;
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
                    }
                }
                else
                {
                    if (map.MapData[i, j].Texture != GroundTexture)
                    {
                        map.MapData[i, j].Texture = GroundTexture;
                        map.MapData[i, j].myObject.GetComponent<SpriteRenderer>().sprite = GroundTexture;
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
                if(roomMap.RoomData[i,j].IsInRoomRange)
                {
                    return true;
                }
            }
        }
        return false;
    }
}