using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lifter : MonoBehaviour
{
    [HideInInspector] public PlayerData player;
    [HideInInspector] public AudioPlayer audioPlayer;
    [HideInInspector] public ShipManager manager;
    [HideInInspector] public ShipPartManager[] parts;

    public bool LiftShipUp(ref DimensionManager currentDimension, int shipNo, CellOccupier occupier)
    {
        if (IsNotWinner(currentDimension))
        {
            audioPlayer.OnShipUp();
            ShipChangeDimension(currentDimension.No + 1, ref currentDimension, shipNo, new Vector3(0, OverworldData.DimensionSize * 2, 0));
            player.input.SwitchCurrentActionMap("GameStart");
            player.HUD.WriteText($"Capt'n {player.number} hide your ship!");
            player.onboarding.ShowTip("OwnShipUp");
            player.opponent.onboarding.ShowTip("OpponentShipUp");
            return true;
        }
        else
        {
            SceneChanger.LoadResolveGame();
            GameData.winner = player.name;
            return false;
        }
    }

    private bool IsNotWinner(DimensionManager currentDimension) => currentDimension.No <OverworldData.DimensionsCount - 2;

    public void SinkShip(ref DimensionManager currentDimension, int shipNo, ShipStatus status)
    {
        ShipChangeDimension(currentDimension.No - 1, ref currentDimension, shipNo, new Vector3(0, OverworldData.DimensionSize * -2, 0));
        transform.position += new Vector3(0, 0.5f, 0);
        gameObject.layer = LayerMask.NameToLayer($"Fleet{player.number}");
        player.inputHandler.continueGame = false;
        StartCoroutine(ResetShip(status));
    }

    private void ShipChangeDimension(int newDimensionNo, ref DimensionManager dimensionBefore, int shipNo, Vector3 vector)
    {
        player.HUD.SetHUDDimension(newDimensionNo);
        player.HUD.UpdateHUDFleets(shipNo, newDimensionNo, dimensionBefore.No);

        SwitchDimension(newDimensionNo, ref dimensionBefore, shipNo);
        transform.position += vector;

        player.world.SetNewDimension(newDimensionNo);
        player.vehicle.SetViewOnDimension(newDimensionNo);
    }

    private void SwitchDimension(int newDimensionNo, ref DimensionManager dimension, int shipNo)
    {
        List<GameObject> cells = player.dimensions.GetCellGroup(manager.GetShipCoodinates(), manager.dimension.No);

        player.dimensions.ReleaseCells(cells);
        dimension.RemoveShip(gameObject);
        dimension = player.dimensions.GetDimension(newDimensionNo);
        SetDimension(dimension, shipNo);
        player.dimensions.OccupyCells(TupleListProvider.GetTuplesList(manager.dimension, manager.GetShipCoodinates(), manager.partsList));
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

        //status = ShipStatus.Intact;
        player.input.SwitchCurrentActionMap("GameStart");
        player.HUD.WriteText($"Capt'n {player.number} hide your ship!");
        player.ActiveShip = gameObject.GetComponent<ShipManager>();
    }
}
