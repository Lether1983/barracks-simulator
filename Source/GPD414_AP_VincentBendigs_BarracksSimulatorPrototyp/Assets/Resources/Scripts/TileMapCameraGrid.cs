using UnityEngine;
using System.Collections;
using System.Collections.Generic;

enum direction { Up,Down,Left,Right};
public class TileMapCameraGrid : MonoBehaviour
{
    Stack<GameObject> inactiveObj;
    GameManager manager;
    CameraControl camera;
    TileMap map = TileMap.Instance();
    direction directions;
    int width = 41;
    int height = 27;
    float Offset = 3;
    public float timer = 0;
    float CameraSize;
    float GridZeroPointX;
    float GridZeroPointY;
    float GridMaxPointX;
    float GridMaxPointY;
    
    
    void Start()
    {
        inactiveObj = new Stack<GameObject>();
        GridZeroPointX = this.transform.position.x - (width / 2);
        GridZeroPointY = this.transform.position.y - (height / 2);
        GridMaxPointX = this.transform.position.x + (width / 2);
        GridMaxPointY = this.transform.position.y + (height / 2);
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        CameraSize = this.gameObject.GetComponent<Camera>().orthographicSize;
        camera = this.gameObject.GetComponent<CameraControl>();
        FillCameraFieldNew();
        ShowMapContentNew();
    }
    
    void Update()
    {
        timer += Time.deltaTime;

        if(camera.moveDirection.x != 0 || camera.moveDirection.y != 0)
        {
            if (timer > 0.5)
            {
                MoveCameraGrid();
                timer = 0;
            }
        }
    }

    public void MoveCameraGrid()
    {
        if(camera.moveDirection.x < -0.1)
        {
            if (this.gameObject.transform.position.x != map.purchasedLandWidthMin)
            {
                CameraMove(direction.Left);
            }
        }
        else if (camera.moveDirection.x > 0.1)
        {
            if (this.gameObject.transform.position.x != map.purchasedLandWidthMax)
            {
                CameraMove(direction.Right);
            }
        }
        if(camera.moveDirection.y < -0.1)
        {
            if (this.gameObject.transform.position.y != map.purchasedLandHeightMin)
            {
                CameraMove(direction.Down);
            }
        }
        else if(camera.moveDirection.y > 0.1)
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

    void MoveHorizontal(int activateC, int deactivateC)
    {
        for (int i = 0; i < height; i++)
        {
            map.MapData[deactivateC, (int)GridZeroPointY + i].myObject.SetActive(false);
            inactiveObj.Push(map.MapData[deactivateC, (int)GridZeroPointY + i].myObject);
            map.MapData[deactivateC, (int)GridZeroPointY + i].myObject = null;
        }
        for (int j = 0; j < height; j++)
        {
            inactiveObj.Peek().transform.position = map.MapData[activateC,(int)GridZeroPointY + j].Position;
            inactiveObj.Peek().SetActive(true);
            map.MapData[activateC, (int)GridZeroPointY + j].myObject = inactiveObj.Peek();
            inactiveObj.Peek().GetComponent<SpriteRenderer>().sprite = map.MapData[activateC, (int)GridZeroPointY + j].Texture;
            inactiveObj.Pop();
        }
    }

    void MoveVertical(int activateC,int deactivateC)
    {
        for (int i = 0; i < width; i++)
        {
            map.MapData[(int)GridZeroPointX + i, deactivateC].myObject.SetActive(false);
            inactiveObj.Push(map.MapData[(int)GridZeroPointX + i, deactivateC].myObject);
            map.MapData[(int)GridZeroPointX + i, deactivateC].myObject = null;
        }
        for (int j = 0; j < width; j++)
        {
            inactiveObj.Peek().transform.position = map.MapData[(int)GridZeroPointX + j, activateC].Position;
            inactiveObj.Peek().SetActive(true);
            map.MapData[(int)GridZeroPointX + j, activateC].myObject = inactiveObj.Peek();
            inactiveObj.Peek().GetComponent<SpriteRenderer>().sprite = map.MapData[(int)GridZeroPointX + j,activateC].Texture;
            inactiveObj.Pop();
        }


    }

    public void ShowMapContentNew()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (inactiveObj.Count > 0)
                {
                    inactiveObj.Peek().transform.position = map.MapData[(int)GridZeroPointX + i, (int)GridZeroPointY + j].Position;
                    inactiveObj.Peek().SetActive(true);
                    map.MapData[(int)GridZeroPointX + i, (int)GridZeroPointY + j].myObject = inactiveObj.Peek();
                    inactiveObj.Peek().GetComponent<SpriteRenderer>().sprite = map.MapData[(int)GridZeroPointX + i, (int)GridZeroPointY + j].Texture;
                    inactiveObj.Pop();
                }
            }
        }
    }

    void FillCameraFieldNew()
    {
        for (int i = 0; i < width*height; i++)
        {
            GameObject obj;
            obj = Instantiate(manager.spriteAtlas) as GameObject;
            obj.AddComponent<SpriteTile>();
            inactiveObj.Push(obj);
            obj.SetActive(false);
        }
    }
}