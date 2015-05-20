using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour 
{
    public GameObject BauPanel;
    public GameObject GroundTilePanel;
    public GameObject WallTilePanel;

    void Start()
    {
        BauPanel.SetActive(false);
        GroundTilePanel.SetActive(false);
        WallTilePanel.SetActive(false);
    }
    public void DesertButtonSet()
    {
        this.gameObject.GetComponent<GameManager>().BuildDesert = !this.gameObject.GetComponent<GameManager>().BuildDesert;
    }

    public void GrassButtonSet()
    {
        this.gameObject.GetComponent<GameManager>().BuildGrass = !this.gameObject.GetComponent<GameManager>().BuildGrass;
    }

    public void WallButtonSet()
    {
        this.gameObject.GetComponent<GameManager>().BuildWalls = !this.gameObject.GetComponent<GameManager>().BuildWalls;
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
        if(GroundTilePanel.activeInHierarchy)
        {
            GroundTilePanel.SetActive(false);
        }
        else
        {
            GroundTilePanel.SetActive(true);
        }
    }

    public void WallTileMenuPanelActivate()
    {
        if (WallTilePanel.activeInHierarchy)
        {
            WallTilePanel.SetActive(false);
        }
        else
        {
            WallTilePanel.SetActive(true);
        }
    }
}
