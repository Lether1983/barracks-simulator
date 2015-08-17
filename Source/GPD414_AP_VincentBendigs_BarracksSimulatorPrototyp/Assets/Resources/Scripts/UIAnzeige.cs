using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIAnzeige : MonoBehaviour 
{
    public GameManager gmanager;

    void Update()
    {
        //.ToString()
        this.gameObject.GetComponent<Text>().text = ""+ gmanager.AllSoldiers.Count;
    }
}
