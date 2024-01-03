using System.Collections;
using UnityEngine;

public class FleetSubmitter : MonoBehaviour
{
    public PlayerData player;

    private void Start()
    {
        player = GetComponent<PlayerData>();
    }

    public bool SubmitFleet(PlayerData player)
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
                player.HUD.WriteText($"Capt'n {player.number}, please wait until your opponent is ready.");
                player.input.enabled = false;
                StartCoroutine(WaitForOpponent());
            }
        }
        else
        {
            player.input.SwitchCurrentActionMap("Player");
            return true;
        }

        return false;
    }

    private IEnumerator WaitForOpponent()
    {
        yield return new WaitUntil(() => (OverworldData.Player1SubmittedFleet && OverworldData.Player2SubmittedFleet));

        player.HUD.WriteText("Your opponent is ready, Capt'n.");

        if(name == "Player1")
        {
            player.HUD.WriteText("Let's go! Choose your attacking ship!");
        }

        OverworldData.GamePhase = GamePhases.Battle;
        player.input.enabled = true;

        if (name == "Player2")
        {
            player.swapper.SwapPlayers();
        }
        else
        {
            player.opponent.swapper.SwapPlayers();
        }
    }
}