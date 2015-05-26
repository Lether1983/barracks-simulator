using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class InputManager : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public static InputManager Instance { get; private set; }
    public GameObject mainCamera;
    public GameObject gmanager;
    public RectTransform rect;

    TileMap map = TileMap.Instance();
    Vector2 startPos;
    Vector2 endPos;
    float yScaler;
    float xScaler;
    private float endX;
    private float endY;
    private float startX;
    private float startY;

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
            Vector2 worldpoint = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(eventData.position);
            RaycastHit2D hit = Physics2D.Raycast(worldpoint, Vector2.zero);
            startPos = eventData.position;
            if (hit.collider != null)
            {
                gmanager.GetComponent<GameManager>().ChangeTileByClick(hit.transform.gameObject);
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            gmanager.GetComponent<GameManager>().BuildDesert = false;
            gmanager.GetComponent<GameManager>().BuildGrass = false;
            gmanager.GetComponent<GameManager>().BuildWalls = false;
            gmanager.GetComponent<GameManager>().BuildFoundation = false;
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
        ChangeTileUndertheRect();
    }

    void DrawSelectionBox(PointerEventData eventData)
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

    void ChangeTileUndertheRect()
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
                map.MapData[i, j].Texture = Texture;
                map.MapData[i, j].myObject.GetComponent<SpriteRenderer>().sprite = Texture;
            }
        }
    }

    void DrawWallLogicOnMap(int startX, int startY, int endX, int endY, Sprite Texture)
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
                    map.MapData[i, j].Texture = Texture;
                    map.MapData[i, j].myObject.GetComponent<SpriteRenderer>().sprite = Texture;
                }
            }
        }
    }

    void DrawFoundationLogicOnMap(int startX, int startY, int endX, int endY, Sprite WallTexture, Sprite GroundTexture)
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
                    map.MapData[i, j].Texture = WallTexture;
                    map.MapData[i, j].myObject.GetComponent<SpriteRenderer>().sprite = WallTexture;
                }
                else
                {
                    map.MapData[i,j].Texture = GroundTexture;
                    map.MapData[i, j].myObject.GetComponent<SpriteRenderer>().sprite = GroundTexture;
                }
            }
        }
    }
}