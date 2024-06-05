using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class Lifter : MonoBehaviour
{
    public PlayerData player;
    public AudioPlayer audioPlayer;
    public ShipManager manager;
    public ShipPartManager[] parts;
    public LayerFilter layerFilter;
    public LayerFilter opponentLayerFilter;

    public string LiftShipUp(ref DimensionManager currentDimension, int shipNo)
    {
        if (IsNotWinner(currentDimension))
        {
            audioPlayer.OnShipUp();
            ShipChangeDimension(currentDimension.No + 1, ref currentDimension, shipNo);
            layerFilter.ShowLayers(true, true, true, false);

            player.input.SwitchCurrentActionMap("PlaceShips");
            player.inputEnabler.DisableChoosingShips();
            player.HUD.WriteText($"Capt'n {player.number} hide your ship!");
            player.onboarding.ShowTip("OwnShipUp");
            player.opponent.onboarding.ShowTip("OpponentShipUp");
            return "yes";
        }
        else
        {
            player.input.currentActionMap.Disable();
            player.opponent.input.currentActionMap.Disable();
            player.Fade.StartEffect();
            player.opponent.Fade.StartEffect();
            StartCoroutine(ResolveGame());
        }

        return "";
    }

    private bool IsNotWinner(DimensionManager currentDimension) => currentDimension.No <OverworldData.DimensionsCount - 2;

    private IEnumerator ResolveGame()
    {
        yield return new WaitUntil(() => player.Fade.finished);
        SceneChanger.LoadResolveGame();
        GameData.winner = player.name;
    }

    public void SinkShip(ref DimensionManager currentDimension, int shipNo, ShipStatus status)
    {
        ShipChangeDimension(currentDimension.No - 1, ref currentDimension, shipNo);
        transform.position += new Vector3(0, 0.5f, 0);
        gameObject.layer = LayerMask.NameToLayer($"Fleet{player.number}");
        layerFilter.ShowLayers(true, true, true, false);
        player.inputHandler.continueGame = false;
        StartCoroutine(ResetShip(status));
    }

    private void ShipChangeDimension(int newDimensionNo, ref DimensionManager dimensionBefore, int shipNo)
    {
        player.HUD.SetHUDDimension(newDimensionNo);
        player.HUD.UpdateHUDFleets(shipNo, newDimensionNo);
        LeaveOldDimension(ref dimensionBefore);
        ArriveOnNewDimension(newDimensionNo, shipNo);
        player.world.SetNewDimension(newDimensionNo);
        player.vehicle.SetViewOnDimension(newDimensionNo);
    }

    private void LeaveOldDimension(ref DimensionManager dimensionBefore)
    {
        player.opponent.world.DeactivateCells();
        //player.opponent.dimensions.ResetCellPositions(dimensionBefore.No);
        player.dimensions.ReleaseCells(player.dimensions.GetCellGroup(manager.GetShipCoordinates(), dimensionBefore.No));
        dimensionBefore.RemoveShip(gameObject);
    }

    private void ArriveOnNewDimension(int newDimensionNo, int shipNo)
    {
        DimensionManager newDimension = player.dimensions.GetDimension(newDimensionNo);
        List<(GameObject cell, GameObject obj)> cellsAndObjects = TupleListProvider.GetTuplesList(newDimension, manager.GetShipCoordinates(), manager.partsList);
        int vacantCellsCount = player.dimensions.CountVacantCells(player.dimensions.GetCellGroup(manager.GetShipCoordinates(), newDimensionNo));

        if (vacantCellsCount != cellsAndObjects.Count)
        {
            List<Vector2> vacantCoords = player.dimensions.FindVacantCoordinates(newDimensionNo, manager.GetShipCoordinates());
            manager.SetShipCoordinates(vacantCoords);
            cellsAndObjects = TupleListProvider.GetTuplesList(newDimension, manager.GetShipCoordinates(), manager.partsList);
        }

        SetDimension(newDimension, shipNo);
        player.dimensions.OccupyCells(cellsAndObjects);
        player.opponent.world.ActivateCells();
    }

    public void SetDimension(DimensionManager newDimension, int shipNo)
    {
        transform.SetParent(newDimension.transform);
        manager.dimension = newDimension;

        for (int i = 0; i <= shipNo; i++)
        {
            parts[i].Dimension = newDimension;
            parts[i].gameObject.layer = LayerMask.NameToLayer($"Fleet{(player.number == 1 ? 1 : 2)}");
        }
    }

    private IEnumerator ResetShip(ShipStatus status)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1f;

        foreach (ShipPartManager part in parts)
        {
            manager.HUD_buttonPartsHandler[manager.No].RepairButtonPart(part.partNo);
            part.ResetPart();
        }

        //status = ShipStatus.Intact;
        player.input.SwitchCurrentActionMap("PlaceShips");
        player.inputEnabler.DisableChoosingShips();
        player.HUD.WriteText($"Capt'n {player.number} hide your ship!");
        player.ActiveShip = gameObject.GetComponent<ShipManager>();
    }
}
