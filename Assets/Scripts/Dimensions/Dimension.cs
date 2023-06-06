using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dimension : MonoBehaviour
{
    //private Player player;
    private GameObject[][] cells;
    private readonly List<GameObject> ships = new();
    
    public int DimensionNr { get; private set; }

    //private void Awake()
    //{
    //    player = GetComponent<Player>();
    //}

    public void InitDimension(Player player, int nr, GameObject cellPrefab, ArrayList fleet)
    {
        DimensionNr = nr;
        CreateCells(player, cellPrefab);

        if(DimensionNr == 0)
        {
            player.data.ActiveDimension = GetComponent<Dimension>();
             
            for (int i = 0; i < fleet.Count; i++)
            {
                GameObject shipObj = (GameObject)fleet[i];
                Ship ship = shipObj.GetComponent<Ship>();
                ship.Dimension = this;
                ship.OccupyCell();
            }

            AddShips(fleet);
        }
    }

    public void CreateCells(Player player, GameObject cellPrefab)
    {
        cells = new GameObject[OverworldData.DimensionSize][];

        for (int j = 0; j < OverworldData.DimensionSize; j++)
        {
            cells[j] = new GameObject[OverworldData.DimensionSize];

            for (int k = 0; k < OverworldData.DimensionSize; k++)
            {
                GameObject cell = Instantiate(cellPrefab, new Vector3(j, OverworldData.DimensionSize * DimensionNr * 2, k), Quaternion.identity);
                cell.layer = Layer.SetLayerPlayer(player);
                cell.transform.parent = transform;
                cell.GetComponent<Cell>().X = j;
                cell.GetComponent<Cell>().Y = k;
                cell.GetComponent<Cell>().Activated = false;
                cell.GetComponent<Cell>().Occupied = false;
                cell.GetComponent<Cell>().Hitted = false;
                cells[j][k] = cell;
            }
        }
    }

    public GameObject GetCell(int x, int y)
    {
        return cells[x][y];
    }

    public void AddShips(ArrayList newShips)
    {
        foreach(GameObject ship in newShips)
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
                if (ship.X == x && ship.Z == y)
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
}
