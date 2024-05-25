using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputEnabler : MonoBehaviour
{
    [HideInInspector] public PlayerData player;
    [HideInInspector] public InputActionMap placeShipsMap, battleMap;
    [HideInInspector] public InputAction chooseLeftShip, chooseRightShip;

    private void Start()
    {
        player = GetComponent<PlayerData>();
        placeShipsMap = player.input.actions.FindActionMap("PlaceShips");
        battleMap = player.input.actions.FindActionMap("Battle");
        chooseLeftShip = placeShipsMap.FindAction("chooseLeftShip");
        chooseRightShip = placeShipsMap.FindAction("chooseRightShip");
    }

    public void SwitchActionMap(string actionMapName)
    {
        switch (actionMapName)
        {
            case "PlaceShips":
                placeShipsMap.Enable();
                battleMap.Disable();
                break;
            case "Battle":
                placeShipsMap.Disable();
                battleMap.Enable();
                break;
            default:
                Debug.LogWarning($"{name}: No such action map!");
                break;
        }
    }

    public void DisableChoosingShips()
    {
        chooseLeftShip.Disable();
        chooseRightShip.Disable();
    }
}
