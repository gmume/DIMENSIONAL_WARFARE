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
        if (!ctx.performed) return;

        int shipNr = player.CurrentShipButton.ShipButtonNr;
        int index = shipButtons.IndexOf(shipButtons[shipNr]);

        if (index > 0)
        {
            player.eventSystem.SetSelectedGameObject(shipButtons[index - 1]);

            player.CurrentShipButton = player.eventSystem.currentSelectedGameObject.GetComponent<ShipButton>();
            player.fleet.ActivateShip(player.CurrentShipButton.ShipButtonNr, player);

            if (OverworldData.GamePhase == GamePhases.Battle) UpdateActiveCellAndHUD();
        }
    }

    public void OnShipRight(CallbackContext ctx)
    {
        if (!ctx.performed) return;

        int shipNr = player.CurrentShipButton.ShipButtonNr;
        int index = shipButtons.IndexOf(shipButtons[shipNr]);

        if (index < OverworldData.FleetSize)
        {
            player.eventSystem.SetSelectedGameObject(shipButtons[index + 1]);
            player.CurrentShipButton = player.eventSystem.currentSelectedGameObject.GetComponent<ShipButton>();
            player.fleet.ActivateShip(player.CurrentShipButton.ShipButtonNr, player);

            if (OverworldData.GamePhase == GamePhases.Battle) UpdateActiveCellAndHUD();
        }
    }

    public void UpdateActiveCellAndHUD()
    {
        int shipX = player.ActiveShip.navigator.PivotX;
        int shipY = player.ActiveShip.navigator.PivotZ;

        player.world.SetNewCellAbsolute(shipX, shipY);
        player.HUD.UpdateHUDCoords(shipX, shipY);
        opponent.HUD.UpdateHUDCoords(shipX, shipY);
    }

    public void OnMoveShip(CallbackContext ctx)
    {
        if (!ctx.performed) return;

        Vector2 vector = ctx.ReadValue<Vector2>();
        float x = vector.x;
        float y = vector.y;

        ShipManager ship = player.ActiveShip;

        //Get axis
        if (Math.Abs(x) > Math.Abs(y))
        {
            // Returns the sign (Vorzeichen) of x. The Mathf.Sign function returns -1 for negative values, 1 for positive values, and 0 for zero. 
            ship.Move((int)Mathf.Sign(x), 0);
        }
        else
        {
            ship.Move(0, (int)Mathf.Sign(y));
        }

        player.HUD.UpdateHUDCoords(ship.navigator.PivotX, ship.navigator.PivotZ);
        opponent.HUD.UpdateHUDCoords(ship.navigator.PivotX, ship.navigator.PivotZ);
    }

    public void OnTurnLeft(CallbackContext ctx)
    {
        if (!ctx.performed) return;
        player.ActiveShip.QuaterTurn(false);
    }

    public void OnTurnRight(CallbackContext ctx)
    {
        if (!ctx.performed) return;
        player.ActiveShip.QuaterTurn(true);
    }

    public void OnSubmitFleet(CallbackContext ctx)
    {
        if (!ctx.performed) return;

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
                player.HUD.WriteText($"Capt'n {player.number}, please wait until your opponent is ready.");
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

    private IEnumerator WaitForOpponent()
    {
        yield return new WaitUntil(() => (OverworldData.Player1SubmittedFleet && OverworldData.Player2SubmittedFleet));

        player.HUD.WriteText("Your opponent is ready, Capt'n. Let's go!");
        player.HUD.WriteText("Choose your attacking ship!");

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
        if (!ctx.performed) return;

        if (name == "Player1" && OverworldData.PlayerTurn == 1 || name == "Player2" && OverworldData.PlayerTurn == 2)
        {
            //Change activeted cell
            Vector2 vector = ctx.ReadValue<Vector2>();
            float x = vector.x;
            float y = vector.y;

            // Get right axis and move in correct direction
            float moveX = (Math.Abs(x) > Math.Abs(y)) ? Mathf.Sign(x) : 0;
            float moveY = (Math.Abs(x) > Math.Abs(y)) ? 0 : Mathf.Sign(y);

            player.world.SetNewCellRelative((int)moveX, (int)moveY);

            player.HUD.UpdateHUDCoords(player.ActiveCell.X, player.ActiveCell.Y);
            opponent.HUD.UpdateHUDCoords(player.ActiveCell.X, player.ActiveCell.Y);
        }
        else
        {
            player.HUD.WriteText("It's not our turn, yet, Capt'n!");
            print("It's not your turn!");
        }
    }

    public void OnFire(CallbackContext ctx)
    {
        if (!ctx.performed) return;

        if (name == "Player1" && OverworldData.PlayerTurn == 1 || name == "Player2" && OverworldData.PlayerTurn == 2)
        {
            bool shipUp = player.ActiveShip.Fire();

            if (shipUp)
            {
                player.playerCamera.GetComponent<LayerFilter>().ShowLayers(true, true, true);

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
        player.playerCamera.GetComponent<LayerFilter>().ShowLayers(false, true, true);
        opponent.playerCamera.GetComponent<LayerFilter>().ShowLayers(true, false, true);

        player.HUD.armed.SetActive(false);
        opponent.HUD.armed.SetActive(true);
    }

    private void DisarmPlayer()
    {
        if (player.ActiveShip != null) player.ActiveShip.Deactivate();
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
                gameStartMap.Enable();
                playerMap.Disable();
                break;
            case "Player":
                gameStartMap.Disable();
                playerMap.Enable();
                break;
            default:
                Debug.LogWarning($"{name}: No such action map!");
                break;
        }
    }

    public void Initialize()
    {
        player = GetComponent<PlayerData>();
        opponent = player.opponent;
        gameStartMap = player.input.actions.FindActionMap("GameStart");
        playerMap = player.input.actions.FindActionMap("Player");
        shipButtons = player.HUD.GetShipButtons();
        player.input.SwitchCurrentActionMap("GameStart");
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
