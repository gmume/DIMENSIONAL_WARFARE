using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public void Awake()
    {
        hudManager.player = player;
        hudManager.hudDimensionActivator = this.hudDimensionActivator;
        hudManager.hudButtonHandler = this.hudButtonHandler;
    }

    private IEnumerator FleetActivateShip()
    {
        yield return new WaitWhile(() => player.fleet.ships.Count == 0);
        player.fleet.ActivateShip(hudManager.hudButtonHandler.currentButton.No);
    }

    public void Initialize()
    {
        GetHUDParts();
        InitializeHUDManger();
        InitializeHUDDimensions();
        InitializeShipButtons();
        StartCoroutine(FleetActivateShip());
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
                case "Instruction":
                    hudManager.instructionImg = HUD_Part.GetComponent<RawImage>();
                    break;
                case "CrewTextBackground":
                    hudManager.crewText = HUD_Part.GetComponentInChildren<TextMeshProUGUI>();
                    break;
                case "ShipButtons":
                    hudButtonInitializer.shipButtonsTransform = HUD_Part.transform;
                    break;
            }
        }
    }

    private void InitializeHUDManger()
    {
        hudManager.shipButtonsObj = hudButtonInitializer.shipButtonsTransform.gameObject;
        hudManager.instructions = BuildDictionary();
        hudManager.Instruct("PlaceShips");
    }

    private Dictionary<string, Texture> BuildDictionary() =>
    new()
    {
        {"None", null},
        {"PlaceShips", Resources.Load<Texture>("HUD_Sprites/Instructions/PlaceShips")},
        {"Ready", Resources.Load<Texture>("HUD_Sprites/Instructions/Ready")},
        {"Attack", Resources.Load<Texture>("HUD_Sprites/Instructions/Fire")},
        {"UnderAttack", Resources.Load<Texture>("HUD_Sprites/Instructions/UnderAttack")},
        {"Wait", Resources.Load<Texture>("HUD_Sprites/Instructions/Wait")},
    };

    private void InitializeHUDDimensions()
    {
        hudDimensionsInitializer.player = this.player;

        hudDimensionsInitializer.InitializeHUDDimensions(true,
                                                         hudFleetInitializer,
                                                         HUD_DimensionsObj,
                                                         HUD_DimensionPrefab,
                                                         player.HUD.HUD_Dimensions,
                                                         player.HUD.HUD_Fleet,
                                                         player.fleetColor);

        hudDimensionsInitializer.InitializeHUDDimensions(false,
                                                         hudFleetInitializer,
                                                         HUD_DimensionsOpponentObj,
                                                         HUD_DimensionPrefab,
                                                         player.HUD.HUD_DimensionsOpponent,
                                                         player.HUD.HUD_FleetOpponent,
                                                         player.opponent.fleetColor);
    }

    private void InitializeShipButtons() => hudButtonInitializer.InitializeShipButtons(hudManager);


}
