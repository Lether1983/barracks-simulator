using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour 
{
    public GameObject BauPanel;
    public GameObject TilePanel;
    void Start()
    {
        BauPanel.SetActive(false);
        TilePanel.SetActive(false);
    }

    public void BauMenuPanelActivate()
    {
       if(BauPanel.activeInHierarchy)
       {
           BauPanel.SetActive(false);
       }
       else
       {
           BauPanel.SetActive(true);
       }
    }
    public void TileMenuPanelActivate()
    {
        if(TilePanel.activeInHierarchy)
        {
            TilePanel.SetActive(false);
        }
        else
        {
            TilePanel.SetActive(true);
        }
    }
}
