using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using static UnityEngine.InputSystem.InputAction;

public class InputHandler : MonoBehaviour
{
    [HideInInspector] public PlayerData player;
    [HideInInspector] public bool continueGame = true;

    //PlaceShips actionMap
    public void OnSubmit(CallbackContext ctx)
    {
        if (ctx.started)
        {
            // To do: if(OverworldData.GamePhase.ToString() == "Start")Start timer animation
        }

        if (ctx.performed)
        {
            // player.onboarding.DeactivateTip(); Deactivated for Werkschau!

            if (OverworldData.GamePhase.ToString() == "Start")
            {
                player.submitter.SubmitAtStart(player);
            }
            else
            {
                continueGame = player.submitter.SubmitShip(player);
            }
        }
    }

    public void OnMoveShip(CallbackContext ctx)
    {
        if (!ctx.performed) return;

        Vector2 vector = ctx.ReadValue<Vector2>();
        ShipManager ship = player.ActiveShip;

        ship.Move(vector);
        player.HUD.UpdateHUDCoords(ship.navigator.PivotX, ship.navigator.PivotZ);
        player.opponent.HUD.UpdateHUDCoords(ship.navigator.PivotX, ship.navigator.PivotZ);
    }

    public void OnTurnRight(CallbackContext ctx)
    {
        if (ctx.performed) player.ActiveShip.QuaterTurn(true);
    }

    public void OnTurnLeft(CallbackContext ctx)
    {
        if (ctx.performed) player.ActiveShip.QuaterTurn(false);
    }

    //SubmitFleet actionMap
    public void OnSubmitFleet(CallbackContext ctx)
    {
        if (ctx.started)
        {
            //if(OverworldData.GamePhase.ToString() == "Start")Start timer animation
        }

        if (ctx.performed) player.submitter.SubmitFleet(player);
    }

    public void OnReturn(CallbackContext ctx)
    {
        if (ctx.performed) player.submitter.Return();
    }

    //PlaceShips and Battle actionMap
    public void OnChooseRightShip(CallbackContext ctx)
    {
        if (!ctx.performed) return;
        int activeShipIndex = player.fleet.GetShipIndex(player.ActiveShip.No);
        if (activeShipIndex < player.fleet.ships.Count - 1 && activeShipIndex != -1) ChooseShip(activeShipIndex + 1);
    }

    public void OnChooseLeftShip(CallbackContext ctx)
    {
        if (!ctx.performed) return;
        int activeShipIndex = player.fleet.GetShipIndex(player.ActiveShip.No);
        if (activeShipIndex > 0) ChooseShip(activeShipIndex - 1);
    }

    public void ChooseShip(int index)
    {
        player.world.DeactivateCells();
        player.fleet.ActivateShip(index, player);
        player.HUD.ChooseShip(index);
        player.audioManager.ChooseShip();
        player.world.ActivateCells();

        if (OverworldData.PlayerTurn == player.number) player.Pointer.ShipPointAtFocussedCell();
    }

    //Battle actionMap
    public void OnDimensionUp(CallbackContext ctx)
    {
        if (ctx.performed && player.ActiveDimension.No < OverworldData.DimensionsCount - 1) GoToDimension(player.ActiveDimension.No + 1);
    }

    public void OnDimensionDown(CallbackContext ctx)
    {
        if (ctx.performed && player.ActiveDimension.No > 0) GoToDimension(player.ActiveDimension.No - 1);
    }

    private void GoToDimension(int no)
    {
        player.audioManager.ChooseDimension();
        player.HUD.ChooseDimension(no);
        player.vehicle.SetViewOnDimension(no);
        player.world.SetNewCellRelative(no, 0, 0);

        if (OverworldData.PlayerTurn == player.number) Invoke("RedirectPointer", player.vehicle.panDuration + 0.1f);
    }

    public void OnMoveSelection(CallbackContext ctx)
    {
        if (!ctx.performed) return;

        if (name == "Player1" && OverworldData.PlayerTurn == 1 || name == "Player2" && OverworldData.PlayerTurn == 2)
        {
            Vector2 vector = ctx.ReadValue<Vector2>();
            float x = vector.x;
            float y = vector.y;

            player.world.MoveSelection(x, y);
            player.HUD.UpdateHUDCoords(player.FocusedCell.X, player.FocusedCell.Y);
            player.opponent.HUD.UpdateHUDCoords(player.FocusedCell.X, player.FocusedCell.Y);
            player.Pointer.ShipPointAtFocussedCell();

            Invoke("RedirectPointer", 0.1f);
        }
        else
        {
            player.HUD.WriteText("It's not our turn, yet, Capt'n!");
        }
    }

    private void RedirectPointer() => player.Pointer.ShipPointAtFocussedCell();

    public void OnFire(CallbackContext ctx)
    {
        if (!ctx.performed) return;

        if (!(name == "Player1" && OverworldData.PlayerTurn == 1 || name == "Player2" && OverworldData.PlayerTurn == 2))
        {
            player.HUD.WriteText("It's not our turn, yet, Capt'n!");
            return;
        }

        player.inputEnabler.battleMap.Disable();
        bool shipUp = player.ActiveShip.Fire();

        if (shipUp)
        {
            bool showVisibleParts1 = player.name == "player1";
            bool showVisibleParts2 = player.name == "player2";

            player.playerCamera.GetComponent<LayerFilter>().ShowLayers(true, true, showVisibleParts1, showVisibleParts2);
            continueGame = false;
            StartCoroutine(WaitBattleToContinue());
        }
        else
        {
            StartCoroutine(PauseAndTakeTurns());
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
        player.swapper.SwapPlayers();
    }

    public void OnOptions(CallbackContext ctx)
    {
        if (ctx.performed) player.options.ShowOptions();
    }

    //Options actionMap
    public void OnRestartGame(CallbackContext ctx)
    {
        if (ctx.performed) SceneChanger.LoadLobby();
    }

    public void OnCloseOptions(CallbackContext ctx)
    {
        if (ctx.performed) player.options.CloseOptions();
    }
}
