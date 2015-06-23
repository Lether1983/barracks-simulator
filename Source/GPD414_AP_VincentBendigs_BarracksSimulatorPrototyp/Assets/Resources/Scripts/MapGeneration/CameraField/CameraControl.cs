using UnityEngine;
using System.Collections;
using System;

public class CameraControl : MonoBehaviour 
{
    public Vector2 moveDirection;
    public float Speed;
    public bool isOnMove;
    private float Boundary = 10;
    TileMap map = TileMap.Instance();
	
    // Update is called once per frame
    void Update()
    {
        if((moveDirection.x < -0.1 && moveDirection.y < -0.1)||(moveDirection.y > 0.1 && moveDirection.x > 0.1)||
           (moveDirection.x < -0.1 && moveDirection.y > 0.1)||(moveDirection.x > 0.1 && moveDirection.y < -0.1))
        {
            isOnMove = true;
        }
        else
        {
            isOnMove = false;
        }

        if (isOnMove)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal") * Speed/1.5f, Input.GetAxis("Vertical") * Speed/1.5f, -10);
        }
        else
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal") * Speed, Input.GetAxis("Vertical") * Speed, -10);
        }

        if (Input.GetAxis("Mouse ScrollWheel") <= 0)
        {
            if (this.GetComponent<Camera>().orthographicSize < 11)
            {
                this.GetComponent<Camera>().orthographicSize++;
            }
        }

        if(Input.GetAxis("Mouse ScrollWheel") >= 0)
        {
            if(this.GetComponent<Camera>().orthographicSize > 5)
            {
                this.GetComponent<Camera>().orthographicSize--;
            }
        }

        if (Input.mousePosition.x >= Screen.width - Boundary)
        {
            if (isOnMove)
            {
                moveDirection.x += Speed / 1.5f;
            }
            else
            {
                moveDirection.x += Speed;
            }
        }

        if (Input.mousePosition.x <= 0 + Boundary)
        {
            if (isOnMove)
            {
                moveDirection.x -= Speed / 1.5f;
            }
            else
            {
                moveDirection.x -= Speed;
            }
        }

        if (Input.mousePosition.y >= Screen.height - Boundary)
        {
            if (isOnMove)
            {
                moveDirection.y += Speed / 1.5f;
            }
            else
            {
                moveDirection.y += Speed;
            }
        }

        if (Input.mousePosition.y <= 0 + Boundary)
        {
            if (isOnMove)
            {
                moveDirection.y -= Speed / 1.5f;
            }
            else
            {
                moveDirection.y -= Speed;
            }
        }



        transform.Translate(moveDirection*Time.deltaTime);
        transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, (float)(map.purchasedLandWidthMin), (float)(map.purchasedLandWidthMax)),
                                         Mathf.Clamp(this.transform.position.y, (float)(map.purchasedLandHeightMin), (float)(map.purchasedLandHeightMax)),-10);
    }
}