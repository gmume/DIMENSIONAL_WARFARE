using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPart : MonoBehaviour
{
    public int partNr;
    public int X { get; private set; }
    public int Y { get; private set; }
    public Dimension Dimension { get; set; }
    public Material PartMaterial { get; private set; }
    private Color colorIntact;
    public bool Damaged { get; set; }

    public void UpdateCoordinatesRelative(int x, int y)
    {
        X += x;
        Y += y;
    }

    public void UpdateCoordinatesAbsolute(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void OccupyCell()
    {
        Cell cell = Dimension.GetCell(X, Y).GetComponent<Cell>();
        cell.Occupied = true;
        cell.Part = this;
    }

    public void ReleaseCell()
    {
        Cell cell = Dimension.GetCell(X, Y).GetComponent<Cell>();
        cell.Occupied = false;
        cell.Part = null;
    }

    public void ResetPart()
    {
        Debug.Log("ResetPart");
        Damaged = false;
        SetColorIntact();
    }

    private void SetColorIntact()
    {
        PartMaterial.color = colorIntact;
    }

    public void InitiateShipPart(Player player, int partNr, Ship ship)
    {
        this.partNr = partNr;
        X = ship.ShipNr;
        Y = partNr;
        Damaged = false;
        PartMaterial = GetComponent<Renderer>().material;

        if (player.number == 1)
        {
            colorIntact = new Color(0.3f, 0.12f, 0, 1); // brown
            
        }
        else
        {
            colorIntact = new Color(0.3f, 0.3f, 0, 1); // olive
        }

        SetColorIntact();
    }
}
