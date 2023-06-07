using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class Debugging : MonoBehaviour
{
    private Player player1;
    private Player player2;
    private GameObject playerObj1;
    private GameObject playerObj2;
    private PlayerWorld playerWorld1;
    private PlayerWorld playerWorld2;
    private PlayerInput playerInput1;
    private PlayerInput playerInput2;
    private Camera camera1;
    private Camera camera2;

    public string GamePhase;
    public int playerTurn;
    public PlayerData playerData1;
    public PlayerData playerData2;
    public Ship activeShip1;
    public Ship activeShip2;
    public Cell activeCell1;
    public Cell activeCell2;
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

    private void Awake()
    {
        player1 = GameObject.Find("Player1").GetComponent<Player>();
        player2 = GameObject.Find("Player1").GetComponent<Player>();
    }

    void Start()
    {
        playerObj1 = player1.obj;
        playerObj2 = GameObject.Find("Player2");
        playerWorld1 = player1.world;
        playerWorld2 = player2.world;
        GamePhase = OverworldData.GamePhase.ToString();
        playerTurn = OverworldData.PlayerTurn;
        playerData1 = player1.data;
        playerData2 = player2.data;

        playerInput1 = player1.input;
        playerInput2 = player2.input;
        camera1 = player1.cameraObj.GetComponent<Camera>();
        camera2 = player2.cameraObj.GetComponent<Camera>();

        eventSystem1 = player1.eventSystem;
        eventSystem2 = player2.eventSystem;
        UIInputModulePlayer1 = player1.inputSystem;
        UIInputModulePlayer2 = player2.inputSystem;
    }

    void Update()
    {
        GamePhase = OverworldData.GamePhase.ToString();
        playerTurn = OverworldData.PlayerTurn;

        activeShip1 = playerWorld1.playerData.ActiveShip;
        activeShip2 = playerWorld2.playerData.ActiveShip;

        activeCell1 = player1.data.ActiveCell;
        activeCell2 = player2.data.ActiveCell;

        if (playerInput1.enabled && playerInput2.enabled)
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
