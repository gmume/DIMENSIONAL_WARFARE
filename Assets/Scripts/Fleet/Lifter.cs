using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifter : MonoBehaviour
{
    [HideInInspector] public PlayerData player;
    [HideInInspector] public AudioPlayer audioPlayer;
    [HideInInspector] public ShipPartManager[] parts;

    public void LiftShipUp(ref DimensionManager currentDimension, int shipNo, CellOccupier occupier)
    {
        if (currentDimension.DimensionNo < OverworldData.DimensionsCount - 2)
        {
            audioPlayer.OnShipUp();
            ShipChangeDimension(currentDimension.DimensionNo + 1, ref currentDimension, shipNo, occupier);
            player.input.SwitchCurrentActionMap("GameStart");
            player.HUD.WriteText($"Capt'n {player.number} hide your ship!");
        }
        else
        {
            audioPlayer.OnVictory();
            player.HUD.WriteText($"Capt'n {player.number} wins for reaching the top dimension!");
            GameData.winner = player.name;
            player.GetComponent<SceneChanger>().LoadResolveGame();
        }
    }

    public void SinkShip(ref DimensionManager currentDimension, int shipNo, ShipStatus status, CellOccupier occupier)
    {
        ShipChangeDimension(currentDimension.DimensionNo - 1, ref currentDimension, shipNo, occupier);
        transform.position += new Vector3(0, 0.5f, 0);
        gameObject.layer = LayerMask.NameToLayer($"Fleet{player.number}");
        player.inputHandler.continueGame = false;
        StartCoroutine(ResetShip(status));
    }

    private void ShipChangeDimension(int newDimensionNo, ref DimensionManager dimensionBefore, int shipNo, CellOccupier occupier)
    {
        //Debug.Log(name + "  HUD_DimensionsOpponent[toDimensionNo]: " + player.opponent.HUD.HUD_FleetOpponent[shipNo]);
        player.world.SetNewDimension(newDimensionNo);
        player.vehicle.SetViewOnDimension(newDimensionNo);

        player.HUD.SetHUDDimension(newDimensionNo);
        
        player.HUD.UpdateHUDFleets(shipNo, newDimensionNo, dimensionBefore.DimensionNo);

        SwitchDimension(newDimensionNo, ref dimensionBefore, occupier);
        SetDimension(player.dimensions.GetDimension(newDimensionNo), shipNo);

        transform.position -= new Vector3(0, OverworldData.DimensionSize * 2, 0);
    }

    private void SwitchDimension(int dimensionNr, ref DimensionManager dimension, CellOccupier occupier)
    {
        occupier.ReleaseCells();
        dimension.RemoveShip(gameObject);
        dimension = player.dimensions.GetDimension(dimensionNr);
        occupier.OccupyCells();
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

        // To do: Wait for player to submit fleet.
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
}
