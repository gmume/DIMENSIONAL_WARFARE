using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class HUD_Manager : MonoBehaviour
{
    [HideInInspector] public PlayerData player;

    [HideInInspector] public HUD_DimensionActivator hudDimensionActivator;
    [HideInInspector] public HUD_ButtonHandler hudButtonHandler;

    private string x = "--", y = "--";
    [HideInInspector] public TextMeshProUGUI xCoord, yCoord;

    [HideInInspector] public GameObject[] HUD_Dimensions { private set; get; }
    [HideInInspector] public GameObject[] HUD_DimensionsOpponent { private set; get; }
    [HideInInspector] public GameObject[] HUD_Fleet { private set; get; }
    [HideInInspector] public GameObject[] HUD_FleetOpponent { private set; get;}

    [HideInInspector] public GameObject armed;

    [HideInInspector] public TextMeshProUGUI crewText;

    private void Awake()
    {
        HUD_Dimensions = new GameObject[OverworldData.DimensionsCount];
        HUD_DimensionsOpponent = new GameObject[OverworldData.DimensionsCount];
        HUD_Fleet = new GameObject[OverworldData.FleetSize];
        HUD_FleetOpponent = new GameObject[OverworldData.FleetSize];
    }

    private void Update()
    {
        xCoord.text = x;
        yCoord.text = y;
    }

    public void ChooseDimension(int no) => hudDimensionActivator.ActivateDimansionAtNo(this, no);

    public void ChooseShip(int index)
    {
        player.eventSystem.SetSelectedGameObject(hudButtonHandler.shipButtons[index]);
        player.CurrentShipButton = player.eventSystem.currentSelectedGameObject.GetComponent<ShipButtonData>();
        player.fleet.ActivateShip(player.CurrentShipButton.ShipButtonNr, player);

        if (OverworldData.GamePhase == GamePhases.Battle) UpdateActiveCellAndHUD();
    }

    public void UpdateActiveCellAndHUD()
    {
        int shipX = player.ActiveShip.navigator.PivotX;
        int shipY = player.ActiveShip.navigator.PivotZ;

        player.HUD.UpdateHUDCoords(shipX, shipY);
        player.opponent.HUD.UpdateHUDCoords(shipX, shipY);
        player.world.SetNewCellAbsolute(shipX, shipY);
    }

    public void SetHUDDimension(int toNo) => hudDimensionActivator.ActivateDimansionAtNo(this, toNo);

    public void UpdateHUDCoords(int xCoord, int yCoord)
    {
        // If string is less than 2 characters, it pads the left side with '0' characters. 
        x = xCoord.ToString().PadLeft(2, '0');
        y = yCoord.ToString().PadLeft(2, '0');
    }

    public void UpdateHUDCoords() => x = y = "--";

    public void UpdateHUDFleets(int shipNo, int toDimensionNo, int dimensionBefore)
    {
        UpdateHUDFleet(shipNo, toDimensionNo, dimensionBefore, HUD_Dimensions, HUD_Fleet);
        UpdateHUDFleet(shipNo, toDimensionNo, dimensionBefore, player.opponent.HUD.HUD_DimensionsOpponent, player.opponent.HUD.HUD_FleetOpponent);
    }

    private void UpdateHUDFleet(int shipNo, int toDimensionNo, int dimensionBefore, GameObject[] HUD_Dimensions, GameObject[] HUD_Fleet)
    {
        Debug.Log(name + "  player.opponent.HUD: " + player.opponent.HUD);
        Vector3 newPosition = new() { x = 0, y = HUD_Dimensions[toDimensionNo].transform.position.y - HUD_Dimensions[dimensionBefore].transform.position.y };
        HUD_Fleet[shipNo].transform.SetParent(HUD_Dimensions[toDimensionNo].transform);
        HUD_Fleet[shipNo].transform.position += newPosition;
    }

    public void WriteText(string text)
    {
        crewText.text = text;
        StartCoroutine(CleanTextField());
    }

    private IEnumerator CleanTextField()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(5f);
        Time.timeScale = 1f;
        crewText.text = null;
    }

    public List<GameObject> GetShipButtons() => hudButtonHandler.GetShipButtons();

    public void SetSelecetedButton() => hudButtonHandler.SetSelecetedButton(player);

    public void RemoveShipButton(int index) => hudButtonHandler.RemoveShipButton(index);
}
