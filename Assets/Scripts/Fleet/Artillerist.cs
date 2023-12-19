using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artillerist : MonoBehaviour
{
    [HideInInspector] public PlayerData player;
                      private Color fireColor = Color.red;

    public bool Fire(ShipManager shipManager, DimensionManager dimension)
    {
        CellData activeCell = player.ActiveCell;
        Material cellMaterial = activeCell.GetComponent<Renderer>().material;
        cellMaterial.color = fireColor;
        activeCell.Hitted = true;

        DimensionManager opponentDimension = player.opponent.dimensions.GetDimension(dimension.DimensionNo);
        CellData opponentCell = opponentDimension.GetCell(activeCell.X, activeCell.Y).GetComponent<CellData>();

        if (opponentCell.Occupied)
        {
            bool opponentSunk;
            ShipPartManager part = opponentCell.Part;

            ShipManager opponentShip = part.GetComponentInParent<ShipManager>();
            opponentSunk = opponentShip.TakeHit(part);

            if (opponentSunk)
            {
                shipManager.ShipUp();
                return true;
            }
        }

        return false;
    }
}
