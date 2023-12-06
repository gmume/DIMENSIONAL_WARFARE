using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using static UnityEngine.InputSystem.InputAction;

public class InputHandling : MonoBehaviour
{
    private PlayerData player, opponent;
    private InputActionMap gameStartMap, playerMap;
    private List<GameObject> shipButtons;

    public bool continueGame = true;

    //StartGame actionMap
    public void OnShipLeft(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            int shipNr = player.CurrentShipButton.ShipButtonNr;
            int index = shipButtons.IndexOf(shipButtons[shipNr]);

            if (index > 0)
            {
                player.eventSystem.SetSelectedGameObject(shipButtons[index - 1]);

                player.CurrentShipButton = player.eventSystem.currentSelectedGameObject.GetComponent<ShipButton>();
                player.fleet.ActivateShip(player.CurrentShipButton.ShipButtonNr, player);

                if (OverworldData.GamePhase == GamePhases.Battle)
                {
                    UpdateActiveCellAndHUD();
                }
            }
        }
    }

    public void OnShipRight(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            int shipNr = player.CurrentShipButton.ShipButtonNr;
            int index = shipButtons.IndexOf(shipButtons[shipNr]);

            if (index < OverworldData.FleetSize)
            {
                //player.eventSystem.SetSelectedGameObject(shipButtons[shipNr + 1]);
                player.eventSystem.SetSelectedGameObject(shipButtons[index + 1]);
                player.CurrentShipButton = player.eventSystem.currentSelectedGameObject.GetComponent<ShipButton>();
                player.fleet.ActivateShip(player.CurrentShipButton.ShipButtonNr, player);

                if (OverworldData.GamePhase == GamePhases.Battle)
                {
                    UpdateActiveCellAndHUD();
                }
            }
        }
    }

    public void UpdateActiveCellAndHUD()
    {
        int shipX = player.ActiveShip.PivotX;
        int shipY = player.ActiveShip.PivotZ;

        player.world.SetNewCellAbsolute(shipX, shipY);
        player.HUD.UpdateHUDCoords(shipX, shipY);
        opponent.HUD.UpdateHUDCoords(shipX, shipY);
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

            player.HUD.UpdateHUDCoords(ship.PivotX, ship.PivotZ);
            opponent.HUD.UpdateHUDCoords(ship.PivotX, ship.PivotZ);
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
                    player.HUD.WriteText("Capt'n " + player.number + " please, wait until your opponent is ready.");
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

        player.HUD.WriteText("Your opponent is ready, Capt'n. Let's go!");
        print("Your opponent is ready. Let's go!");
        player.HUD.WriteText("Choose your attacking ship!");
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

                player.HUD.UpdateHUDCoords(player.ActiveCell.X, player.ActiveCell.Y);
                opponent.HUD.UpdateHUDCoords(player.ActiveCell.X, player.ActiveCell.Y);
            }
            else
            {
                player.HUD.WriteText("It's not our turn, yet, Capt'n!");
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
                bool shipUp = player.ActiveShip.Fire();

                if (shipUp)
                {
                    player.playerCamera.GetComponent<LayerFilter>().ShowLayers(true, true);

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
                player.HUD.WriteText("It's not our turn, yet, Capt'n!");
                print(player.name + "It's not our turn, yet, Capt'n!");
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

        UpdateGame();
        player.input.SwitchCurrentActionMap("Player");
        opponent.input.SwitchCurrentActionMap("Player");
        player.input.actions.FindAction("ShipRight").Disable();
        player.input.actions.FindAction("ShipLeft").Disable();
        DisarmPlayer();
        ArmOpponent();
    }

    private void UpdateGame()
    {
        if (name == "Player1")
        {
            player.playerCamera.cullingMask = LayerMask.GetMask("Default", "Water", "Player1", "HUD1", "VisibleHUDShips2", "VisibleShips2", "Fleet1");
            opponent.playerCamera.cullingMask = LayerMask.GetMask("Default", "Water", "Player2", "HUD2", "VisibleHUDShips1", "VisibleShips1");
        }
        else
        {
            player.playerCamera.cullingMask = LayerMask.GetMask("Default", "Water", "Player2", "HUD2", "VisibleHUDShips1", "Fleet2", "VisibleShips1", "Fleet2");
            opponent.playerCamera.cullingMask = LayerMask.GetMask("Default", "Water", "Player1", "HUD1", "VisibleHUDShips2", "VisibleShips2");
        }

        player.HUD.armed.SetActive(false);
        opponent.HUD.armed.SetActive(true);
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
        opponent.HUD.SetSelecetedButton();
        opponent.fleet.ActivateShip(0, opponent);
        opponent.inputHandling.UpdateActiveCellAndHUD();
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
                break;
            default:
                Debug.LogWarning(name + ": No such action map!");
                break;
        }
    }

    public void InitImputHandling()
    {
        player = GetComponent<PlayerData>();
        opponent = player.opponent;
        gameStartMap = player.input.actions.FindActionMap("GameStart");
        playerMap = player.input.actions.FindActionMap("Player");
        shipButtons = player.HUD.GetShipButtons();
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
            Debug.LogWarning("Gamepad missing!");
        }
    }
}
