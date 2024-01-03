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

    [HideInInspector] public GameObject[] HUD_Dimensions, HUD_DimensionsOpponent;
    [HideInInspector] public GameObject[] HUD_Fleet, HUD_FleetOpponent;

    [HideInInspector] public GameObject armed;

    [HideInInspector] public TextMeshProUGUI crewText;

    private void Update()
    {
        xCoord.text = x;
        yCoord.text = y;
    }

    public void ChooseDimension(int no)
    {
        hudDimensionActivator.ActivateDimansionAtNo(this, no);
    }

    public void ChooseLeftShip(int index)
    {
        player.eventSystem.SetSelectedGameObject(hudButtonHandler.shipButtons[index]);
        player.CurrentShipButton = player.eventSystem.currentSelectedGameObject.GetComponent<ShipButtonData>();
        player.fleet.ActivateShip(player.CurrentShipButton.ShipButtonNr, player);

        if (OverworldData.GamePhase == GamePhases.Battle) UpdateActiveCellAndHUD();
    }

    public void ChooseRightShip(int index)
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

    public void SetHUDDimension(int toNo)
    {
        hudDimensionActivator.ActivateDimansionAtNo(this, toNo);
    }

    public void UpdateHUDCoords(int xCoord, int yCoord)
    {
        // If string is less than 2 characters, it pads the left side with '0' characters. 
        x = xCoord.ToString().PadLeft(2, '0');
        y = yCoord.ToString().PadLeft(2, '0');
    }

    public void UpdateHUDCoords()
    {
        x = "--";
        y = "--";
    }

    //private void UpdateHUDDimension(int dimension)
    //{
    //    HUD_Dimensions[currentHUD_Dimension].GetComponent<RawImage>().texture = HUD_DimensionInactive;
    //    HUD_Dimensions[dimension].GetComponent<RawImage>().texture = HUD_DimensionActive;
    //}

    public void UpdateHUDFleet(int shipNo, int toDimensionNo, int dimensionBefore)
    {
        Vector3 newPosition = new() { x = 0, y = HUD_Dimensions[toDimensionNo].transform.position.y - HUD_Dimensions[dimensionBefore].transform.position.y };
        HUD_Fleet[shipNo].transform.SetParent(HUD_Dimensions[toDimensionNo].transform);
        HUD_Fleet[shipNo].transform.position += newPosition;

        newPosition = new() { x = 0, y = HUD_DimensionsOpponent[toDimensionNo].transform.position.y - HUD_DimensionsOpponent[dimensionBefore].transform.position.y };
        player.opponent.HUD.HUD_FleetOpponent[shipNo].transform.SetParent(player.opponent.HUD.HUD_DimensionsOpponent[toDimensionNo].transform);
        player.opponent.HUD.HUD_FleetOpponent[shipNo].transform.position += newPosition;
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

    public List<GameObject> GetShipButtons()
    {
        return hudButtonHandler.GetShipButtons();
    }

    public void SetSelecetedButton()
    {
        hudButtonHandler.SetSelecetedButton(player);
    }

    public void RemoveShipButton(int index)
    {
        hudButtonHandler.RemoveShipButton(index);
    }
}
