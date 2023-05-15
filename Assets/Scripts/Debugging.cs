using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class Debugging : MonoBehaviour
{
    private GameObject playerObj1;
    private GameObject playerObj2;
    private PlayerInput playerInput1;
    private PlayerInput playerInput2;
    private Camera camera1;
    private Camera camera2;

    public bool inputEnabled1;
    public bool inputEnabled2;
    public string actionMapPlayer1;
    public string actionMapPlayer2;
    public string gamepadPlayer1;
    public string gamepadPlayer2;
    public string controlScemePlayer1;
    public string controlScemePlayer2;
    public MultiplayerEventSystem eventSystem1;
    public MultiplayerEventSystem eventSystem2;
    public GameObject currentSelectedButton1;
    public GameObject currentSelectedButton2;
    public InputSystemUIInputModule UIInputModulePlayer1;
    public InputSystemUIInputModule UIInputModulePlayer2;
    public LayerMask layerMask1;
    public LayerMask layerMask2;

    void Start()
    {
        playerObj1 = GameObject.Find("Player1");
        playerObj2 = GameObject.Find("Player2");
        playerInput1 = playerObj1.GetComponent<PlayerInput>();
        playerInput2 = playerObj2.GetComponent<PlayerInput>();
        camera1 = GameObject.Find("Camera1").GetComponent<Camera>();
        camera2 = GameObject.Find("Camera2").GetComponent<Camera>();

        eventSystem1 = GameObject.Find("EventSystem1").GetComponent<MultiplayerEventSystem>();
        eventSystem2 = GameObject.Find("EventSystem2").GetComponent<MultiplayerEventSystem>();
        UIInputModulePlayer1 = playerInput1.uiInputModule;
        UIInputModulePlayer2 = playerInput2.uiInputModule;
    }

    void Update()
    {
        if(playerInput1.enabled && playerInput2.enabled)
        {
            actionMapPlayer1 = playerInput1.currentActionMap.name.ToString();
            actionMapPlayer2 = playerInput2.currentActionMap.name.ToString();
        }

        if (playerInput1.devices.Count >= 1)
        {
            gamepadPlayer1 = playerInput1.user.pairedDevices.ToString();
        }
        if (playerInput2.devices.Count >= 1)
        {
            gamepadPlayer2 = playerInput2.user.pairedDevices.ToString();
        }

        layerMask1 = camera1.cullingMask;
        layerMask2 = camera2.cullingMask;

        controlScemePlayer1 = playerInput1.currentControlScheme;
        controlScemePlayer2 = playerInput2.currentControlScheme;

        inputEnabled1 = playerInput1.enabled;
        inputEnabled2 = playerInput2.enabled;

        currentSelectedButton1 = eventSystem1.currentSelectedGameObject;
        currentSelectedButton2 = eventSystem2.currentSelectedGameObject;
    }
}
