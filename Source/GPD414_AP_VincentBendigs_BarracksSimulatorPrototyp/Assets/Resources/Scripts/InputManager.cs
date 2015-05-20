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
                gmanager.GetComponent<GameManager>().ChangeTileByClick(hit.transform.gameObject);
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
            if (endX < startX && endY < startY)
            {
                ChangeSpriteOnMap((int)endX, (int)endY, (int)startX, (int)startY, texture);
            }
            else if (endX > startX && endY < startY)
            {
                ChangeSpriteOnMap((int)startX, (int)endY, (int)endX, (int)startY, texture);
            }
            else if (endX > startX && endY > startY)
            {
                ChangeSpriteOnMap((int)startX, (int)startY, (int)endX, (int)endY, texture);
            }
            else if (endX < startX && endY > startY)
            {
                ChangeSpriteOnMap((int)endX, (int)startY, (int)startX, (int)endY, texture);
            }
        }
        else if (gmanager.GetComponent<GameManager>().BuildGrass)
        {
            Sprite texture = gmanager.GetComponent<GameManager>().Grass;
            
            if (endX <= startX && endY <= startY)
            {
                ChangeSpriteOnMap((int)endX, (int)endY, (int)startX, (int)startY, texture);
            }
            else if (endX >= startX && endY <= startY)
            {
                ChangeSpriteOnMap((int)startX, (int)endY, (int)endX, (int)startY, texture);
            }
            else if (endX >= startX && endY >= startY)
            {
                ChangeSpriteOnMap((int)startX, (int)startY, (int)endX, (int)endY, texture);
            }
            else if (endX <= startX && endY >= startY)
            {
                ChangeSpriteOnMap((int)endX, (int)startY, (int)startX, (int)endY, texture);
            }
        }
        else if(gmanager.GetComponent<GameManager>().BuildWalls)
        {
            Sprite texture = gmanager.GetComponent<GameManager>().Wall;
            
            if (endX <= startX && endY <= startY)
            {
                DrawWallLogicOnMap((int)endX, (int)endY, (int)startX, (int)startY, texture);
            }
            else if (endX >= startX && endY <= startY)
            {
                DrawWallLogicOnMap((int)startX, (int)endY, (int)endX, (int)startY, texture);
            }
            else if (endX >= startX && endY >= startY)
            {
                DrawWallLogicOnMap((int)startX, (int)startY, (int)endX, (int)endY, texture);
            }
            else if (endX <= startX && endY >= startY)
            {
                DrawWallLogicOnMap((int)endX, (int)startY, (int)startX, (int)endY, texture);
            }
        }
    }

    private void ChangeSpriteOnMap(int minX,int minY, int maxX,int maxY,Sprite Texture)
    {
        for (int i = minX; i < maxX+1; i++)
        {
            for (int j = minY; j < maxY+1; j++)
            {
                map.MapData[i, j].Texture = Texture;
            }
        }
    }
    void DrawWallLogicOnMap(int minX, int minY, int maxX, int maxY, Sprite Texture)
    {
        for (int i= minX; i < maxX; i++)
        {
            for (int j = minY; j < maxY; j++)
            {
                if(i == minX || i == maxX-1 || j == minY || j == maxY-1)
                {
                    map.MapData[i, j].Texture = Texture;
                }
            }
        }
    }
}
