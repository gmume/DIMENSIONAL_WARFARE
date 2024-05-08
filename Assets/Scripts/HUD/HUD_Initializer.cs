using System.Collections;
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
    [HideInInspector] public GameObject HUD_DimensionsObj;
    [HideInInspector] public GameObject HUD_DimensionsOpponentObj;
    public GameObject HUD_DimensionPrefab;
    public GameObject HUD_DimensionOpponentPrefab;

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
                case "Coordinates":
                    hudManager.xCoord = HUD_Part.transform.Find("X-Coordinate").GetComponent<TextMeshProUGUI>();
                    hudManager.yCoord = HUD_Part.transform.Find("Y-Coordinate").GetComponent<TextMeshProUGUI>();
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
                case "UnderAttack":
                    hudManager.underAttack = HUD_Part;
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
        hudDimensionsInitializer.InitializeHUDDimensions(hudManager, hudFleetInitializer, HUD_DimensionsObj, HUD_DimensionPrefab, player.HUD.HUD_Dimensions, player.HUD.HUD_Fleet, player.fleetColor);
        hudDimensionsInitializer.InitializeHUDDimensions(hudManager, hudFleetInitializer, HUD_DimensionsOpponentObj, HUD_DimensionOpponentPrefab, player.HUD.HUD_DimensionsOpponent, player.HUD.HUD_FleetOpponent, player.opponent.fleetColor);
        hudDimensionActivator.currentHUD_Dimension = hudDimensionActivator.currentHUD_DimensionOpponent = 0;
    }

    private void InitializeShipButtons() => hudButtonInitializer.InitializeShipButtons(hudManager);
}
