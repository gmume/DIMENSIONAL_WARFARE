using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class InputHandler : MonoBehaviour
{
    [HideInInspector] public PlayerData player;
    [HideInInspector] public bool continueGame = true;
    public GameObject onboarding;

    //StartGame actionMap
    public void OnSubmitFleet(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            onboarding.SetActive(false);
            continueGame = player.submitter.SubmitFleet(player);
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

    //StartGame and Player actionMap
    public void OnChooseRightShip(CallbackContext ctx)
    {
        if (ctx.performed && player.ActiveShip.ShipNo < OverworldData.FleetSize - 1) ChooseShip(player.ActiveShip.ShipNo + 1);
    }

    public void OnChooseLeftShip(CallbackContext ctx)
    {
        if (ctx.performed && player.ActiveShip.ShipNo > 0) ChooseShip(player.ActiveShip.ShipNo - 1);
    }

    private void ChooseShip(int index)
    {
        player.HUD.ChooseShip(index);
        player.audioManager.ChooseShip();
    }

    //Player actionMap
    public void OnDimensionUp(CallbackContext ctx)
    {
        if (ctx.performed && player.ActiveDimension.DimensionNo < OverworldData.DimensionsCount - 1) GoToDimension(player.ActiveDimension.DimensionNo + 1);
    }

    public void OnDimensionDown(CallbackContext ctx)
    {
        if (ctx.performed && player.ActiveDimension.DimensionNo > 0) GoToDimension(player.ActiveDimension.DimensionNo - 1);
    }

    private void GoToDimension(int no)
    {
        player.audioManager.ChooseDimension();
        player.HUD.ChooseDimension(no);
        player.vehicle.SetViewOnDimension(no);
        player.world.SetNewDimension(no);
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
            player.HUD.UpdateHUDCoords(player.ActiveCell.X, player.ActiveCell.Y);
            player.opponent.HUD.UpdateHUDCoords(player.ActiveCell.X, player.ActiveCell.Y);
        }
        else
        {
            player.HUD.WriteText("It's not our turn, yet, Capt'n!");
        }
    }

    public void OnFire(CallbackContext ctx)
    {
        if (!ctx.performed) return;

        if (!(name == "Player1" && OverworldData.PlayerTurn == 1 || name == "Player2" && OverworldData.PlayerTurn == 2))
        {
            player.HUD.WriteText("It's not our turn, yet, Capt'n!");
            return;
        }

        player.inputEnabler.playerMap.Disable();
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
}
