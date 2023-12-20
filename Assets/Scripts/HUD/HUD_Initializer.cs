using TMPro;
using UnityEngine;

public class HUD_Initializer : MonoBehaviour
{
    public PlayerData player;
    public HUD_Manager hudManager;

    [Header("Manager helpers")]
    public HUD_DimensionActivator hudDimensionActivator;
    public HUD_ButtonHandler hudButtonHandler;

    [Header("Initializers")]
    public HUD_FleetInitializer hudFleetInitializer;
    public HUD_DimensionsInitializer hudDimensionsInitializer;
    public HUD_ButtonInitializer hudButtonInitializer;

    [Header("Prefabs")]
    public GameObject HUD_DimensionsObj;
    public GameObject HUD_DimensionsOpponentObj;

    public void Initialize()
    {
        hudManager.hudDimensionActivator = this.hudDimensionActivator;
        hudManager.hudButtonHandler = this.hudButtonHandler;

        GetHUDParts();
        InitializeHUDDimensions();
        InitializeShipButtons();
        player.fleet.ActivateShip(hudManager.hudButtonHandler.currentButton.ShipButtonNr, player);
    }

    private void GetHUDParts()
    {
        GameObject[] HUD_Parts = player.name == "Player1" ? GameObject.FindGameObjectsWithTag("HUD1") : GameObject.FindGameObjectsWithTag("HUD2");

        foreach (GameObject HUD_Part in HUD_Parts)
        {
            switch (HUD_Part.name)
            {
                case "X-Koordinate":
                    hudManager.xCoord = HUD_Part.GetComponent<TextMeshProUGUI>();
                    break;
                case "Y-Koordinate":
                    hudManager.yCoord = HUD_Part.GetComponent<TextMeshProUGUI>();
                    break;
                case "HUD_Dimensions":
                    HUD_DimensionsObj = HUD_Part;
                    break;
                case "HUD_DimensionsOpponent":
                    HUD_DimensionsOpponentObj = HUD_Part;
                    break;
                case "Armed":
                    hudManager.armed = HUD_Part;
                    HUD_Part.SetActive(false);
                    break;
                case "CrewText":
                    hudManager.crewText = HUD_Part.GetComponent<TextMeshProUGUI>();
                    break;
                case "ShipButtons":
                    hudButtonInitializer.shipButtonsTransform = HUD_Part.transform;
                    break;
            }
        }
    }

    private void InitializeHUDDimensions()
    {
        hudDimensionsInitializer.InitializeHUDDimensions(hudManager, hudFleetInitializer, HUD_DimensionsObj, player.fleetColor);
        hudDimensionsInitializer.InitializeHUDDimensions(hudManager, hudFleetInitializer, HUD_DimensionsOpponentObj, player.opponent.fleetColor);
        hudDimensionActivator.currentHUD_Dimension = hudDimensionActivator.currentHUD_DimensionOpponent = 0;
    }

    private void InitializeShipButtons()
    {
        hudButtonInitializer.InitializeShipButtons(hudManager);
    }
}
