using System.Collections;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class PlayerSwapper : MonoBehaviour
{
    [HideInInspector] public PlayerData player;
    [HideInInspector] public PlayerData opponent;
    private bool updated;

    private void Start()
    {
        player = GetComponent<PlayerData>();
        opponent = player.opponent;
    }

    public void SwapPlayers()
    {
        player.Fade.StartEffect();
        opponent.Fade.StartEffect();
        OverworldData.PlayerTurn = 3 - OverworldData.PlayerTurn;
        StartCoroutine(UpdateAndContinue());
    }

    private IEnumerator UpdateAndContinue()
    {
        updated = false;

        yield return new WaitUntil(() => player.Fade.finished && opponent.Fade.finished);
        updated = UpdateGame();
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitUntil(() => updated);
        player.Fade.StartEffect();
        opponent.Fade.StartEffect();
        StartCoroutine (TakeTurn());
    }

    private IEnumerator TakeTurn()
    {
        yield return new WaitUntil(() => player.Fade.finished && opponent.Fade.finished);
        DisarmPlayer();
        ArmOpponent();
    }

    private bool UpdateGame()
    {
        player.playerCamera.GetComponent<LayerFilter>().ShowLayers(true, true, true, false);
        opponent.playerCamera.GetComponent<LayerFilter>().ShowLayers(false, false, false, true);
        player.HUD.Instruct("UnderAttack");
        opponent.HUD.Instruct("Attack");
        return true;
    }

    private void DisarmPlayer()
    {
        player.onboarding.ShowTip("UnderAttack");
        player.LastActiveShip = player.ActiveShip;
        if (player.ActiveShip != null) player.ActiveShip.Deactivate();
        player.FocusedCell = null;
        player.eventSystem.SetSelectedGameObject(null);
        player.input.SwitchCurrentActionMap("Battle");
        player.input.currentActionMap.FindAction("chooseLeftShip").Disable();
        player.input.currentActionMap.FindAction("chooseRightShip").Disable();
    }

    private void ArmOpponent()
    {
        opponent.onboarding.ShowTip("Attack");
        opponent.inputEnabler.battleMap.Enable();
        opponent.HUD.SetSelecetedButton();

        if (player.opponent.LastActiveShip == null)
        {
            opponent.fleet.ActivateShip(0);
        }
        else
        {
            opponent.fleet.ActivateShip(opponent.fleet.GetShipIndex(opponent.LastActiveShip.No));
        }
        
        opponent.HUD.UpdateHUDCoords();
        player.opponent.world.SetNewCellAbsolute(false, OverworldData.MiddleCoordNo, OverworldData.MiddleCoordNo);
        player.opponent.Pointer.Activate();
        opponent.input.SwitchCurrentActionMap("Battle");
    }
}
