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
        opponent.HUD.UpdateActiveCellAndHUD();
    }
}