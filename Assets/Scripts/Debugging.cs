using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

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
    private Ship actShip1;
    private Ship actShip2;

    public string GamePhase;
    public int playerTurn;
    public string activeShip1;
    public string activeShip2;
    public string activeCell1;
    public string activeCell2;
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

        playerInput1 = player1.input;
        playerInput2 = player2.input;
        camera1 = player1.cameraObj.GetComponent<Camera>();
        camera2 = player2.cameraObj.GetComponent<Camera>();

        eventSystem1 = player1.eventSystem;
        eventSystem2 = player2.eventSystem;
        UIInputModulePlayer1 = player1.inputSystem;
        UIInputModulePlayer2 = player2.inputSystem;

        ShowCellCoords();
    }

    void Update()
    {
        GamePhase = OverworldData.GamePhase.ToString();
        playerTurn = OverworldData.PlayerTurn;

        if(player1.ActiveShip || player2.ActiveShip)
        {
            actShip1 = player1.ActiveShip;
            actShip2 = player2.ActiveShip;
            activeShip1 = actShip1.ShipName + ": " + actShip1.PivotX + ", " + actShip1.PivotZ;
            activeShip2 = actShip2.ShipName + ": " + actShip2.PivotX + ", " + actShip2.PivotZ;
        }

        if(player1.ActiveCell && player2.ActiveCell)
        {
            activeCell1 = player1.ActiveCell.X + ", " + player1.ActiveCell.Y;
            activeCell2 = player2.ActiveCell.X + ", " + player2.ActiveCell.Y;
        }

        currentSelectedButton1 = eventSystem1.currentSelectedGameObject;
        currentSelectedButton2 = eventSystem2.currentSelectedGameObject;

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
    }

    private void ShowCellCoords()
    {
        Dimension dimension = player1.dimensions.GetDimension(0);

        for (int i = 0; i < OverworldData.DimensionSize; i++)
        {
            foreach (var cellObj in dimension.cells[i])
            {
                Cell cell = cellObj.GetComponent<Cell>();
                GameObject canvasObj;
                GameObject myText;
                Canvas myCanvas;
                Text text;

                canvasObj = new GameObject("TestCanvas", typeof(Canvas), typeof(GraphicRaycaster));
                canvasObj.transform.SetParent(cellObj.transform, false);
                RectTransform trans = canvasObj.GetComponent<RectTransform>();
                trans.Rotate(new Vector3(90, 0, 0));

                Transform transParent = canvasObj.GetComponent<Transform>();
                trans.position = new Vector3(transParent.position.x, 0.51f, transParent.position.z);
                trans.sizeDelta = new Vector3(1, 1, 0);

                myCanvas = canvasObj.GetComponent<Canvas>();
                myCanvas.renderMode = RenderMode.WorldSpace;

                myText = new GameObject("cellText", typeof(Text));
                myText.transform.SetParent(myCanvas.transform, false);
                myText.GetComponent<RectTransform>().localScale = new Vector3(0.003f, 0.003f, 1);

                text = myText.GetComponent<Text>();
                text.font = (Font)Resources.Load("arial");
                text.text = cell.X + "," + cell.Y;
                text.fontSize = 150;
                text.horizontalOverflow = HorizontalWrapMode.Overflow;
                text.verticalOverflow = VerticalWrapMode.Overflow;
                text.alignment = TextAnchor.MiddleCenter;
            }
        }
    }
}
