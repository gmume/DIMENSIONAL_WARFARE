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
        player.playerCamera.GetComponent<LayerFilter>().ShowLayers(false, true, true);
        opponent.playerCamera.GetComponent<LayerFilter>().ShowLayers(true, false, true);

        player.HUD.armed.SetActive(false);
        opponent.HUD.armed.SetActive(true);

        player.HUD.underAttack.SetActive(true);
        opponent.HUD.underAttack.SetActive(false);
    }

    private void DisarmPlayer()
    {
        player.onboarding.ShowTip("UnderAttack");
        
        if (player.ActiveShip != null) player.ActiveShip.Deactivate();
        player.eventSystem.SetSelectedGameObject(null);
    }

    private void ArmOpponent()
    {
        opponent.onboarding.ShowTip("Attack");
        opponent.inputEnabler.playerMap.Enable();
        opponent.HUD.SetSelecetedButton();
        opponent.fleet.ActivateShip(0, opponent);
        opponent.HUD.UpdateFocusedCellAndHUD();
    }
}
