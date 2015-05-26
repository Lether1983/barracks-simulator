using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour 
{
    public GameObject BauPanel;
    public GameObject GroundTilePanel;
    public GameObject WallTilePanel;
    public GameObject FoundationTilePanel;
    public GameObject ObjectTilePanel;
    public GameObject RoomTilePanel;

    void Start()
    {
        BauPanel.SetActive(false);
        GroundTilePanel.SetActive(false);
        WallTilePanel.SetActive(false);
        FoundationTilePanel.SetActive(false);
        ObjectTilePanel.SetActive(false);
        RoomTilePanel.SetActive(false);
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

    public void FoundationButtonSet()
    {
        this.gameObject.GetComponent<GameManager>().BuildFoundation = !this.gameObject.GetComponent<GameManager>().BuildFoundation;
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
            WallTilePanel.SetActive(false);
            FoundationTilePanel.SetActive(false);
            ObjectTilePanel.SetActive(false);
            RoomTilePanel.SetActive(false);
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
            GroundTilePanel.SetActive(false);
            FoundationTilePanel.SetActive(false);
            ObjectTilePanel.SetActive(false);
            RoomTilePanel.SetActive(false);
        }
    }

    public void FoundationTileMenuPanelActivate()
    {
        if (FoundationTilePanel.activeInHierarchy)
        {
            FoundationTilePanel.SetActive(false);
        }
        else
        {
            FoundationTilePanel.SetActive(true);
            GroundTilePanel.SetActive(false);
            WallTilePanel.SetActive(false);
            ObjectTilePanel.SetActive(false);
            RoomTilePanel.SetActive(false);
        }
    }

    public void ObjectTileMenuPanelActivate()
    {
        if (ObjectTilePanel.activeInHierarchy)
        {
            ObjectTilePanel.SetActive(false);
        }
        else
        {
            ObjectTilePanel.SetActive(true);
            GroundTilePanel.SetActive(false);
            WallTilePanel.SetActive(false);
            FoundationTilePanel.SetActive(false);
            RoomTilePanel.SetActive(false);
        }
    }

    public void RoomTileMenuPanelActivate()
    {
        if (RoomTilePanel.activeInHierarchy)
        {
            RoomTilePanel.SetActive(false);
        }
        else
        {
            RoomTilePanel.SetActive(true);
            GroundTilePanel.SetActive(false);
            WallTilePanel.SetActive(false);
            FoundationTilePanel.SetActive(false);
            ObjectTilePanel.SetActive(false);
        }
    }
}
