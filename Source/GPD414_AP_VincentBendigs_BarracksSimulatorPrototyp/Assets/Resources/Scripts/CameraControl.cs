using UnityEngine;
using System.Collections;
using System;

public class CameraControl : MonoBehaviour 
{
    public Vector2 moveDirection;
    public float Speed;
    GameManager gmanager;
    TileMap map = TileMap.Instance();
    Camera camera;
   
    void Start()
    {
        gmanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        camera = GetComponent<Camera>();
    }

	// Update is called once per frame
    void Update()
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal")*Speed, Input.GetAxis("Vertical")*Speed,-10);

        transform.Translate(moveDirection * Time.deltaTime);
        transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, (float)(map.purchasedLandWidthMin), (float)(map.purchasedLandWidthMax)),
                                         Mathf.Clamp(this.transform.position.y, (float)(map.purchasedLandHeightMin), (float)(map.purchasedLandHeightMax)),-10);
    
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Penis");
            Vector2 worldpoint = camera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldpoint,Vector2.zero);
            if (hit.collider != null)
            {
                Debug.Log(hit.transform.gameObject.GetComponent<SpriteRenderer>().sprite);
                hit.transform.gameObject.GetComponent<SpriteRenderer>().sprite = gmanager.Desert;
                
            }
        }
    }
}