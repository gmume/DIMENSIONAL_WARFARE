using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class Debugging : MonoBehaviour
{
    public Player player1, player2;
    private PlayerInput playerInput1, playerInput2;
    private Camera camera1, camera2;
    private Ship actShip1 = null, actShip2 = null;

    public string GamePhase;
    public int playerTurn;
    public string activeShip1, activeShip2, activeCell1, activeCell2;
    public GameObject activeDimension1, activeDimension2;
    public bool inputEnabled1, inputEnabled2;
    public string actionMapPlayer1, actionMapPlayer2, gamepadPlayer1, gamepadPlayer2, controlScemePlayer1, controlScemePlayer2;
    public MultiplayerEventSystem eventSystem1, eventSystem2;
    public GameObject currentSelectedButton1, currentSelectedButton2;
    public InputSystemUIInputModule UIInputModulePlayer1, UIInputModulePlayer2;
    public LayerMask layerMask1, layerMask2;

    public GameObject currentActiveDimension = null;

    private void FixedUpdate()
    {
        GamePhase = OverworldData.GamePhase.ToString();
        playerTurn = OverworldData.PlayerTurn;

        actShip1 = player1.ActiveShip;
        if (player1.ActiveShip != null)
        {
            activeShip1 = actShip1 + ": " + actShip1.PivotX + ", " + actShip1.PivotZ;
        }
        else
        {
            activeShip1 = "";
        }

        actShip2 = player2.ActiveShip;
        if (player2.ActiveShip != null)
        {
            activeShip2 = actShip2 + ": " + actShip2.PivotX + ", " + actShip2.PivotZ;
        }
        else
        {
            activeShip2 = "";
        }

        if (player1.ActiveCell && player2.ActiveCell)
        {
            activeCell1 = player1.ActiveCell.X + ", " + player1.ActiveCell.Y;
            activeCell2 = player2.ActiveCell.X + ", " + player2.ActiveCell.Y;
        }

        if (player1.ActiveDimension && player2.ActiveDimension)
        {
            //activeDimension1 = player1.ActiveDimension.gameObject;
            //activeDimension2 = player2.ActiveDimension.gameObject;

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

        // Track player 2, active dimension.
        if (name == "Player2" && currentActiveDimension != activeDimension2)
        {
            Debug.Log("current active diemension: " + currentActiveDimension + " new active dimension: " + activeDimension2);
            currentActiveDimension = activeDimension2;
        }
    }

    public void ShowCellCoords()
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
        List<GameObject> fleet = player1.fleet.GetFleet();

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
        List<GameObject> fleet = player2.fleet.GetFleet();

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

    public void InitDebugging()
    {
        player1 = GameObject.Find("Player1").GetComponent<Player>();
        player2 = GameObject.Find("Player2").GetComponent<Player>();
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
