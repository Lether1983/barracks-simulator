using UnityEngine;
using System.Collections;
using System;

public class CameraControl : MonoBehaviour 
{
    public Vector2 moveDirection;
    public float Speed;
    public GameObject gmanager;
    TileMap map = TileMap.Instance();
	
    // Update is called once per frame
    void Update()
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal")*Speed, Input.GetAxis("Vertical")*Speed,-10);

        transform.Translate(moveDirection * Time.deltaTime);
        transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, (float)(map.purchasedLandWidthMin), (float)(map.purchasedLandWidthMax)),
                                         Mathf.Clamp(this.transform.position.y, (float)(map.purchasedLandHeightMin), (float)(map.purchasedLandHeightMax)),-10);
    }
}