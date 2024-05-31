using System.Collections;
using UnityEngine;

public class Submitter : MonoBehaviour
{
    public PlayerData player;
    public bool[] shipsPlaced = new bool[] { false, false, false };

    private void Start() => player = GetComponent<PlayerData>();

    public void SubmitShipAtStart(PlayerData player)
    {
        player.LastActiveShip = player.ActiveShip;
        shipsPlaced[player.ActiveShip.No] = true;
        int indexOfUnplacedShip = FindIndexOfUnplacedShip();

        if(indexOfUnplacedShip == -1)
        {
            player.HUD.Instruct("Ready");
            player.ActiveShip.Deactivate();
            player.input.SwitchCurrentActionMap("SubmitFleet");
            player.onboarding.ShowTip("SubmitFleet");
        } 
        else
        {
            player.inputHandler.ChooseShip(indexOfUnplacedShip);
        }
    }

    private int FindIndexOfUnplacedShip()
    {
        int indexShipToPlace;

        for (int i = 0; i < 2; i++)
        {
            indexShipToPlace = (player.ActiveShip.No + i) % 3;
            if (!shipsPlaced[indexShipToPlace]) return indexShipToPlace;
        }

        return -1;
    }

    public void Return()
    {
        player.HUD.Instruct("PlaceShips");
        player.LastActiveShip.Activate();
        player.input.SwitchCurrentActionMap("PlaceShips");
        player.onboarding.ShowTip("PlaceShips");

        for (int i = 0; i < player.submitter.shipsPlaced.Length; i++)
        {
            player.submitter.shipsPlaced[i] = false;
        }
    }

    public bool SubmitFleet(PlayerData player)
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
            player.opponent.HUD.WriteText($"Your opponent is ready, Capt'n.");
            player.input.enabled = false;
            StartCoroutine(WaitForOpponent());
        }

        return false;
    }

    private IEnumerator WaitForOpponent()
    {
        yield return new WaitUntil(() => (OverworldData.Player1SubmittedFleet && OverworldData.Player2SubmittedFleet));

        if(name == "Player1")
        {
            player.HUD.WriteText("Your opponent is ready, Capt'n.\nLet's go! Choose your attacking ship!");
            player.opponent.HUD.WriteText("Caution, we're under attack!");
        }
        else
        {
            player.opponent.HUD.WriteText("Let's go! Choose your attacking ship!");
            player.HUD.WriteText("Your opponent is ready, Capt'n.\nCaution, we're under attack!");
        }

        OverworldData.PlayerTurn = 2;
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

    public bool SubmitShip(PlayerData player)
    {
        player.LastActiveShip = player.ActiveShip;
        player.input.SwitchCurrentActionMap("Battle");
        return true;
    }
}