using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// ungenutzt
enum direction { Up,Down,Left,Right};
public class TileMapCameraGrid : MonoBehaviour
{
    #region Fields
    // Auslagern
    public Stack<GameObject> inactiveObjects;
    // Auslagern
    public Stack<GameObject> inactiveRoomObjects;
    public float timer= 0;
    public GameObject TileSpawn;
    public GameObject ObjectSpawn;
    public GameObject RoomSpawn;
    // Auslagern
    Stack<GameObject> inactiveTiles;
    GameManager manager;
    CameraControl mainCamera;
    TileMap map = TileMap.Instance();
    ObjectTileMap objectMap = ObjectTileMap.Instance();
    RoomMap roomMap = RoomMap.Instance();
    int width = 41;
    int height = 27;
    float GridZeroPointX;
    float GridZeroPointY;
    float GridMaxPointX;
    float GridMaxPointY;
    #endregion

    void Start()
    {
        // weg
        inactiveTiles = new Stack<GameObject>();
        // weg
        inactiveObjects = new Stack<GameObject>();
        // weg
        inactiveRoomObjects = new Stack<GameObject>();
        GridZeroPointX = this.transform.position.x - (width / 2);
        GridZeroPointY = this.transform.position.y - (height / 2);
        GridMaxPointX = this.transform.position.x + (width / 2);
        GridMaxPointY = this.transform.position.y + (height / 2);
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        mainCamera = this.gameObject.GetComponent<CameraControl>();
        // entfällt/wird angepasst
        FillCameraFieldNew();
        // entfällt/wird angepasst
        FillCameraObjectField();
        // entfällt/wird angepasst
        FillStackOfRoomObjects();
        // entfällt/wird angepasst
        ShowMapContentNew();
    }
    
    void Update()
    {
        timer += Time.deltaTime;
       
        // wird angepasst
        if(mainCamera.moveDirection.x != 0 && mainCamera.moveDirection.y != 0)
        {
            if (timer > 0.3)
            {
                MoveCameraGrid();
                timer = 0;
            }
        }
        else if(mainCamera.moveDirection.x != 0 || mainCamera.moveDirection.y != 0)
        {
            if (timer > 0.19)
            {
                MoveCameraGrid();
                timer = 0;
            }
        }
        
    }

    // weg
    public void MoveCameraGrid()
    {
        if(mainCamera.moveDirection.x < -0.1)
        {
            if (this.gameObject.transform.position.x != map.purchasedLandWidthMin)
            {
                CameraMove(direction.Left);
            }
        }
        else if (mainCamera.moveDirection.x > 0.1)
        {
            if (this.gameObject.transform.position.x != map.purchasedLandWidthMax)
            {
                CameraMove(direction.Right);
            }
        }
        if(mainCamera.moveDirection.y < -0.1)
        {
            if (this.gameObject.transform.position.y != map.purchasedLandHeightMin)
            {
                CameraMove(direction.Down);
            }
        }
        else if(mainCamera.moveDirection.y > 0.1)
        {
            if(this.gameObject.transform.position.y != map.purchasedLandHeightMax)
            {
                CameraMove(direction.Up);
            }
        }
    }

    // wird angepasst/vereinfacht
    private void CameraMove(direction directions)
    {
        int deActivateCoord;
        int activateCoord;
        
        if(directions == direction.Left)
        {
            GridZeroPointX -= 1;
            deActivateCoord = (int)GridMaxPointX;
            activateCoord = (int)GridZeroPointX;
            MoveHorizontal(activateCoord, deActivateCoord);
            GridMaxPointX -= 1;
        }
        else if(directions == direction.Right)
        {
            GridMaxPointX += 1;
            deActivateCoord = (int)GridZeroPointX;
            activateCoord = (int)GridMaxPointX;
            MoveHorizontal(activateCoord, deActivateCoord);
            GridZeroPointX += 1;
        }
        else if (directions == direction.Up)
        {
            GridMaxPointY++;
            deActivateCoord = (int)GridZeroPointY;
            activateCoord = (int)GridMaxPointY;
            MoveVertical(activateCoord, deActivateCoord);
            GridZeroPointY++;
        }
        else if(directions == direction.Down)
        {
            GridZeroPointY --;
            deActivateCoord = (int)GridMaxPointY;
            activateCoord = (int)GridZeroPointY;
            MoveVertical(activateCoord, deActivateCoord);
            GridMaxPointY--;
        }
    }
 
    #region VerticalPooling

    // wird verändert, vereinfacht
    void MoveVertical(int activateC, int deactivateC)
    {
        for (int i = 0; i < width; i++)
        {
            if (objectMap.ObjectData[(int)GridZeroPointX + i, deactivateC].myObject != null)
            {
                DeactivateObjectsVerticalForPooling(deactivateC, i);
            }
            if (roomMap.RoomData[(int)GridZeroPointX + i, deactivateC].myObject != null)
            {
                DeactivateRoomObjectsVerticalForPooling(deactivateC, i);
            }
            DeactivateTilesVerticalForPooling(deactivateC, i);
        }
        for (int j = 0; j < width; j++)
        {
            if (objectMap.ObjectData[(int)GridZeroPointX + j, activateC].Position != Vector2.zero)
            {
                ActivateObjectsVerticalForPooling(activateC, j);
            }
            if (roomMap.RoomData[(int)GridZeroPointX + j, activateC].Position != Vector2.zero)
            {
                ActivateRoomObjectsVerticalForPooling(activateC, j);
            }
            ActivateTilesVerticalForPooling(activateC, j);
        }


    }

    // weg
    private void ActivateTilesVerticalForPooling(int activateC, int j)
    {
        inactiveTiles.Peek().transform.position = map.MapData[(int)GridZeroPointX + j, activateC].Position;
        inactiveTiles.Peek().SetActive(true);
        map.MapData[(int)GridZeroPointX + j, activateC].myObject = inactiveTiles.Peek();
        inactiveTiles.Peek().GetComponent<SpriteRenderer>().sprite = map.MapData[(int)GridZeroPointX + j, activateC].Texture;
        inactiveTiles.Pop();
    }

    // weg
    private void ActivateRoomObjectsVerticalForPooling(int activateC, int j)
    {
        inactiveRoomObjects.Peek().transform.position = new Vector3((int)GridZeroPointX + j, activateC, -0.5f);
        inactiveRoomObjects.Peek().SetActive(true);
        roomMap.RoomData[(int)GridZeroPointX + j, activateC].myObject = inactiveRoomObjects.Peek();
        inactiveRoomObjects.Peek().GetComponent<SpriteRenderer>().sprite = roomMap.RoomData[(int)GridZeroPointX + j, activateC].Texture;
        inactiveRoomObjects.Pop();
    }

    // weg
    private void ActivateObjectsVerticalForPooling(int activateC, int j)
    {
        inactiveObjects.Peek().transform.position = new Vector3((int)GridZeroPointX + j, activateC, -1);
        inactiveObjects.Peek().SetActive(true);
        objectMap.ObjectData[(int)GridZeroPointX + j, activateC].myObject = inactiveObjects.Peek();
        inactiveObjects.Peek().GetComponent<SpriteRenderer>().sprite = objectMap.ObjectData[(int)GridZeroPointX + j, activateC].Texture;
        inactiveObjects.Pop();
    }

    // weg
    private void DeactivateTilesVerticalForPooling(int deactivateC, int i)
    {
        map.MapData[(int)GridZeroPointX + i, deactivateC].myObject.SetActive(false);
        inactiveTiles.Push(map.MapData[(int)GridZeroPointX + i, deactivateC].myObject);
        map.MapData[(int)GridZeroPointX + i, deactivateC].myObject = null;
    }

    // weg
    private void DeactivateRoomObjectsVerticalForPooling(int deactivateC, int i)
    {
        roomMap.RoomData[(int)GridZeroPointX + i, deactivateC].myObject.SetActive(false);
        inactiveRoomObjects.Push(roomMap.RoomData[(int)GridZeroPointX + i, deactivateC].myObject);
        roomMap.RoomData[(int)GridZeroPointX + i, deactivateC].myObject = null;
    }

    // weg
    private void DeactivateObjectsVerticalForPooling(int deactivateC, int i)
    {
        objectMap.ObjectData[(int)GridZeroPointX + i, deactivateC].myObject.SetActive(false);
        inactiveObjects.Push(objectMap.ObjectData[(int)GridZeroPointX + i, deactivateC].myObject);
        objectMap.ObjectData[(int)GridZeroPointX + i, deactivateC].myObject = null;
    }

    #endregion

    #region HorizontalPooling

    // wird verändert/vereinfacht
    void MoveHorizontal(int activateC, int deactivateC)
    {

        for (int i = 0; i < height; i++)
        {
            if (objectMap.ObjectData[deactivateC, (int)GridZeroPointY + i].myObject != null)
            {
                DeactivateObjectsHorizontalForPooling(deactivateC, i);
            }
            if (roomMap.RoomData[deactivateC, (int)GridZeroPointY + i].myObject != null)
            {
                DeactivateRoomObjectsHorizontalForPooling(deactivateC, i);
            }

            DeactivateTilesHorizontalForPooling(deactivateC, i);
        }

        for (int j = 0; j < height; j++)
        {
            if (objectMap.ObjectData[activateC, (int)GridZeroPointY + j].Position != Vector2.zero)
            {
                ActivateObjectsHorizontalForPooling(activateC, j);
            }
            if (roomMap.RoomData[activateC, (int)GridZeroPointY + j].Position != Vector2.zero)
            {
                ActivateRoomObjectsHorizontalForPooling(activateC, j);
            }

            ActivateTilesHorizontalForPooling(activateC, j);
        }
    }

    // weg
    private void ActivateTilesHorizontalForPooling(int activateC, int j)
    {
        inactiveTiles.Peek().transform.position = map.MapData[activateC, (int)GridZeroPointY + j].Position;
        inactiveTiles.Peek().SetActive(true);
        map.MapData[activateC, (int)GridZeroPointY + j].myObject = inactiveTiles.Peek();
        inactiveTiles.Peek().GetComponent<SpriteRenderer>().sprite = map.MapData[activateC, (int)GridZeroPointY + j].Texture;
        inactiveTiles.Pop();
    }

    // weg
    private void ActivateRoomObjectsHorizontalForPooling(int activateC, int j)
    {
        inactiveRoomObjects.Peek().transform.position = new Vector3(activateC, (int)GridZeroPointY + j, -0.5f);
        inactiveRoomObjects.Peek().SetActive(true);
        roomMap.RoomData[activateC, (int)GridZeroPointY + j].myObject = inactiveRoomObjects.Peek();
        inactiveRoomObjects.Peek().GetComponent<SpriteRenderer>().sprite = roomMap.RoomData[activateC, (int)GridZeroPointY + j].Texture;
        inactiveRoomObjects.Pop();
    }

    // weg
    private void ActivateObjectsHorizontalForPooling(int activateC, int j)
    {
        inactiveObjects.Peek().transform.position = new Vector3(activateC, (int)GridZeroPointY + j, -1);
        inactiveObjects.Peek().SetActive(true);
        objectMap.ObjectData[activateC, (int)GridZeroPointY + j].myObject = inactiveObjects.Peek();
        inactiveObjects.Peek().GetComponent<SpriteRenderer>().sprite = objectMap.ObjectData[activateC, (int)GridZeroPointY + j].Texture;
        inactiveObjects.Pop();
    }

    // weg
    private void DeactivateTilesHorizontalForPooling(int deactivateC, int i)
    {
        map.MapData[deactivateC, (int)GridZeroPointY + i].myObject.SetActive(false);
        inactiveTiles.Push(map.MapData[deactivateC, (int)GridZeroPointY + i].myObject);
        map.MapData[deactivateC, (int)GridZeroPointY + i].myObject = null;
    }

    // weg
    private void DeactivateRoomObjectsHorizontalForPooling(int deactivateC, int i)
    {
        roomMap.RoomData[deactivateC, (int)GridZeroPointY + i].myObject.SetActive(false);
        inactiveRoomObjects.Push(roomMap.RoomData[deactivateC, (int)GridZeroPointY + i].myObject);
        roomMap.RoomData[deactivateC, (int)GridZeroPointY + i].myObject = null;
    }

    // weg
    private void DeactivateObjectsHorizontalForPooling(int deactivateC, int i)
    {
        objectMap.ObjectData[deactivateC, (int)GridZeroPointY + i].myObject.SetActive(false);
        inactiveObjects.Push(objectMap.ObjectData[deactivateC, (int)GridZeroPointY + i].myObject);
        objectMap.ObjectData[deactivateC, (int)GridZeroPointY + i].myObject = null;
    }
    
    #endregion

    // weg
    public void ShowMapContentNew()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (inactiveTiles.Count > 0)
                {
                    inactiveTiles.Peek().transform.position = map.MapData[(int)GridZeroPointX + i, (int)GridZeroPointY + j].Position;
                    inactiveTiles.Peek().SetActive(true);
                    map.MapData[(int)GridZeroPointX + i, (int)GridZeroPointY + j].myObject = inactiveTiles.Peek();
                    inactiveTiles.Peek().GetComponent<SpriteRenderer>().sprite = map.MapData[(int)GridZeroPointX + i, (int)GridZeroPointY + j].Texture;
                    inactiveTiles.Pop();
                }
            }
        }
    }

    // weg
    void FillCameraObjectField()
    {
        for (int i = 0; i < width * height; i++)
        {
            GameObject obj;
            obj = Instantiate(manager.spriteAtlas,new Vector3(0,0,this.ObjectSpawn.transform.position.z),Quaternion.identity) as GameObject;
            obj.layer = 2; // Ignore Raycast
            inactiveObjects.Push(obj);
            obj.SetActive(false);
        }
    }

    // weg
    void FillCameraFieldNew()
    {
        for (int i = 0; i < width * height; i++)
        {
            GameObject obj;
            obj = Instantiate(manager.spriteAtlas, new Vector3(0, 0, this.TileSpawn.transform.position.z), Quaternion.identity) as GameObject;
            inactiveTiles.Push(obj);
            obj.SetActive(false);
        }
    }

    // weg
    void FillStackOfRoomObjects()
    {
        for (int i = 0; i < width * height; i++)
        {
            GameObject obj;
            obj = Instantiate(manager.spriteAtlas,new Vector3(0,0,this.RoomSpawn.transform.position.z),Quaternion.identity) as GameObject;
            obj.layer = 2;
            inactiveRoomObjects.Push(obj);
            obj.SetActive(false);
        }
    }
}