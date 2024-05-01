using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputEnabler : MonoBehaviour
{
    [HideInInspector] public PlayerData player;
    [HideInInspector] public InputActionMap gameStartMap, playerMap;
    [HideInInspector] public InputAction chooseLeftShip, chooseRightShip;

    private void Start()
    {
        player = GetComponent<PlayerData>();
        gameStartMap = player.input.actions.FindActionMap("GameStart");
        playerMap = player.input.actions.FindActionMap("Player");
        chooseLeftShip = gameStartMap.FindAction("chooseLeftShip");
        chooseRightShip = gameStartMap.FindAction("chooseRightShip");
    }

    public void SwitchActionMap(string actionMapName)
    {
        switch (actionMapName)
        {
            case "GameStart":
                gameStartMap.Enable();
                playerMap.Disable();
                break;
            case "Player":
                gameStartMap.Disable();
                playerMap.Enable();
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
