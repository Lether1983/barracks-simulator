using UnityEngine;
using System.Collections;
using System.Collections.Generic;

enum direction { Up,Down,Left,Right};
public class TileMapCameraGrid : MonoBehaviour
{
    #region Fields
    public Stack<GameObject> inactiveObjects;
    public Stack<GameObject> inactiveRoomObjects;
    public float timer= 0;
    public GameObject TileSpawn;
    public GameObject ObjectSpawn;
    public GameObject RoomSpawn;
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
        inactiveTiles = new Stack<GameObject>();
        inactiveObjects = new Stack<GameObject>();
        inactiveRoomObjects = new Stack<GameObject>();
        GridZeroPointX = this.transform.position.x - (width / 2);
        GridZeroPointY = this.transform.position.y - (height / 2);
        GridMaxPointX = this.transform.position.x + (width / 2);
        GridMaxPointY = this.transform.position.y + (height / 2);
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        mainCamera = this.gameObject.GetComponent<CameraControl>();
        FillCameraFieldNew();
        FillCameraObjectField();
        FillStackOfRoomObjects();
        ShowMapContentNew();
    }
    
    void Update()
    {
        timer += Time.deltaTime;
       
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

    private void ActivateTilesVerticalForPooling(int activateC, int j)
    {
        inactiveTiles.Peek().transform.position = map.MapData[(int)GridZeroPointX + j, activateC].Position;
        inactiveTiles.Peek().SetActive(true);
        map.MapData[(int)GridZeroPointX + j, activateC].myObject = inactiveTiles.Peek();
        inactiveTiles.Peek().GetComponent<SpriteRenderer>().sprite = map.MapData[(int)GridZeroPointX + j, activateC].Texture;
        inactiveTiles.Pop();
    }

    private void ActivateRoomObjectsVerticalForPooling(int activateC, int j)
    {
        inactiveRoomObjects.Peek().transform.position = new Vector3(roomMap.RoomData[(int)GridZeroPointX + j, activateC].Position.x, roomMap.RoomData[(int)GridZeroPointX + j, activateC].Position.y, -0.5f);
        inactiveRoomObjects.Peek().SetActive(true);
        roomMap.RoomData[(int)GridZeroPointX + j, activateC].myObject = inactiveRoomObjects.Peek();
        inactiveRoomObjects.Peek().GetComponent<SpriteRenderer>().sprite = roomMap.RoomData[(int)GridZeroPointX + j, activateC].Texture;
        inactiveRoomObjects.Pop();
    }

    private void ActivateObjectsVerticalForPooling(int activateC, int j)
    {
        inactiveObjects.Peek().transform.position = new Vector3(objectMap.ObjectData[(int)GridZeroPointX + j, activateC].Position.x, objectMap.ObjectData[(int)GridZeroPointX + j, activateC].Position.y, -1);
        inactiveObjects.Peek().SetActive(true);
        objectMap.ObjectData[(int)GridZeroPointX + j, activateC].myObject = inactiveObjects.Peek();
        inactiveObjects.Peek().GetComponent<SpriteRenderer>().sprite = objectMap.ObjectData[(int)GridZeroPointX + j, activateC].Texture;
        inactiveObjects.Pop();
    }

    private void DeactivateTilesVerticalForPooling(int deactivateC, int i)
    {
        map.MapData[(int)GridZeroPointX + i, deactivateC].myObject.SetActive(false);
        inactiveTiles.Push(map.MapData[(int)GridZeroPointX + i, deactivateC].myObject);
        map.MapData[(int)GridZeroPointX + i, deactivateC].myObject = null;
    }

    private void DeactivateRoomObjectsVerticalForPooling(int deactivateC, int i)
    {
        roomMap.RoomData[(int)GridZeroPointX + i, deactivateC].myObject.SetActive(false);
        inactiveRoomObjects.Push(roomMap.RoomData[(int)GridZeroPointX + i, deactivateC].myObject);
        roomMap.RoomData[(int)GridZeroPointX + i, deactivateC].myObject = null;
    }

    private void DeactivateObjectsVerticalForPooling(int deactivateC, int i)
    {
        objectMap.ObjectData[(int)GridZeroPointX + i, deactivateC].myObject.SetActive(false);
        inactiveObjects.Push(objectMap.ObjectData[(int)GridZeroPointX + i, deactivateC].myObject);
        objectMap.ObjectData[(int)GridZeroPointX + i, deactivateC].myObject = null;
    }

    #endregion

    #region HorizontalPooling

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

    private void ActivateTilesHorizontalForPooling(int activateC, int j)
    {
        inactiveTiles.Peek().transform.position = map.MapData[activateC, (int)GridZeroPointY + j].Position;
        inactiveTiles.Peek().SetActive(true);
        map.MapData[activateC, (int)GridZeroPointY + j].myObject = inactiveTiles.Peek();
        inactiveTiles.Peek().GetComponent<SpriteRenderer>().sprite = map.MapData[activateC, (int)GridZeroPointY + j].Texture;
        inactiveTiles.Pop();
    }

    private void ActivateRoomObjectsHorizontalForPooling(int activateC, int j)
    {
        inactiveRoomObjects.Peek().transform.position = new Vector3(roomMap.RoomData[activateC, (int)GridZeroPointY + j].Position.x, roomMap.RoomData[activateC, (int)GridZeroPointY + j].Position.y, -0.5f);
        inactiveRoomObjects.Peek().SetActive(true);
        roomMap.RoomData[activateC, (int)GridZeroPointY + j].myObject = inactiveRoomObjects.Peek();
        inactiveRoomObjects.Peek().GetComponent<SpriteRenderer>().sprite = roomMap.RoomData[activateC, (int)GridZeroPointY + j].Texture;
        inactiveRoomObjects.Pop();
    }

    private void ActivateObjectsHorizontalForPooling(int activateC, int j)
    {
        inactiveObjects.Peek().transform.position = new Vector3(objectMap.ObjectData[activateC, (int)GridZeroPointY + j].Position.x, objectMap.ObjectData[activateC, (int)GridZeroPointY + j].Position.y, -1);
        inactiveObjects.Peek().SetActive(true);
        objectMap.ObjectData[activateC, (int)GridZeroPointY + j].myObject = inactiveObjects.Peek();
        inactiveObjects.Peek().GetComponent<SpriteRenderer>().sprite = objectMap.ObjectData[activateC, (int)GridZeroPointY + j].Texture;
        inactiveObjects.Pop();
    }

    private void DeactivateTilesHorizontalForPooling(int deactivateC, int i)
    {
        map.MapData[deactivateC, (int)GridZeroPointY + i].myObject.SetActive(false);
        inactiveTiles.Push(map.MapData[deactivateC, (int)GridZeroPointY + i].myObject);
        map.MapData[deactivateC, (int)GridZeroPointY + i].myObject = null;
    }

    private void DeactivateRoomObjectsHorizontalForPooling(int deactivateC, int i)
    {
        roomMap.RoomData[deactivateC, (int)GridZeroPointY + i].myObject.SetActive(false);
        inactiveRoomObjects.Push(roomMap.RoomData[deactivateC, (int)GridZeroPointY + i].myObject);
        roomMap.RoomData[deactivateC, (int)GridZeroPointY + i].myObject = null;
    }

    private void DeactivateObjectsHorizontalForPooling(int deactivateC, int i)
    {
        objectMap.ObjectData[deactivateC, (int)GridZeroPointY + i].myObject.SetActive(false);
        inactiveObjects.Push(objectMap.ObjectData[deactivateC, (int)GridZeroPointY + i].myObject);
        objectMap.ObjectData[deactivateC, (int)GridZeroPointY + i].myObject = null;
    }
    
    #endregion

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

    void FillCameraObjectField()
    {
        for (int i = 0; i < width * height; i++)
        {
            GameObject obj;
            obj = Instantiate(manager.spriteAtlas,new Vector3(0,0,this.ObjectSpawn.transform.position.z),Quaternion.identity) as GameObject;
            inactiveObjects.Push(obj);
            obj.SetActive(false);
        }
    }

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

    void FillStackOfRoomObjects()
    {
        for (int i = 0; i < width * height; i++)
        {
            GameObject obj;
            obj = Instantiate(manager.spriteAtlas,new Vector3(0,0,this.RoomSpawn.transform.position.z),Quaternion.identity) as GameObject;
            inactiveRoomObjects.Push(obj);
            obj.SetActive(false);
        }
    }
}