using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Submitter : MonoBehaviour
{
    [HideInInspector] public PlayerData player;
    [HideInInspector] public bool[] shipsPlaced = new bool[] { false, false, false };

    private void Start() => player = GetComponent<PlayerData>();

    public void SubmitAtStart(PlayerData player)
    {
        player.LastActiveShip = player.ActiveShip;
        shipsPlaced[player.ActiveShip.No] = true;
        int indexOfUnplacedShip = FindIndexOfUnplacedShip();

        if(indexOfUnplacedShip == -1)
        {
            player.HUD.DeactivateLayoutgroup();
            player.ActiveShip.Deactivate();
            player.input.SwitchCurrentActionMap("SubmitFleet");
            player.HUD.Instruct("Ready");
            player.onboarding.ShowTip("SubmitFleet");
            player.Pointer.enabled = true;
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
        player.HUD.Instruct("Wait");
        player.onboarding.ShowTip("Wait");

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
            StartCoroutine(WaitForOpponent());
        }

        return false;
    }

    private IEnumerator WaitForOpponent()
    {
        yield return new WaitUntil(() => (OverworldData.Player1SubmittedFleet && OverworldData.Player2SubmittedFleet));

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

        if (name == "Player1")
        {
            player.HUD.WriteText("Your opponent is ready, Capt'n.\nLet's go! Choose your attacking ship!");
            player.opponent.HUD.WriteText("Caution, we're under attack!");
        }
        else
        {
            player.opponent.HUD.WriteText("Let's go! Choose your attacking ship!");
            player.HUD.WriteText("Your opponent is ready, Capt'n.\nCaution, we're under attack!");
        }
    }

    public bool SubmitShip(PlayerData player)
    {
        player.LastActiveShip = player.ActiveShip;
        player.input.SwitchCurrentActionMap("Battle");
        return true;
    }
}