using UnityEngine;
using System.Collections;
using DH.Messaging.Bus;

public class MenuController : MonoBehaviour
{
    //TODO: Aufpassen
    /*
    Ein Großteil der Logik hier, kann durch ein "Tool-System" verhindert werden.
    Dieses System erlaubt es, der Maus ein Tool mitzugeben, das diverse Dinge veranstaltet.
    Beispielsweise gibt es ein RoomPlaceTool, welches ein RoomObject bekommt, und damit dann die Logik
    durchführt.
    Oder ein TilePlaceTool, das ein GroundTile-Dingens bekommt und damit logik durchführt. So wäre auch
    der redundante Teil des Input- und GameManagers optimiert.
    */
    #region Panel GameObjects
    public GameObject BauPanel;
    public GameObject DienstPanel;
    public GameObject GroundTilePanel;
    public GameObject WallTilePanel;
    public GameObject FoundationTilePanel;
    public GameObject ObjectTilePanel;
    public GameObject RoomTilePanel;
    public GameObject DienstplanPanel;
    public GameObject ToolTipTextImage;
    #endregion

    void Start()
    {
        BauPanel.SetActive(false);
        DienstPanel.SetActive(false);
        GroundTilePanel.SetActive(false);
        WallTilePanel.SetActive(false);
        FoundationTilePanel.SetActive(false);
        ObjectTilePanel.SetActive(false);
        RoomTilePanel.SetActive(false);
        DienstplanPanel.SetActive(false);
        ToolTipTextImage.SetActive(false);
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

    public void ObjectDoorPlace()
    {
        this.gameObject.GetComponent<GameManager>().ResetAllBuildingModi();
        this.gameObject.GetComponent<GameManager>().InObjectBuildMode = !this.gameObject.GetComponent<GameManager>().InObjectBuildMode;
        this.GetComponent<ObjectManager>().DoorPlacement = !this.GetComponent<ObjectManager>().DoorPlacement;
    }

    public void ObjectBedPlace()
    {
        this.gameObject.GetComponent<GameManager>().ResetAllBuildingModi();
        this.gameObject.GetComponent<GameManager>().InObjectBuildMode = !this.gameObject.GetComponent<GameManager>().InObjectBuildMode;
    }

    //RoomButtons
    public void RoomStubePlacement()
    {
        this.gameObject.GetComponent<GameManager>().ResetAllBuildingModi();
        this.GetComponent<GameManager>().InRoomBuildMode = !this.GetComponent<GameManager>().InRoomBuildMode;
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
    
    public void DestroyRoomButtonSet()
    {
        this.gameObject.GetComponent<GameManager>().ResetAllBuildingModi();
        this.GetComponent<GameManager>().DestroyRooms = !this.GetComponent<GameManager>().DestroyRooms;
    }

    public void DestroyObjectsButtonSet()
    {
        this.gameObject.GetComponent<GameManager>().ResetAllBuildingModi();
        this.GetComponent<GameManager>().DestroyObjects = !this.GetComponent<GameManager>().DestroyObjects;
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

    //Panel Activation and Deactivation

    public void BauMenuPanelActivate()
    {
       if(BauPanel.activeInHierarchy)
       {
           BauPanel.SetActive(false);
       }
       else
       {

           BauPanel.SetActive(true);
           DienstPanel.SetActive(false);
       }
    }
   
    public void DienstMenuPanelActivate()
    {
        if (DienstPanel.activeInHierarchy)
        {
            DienstPanel.SetActive(false);
        }
        else
        {
            DienstPanel.SetActive(true);
            BauPanel.SetActive(false);
        }
    }

    public void DienstplanPanelActivate()
    {
        if(DienstplanPanel.activeInHierarchy)
        {
            DienstplanPanel.SetActive(false);
        }
        else
        {
            DienstplanPanel.SetActive(true);
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

    public void GroundTileButtonSet(GroundObject ground_Object)
    {
        this.GetComponent<GameManager>().ground_Object = ground_Object;
    }

    public void WallTileButtonSet(GroundObject ground_object)
    {
        this.GetComponent<GameManager>().wall_Object = ground_object;
    }

    public void ObjectTileButtonSet(ObjectsObject object_object)
    {
        this.GetComponent<GameManager>().object_object = object_object;
    }

    public void RoomTileButtonSet(RoomObjects room_object)
    {
        this.GetComponent<GameManager>().room_object = room_object;
    }

    public void TrusterState(trusterState state)
    {
        this.GetComponent<GameManager>().state = state;
    }

    public void SetKompanie(KompanieObject kompanie)
    {
        this.GetComponent<GameManager>().kompanie = kompanie;
    }
}
