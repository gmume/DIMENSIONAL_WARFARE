using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipLifter : MonoBehaviour
{
    [HideInInspector] public PlayerData player;

    [HideInInspector] public AudioPlayer audioPlayer;

    [HideInInspector] public ShipPart[] parts;

   public void LiftShipUp(ref Dimension dimension, int shipNo, CellOccupier occupier)
    {
        if (dimension.DimensionNo < OverworldData.DimensionsCount - 2)
        {
            audioPlayer.OnShipUp();
            player.world.SetNewDimension(dimension.DimensionNo + 1);
            player.HUD.SetHUDDimension(player.ActiveDimension.DimensionNo);

            player.vehicle.SetViewOnDimension(player.ActiveDimension.DimensionNo);
            player.HUD.UpdateHUDFleet(shipNo, player.ActiveDimension.DimensionNo, dimension.DimensionNo);

            SwitchDimension(player.ActiveDimension.DimensionNo, ref dimension, occupier);
            SetDimension(dimension, shipNo);
            transform.position += new Vector3(0, OverworldData.DimensionSize * 2, 0);
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

    private void SwitchDimension(int dimensionNr, ref Dimension dimension, CellOccupier occupier)
    {
        occupier.ReleaseCells();
        dimension.RemoveShip(gameObject);
        dimension = player.dimensions.GetDimension(dimensionNr);
        occupier.OccupyCells();
    }

    public void SinkShip(ref Dimension currentDimension, int shipNo, ShipStatus status, CellOccupier occupier)
    {
        player.world.SetNewDimension(currentDimension.DimensionNo - 1);

        player.HUD.SetHUDDimension(player.ActiveDimension.DimensionNo);
        player.HUD.UpdateHUDFleet(shipNo, player.ActiveDimension.DimensionNo, currentDimension.DimensionNo);

        player.vehicle.SetViewOnDimension(player.ActiveDimension.DimensionNo);

        gameObject.layer = LayerMask.NameToLayer($"Fleet{player.number}");
        transform.position += new Vector3(0, 0.5f, 0);
        transform.position -= new Vector3(0, OverworldData.DimensionSize * 2, 0);

        SwitchDimension(currentDimension.DimensionNo - 1, ref currentDimension, occupier);
        SetDimension(player.dimensions.GetDimension(currentDimension.DimensionNo - 1), shipNo);

        player.inputHandling.continueGame = false;
        StartCoroutine(ResetShip(status));
    }

    private IEnumerator ResetShip(ShipStatus status)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1f;

        foreach (ShipPart part in parts)
        {
            part.ResetPart();
        }

        status = ShipStatus.Intact;
        player.input.SwitchCurrentActionMap("GameStart");
        player.HUD.WriteText($"Capt'n {player.number} hide your ship!");

        // To do: Wait for player to submit fleet.
    }

    public void SetDimension(Dimension newDimension, int shipNo)
    {
        transform.parent = newDimension.transform;

        for (int i = 0; i <= shipNo; i++)
        {
            ShipPart part = parts[i];
            part.Dimension = newDimension;
            part.transform.parent = transform;
        }
    }
}
