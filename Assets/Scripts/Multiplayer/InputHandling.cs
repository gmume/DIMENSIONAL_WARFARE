using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.Users;
using UnityEngine.Windows;
using static UnityEngine.InputSystem.InputAction;

public class InputHandling : MonoBehaviour
{
    private Player player;
    private Player opponent;
    private InputActionMap gameStartMap, playerMap, fleetMenuMap;
    private GameObject[] shipButtons;

    public bool continueGame = true;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Start()
    {
        opponent = player.opponent;

        gameStartMap = player.input.actions.FindActionMap("GameStart");
        playerMap = player.input.actions.FindActionMap("Player");
        fleetMenuMap = player.input.actions.FindActionMap("FleetMenu");

        shipButtons = player.fleetMenu.GetShipButtons();
        player.input.SwitchCurrentActionMap("GameStart");
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
            player.input.user.UnpairDevices();

            if (name == "Player1")
            {
                InputUser.PerformPairingWithDevice((InputDevice)devices[0], player.input.user);
            }
            else
            {
                InputUser.PerformPairingWithDevice((InputDevice)devices[1], player.input.user);
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
            int shipNr = player.CurrentShipButton.ShipButtonNr;

            if (shipNr > 0)
            {
                player.eventSystem.SetSelectedGameObject(shipButtons[shipNr - 1]);
                player.CurrentShipButton = player.eventSystem.currentSelectedGameObject.GetComponent<ShipButton>();
                player.fleet.ActivateShip(player.CurrentShipButton.ShipButtonNr, player);

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
            int shipNr = player.CurrentShipButton.ShipButtonNr;

            if (shipNr < OverworldData.FleetSize)
            {
                player.eventSystem.SetSelectedGameObject(shipButtons[shipNr + 1]);
                player.CurrentShipButton = player.eventSystem.currentSelectedGameObject.GetComponent<ShipButton>();
                player.fleet.ActivateShip(player.CurrentShipButton.ShipButtonNr, player);

                if (OverworldData.GamePhase == GamePhases.Battle)
                {
                    UpdateActiveCellAndFleetMenu();
                }
            }
        }
    }

    public void UpdateActiveCellAndFleetMenu()
    {
        int shipX = player.ActiveShip.PivotX;
        int shipY = player.ActiveShip.PivotZ;

        player.world.SetNewCellAbsolute(shipX, shipY);
        player.fleetMenu.UpdateFleetMenuCoords(shipX, shipY);
        opponent.fleetMenu.UpdateFleetMenuCoords(shipX, shipY);
    }

    public void OnMoveShip(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Vector2 vector = ctx.ReadValue<Vector2>();
            float x = vector.x;
            float y = vector.y;

            Ship ship = player.ActiveShip;

            //Get axis
            if (Math.Abs(x) > Math.Abs(y))
            {
                if (x > 0)
                {
                    ship.Move(1, 0);
                }
                else
                {
                    ship.Move(-1, 0);
                }
            }
            else
            {
                if (y > 0)
                {
                    ship.Move(0, 1);
                }
                else
                {
                    ship.Move(0, -1);
                }
            }

            player.fleetMenu.UpdateFleetMenuCoords(ship.PivotX, ship.PivotZ);
            opponent.fleetMenu.UpdateFleetMenuCoords(ship.PivotX, ship.PivotZ);
        }
    }

    public void OnTurnLeft(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            player.ActiveShip.QuaterTurn(false);
        }
    }

    public void OnTurnRight(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            player.ActiveShip.QuaterTurn(true);
        }
    }

    public void OnSubmitFleet(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (OverworldData.GamePhase == GamePhases.Start)
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
                    player.input.enabled = false;
                    StartCoroutine(WaitForOpponent());
                }
            }
            else
            {
                player.input.SwitchCurrentActionMap("Player");
                continueGame = true;
            }
        }
    }

    private IEnumerator WaitForOpponent()
    {
        yield return new WaitUntil(() => (OverworldData.Player1SubmittedFleet && OverworldData.Player2SubmittedFleet));

        print("Your opponent is ready. Let's go!");
        print("Choose your attacking ship!");

        OverworldData.GamePhase = GamePhases.Battle;
        player.input.enabled = true;

        if (name == "Player2")
        {
            SwapPlayers();
        }
        else
        {
            opponent.inputHandling.SwapPlayers();
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
                        player.world.SetNewCellRelative(1, 0);
                    }
                    else
                    {
                        player.world.SetNewCellRelative(-1, 0);
                    }
                }
                else
                {
                    if (y > 0)
                    {
                        player.world.SetNewCellRelative(0, 1);
                    }
                    else
                    {
                        player.world.SetNewCellRelative(0, -1);
                    }
                }

                player.fleetMenu.UpdateFleetMenuCoords(player.ActiveCell.X, player.ActiveCell.Y);
                opponent.fleetMenu.UpdateFleetMenuCoords(player.ActiveCell.X, player.ActiveCell.Y);
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
                if(player.ActiveShip.ShipStatus == ShipStatus.Intact)
                {
                    bool shipUp = player.ActiveShip.Fire();

                    if (shipUp)
                    {
                        continueGame = false;
                        StartCoroutine(WaitBattleToContinue());
                    }
                    else
                    {
                        StartCoroutine(PauseAndTakeTurns());
                    }
                }
                else
                {
                    print("You can't fire with a sunken ship, capt'n!");
                }
            }
            else
            {
                print("It's not your turn, yet!");
            }
        }
    }

    private IEnumerator WaitBattleToContinue()
    {
        yield return new WaitUntil(() => (continueGame));

        StartCoroutine(PauseAndTakeTurns());
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
        player.cameraBehavior.UpdateCamera(GamePhases.Attacked);
        opponent.cameraBehavior.UpdateCamera(GamePhases.Armed);
        player.input.SwitchCurrentActionMap("FleetMenu");
        opponent.input.SwitchCurrentActionMap("Player");
        player.input.actions.FindAction("ShipRight").Disable();
        player.input.actions.FindAction("ShipLeft").Disable();
        DisarmPlayer();
        ArmOpponent();
    }

    private void DisarmPlayer()
    {
        if (player.ActiveShip != null)
        {
            player.ActiveShip.Deactivate();
        }

        player.eventSystem.SetSelectedGameObject(null);
    }

    private void ArmOpponent()
    {
        opponent.fleetMenu.SetFirstSelecetedButton();
        opponent.fleet.ActivateShip(0, opponent);
        opponent.inputHandling.UpdateActiveCellAndFleetMenu();
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
