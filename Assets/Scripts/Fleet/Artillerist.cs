using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artillerist : MonoBehaviour
{
    [HideInInspector] public PlayerData player;
                      private Color fireColor = Color.red;

    public bool Fire(ShipManager shipManager, DimensionManager dimension)
    {
        CellData activeCell = HitCell();
        CellData opponentCell = GetOpponentCell(activeCell, dimension);

        if (!CanShipAscend(opponentCell)) return false;
        player.HUD.armed.SetActive(false);
        return shipManager.ShipUp();
    }

    private CellData HitCell()
    {
        player.ActiveCell.GetComponent<Renderer>().material.color = fireColor;
        player.ActiveCell.Hitted = true;
        return player.ActiveCell;
    }

    private CellData GetOpponentCell(CellData activeCell, DimensionManager dimension) => player.opponent.dimensions.GetDimension(dimension.DimensionNo).GetCell(activeCell.X, activeCell.Y).GetComponent<CellData>();

    private bool CanShipAscend(CellData opponentCell) => opponentCell.Occupied && IsOpponentSunk(opponentCell);

    private bool IsOpponentSunk(CellData opponentCell) => opponentCell.Part.GetComponentInParent<ShipManager>().TakeHit(opponentCell.Part);
}
