using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.Users;
using static UnityEngine.InputSystem.InputAction;

public class InputHandling : MonoBehaviour
{
    private MultiplayerEventSystem eventSystem;
    private GameObject opponentObj;
    private InputHandling opponentInputHandling;
    private PlayerWorld playerWorld, opponentScript;
    private PlayerInput playerInput, opponentInput;
    private InputActionMap gameStartMap, playerMap, fleetMenuMap;
    private CameraBehavior playerCameraBehavior, opponentCameraBehavior;
    private Fleet fleet, opponentFleet;
    private FleetMenuScript fleetMenuScript, opponentFleetMenuScript;
    private GameObject[] shipButtons;

    private void Awake()
    {
        playerWorld = GetComponent<PlayerWorld>();
        playerInput = GetComponent<PlayerInput>();
        gameStartMap = playerInput.actions.FindActionMap("GameStart");
        playerMap = playerInput.actions.FindActionMap("Player");
        fleetMenuMap = playerInput.actions.FindActionMap("FleetMenu");
    }

    private void Start()
    {
        if (name == "Player1")
        {
            playerCameraBehavior = GameObject.Find("Camera1").GetComponent<CameraBehavior>();
            opponentCameraBehavior = GameObject.Find("Camera2").GetComponent<CameraBehavior>();
            eventSystem = GameObject.Find("EventSystem1").GetComponent<MultiplayerEventSystem>();
            opponentObj = GameObject.Find("Player2");
            opponentScript = opponentObj.GetComponent<PlayerWorld>();
            fleetMenuScript = GameObject.Find("FleetMenu1").GetComponent<FleetMenuScript>();
            opponentFleetMenuScript = GameObject.Find("FleetMenu2").GetComponent<FleetMenuScript>();
        }
        else
        {
            playerCameraBehavior = GameObject.Find("Camera2").GetComponent<CameraBehavior>();
            opponentCameraBehavior = GameObject.Find("Camera1").GetComponent<CameraBehavior>();
            eventSystem = GameObject.Find("EventSystem2").GetComponent<MultiplayerEventSystem>();
            opponentObj = GameObject.Find("Player1");
            opponentScript = opponentObj.GetComponent<PlayerWorld>();
            fleetMenuScript = GameObject.Find("FleetMenu2").GetComponent<FleetMenuScript>();
            opponentFleetMenuScript = GameObject.Find("FleetMenu1").GetComponent<FleetMenuScript>();
        }

        opponentInputHandling = opponentObj.GetComponent<InputHandling>();
        shipButtons = fleetMenuScript.GetShipButtons();
        opponentInput = opponentScript.GetComponent<PlayerInput>();
        playerInput.SwitchCurrentActionMap("GameStart");
        fleet = playerWorld.dimensions.GetFleet();
        opponentFleet = opponentScript.dimensions.GetFleet();
        ArrayList devices = new();

        foreach (var device in InputSystem.devices)
        {
            if (device.ToString().Contains("Gamepad"))
            {
                devices.Add(device);
            }
        }

        if (devices.Count >= 2)
        {
            playerInput.user.UnpairDevices();

            if (name == "Player1")
            {
                InputUser.PerformPairingWithDevice((InputDevice)devices[0], playerInput.user);
            }
            else
            {
                InputUser.PerformPairingWithDevice((InputDevice)devices[1], playerInput.user);
            }
        }
        else
        {
            Debug.Log("Gamepad missing!");
        }
    }

    //StartGame actionMap
    public void OnShipLeft(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            int shipNr = playerWorld.playerData.currentShipButton.ShipButtonNr;

            if (shipNr > 0)
            {
                eventSystem.SetSelectedGameObject(shipButtons[shipNr - 1]);
                playerWorld.playerData.currentShipButton = eventSystem.currentSelectedGameObject.GetComponent<ShipButton>();
                fleet.ActivateShip(playerWorld.playerData.currentShipButton.ShipButtonNr, this.gameObject);

                if (OverworldData.GamePhase == GamePhases.Battle)
                {
                    UpdateActiveCellAndFleetMenu();
                }
            }
        }
    }

    public void OnShipRight(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            int shipNr = playerWorld.playerData.currentShipButton.ShipButtonNr;

            if (shipNr < OverworldData.FleetSize)
            {
                eventSystem.SetSelectedGameObject(shipButtons[shipNr + 1]);
                playerWorld.playerData.currentShipButton = eventSystem.currentSelectedGameObject.GetComponent<ShipButton>();
                fleet.ActivateShip(playerWorld.playerData.currentShipButton.ShipButtonNr, this.gameObject);

                if (OverworldData.GamePhase == GamePhases.Battle)
                {
                    UpdateActiveCellAndFleetMenu();
                }
            }
        }
    }

    public void UpdateActiveCellAndFleetMenu()
    {
        int shipX = playerWorld.playerData.ActiveShip.X;
        int shipY = playerWorld.playerData.ActiveShip.Z;

        playerWorld.SetNewCellAbsolute(shipX, shipY);
        fleetMenuScript.UpdateFleetMenuCoords(shipX, shipY);
        opponentFleetMenuScript.UpdateFleetMenuCoords(shipX, shipY);
    }

    public void OnMoveShip(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Vector2 vector = ctx.ReadValue<Vector2>();
            float x = vector.x;
            float y = vector.y;

            //Get axis
            if (Math.Abs(x) > Math.Abs(y))
            {
                if (x > 0)
                {
                    playerWorld.playerData.ActiveShip.Move(1, 0);
                }
                else
                {
                    playerWorld.playerData.ActiveShip.Move(-1, 0);
                }
            }
            else
            {
                if (y > 0)
                {
                    playerWorld.playerData.ActiveShip.Move(0, 1);
                }
                else
                {
                    playerWorld.playerData.ActiveShip.Move(0, -1);
                }
            }

            fleetMenuScript.UpdateFleetMenuCoords(playerWorld.playerData.ActiveShip.X, playerWorld.playerData.ActiveShip.Z);
            opponentFleetMenuScript.UpdateFleetMenuCoords(playerWorld.playerData.ActiveShip.X, playerWorld.playerData.ActiveShip.Z);
        }
    }

    public void OnTurnLeft(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            //Turn ship left
            playerWorld.playerData.ActiveShip.GetComponent<Transform>().Rotate(0, -90, 0);
        }
    }

    public void OnTurnRight(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            //Turn ship right
            playerWorld.playerData.ActiveShip.GetComponent<Transform>().Rotate(0, 90, 0);
        }
    }

    public void OnSubmitFleet(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (name == "Player1")
            {
                OverworldData.Player1SubmittedFleet = true;
            }
            else
            {
                OverworldData.Player2SubmittedFleet = true;
            }

            if (!OverworldData.Player1SubmittedFleet || !OverworldData.Player2SubmittedFleet)
            {
                print("Please, wait until your opponent is ready.");
                playerInput.enabled = false;
                StartCoroutine(WaitForOpponent());
            }
        }
    }

    private IEnumerator WaitForOpponent()
    {
        yield return new WaitUntil(() => (OverworldData.Player1SubmittedFleet && OverworldData.Player2SubmittedFleet));

        print("Your opponent is ready. Let's go!");
        print("Choose your attacking ship!");

        OverworldData.GamePhase = GamePhases.Battle;
        playerInput.enabled = true;

        if (name == "Player2")
        {
            SwapPlayers();
        }
        else
        {
            opponentObj.GetComponent<InputHandling>().SwapPlayers();
        }
    }

    //StartGame and Player actionMap
    public void OnReturnToFleetMenu(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            playerWorld.playerData.ActiveShip.Deactivate(playerWorld);
            playerInput.SwitchCurrentActionMap("FleetMenu");

            if (name == "Player1")
            {
                GameObject.Find("FleetMenu1").GetComponent<FleetMenuScript>().SetFirstSelecetedButton();
            }
            else
            {
                GameObject.Find("FleetMenu2").GetComponent<FleetMenuScript>().SetFirstSelecetedButton();
            }
        }
    }

    //Player actionMap
    public void OnMoveSelection(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (name == "Player1" && OverworldData.PlayerTurn == 1 || name == "Player2" && OverworldData.PlayerTurn == 2)
            {
                //Change activeted cell
                Vector2 vector = ctx.ReadValue<Vector2>();
                float x = vector.x;
                float y = vector.y;

                //Get right axis
                if (Math.Abs(x) > Math.Abs(y))
                {
                    //negative or positive?
                    if (x > 0)
                    {
                        playerWorld.SetNewCellRelative(1, 0);
                    }
                    else
                    {
                        playerWorld.SetNewCellRelative(-1, 0);
                    }
                }
                else
                {
                    if (y > 0)
                    {
                        playerWorld.SetNewCellRelative(0, 1);
                    }
                    else
                    {
                        playerWorld.SetNewCellRelative(0, -1);
                    }
                }

                fleetMenuScript.UpdateFleetMenuCoords(playerWorld.playerData.ActiveCell.X, playerWorld.playerData.ActiveCell.Y);
                opponentFleetMenuScript.UpdateFleetMenuCoords(playerWorld.playerData.ActiveCell.X, playerWorld.playerData.ActiveCell.Y);
            }
            else
            {
                print("It's not your turn!");
            }
        }
    }

    public void OnFire(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (name == "Player1" && OverworldData.PlayerTurn == 1 || name == "Player2" && OverworldData.PlayerTurn == 2)
            {
                playerWorld.playerData.ActiveShip.Fire(playerWorld);
                StartCoroutine(PauseAndTakeTurns());
            }
            else
            {
                print("It's not your turn, yet!");
            }
        }
    }

    public IEnumerator PauseAndTakeTurns()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 1f;

        SwapPlayers();
    }

    private void SwapPlayers()
    {
        OverworldData.PlayerTurn = 3 - OverworldData.PlayerTurn;
        playerCameraBehavior.UpdateCamera(GamePhases.Attacked);
        opponentCameraBehavior.UpdateCamera(GamePhases.Armed);
        playerInput.SwitchCurrentActionMap("FleetMenu");
        opponentInput.SwitchCurrentActionMap("Player");
        DisarmPlayer();
        ArmOpponent();
    }

    private void DisarmPlayer()
    {
        if (playerWorld.playerData.ActiveShip != null)
        {
            playerWorld.playerData.ActiveShip.Deactivate(playerWorld);
        }

        eventSystem.SetSelectedGameObject(null);
    }

    private void ArmOpponent()
    {
        opponentFleetMenuScript.SetFirstSelecetedButton();
        opponentFleet.ActivateShip(0, opponentObj);
        opponentInputHandling.UpdateActiveCellAndFleetMenu();
    }

    public void SwitchActionMap(string actionMapName)
    {
        switch (actionMapName)
        {
            case "GameStart":
                if (!gameStartMap.enabled)
                {
                    gameStartMap.Enable();
                }
                if (playerMap.enabled)
                {
                    playerMap.Disable();
                }
                if (fleetMenuMap.enabled)
                {
                    fleetMenuMap.Disable();
                }
                break;
            case "Player":
                if (gameStartMap.enabled)
                {
                    gameStartMap.Disable();
                }
                if (!playerMap.enabled)
                {
                    playerMap.Enable();
                }
                if (fleetMenuMap.enabled)
                {
                    fleetMenuMap.Disable();
                }
                break;
            case "FleetMenu":
                if (gameStartMap.enabled)
                {
                    gameStartMap.Disable();
                }
                if (playerMap.enabled)
                {
                    playerMap.Disable();
                }
                if (!fleetMenuMap.enabled)
                {
                    fleetMenuMap.Enable();
                }
                break;
            default:
                Debug.Log("No such action map!");
                break;
        }
    }
}
