using UnityEngine;

public class ShipPartManager : MonoBehaviour
{
    public int partNo;
    public int X { get; private set; }
    public int Y { get; private set; }
    public DimensionManager Dimension { get; set; }
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
        CellData cell = Dimension.GetCell(X, Y).GetComponent<CellData>();
        cell.Occupied = true;
        cell.Part = this;
    }

    public void ReleaseCell()
    {
        CellData cell = Dimension.GetCell(X, Y).GetComponent<CellData>();
        cell.Occupied = false;
        cell.Part = null;
    }

    public void ResetPart()
    {
        Damaged = false;
        SetColorIntact();
    }

    private void SetColorIntact()
    {
        PartMaterial.color = colorIntact;
    }

    public void Initialize(PlayerData player, int partNo, ShipManager ship)
    {
        this.partNo = partNo;
        X = ship.ShipNo;
        Y = partNo;
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
