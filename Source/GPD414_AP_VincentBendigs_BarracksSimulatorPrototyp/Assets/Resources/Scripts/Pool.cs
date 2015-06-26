using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Pool : MonoBehaviour 
{
    public Stack<GameObject> inactiveObjects;
    public GameObject Prefab;

    public GameObjectEvent factory;
    public GameObjectEvent enable;
    public GameObjectEvent disable;

    void Start()
    {
        inactiveObjects = new Stack<GameObject>();
    }
    
    public GameObject Get()
    {
        StackIsNotEmpty();

        GameObject instance;
        
        instance = inactiveObjects.Pop();

        instance.SetActive(true);
        if (enable != null)
        {
            enable.Invoke(instance);
        }
        return instance;

    }

    public void Put(GameObject gameObject)
    {
        gameObject.SetActive(false);
        if (disable != null)
        {
            disable.Invoke(gameObject);
        }

        inactiveObjects.Push(gameObject);
    }

    private void StackIsNotEmpty()
    {
        if (inactiveObjects.Count > 0) return;
        GameObject instance;

        instance = GameObject.Instantiate(Prefab);
        if(factory != null)
        {
            factory.Invoke(instance);
        }
        inactiveObjects.Push(instance);
    }
}
