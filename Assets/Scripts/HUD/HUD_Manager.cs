using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class HUD_Manager : MonoBehaviour
{
    private string x = "--", y = "--";
    private TextMeshProUGUI xCoord, yCoord;
    public GameObject armed;
    private List<GameObject> shipButtons = new();

    private GameObject[] HUD_Dimensions;
    private int currentHUD_Dimension;
    
    public Player player;
    public Texture2D HUD_DimensionInactive, HUD_DimensionActive;
    public ShipButton currentButton;
    public GameObject selectedButton, selectedElement;

    private void Update()
    {
        xCoord.text = x;
        yCoord.text = y;
    }

    public void OnDimensionUp(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (currentHUD_Dimension + 1 < OverworldData.DimensionsCount)
            {
                SetHUDDimension(currentHUD_Dimension + 1);
            }
            else
            {
                Debug.LogWarning(name + ": Desired dimension out of scope!");
            }
        }
    }

    public void OnDimensionDown(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (currentHUD_Dimension - 1 >= 0)
            {
                SetHUDDimension(currentHUD_Dimension - 1);
            }
            else
            {
                Debug.LogWarning(name + ": Desired dimension out of scope!");
            }
        }
    }

    public List<GameObject> GetShipButtons()
    {
        return shipButtons;
    }

    public void SetSelecetedButton()
    {
        if (!selectedButton)
        {
            selectedButton = shipButtons[0];
        }

        player.eventSystem.firstSelectedGameObject = selectedButton;
        player.eventSystem.SetSelectedGameObject(selectedButton);
        currentButton = selectedButton.GetComponent<ShipButton>();
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
        HUD_DimensionToSetActive.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
        HUD_DimensionToSetActive.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Underline;
        HUD_DimensionToSetActive.GetComponentInChildren<TextMeshProUGUI>().fontSize = 28;

        currentHUD_Dimension = toNo;
    }

    public void UpdateHUDCoords(int xCoord, int yCoord)
    {
        if (xCoord.ToString().Length < 2)
        {
            x = "0" + xCoord.ToString();
        }
        else
        {
            x = xCoord.ToString();
        }

        if (yCoord.ToString().Length < 2)
        {
            y = "0" + yCoord.ToString();
        }
        else
        {
            y = yCoord.ToString();
        }
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

    public void RemoveButton(int index)
    {
        Destroy(shipButtons[index]);
        shipButtons.RemoveAt(index);
    }

    public void InitHUD() => GetComponent<HUD_Creator>().CreateHUD(ref shipButtons, ref xCoord, ref yCoord,  ref armed, ref currentHUD_Dimension, ref currentButton, ref selectedButton, ref HUD_Dimensions);
}
