using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifter : MonoBehaviour
{
    [HideInInspector] public PlayerData player;
    [HideInInspector] public AudioPlayer audioPlayer;
    [HideInInspector] public ShipPartManager[] parts;

    public bool LiftShipUp(ref DimensionManager currentDimension, int shipNo, CellOccupier occupier)
    {
        if (IsNotWinner(currentDimension))
        {
            audioPlayer.OnShipUp();
            ShipChangeDimension(currentDimension.DimensionNo + 1, ref currentDimension, shipNo, new Vector3(0, OverworldData.DimensionSize * 2, 0), occupier);
            player.input.SwitchCurrentActionMap("GameStart");
            player.HUD.WriteText($"Capt'n {player.number} hide your ship!");
            player.onboarding.ShowTip("OwnShipUp");
            player.opponent.onboarding.ShowTip("OpponentShipUp");
            return true;
        }
        else
        {
            SceneChanger.LoadResolveGame();
            //player.HUD.WriteText($"Capt'n {player.number} wins for reaching the top dimension!");
            GameData.winner = player.name;
            //audioPlayer.OnVictory();
            return false;
        }
    }

    private bool IsNotWinner(DimensionManager currentDimension) => currentDimension.DimensionNo<OverworldData.DimensionsCount - 2;

    public void SinkShip(ref DimensionManager currentDimension, int shipNo, ShipStatus status, CellOccupier occupier)
    {
        ShipChangeDimension(currentDimension.DimensionNo - 1, ref currentDimension, shipNo, new Vector3(0, OverworldData.DimensionSize * -2, 0), occupier);
        transform.position += new Vector3(0, 0.5f, 0);
        gameObject.layer = LayerMask.NameToLayer($"Fleet{player.number}");
        player.inputHandler.continueGame = false;
        StartCoroutine(ResetShip(status));
    }

    private void ShipChangeDimension(int newDimensionNo, ref DimensionManager dimensionBefore, int shipNo, Vector3 vector, CellOccupier occupier)
    {
        player.HUD.SetHUDDimension(newDimensionNo);
        player.HUD.UpdateHUDFleets(shipNo, newDimensionNo, dimensionBefore.DimensionNo);

        SwitchDimension(newDimensionNo, ref dimensionBefore, occupier, shipNo);
        transform.position += vector;

        player.world.SetNewDimension(newDimensionNo);
        player.vehicle.SetViewOnDimension(newDimensionNo);
    }

    private void SwitchDimension(int newDimensionNo, ref DimensionManager dimension, CellOccupier occupier, int shipNo)
    {
        occupier.ReleaseCells();
        dimension.RemoveShip(gameObject);
        dimension = player.dimensions.GetDimension(newDimensionNo);
        SetDimension(dimension, shipNo);
        occupier.OccupyCells();
    }

    public void SetDimension(DimensionManager newDimension, int shipNo)
    {
        transform.parent = newDimension.transform;

        for (int i = 0; i <= shipNo; i++)
        {
            ShipPartManager part = parts[i];
            part.Dimension = newDimension;
            part.transform.parent = transform;
        }
    }

    private IEnumerator ResetShip(ShipStatus status)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1f;

        foreach (ShipPartManager part in parts)
        {
            part.ResetPart();
        }

        status = ShipStatus.Intact;
        player.input.SwitchCurrentActionMap("GameStart");
        player.HUD.WriteText($"Capt'n {player.number} hide your ship!");
        player.ActiveShip = gameObject.GetComponent<ShipManager>();

        // To do: Wait for player to submit fleet.
    }
}
