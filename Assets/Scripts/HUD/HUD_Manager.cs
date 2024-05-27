using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    //[HideInInspector] public GameObject armed;
    //[HideInInspector] public GameObject underAttack;
    [HideInInspector] public RawImage instructionImg;
    [HideInInspector] public Dictionary<string, Texture> instructions;

    [HideInInspector] public TextMeshProUGUI crewText;

    private void Awake()
    {
        HUD_Dimensions = new GameObject[OverworldData.DimensionsCount];
        HUD_DimensionsOpponent = new GameObject[OverworldData.DimensionsCount];
        HUD_Fleet = new GameObject[OverworldData.FleetSize];
        HUD_FleetOpponent = new GameObject[OverworldData.FleetSize];
    }

    //private void Update()
    //{
    //    xCoord.text = x;
    //    yCoord.text = y;
    //}

    public void ChooseDimension(int no) => hudDimensionActivator.ActivateDimensionAtNo(this, no);

    public void ChooseShip(int index)
    {
        player.eventSystem.SetSelectedGameObject(hudButtonHandler.shipButtons[index]);
        player.CurrentShipButton = player.eventSystem.currentSelectedGameObject.GetComponent<ShipButtonData>();

        if (OverworldData.GamePhase == GamePhases.Battle)
        {
            UpdateHUDCoords(OverworldData.MiddleCoordNo, OverworldData.MiddleCoordNo);
            player.opponent.HUD.UpdateHUDCoords(OverworldData.MiddleCoordNo, OverworldData.MiddleCoordNo);
        }
    }
    
    public void SetHUDDimension(int toNo) => hudDimensionActivator.ActivateDimensionAtNo(this, toNo);

    public void UpdateHUDCoords(int xCoord, int yCoord)
    {
        // If string is less than 2 characters, it pads the left side with '0' characters. 
        x = xCoord.ToString().PadLeft(2, '0');
        y = yCoord.ToString().PadLeft(2, '0');
    }

    public void UpdateHUDCoords() => x = y = "--";

    public void UpdateHUDFleets(int shipNo, int toDimensionNo)
    {
        UpdateHUDFleet(shipNo, toDimensionNo, HUD_Dimensions, HUD_Fleet);
        UpdateHUDFleet(shipNo, toDimensionNo, player.opponent.HUD.HUD_DimensionsOpponent, player.opponent.HUD.HUD_FleetOpponent);
    }

    private void UpdateHUDFleet(int shipNo, int toDimensionNo, GameObject[] HUD_Dimensions, GameObject[] HUD_Fleet) => HUD_Fleet[shipNo].transform.SetParent(HUD_Dimensions[toDimensionNo].transform, false);

    public void Instruct(string instruction)
    {
        instructionImg.texture = instructions[instruction];
        instructionImg.SetNativeSize();
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
