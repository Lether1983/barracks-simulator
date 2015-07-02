using UnityEngine;
using System.Collections;

public class PoolCheck : MonoBehaviour
{
    public Pool pool;
    public CameraControl cameraControll;
    public Rect Size;

    void FixedUpdate()
    {
        Transform tempTransform;
        Rect TempRect;

        TempRect = cameraControll.fieldOfView;
        tempTransform = cameraControll.gameObject.transform;

        TempRect.xMin -= cameraControll.ViewExtention;
        TempRect.xMax += cameraControll.ViewExtention;
        TempRect.yMin += cameraControll.ViewExtention;
        TempRect.yMax -= cameraControll.ViewExtention;

        Size.position = new Vector2(transform.position.x, transform.position.y - Size.height);

        if(!TempRect.Overlaps(Size, true))
        {
            pool.Put(gameObject);
        }
    }
}
