using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public string ShipName { get; private set; }
    public Dimension Dimension { get; set; }
    private Transform position;
    public int X { get; private set; }
    public int Z { get; private set; }
    public Directions Direction { get; set; }
    public int PartsCount { get; private set; }

    private bool[] partDamaged;

    public void Activate(PlayerScript player)
    {
        if (player.playerData.ActiveShip != this)
        {
            GetComponent<Renderer>().material.color = Color.black;
            Vector3 vectorUp = new(0f, 0.1f, 0f);
            GetComponent<Transform>().position += vectorUp;
            player.playerData.ActiveShip = this;
            ReleaseCell(X, Z);
            player.playerData.ActiveShip = this;
        }
    }

    public void Deactivate(PlayerScript playerScript)
    {
        if (playerScript.name == "Player1")
        {
            GetComponent<Renderer>().material.color = new Color(0.8f, 0.5f, 0.5f, 1);
        }
        else
        {
            GetComponent<Renderer>().material.color = new Color(0.8f, 0.7f, 0.5f, 1);
        }

        Vector3 vectorDown = new(0f, -0.1f, 0f);
        GetComponent<Transform>().position += vectorDown;
        OccupyCell();
        playerScript.playerData.ActiveShip = null;
    }

    public void Move(int x, int y)
    {
        if (X + x < OverworldData.DimensionSize && Z + y < OverworldData.DimensionSize && X + x >= 0 && Z + y >= 0)
        {
            if (x == 0)
            {
                Z += y;
            }
            else
            {
                X += x;
            }

            position.position += new Vector3(x, 0, y);
        }
        else
        {
            print("Don't let your ship run aground, capt'n!");
        }
    }

    public void QuaterTurnRight()
    {
        Quaternion quaterTurn = Quaternion.Euler(Vector3.forward * 90);
        if (Direction == Directions.North)
        {
            transform.rotation = quaterTurn;
            Direction = Directions.East;
        }
        else if (Direction == Directions.South)
        {
            transform.rotation = quaterTurn;
            Direction = Directions.West;
        }
        else if (Direction == Directions.East)
        {
            transform.rotation = quaterTurn;
            Direction = Directions.South;
        }
        else if (Direction == Directions.West)
        {
            transform.rotation = quaterTurn;
            Direction = Directions.North;
        }
    }

    public void QuaterTurnLeft()
    {
        Quaternion quaterTurn = Quaternion.Euler(Vector3.forward * 90);
        if (Direction == Directions.North)
        {
            transform.rotation = quaterTurn;
            Direction = Directions.West;
        }
        else if (Direction == Directions.South)
        {
            transform.rotation = quaterTurn;
            Direction = Directions.East;
        }
        else if (Direction == Directions.East)
        {
            transform.rotation = quaterTurn;
            Direction = Directions.North;
        }
        else if (Direction == Directions.West)
        {
            transform.rotation = quaterTurn;
            Direction = Directions.South;
        }
    }

    public void OccupyCell()
    {
        Dimension.GetCell(X, Z).GetComponent<Cell>().Occupied = true;
    }

    public void Fire(PlayerScript playerScript)
    {
        //Fire on selected cell
        Cell activeCell = playerScript.playerData.ActiveCell;
        Material cellMaterial = activeCell.GetComponent<Renderer>().material;

        if (name == "Player1")
        {
            cellMaterial.color = Color.red;
        }
        else
        {
            cellMaterial.color = Color.yellow;

        }

        playerScript.playerData.ActiveCell.Hitted = true;

        if (activeCell.Occupied)
        {
            Debug.Log("entred activeCell.Occupied");
            GameObject opponentShipObj = Dimension.GetShipOnCell(activeCell.X, activeCell.Y);
            Material shipMaterial = opponentShipObj.GetComponent<Renderer>().material;
            opponentShipObj.GetComponent<Renderer>().material = shipMaterial;

            if (name == "Player1")
            {
                shipMaterial.color = Color.red;
            }
            else
            {
                shipMaterial.color = Color.yellow;
            }
        }
    }

    private void ReleaseCell(int x, int y)
    {
        Dimension.GetCell(x, y).GetComponent<Cell>().Occupied = false;
    }

    public bool[] GetDamagedParts()
    {
        return partDamaged;
    }

    public void InitiateShip(int shipNr)
    {
        ShipName = "ship" + PartsCount.ToString();
        position = GetComponent<Transform>();
        X = shipNr;
        Direction = Directions.North;
        PartsCount = shipNr + 2;
        partDamaged = new bool[PartsCount];

        for (int i = 0; i < shipNr; i++)
        {
            partDamaged[i] = false;
        }
    }
}
