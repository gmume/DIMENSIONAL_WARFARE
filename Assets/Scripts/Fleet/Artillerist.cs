using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artillerist : MonoBehaviour
{
    [HideInInspector] public PlayerData player;
                      private Color fireColor = Color.red;

    public bool Fire(ShipManager shipManager)
    {
        CellData focusedCell = player.FocusedCell;
        HitCells(focusedCell, shipManager);
        List<GameObject> opponentCells = GetOpponentCells(focusedCell, shipManager);
        List<bool> sunkenShips = new();

        foreach (GameObject opponentCell in opponentCells)
        {
            CellData opponentCellData = opponentCell.GetComponent<CellData>();
            sunkenShips.Add(opponentCellData.Occupied && IsOpponentSunk(opponentCellData));
        }

        if (!CanShipAscend(sunkenShips)) return false;
        player.HUD.armed.SetActive(false);
        return shipManager.ShipUp();
    }
    
    private CellData HitCells(CellData focusedCell, ShipManager attackingShip)
    {
        List<GameObject> cells = player.dimensions.GetCellGroup(attackingShip.No, attackingShip.dimension.No, focusedCell.Dimension.No, new(focusedCell.X, focusedCell.Y));

        foreach (GameObject cell in cells)
        {
            cell.GetComponent<Renderer>().material.color = fireColor;
            cell.GetComponent<CellData>().Hitted = true;
        }

        return player.FocusedCell;
    }

    private List<GameObject> GetOpponentCells(CellData focusedCell, ShipManager attackingShip) => player.opponent.dimensions.GetCellGroup(attackingShip.No, attackingShip .dimension.No, focusedCell.Dimension.No, new(focusedCell.X, focusedCell.Y));

    private bool IsOpponentSunk(CellData opponentCell) => opponentCell.Part.GetComponentInParent<ShipManager>().TakeHit(opponentCell.Part);

    private bool CanShipAscend(List<bool> sunkenShips)
    {
        foreach (bool sunkenShip in sunkenShips)
        {
            if (sunkenShip) return true;
        }

        return false;
    }
}
