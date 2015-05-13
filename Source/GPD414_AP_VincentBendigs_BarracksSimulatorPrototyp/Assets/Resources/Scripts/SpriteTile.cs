using UnityEngine;
using System.Collections;

public class SpriteTile : MonoBehaviour 
{
    public Rect Rectangle;

    void Start()
    {
        Rectangle = new Rect(transform.position.x,transform.position.y,1, 1);
    }

	public void Destroy()
    {
        gameObject.SetActive(false);
    }
}
