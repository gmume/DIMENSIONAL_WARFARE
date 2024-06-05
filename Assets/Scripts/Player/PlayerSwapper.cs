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
        player.opponent.Fade.StartEffect();

        OverworldData.PlayerTurn = 3 - OverworldData.PlayerTurn;

        StartCoroutine(UpdateAndContinue());
    }

    private IEnumerator UpdateAndContinue()
    {
        updated = false;

        yield return new WaitUntil(() => player.Fade.finished);
        updated = UpdateGame();
        player.input.SwitchCurrentActionMap("Battle");
        opponent.input.SwitchCurrentActionMap("Battle");
        player.input.actions.FindAction("ChooseRightShip").Disable();
        player.input.actions.FindAction("ChooseLeftShip").Disable();
        DisarmPlayer();
        ArmOpponent();

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitUntil(() => updated);
        player.Fade.StartEffect();
        player.opponent.Fade.StartEffect();
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
