using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class InputInitializer : MonoBehaviour
{
    public PlayerData player;
    public InputHandler inputHandler;

    public void Initialize()
    {
        inputHandler.player = player;
        inputHandler.player.input.SwitchCurrentActionMap("GameStart");
        List<InputDevice> devices = new();

        foreach (var device in InputSystem.devices)
        {
            if (device.ToString().Contains("Gamepad")) devices.Add(device);
        }

        if (devices.Count >= 2)
        {
            player.input.user.UnpairDevices();
            InputDevice targetDevice = (name == "Player1") ? devices[0] : devices[1];
            InputUser.PerformPairingWithDevice(targetDevice, player.input.user);
        }
        else
        {
            Debug.LogWarning("Gamepad missing!");
        }
    }
}
