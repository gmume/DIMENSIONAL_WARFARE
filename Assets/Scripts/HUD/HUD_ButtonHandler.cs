using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD_ButtonHandler : MonoBehaviour
{
    [HideInInspector] public readonly List<GameObject> shipButtons = new();
    [HideInInspector] public ShipButtonData currentButton;
    [HideInInspector] public GameObject selectedButton;

    public List<GameObject> GetShipButtons() => shipButtons;

    public void SetSelecetedButton(PlayerData player)
    {
        if (player.LastActiveShip != null)
        {
            for (int i = 0; i < shipButtons.Count; i++)
            {
                if (player.LastActiveShip.No == shipButtons[i].GetComponent<ShipButtonData>().No)
                {
                    selectedButton = shipButtons[i];
                    break;
                }
            }

        }
        else if (!selectedButton)
        {
            selectedButton = shipButtons[0];
        }

        player.eventSystem.firstSelectedGameObject = selectedButton;
        player.eventSystem.SetSelectedGameObject(selectedButton);
        currentButton = selectedButton.GetComponent<ShipButtonData>();
        player.CurrentShipButton = currentButton;
    }

    public void RemoveShipButton(int index)
    {
        Destroy(shipButtons[index]);
        shipButtons.RemoveAt(index);
    }
}
