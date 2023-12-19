using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using static UnityEngine.InputSystem.InputAction;

public class HUD_Manager : MonoBehaviour
{
    public PlayerData player;

    private string x = "--", y = "--";
    private TextMeshProUGUI xCoord, yCoord;

    private GameObject[] HUD_Dimensions, HUD_DimensionsOpponent;
    private int currentHUD_Dimension, currentHUD_DimensionOpponent;
    public Texture2D HUD_DimensionInactive, HUD_DimensionActive;
    public GameObject[] HUD_Fleet, HUD_FleetOpponent;

    [HideInInspector] public GameObject armed;

    [HideInInspector] public TextMeshProUGUI crewText;

    private List<GameObject> shipButtons = new();
    public ShipButtonData currentButton;
    public GameObject selectedButton, selectedElement;

    private void Update()
    {
        xCoord.text = x;
        yCoord.text = y;
    }

    public void OnDimensionUp(CallbackContext ctx)
    {
        if (!ctx.performed) return;

        if (currentHUD_Dimension + 1 < OverworldData.DimensionsCount)
        {
            SetHUDDimension(currentHUD_Dimension + 1);
        }
        else
        {
            Debug.LogWarning($"{name}: Desired dimension out of scope!");
        }
    }

    public void OnDimensionDown(CallbackContext ctx)
    {
        if (!ctx.performed) return;

        if (currentHUD_Dimension - 1 >= 0)
        {
            SetHUDDimension(currentHUD_Dimension - 1);
        }
        else
        {
            Debug.LogWarning($"{name}: Desired dimension out of scope!");
        }
    }

    public List<GameObject> GetShipButtons()
    {
        return shipButtons;
    }

    public void SetSelecetedButton()
    {
        if (!selectedButton) selectedButton = shipButtons[0];

        player.eventSystem.firstSelectedGameObject = selectedButton;
        player.eventSystem.SetSelectedGameObject(selectedButton);
        currentButton = selectedButton.GetComponent<ShipButtonData>();
        player.CurrentShipButton = currentButton;
    }

    public void SetHUDDimension(int toNo)
    {
        GameObject HUD_DimensionToSetInactive = HUD_Dimensions[currentHUD_Dimension];
        HUD_DimensionToSetInactive.GetComponent<RawImage>().texture = HUD_DimensionInactive;
        HUD_DimensionToSetInactive.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
        HUD_DimensionToSetInactive.GetComponentInChildren<TextMeshProUGUI>().fontSize = 22;

        GameObject HUD_DimensionToSetActive = HUD_Dimensions[toNo];
        HUD_DimensionToSetActive.GetComponent<RawImage>().texture = HUD_DimensionActive;
        HUD_DimensionToSetActive.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Bold | FontStyles.Underline;
        HUD_DimensionToSetActive.GetComponentInChildren<TextMeshProUGUI>().fontSize = 28;

        currentHUD_Dimension = toNo;
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

    private void UpdateHUDDimension(int dimension)
    {
        HUD_Dimensions[currentHUD_Dimension].GetComponent<RawImage>().texture = HUD_DimensionInactive;
        HUD_Dimensions[dimension].GetComponent<RawImage>().texture = HUD_DimensionActive;
    }

    public void UpdateHUDFleet(int shipNo, int toDimensionNo, int dimensionBefore)
    {
        Vector3 newPosition = new() { x = 0, y = HUD_Dimensions[toDimensionNo].transform.position.y - HUD_Dimensions[dimensionBefore].transform.position.y };
        HUD_Fleet[shipNo].transform.SetParent(HUD_Dimensions[toDimensionNo].transform);
        HUD_Fleet[shipNo].transform.position += newPosition;

        newPosition = new() { x = 0, y = HUD_DimensionsOpponent[toDimensionNo].transform.position.y - HUD_DimensionsOpponent[dimensionBefore].transform.position.y };
        player.opponent.HUD.HUD_FleetOpponent[shipNo].transform.SetParent(player.opponent.HUD.HUD_DimensionsOpponent[toDimensionNo].transform);
        player.opponent.HUD.HUD_FleetOpponent[shipNo].transform.position += newPosition;
    }

    public void RemoveButton(int index)
    {
        Destroy(shipButtons[index]);
        shipButtons.RemoveAt(index);
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

    public void Initialize() => GetComponent<HUD_Creator>().CreateHUD(ref shipButtons,
                                                                   ref xCoord,
                                                                   ref yCoord,
                                                                   ref HUD_Dimensions,
                                                                   ref currentHUD_Dimension,
                                                                   ref HUD_Fleet,
                                                                   ref HUD_DimensionsOpponent,
                                                                   ref currentHUD_DimensionOpponent,
                                                                   ref HUD_FleetOpponent,
                                                                   ref armed,
                                                                   ref crewText,
                                                                   ref currentButton,
                                                                   ref selectedButton);
}
