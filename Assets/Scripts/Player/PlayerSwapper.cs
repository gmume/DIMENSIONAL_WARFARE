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
        player.input.SwitchCurrentActionMap("Player");
        opponent.input.SwitchCurrentActionMap("Player");
        player.input.actions.FindAction("ChooseRightShip").Disable();
        player.input.actions.FindAction("ChooseLeftShip").Disable();
        DisarmPlayer();
        ArmOpponent();
    }

    private void UpdateGame()
    {
        player.playerCamera.GetComponent<LayerFilter>().ShowLayers(true, true, true, false);
        opponent.playerCamera.GetComponent<LayerFilter>().ShowLayers(false, false, false, true);

        player.HUD.armed.SetActive(false);
        opponent.HUD.armed.SetActive(true);

        player.HUD.underAttack.SetActive(true);
        opponent.HUD.underAttack.SetActive(false);
    }

    private void DisarmPlayer()
    {
        player.onboarding.ShowTip("UnderAttack");

        player.LastActiveShip = player.ActiveShip;

        if (player.input.currentActionMap.name == "Player") player.world.DeactivateCells();
        if (player.ActiveShip != null) player.ActiveShip.Deactivate();
        player.FocusedCell = null;
        player.eventSystem.SetSelectedGameObject(null);
    }

    private void ArmOpponent()
    {
        opponent.onboarding.ShowTip("Attack");
        opponent.inputEnabler.playerMap.Enable();
        opponent.HUD.SetSelecetedButton();

        if(player.opponent.LastActiveShip == null)
        {
            opponent.fleet.ActivateShip(0, opponent);
        } else
        {
            opponent.fleet.ActivateShip(player.opponent.LastActiveShip.No, opponent);
        }
        
        opponent.HUD.UpdateHUDCoords();
        player.opponent.dimensions.ResetCellPositions(player.ActiveDimension.No);
        player.opponent.world.SetNewCellAbsolute(OverworldData.MiddleCoordNo, OverworldData.MiddleCoordNo);
    }
}
