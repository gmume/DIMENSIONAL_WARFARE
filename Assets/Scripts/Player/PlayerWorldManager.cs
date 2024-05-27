using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWorldManager : MonoBehaviour
{
    public GameObject dimensionPrefab, cellPrefab;

    private PlayerData player;
    private int currentX = 0, currentY = 0;

    public void SetNewDimension(int no) => player.ActiveDimension = player.dimensions.GetDimension(no);

    public void MoveSelection(float x, float y)
    {
        // Get right axis and move in correct direction
        SetNewCellRelative((int)((Math.Abs(x) > Math.Abs(y)) ? Mathf.Sign(x) : 0), (int)((Math.Abs(x) > Math.Abs(y)) ? 0 : Mathf.Sign(y)));
    }
    
    private void SetNewCellRelative(bool deactivateCells, int onNewDimensionNo, int deltaX, int deltaY)
    {
        int newX = currentX + deltaX;
        int newY = currentY + deltaY;

        if (IsWithinDimensionBounds(newX, newY))
        {
            currentX = newX;
            currentY = newY;

            if (deactivateCells) player.world.DeactivateCells();
            if (onNewDimensionNo > -1) player.world.SetNewDimension(onNewDimensionNo);

            player.FocusedCell = player.opponent.dimensions.GetDimension(player.ActiveDimension.No).GetCell(currentX, currentY).GetComponent<CellData>();
            ActivateCells();
        }
        else
        {
            player.HUD.WriteText($"Capt'n {player.number}, target coordinates are out of range!");
        }
    }

    public void SetNewCellRelative(int deltaX, int deltaY) => SetNewCellRelative(true, -1, deltaX, deltaY);

    public void SetNewCellRelative(bool deactivateCells, int deltaX, int deltaY) => SetNewCellRelative(false, -1, deltaX, deltaY);

    public void SetNewCellRelative(int onNewDimensionNo, int deltaX, int deltaY) => SetNewCellRelative(true, onNewDimensionNo, deltaX, deltaY);

    private bool IsWithinDimensionBounds(int x, int y) => x >= 0 && x < OverworldData.DimensionSize && y >= 0 && y < OverworldData.DimensionSize;

    public void SetNewCellAbsolute(bool deactivating, int newX, int newY)
    {
        currentX = 0;
        currentY = 0;

        SetNewCellRelative(deactivating, newX, newY);
    }

    public void ActivateCells()
    {
        if (player.FocusedCell == null || player.ActiveShip == null) return;
        List<Vector2> patternCoords = player.ActiveShip.GetFocusedCoordinates(player.FocusedCell);
        List<GameObject> cells = player.opponent.dimensions.GetCellGroup(patternCoords, player.ActiveDimension.No);

        foreach (GameObject cell in cells)
        {
            cell.transform.position += new Vector3(0, 0.2f, 0);
            cell.GetComponent<CellData>().ColorBefore = cell.GetComponent<Renderer>().material.color;
            cell.GetComponent<Renderer>().material.color += Colors.deltaActiveCell;
        }
    }

    public void DeactivateCells()
    {
        if (!player.FocusedCell || !player.ActiveShip) return;

        List<Vector2> patternCoords = player.ActiveShip.GetFocusedCoordinates(player.FocusedCell);
        List<GameObject> cells = player.opponent.dimensions.GetCellGroup(patternCoords, player.ActiveDimension.No);

        foreach (GameObject cell in cells)
        {
            cell.transform.position -= new Vector3(0, 0.2f, 0);
            cell.GetComponent<Renderer>().material.color = cell.GetComponent<CellData>().ColorBefore;
        }
    }

    public void Initialize(PlayerData player)
    {
        this.player = player;
        Materials.cellBlueMat.color = Colors.cellBlue;
        Materials.cellTurqoiseMat.color = Colors.cellTurqoise;
        player.dimensions.name = $"Dimensions{player.number}";
        player.dimensions.Initialize();
        SetNewDimension(0);
    }
}
