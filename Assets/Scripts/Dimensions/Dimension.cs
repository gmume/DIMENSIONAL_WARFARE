using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dimension : MonoBehaviour
{
    private Player player;
    public GameObject[][] cells;
    private readonly List<GameObject> ships = new();

    public int DimensionNr { get; private set; }

    public void InitDimension(Player player, int nr, GameObject cellPrefab, ArrayList fleet)
    {
        this.player = player;
        DimensionNr = nr;
        CreateCells(cellPrefab);

        if (DimensionNr == 0)
        {
            player.ActiveDimension = GetComponent<Dimension>();

            for (int i = 0; i < fleet.Count; i++)
            {
                GameObject shipObj = (GameObject)fleet[i];
                Ship ship = shipObj.GetComponent<Ship>();
                ship.SetDimension(this);
                ship.OccupyCells();
            }

            AddShips(fleet);
        }
    }

    public void CreateCells(GameObject cellPrefab)
    {
        cells = new GameObject[OverworldData.DimensionSize][];

        for (int x = 0; x < OverworldData.DimensionSize; x++)
        {
            cells[x] = new GameObject[OverworldData.DimensionSize];

            for (int z = 0; z < OverworldData.DimensionSize; z++)
            {
                GameObject cellObj = Instantiate(cellPrefab, new Vector3(x, OverworldData.DimensionSize * DimensionNr * 2, z), Quaternion.identity);

                cellObj.layer = Layer.SetLayerPlayer(player);
                cellObj.transform.parent = transform;

                Cell cell = cellObj.GetComponent<Cell>();
                cell.X = x;
                cell.Y = z;
                cell.Activated = false;
                cell.Occupied = false;
                cell.Hitted = false;
                cells[cell.X][cell.Y] = cellObj;
            }
        }
    }

    public GameObject GetCell(int x, int y)
    {
        try
        {
            if (x < OverworldData.DimensionSize && y < OverworldData.DimensionSize)
            {
                return cells[x][y];
            }
        }
        catch (System.Exception)
        {
            Debug.Log("No cell found!");
            throw;
        }

        return null;
    }

    public void AddShips(ArrayList newShips)
    {
        foreach (GameObject ship in newShips)
        {
            if (ship.GetComponent<Ship>().Dimension == this)
            {
                ship.transform.SetParent(GetComponent<Transform>().transform, true);
                ships.Add(ship);
            }
        }
    }

    public GameObject GetShipOnCell(int x, int y)
    {
        bool found = false;
        int i = 0;

        try
        {
            while (!found && i < ships.Count)
            {
                GameObject shipObj = ships[i];
                Ship ship = shipObj.GetComponent<Ship>();
                if (ship.PivotX == x && ship.PivotZ == y)
                {
                    found = true;
                    return shipObj;
                }
                else
                {
                    i++;
                }
            }
        }
        catch (System.Exception)
        {
            Debug.Log("No ship found!");
            throw;
        }

        return null;
    }

    public void RemoveShip(GameObject shipObj)
    {
        ships.Remove(shipObj);
    }
}
