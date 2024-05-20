using System;
using System.Collections.Generic;
using UnityEngine;

public class DimensionManager : MonoBehaviour
{
    private PlayerData player;
    public GameObject[][] Cells { private set; get; }
    private float cellDefaultYPos;
    private readonly List<GameObject> ships = new();

    public int No { get; private set; }
    
    public GameObject GetCell(int x, int y)
    {
        if (x >= 0 && x < OverworldData.DimensionSize && y >= 0 && y < OverworldData.DimensionSize) return Cells[x][y];

        return null;
    }

    public void ResetCellPositions()
    {
        foreach (GameObject[] cells in Cells)
        {
            foreach(GameObject cell in cells)
            {
                if (cell.transform.position.y != cellDefaultYPos)
                {
                    Vector3 cellPosition = cell.transform.position;
                    cellPosition = new(cellPosition.x, cellDefaultYPos, cellPosition.z);
                    cell.transform.position = cellPosition;
                }
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

        Debug.LogWarning($"{name}: No ship found on cell (x={x}, y={y})");
        return null;
    }

    public void RemoveShip(GameObject shipObj) => ships.Remove(shipObj);

    public void Initialize(PlayerData player, int dimensionNo, GameObject cellPrefab, List<GameObject> fleet)
    {
        this.player = player;
        No = dimensionNo;
        CreateCells(cellPrefab);
        if (No != 0) AddRocks();
        if (No == 0) AddShips(fleet);
    }

    public void CreateCells(GameObject cellPrefab)
    {
        Cells = new GameObject[OverworldData.DimensionSize][];

        for (int x = 0; x < OverworldData.DimensionSize; x++)
        {
            Cells[x] = new GameObject[OverworldData.DimensionSize];

            for (int z = 0; z < OverworldData.DimensionSize; z++)
            {
                GameObject cellObj = Instantiate(cellPrefab, new Vector3(x, OverworldData.DimensionSize * No * 2, z), Quaternion.identity);

                cellObj.layer = LayerSetter.SetLayerDimensions(player);
                cellObj.transform.SetParent(transform);
                CellData cell = cellObj.GetComponent<CellData>();
                cell.Dimension = this;
                cell.X = x;
                cell.Y = z;
                cell.Focused = false;
                cell.Active = false;
                cell.Occupied = false;
                cell.Hit = false;
                Cells[cell.X][cell.Y] = cellObj;
            }
        }

        cellDefaultYPos = Cells[0][0].GetComponent<Transform>().position.y;
    }

    private void AddRocks()
    {
        RocksManger rocksManager = player.dimensions.GetComponent<RocksManger>();
        List<Vector2> rocksCoordinates = rocksManager.GetRocksCoordinates(No);
        List<GameObject> cells = player.dimensions.GetCellGroup(rocksCoordinates, this);
        List<GameObject> rocks = new();

        for (int i = 0; i < cells.Count; i++)
        {
            GameObject rock = rocksManager.GetRock();
            rocks.Add(rock);
        }

        player.dimensions.OccupyCells(TupleListProvider.GetTuplesList(cells, rocks));
    }

    private void AddShips(List<GameObject> fleet)
    {
        player.ActiveDimension = this;

        foreach (GameObject shipObj in fleet)
        {
            ShipManager ship = shipObj.GetComponent<ShipManager>();
            ship.SetDimension(this);
            player.dimensions.OccupyCells(TupleListProvider.GetTuplesList(this, ship.GetShipCoordinates(), ship.partsList));
            if (ship.dimension == this) ships.Add(shipObj);
        }
    }
}
