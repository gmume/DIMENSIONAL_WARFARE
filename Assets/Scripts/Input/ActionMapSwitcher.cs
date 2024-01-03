using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionMapSwitcher : MonoBehaviour
{
    [HideInInspector] public PlayerData player;
    [HideInInspector] public InputActionMap gameStartMap, playerMap;

    private void Start()
    {
        player = GetComponent<PlayerData>();
        gameStartMap = player.input.actions.FindActionMap("GameStart");
        playerMap = player.input.actions.FindActionMap("Player");
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
}
