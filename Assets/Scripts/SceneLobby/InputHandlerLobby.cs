using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using static UnityEngine.InputSystem.InputAction;

public class InputHandlerLobby : MonoBehaviour
{
    public void Start()
    {
        List<InputDevice> devices = new();

        foreach (var device in InputSystem.devices)
        {
            if (device.ToString().Contains("Gamepad")) devices.Add(device);
        }

        if (devices.Count >= 2)
        {
            GetComponent<PlayerInput>().user.UnpairDevices();
            InputDevice targetDevice = (name == "EventSystem1") ? devices[0] : devices[1];
            InputUser.PerformPairingWithDevice(targetDevice, GetComponent<PlayerInput>().user);
        }
        else
        {
            Debug.LogWarning("Gamepad missing!");
        }
    }

    public void OnStartGame(CallbackContext ctx)
    {
        if (ctx.performed) SceneChanger.LoadPlay();
    }
}
