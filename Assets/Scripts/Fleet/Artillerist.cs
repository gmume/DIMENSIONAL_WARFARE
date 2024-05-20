using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artillerist : MonoBehaviour
{
    [HideInInspector] public PlayerData player;
    public AttackPattern attackPattern;

    public bool Fire(ShipManager shipManager)
    {
        CellData focusedCell = player.FocusedCell;
        List<Vector2> hitCellsCoordinates = GetCellCoordinates(focusedCell);

        HitCells(hitCellsCoordinates, focusedCell);
        List<GameObject> opponentCells = GetOpponentCells(hitCellsCoordinates, focusedCell);
        List<bool> sunkenShips = new();

        foreach (GameObject opponentCell in opponentCells)
        {
            CellData opponentCellData = opponentCell.GetComponent<CellData>();

            sunkenShips.Add(opponentCellData.Occupied && opponentCellData.OccupyingObj.CompareTag("ShipPart") && IsOpponentSunk(opponentCellData));
        }

        if (!CanShipAscend(sunkenShips)) return false;
        player.HUD.armed.SetActive(false);
        return shipManager.ShipUp();
    }

    private CellData HitCells(List<Vector2> hitCellsCoordinates, CellData focusedCell)
    {
        List<GameObject> cells = player.opponent.dimensions.GetCellGroup(hitCellsCoordinates, focusedCell.Dimension.No);

        foreach (GameObject cell in cells)
        {
            cell.GetComponent<Renderer>().material.color = Colors.hitCell;
            cell.GetComponent<CellData>().Hit = true;
        }

        return player.FocusedCell;
    }

    public List<Vector2> GetCellCoordinates(CellData focusedCell)
    {
        List<Vector2> cellsCoodinates = new();
        int shipLevel = GetComponent<ShipManager>().dimension.No;

        for (int level = 0; level <= shipLevel; level++)
        {
            cellsCoodinates.AddRange(GetCoordinatesPerLevel(focusedCell, attackPattern.GetPatternLevel(level)));
        }

        return cellsCoodinates;
    }

    private List<Vector2> GetCoordinatesPerLevel(CellData focusedCell, List<Vector2> deltaCoordinates)
    {
        List<Vector2> cellCoordinatesPerLevel = new();

        foreach (Vector2 deltaCoordinate in deltaCoordinates)
        {
            int x = focusedCell.X + (int)deltaCoordinate[0];
            int y = focusedCell.Y + (int)deltaCoordinate[1];
            cellCoordinatesPerLevel.Add(new Vector2(x, y));
        }

        return cellCoordinatesPerLevel;
    }

    private List<GameObject> GetOpponentCells(List<Vector2> hitCellsCoordinates, CellData focusedCell) => player.opponent.dimensions.GetCellGroup(hitCellsCoordinates, focusedCell.Dimension.No);

    private bool IsOpponentSunk(CellData opponentCell) => opponentCell.OccupyingObj.GetComponentInParent<ShipManager>().TakeHit(opponentCell.OccupyingObj.GetComponent<ShipPartManager>());

    private bool CanShipAscend(List<bool> sunkenShips)
    {
        foreach (bool sunkenShip in sunkenShips)
        {
            if (sunkenShip) return true;
        }

        return false;
    }
}
