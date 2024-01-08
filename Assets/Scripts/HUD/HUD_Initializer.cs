using System.Collections;
using TMPro;
using Unity.VisualScripting;
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
    [HideInInspector] public GameObject HUD_DimensionsObj;
    [HideInInspector] public GameObject HUD_DimensionsOpponentObj;

    public void Start()
    {
        hudManager.player = player;
        hudManager.hudDimensionActivator = this.hudDimensionActivator;
        hudManager.hudButtonHandler = this.hudButtonHandler;
        
        GetHUDParts();
        InitializeHUDDimensions();
        InitializeShipButtons();
        
        StartCoroutine(FleetActivateShip());
    }

    IEnumerator FleetActivateShip()
    {
        yield return new WaitWhile(() => player.fleet.GetFleet().Count == 0);
        player.fleet.ActivateShip(hudManager.hudButtonHandler.currentButton.ShipButtonNr, player);
    }

    private void GetHUDParts()
    {
        GameObject[] HUD_Parts = name == "HUD1" ? GameObject.FindGameObjectsWithTag("HUD1") : GameObject.FindGameObjectsWithTag("HUD2");

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
        hudDimensionsInitializer.InitializeHUDDimensions(hudManager, hudFleetInitializer, HUD_DimensionsObj, player.HUD.HUD_Dimensions, player.HUD.HUD_Fleet, player.fleetColor);
        hudDimensionsInitializer.InitializeHUDDimensions(hudManager, hudFleetInitializer, HUD_DimensionsOpponentObj, player.HUD.HUD_DimensionsOpponent, player.HUD.HUD_FleetOpponent, player.opponent.fleetColor);
        hudDimensionActivator.currentHUD_Dimension = hudDimensionActivator.currentHUD_DimensionOpponent = 0;

        //Debug.Log(name + "-HUD_Fleet: " + player.HUD.HUD_Fleet[0] +", "+ player.HUD.HUD_Fleet[1]+"\n"+name +"-HUD_OpponentFleet: " + player.HUD.HUD_FleetOpponent[0] + ", " + player.HUD.HUD_FleetOpponent[1]);
    }

    private void InitializeShipButtons() => hudButtonInitializer.InitializeShipButtons(hudManager);
}