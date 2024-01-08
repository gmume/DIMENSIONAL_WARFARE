using System.Collections.Generic;
using UnityEngine;

public class DimensionManager : MonoBehaviour
{
    private PlayerData player;
    public GameObject[][] Cells { private set; get; }
    private readonly List<GameObject> ships = new();

    public int DimensionNo { get; private set; }

    public void Initialize(PlayerData player, int dimensionNo, GameObject cellPrefab, List<GameObject> fleet)
    {
        this.player = player;
        DimensionNo = dimensionNo;
        CreateCells(cellPrefab);

        if (DimensionNo == 0)
        {
            player.ActiveDimension = GetComponent<DimensionManager>();

            foreach (GameObject shipObj in fleet)
            {
                ShipManager ship = shipObj.GetComponent<ShipManager>();
                ship.SetDimension(this);
                ship.OccupyCells();
            }

            AddShips(fleet);
        }
    }

    public void CreateCells(GameObject cellPrefab)
    {
        Cells = new GameObject[OverworldData.DimensionSize][];

        for (int x = 0; x < OverworldData.DimensionSize; x++)
        {
            Cells[x] = new GameObject[OverworldData.DimensionSize];

            for (int z = 0; z < OverworldData.DimensionSize; z++)
            {
                GameObject cellObj = Instantiate(cellPrefab, new Vector3(x, OverworldData.DimensionSize * DimensionNo * 2, z), Quaternion.identity);

                cellObj.layer = LayerSetter.SetLayerDimensions(player);
                cellObj.transform.parent = transform;
                CellData cell = cellObj.GetComponent<CellData>();
                cell.X = x;
                cell.Y = z;
                cell.Activated = false;
                cell.Occupied = false;
                cell.Hitted = false;
                Cells[cell.X][cell.Y] = cellObj;
            }
        }
    }

    public GameObject GetCell(int x, int y)
    {
        if (x >= 0 && x < OverworldData.DimensionSize && y >= 0 && y < OverworldData.DimensionSize)
        {
            return Cells[x][y];
        }
        else
        {
            Debug.LogWarning($"{name}: Invalid cell coordinates (x={x}, y={y})");
            return null;
        }
    }

    public void AddShips(List<GameObject> newShips)
    {
        foreach (GameObject ship in newShips)
        {
            if (ship.GetComponent<ShipManager>().dimension == this)
            {
                ship.transform.SetParent(transform, true);
                ships.Add(ship);
            }
        }
    }

    public GameObject GetShipOnCell(int x, int y)
    {
        foreach (GameObject shipObj in ships)
        {
            ShipManager ship = shipObj.GetComponent<ShipManager>();

            if (ship.navigator.PivotX == x && ship.navigator.PivotZ == y) return shipObj;
        }

        Debug.LogWarning($"{name}: No ship found on cell (x={x}, y={ y})");
        return null;
    }

    public void RemoveShip(GameObject shipObj) => ships.Remove(shipObj);
}
