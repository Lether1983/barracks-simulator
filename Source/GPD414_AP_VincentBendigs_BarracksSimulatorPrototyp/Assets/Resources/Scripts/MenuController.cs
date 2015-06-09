using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour
{
    #region Panel GameObjects
    public GameObject BauPanel;
    public GameObject GroundTilePanel;
    public GameObject WallTilePanel;
    public GameObject FoundationTilePanel;
    public GameObject ObjectTilePanel;
    public GameObject RoomTilePanel;
    #endregion

    void Start()
    {
        BauPanel.SetActive(false);
        GroundTilePanel.SetActive(false);
        WallTilePanel.SetActive(false);
        FoundationTilePanel.SetActive(false);
        ObjectTilePanel.SetActive(false);
        RoomTilePanel.SetActive(false);
    }

    //GroundTileButtons
    public void DesertButtonSet()
    {
        this.gameObject.GetComponent<GameManager>().ResetAllBuildingModi();
        this.gameObject.GetComponent<GameManager>().BuildDesert = !this.gameObject.GetComponent<GameManager>().BuildDesert;
    }

    public void GrassButtonSet()
    {
        this.gameObject.GetComponent<GameManager>().ResetAllBuildingModi();
        this.gameObject.GetComponent<GameManager>().BuildGrass = !this.gameObject.GetComponent<GameManager>().BuildGrass;
    }
    
    public void BetonButtonSet()
    {
        this.gameObject.GetComponent<GameManager>().ResetAllBuildingModi();
        this.gameObject.GetComponent<GameManager>().BuildBeton = !this.gameObject.GetComponent<GameManager>().BuildBeton;
    }

    // WallTilesButtons
    public void WallButtonSet()
    {
        this.gameObject.GetComponent<GameManager>().ResetAllBuildingModi();
        this.gameObject.GetComponent<GameManager>().BuildWalls = !this.gameObject.GetComponent<GameManager>().BuildWalls;
    }

    //FoundationsButtons
    public void FoundationButtonSet()
    {
        this.gameObject.GetComponent<GameManager>().ResetAllBuildingModi();
        this.gameObject.GetComponent<GameManager>().BuildFoundation = !this.gameObject.GetComponent<GameManager>().BuildFoundation;
    }

    //ObjectsButtons
    public void ObjectShowerPlace()
    {
        this.gameObject.GetComponent<GameManager>().ResetAllBuildingModi();
        this.gameObject.GetComponent<GameManager>().InObjectBuildMode = !this.gameObject.GetComponent<GameManager>().InObjectBuildMode;
        this.GetComponent<ObjectManager>().IplaceShower = !this.GetComponent<ObjectManager>().IplaceShower;
    }

    //RoomButtons
    public void RoomStubePlacement()
    {
        this.gameObject.GetComponent<GameManager>().ResetAllBuildingModi();
        this.GetComponent<GameManager>().InRoomBuildMode = !this.GetComponent<GameManager>().InRoomBuildMode;
        this.GetComponent<RoomManager>().MeIsAStube = !this.GetComponent<RoomManager>().MeIsAStube;
    }
   
    //Destroy Buttons
    public void DestroyTileButtonSet()
    {
        this.gameObject.GetComponent<GameManager>().ResetAllBuildingModi();
        this.gameObject.GetComponent<GameManager>().DestroyTiles = !this.gameObject.GetComponent<GameManager>().DestroyTiles;
    }
   
    public void DestroyWallsButtonSet()
    {
        this.gameObject.GetComponent<GameManager>().ResetAllBuildingModi();
        this.gameObject.GetComponent<GameManager>().DestroyWalls = !this.gameObject.GetComponent<GameManager>().DestroyWalls;
    }

    public void DestroyFoundationButtonSet()
    {
        this.gameObject.GetComponent<GameManager>().ResetAllBuildingModi();
        this.gameObject.GetComponent<GameManager>().DestroyFoundation = !this.gameObject.GetComponent<GameManager>().DestroyFoundation;
    }

    // Build Modi

    public void RoomBuildButtonSet()
    {
        this.GetComponent<GameManager>().InRoomBuildMode = !this.GetComponent<GameManager>().InRoomBuildMode;
    }

    public void DestroyModusSet()
    {
        this.GetComponent<GameManager>().DestroyModus = !this.GetComponent<GameManager>().DestroyModus;
    }

    public void ObjectBuildModeButtonSet()
    {
        this.gameObject.GetComponent<GameManager>().InObjectBuildMode = !this.gameObject.GetComponent<GameManager>().InObjectBuildMode;
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
