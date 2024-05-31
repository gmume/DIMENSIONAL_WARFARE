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

    public void Start()
    {
        hudManager.player = player;
        hudManager.hudDimensionActivator = this.hudDimensionActivator;
        hudManager.hudButtonHandler = this.hudButtonHandler;
        
        GetHUDParts();
        InitializeHUDManger();
        InitializeHUDDimensions();
        InitializeShipButtons();

        Invoke("DisableLayoutGroup", 0.05f);
        StartCoroutine(FleetActivateShip());
        Invoke("EnablePointer", 0.1f);
    }

    private void DisableLayoutGroup() => hudButtonInitializer.shipButtonsTransform.GetComponent<HorizontalLayoutGroup>().enabled = false;

    private void EnablePointer() => player.Pointer.enabled = true;

    IEnumerator FleetActivateShip()
    {
        yield return new WaitWhile(() => player.fleet.ships.Count == 0);
        player.fleet.ActivateShip(hudManager.hudButtonHandler.currentButton.No, player);
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
        hudManager.instructions = BuildDictionary();
        hudManager.Instruct("PlaceShips");
    }

    private Dictionary<string, Texture> BuildDictionary() =>
    new()
    {
        {"None", null},
        {"PlaceShips", Resources.Load<Texture>("HUD_Sprites/Instructions/PlaceShips")},
        {"Ready", Resources.Load<Texture>("HUD_Sprites/Instructions/Ready")},
        {"Attack", Resources.Load<Texture>("HUD_Sprites/Instructions/Armed")},
        {"UnderAttack", Resources.Load<Texture>("HUD_Sprites/Instructions/UnderAttack")},
        //{"OwnShipUp", Resources.Load<Texture>("HUD_Sprites/Instructions/")},
        //{"OpponentShipUp", Resources.Load<Texture>("HUD_Sprites/Instructions/")},
        //{"OwnShipDown", Resources.Load<Texture>("HUD_Sprites/Instructions/")},
        //{"OwnShipDestroyed", Resources.Load<Texture>("HUD_Sprites/Instructions/")},
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
