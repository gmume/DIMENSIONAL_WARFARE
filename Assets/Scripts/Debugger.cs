using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class Debugger : MonoBehaviour
{
    public PlayerData player1, player2;
    public string GamePhase;
    public int playerTurn;
    private Camera camera1, camera2;
    public LayerMask layerMask1, layerMask2;

    private PlayerInput playerInput1, playerInput2;
    public bool inputEnabled1, inputEnabled2;
    public InputSystemUIInputModule UIInputModulePlayer1, UIInputModulePlayer2;
    public MultiplayerEventSystem eventSystem1, eventSystem2;
    public string actionMapPlayer1, actionMapPlayer2, gamepadPlayer1, gamepadPlayer2, controlScemePlayer1, controlScemePlayer2;

    private ShipManager actShip1 = null, actShip2 = null;
    public string activeShip1, activeShip2;
    public string focusedCell1;
    public string focusedCell1OnDimension;
    public string focusedCell2;
    public string focusedCell2OnDimension;
    public GameObject activeDimension1, activeDimension2;

    public GameObject currentSelectedButton1, currentSelectedButton2;

    private void FixedUpdate()
    {
        GamePhase = OverworldData.GamePhase.ToString();
        playerTurn = OverworldData.PlayerTurn;

        actShip1 = player1.ActiveShip;
        if (player1.ActiveShip != null)
        {
            activeShip1 = actShip1 + ": " + actShip1.navigator.PivotX + ", " + actShip1.navigator.PivotZ;
        }
        else
        {
            activeShip1 = "";
        }

        actShip2 = player2.ActiveShip;
        if (player2.ActiveShip != null)
        {
            activeShip2 = actShip2 + ": " + actShip2.navigator.PivotX + ", " + actShip2.navigator.PivotZ;
        }
        else
        {
            activeShip2 = "";
        }

        if (player1.FocusedCell && player2.FocusedCell)
        {
            focusedCell1 = player1.FocusedCell.X + ", " + player1.FocusedCell.Y;
            focusedCell1OnDimension = player1.FocusedCell.Dimension.name;

            focusedCell2 = player2.FocusedCell.X + ", " + player2.FocusedCell.Y;
            focusedCell1OnDimension = player2.FocusedCell.Dimension.name;
        }

        if (player1.ActiveDimension && player2.ActiveDimension)
        {
            activeDimension1 = player1.ActiveDimension.gameObject;
            activeDimension2 = player2.ActiveDimension.gameObject;
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

    public void ShowCellCoords()
    {
        DimensionManager dimension = player1.dimensions.GetDimension(2);

        for (int i = 0; i < OverworldData.DimensionSize; i++)
        {
            foreach (var cellObj in dimension.Cells[i])
            {
                CellData cell = cellObj.GetComponent<CellData>();
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
                text.font = Resources.Load<Font>("arial");
                text.text = cell.X + "," + cell.Y;
                text.fontSize = 150;
                text.horizontalOverflow = HorizontalWrapMode.Overflow;
                text.verticalOverflow = VerticalWrapMode.Overflow;
                text.alignment = TextAnchor.MiddleCenter;
            }
        }
    }

    public void ShowShipsOwner()
    {
        List<GameObject> fleet = player1.fleet.ships;  

        foreach (GameObject shipObj in fleet)
        {
            GameObject canvasObj;
            GameObject myText;
            Canvas myCanvas;
            Text text;

            canvasObj = new GameObject("TestCanvas", typeof(Canvas), typeof(GraphicRaycaster));
            canvasObj.transform.SetParent(shipObj.transform.GetChild(0).transform, false);
            RectTransform trans = canvasObj.GetComponent<RectTransform>();

            Transform transParent = canvasObj.GetComponent<Transform>();
            trans.position = new Vector3(transParent.position.x, transParent.position.y + 0.51f, transParent.position.z);
            trans.sizeDelta = new Vector3(1, 1, 0);

            myCanvas = canvasObj.GetComponent<Canvas>();
            myCanvas.renderMode = RenderMode.WorldSpace;

            myText = new GameObject("shipText", typeof(Text));
            myText.transform.SetParent(myCanvas.transform, false);
            myText.GetComponent<RectTransform>().localScale = new Vector3(0.003f, 0.003f, 1);

            text = myText.GetComponent<Text>();
            text.font = Resources.Load<Font>("arial");
            text.text = player1.number.ToString();
            text.fontSize = 150;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.alignment = TextAnchor.MiddleCenter;
        }

        Invoke("ShowShipOwner2", 0.1f);
    }

    private void ShowShipOwner2()
    {
        List<GameObject> fleet = player2.fleet.ships;

        foreach (GameObject shipObj in fleet)
        {
            GameObject canvasObj;
            GameObject myText;
            Canvas myCanvas;
            Text text;

            canvasObj = new GameObject("TestCanvas", typeof(Canvas), typeof(GraphicRaycaster));
            canvasObj.transform.SetParent(shipObj.transform.GetChild(0).transform, false);
            RectTransform trans = canvasObj.GetComponent<RectTransform>();

            Transform transParent = canvasObj.GetComponent<Transform>();
            trans.position = new Vector3(transParent.position.x, transParent.position.y + 0.51f, transParent.position.z);
            trans.sizeDelta = new Vector3(1, 1, 0);

            myCanvas = canvasObj.GetComponent<Canvas>();
            myCanvas.renderMode = RenderMode.WorldSpace;

            myText = new GameObject("shipText", typeof(Text));
            myText.transform.SetParent(myCanvas.transform, false);
            myText.GetComponent<RectTransform>().localScale = new Vector3(0.003f, 0.003f, 1);

            text = myText.GetComponent<Text>();
            text.font = Resources.Load<Font>("arial");
            text.text = player2.number.ToString();
            text.fontSize = 150;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.alignment = TextAnchor.MiddleCenter;
        }
    }

    public void Initialize()
    {
        player1 = GameObject.Find("Player1").GetComponent<PlayerData>();
        player2 = GameObject.Find("Player2").GetComponent<PlayerData>();
        GamePhase = OverworldData.GamePhase.ToString();
        playerTurn = OverworldData.PlayerTurn;

        playerInput1 = player1.input;
        playerInput2 = player2.input;
        camera1 = player1.playerCamera;
        camera2 = player2.playerCamera;

        eventSystem1 = player1.eventSystem;
        eventSystem2 = player2.eventSystem;
        UIInputModulePlayer1 = player1.inputSystem;
        UIInputModulePlayer2 = player2.inputSystem;
    }
}
