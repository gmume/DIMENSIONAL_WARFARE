using System.ComponentModel;
using TMPro;
using UnityEngine;

public class PlayerSwapper : MonoBehaviour
{
    public PlayerData player;
    public PlayerData opponent;

    private void Start()
    {
        player = GetComponent<PlayerData>();
        opponent = player.opponent;
    }

    public void SwapPlayers()
    {
        OverworldData.PlayerTurn = 3 - OverworldData.PlayerTurn;

        UpdateGame();
        player.input.SwitchCurrentActionMap("Battle");
        opponent.input.SwitchCurrentActionMap("Battle");
        player.input.actions.FindAction("ChooseRightShip").Disable();
        player.input.actions.FindAction("ChooseLeftShip").Disable();
        DisarmPlayer();
        ArmOpponent();
    }

    private void UpdateGame()
    {
        player.playerCamera.GetComponent<LayerFilter>().ShowLayers(true, true, true, false);
        opponent.playerCamera.GetComponent<LayerFilter>().ShowLayers(false, false, false, true);
        player.HUD.Instruct("UnderAttack");
        opponent.HUD.Instruct("Attack");
    }

    private void DisarmPlayer()
    {
        player.onboarding.ShowTip("UnderAttack");
        player.LastActiveShip = player.ActiveShip;
        //if (player.input.currentActionMap.name == "Battle") player.world.DeactivateCells();
        if (player.ActiveShip != null) player.ActiveShip.Deactivate();
        player.FocusedCell = null;
        player.eventSystem.SetSelectedGameObject(null);
        player.Pointer.Deactivate();
    }

    private void ArmOpponent()
    {
        opponent.onboarding.ShowTip("Attack");
        opponent.inputEnabler.battleMap.Enable();
        opponent.HUD.SetSelecetedButton();

        if(player.opponent.LastActiveShip == null)
        {
            opponent.fleet.ActivateShip(0, opponent);
        }
        else
        {
            opponent.fleet.ActivateShip(player.opponent.fleet.GetShipIndex(player.opponent.LastActiveShip.No), opponent);
        }
        
        opponent.HUD.UpdateHUDCoords();
        player.opponent.world.SetNewCellAbsolute(false, OverworldData.MiddleCoordNo, OverworldData.MiddleCoordNo);
        player.opponent.Pointer.Activate();
    }
}
