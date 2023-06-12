using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    private Player player;
    private bool[] partDamaged;

    public string ShipName { get; private set; }
    public Dimension Dimension { get; set; }
    private Transform position;
    public int X { get; private set; }
    public int Z { get; private set; }
    public Directions Direction { get; set; }
    public int PartsCount { get; private set; }

    public void GetStatus()
    {
        string message = ShipName + " on dimension " + Dimension.DimensionNr + "\nCoordiantes: " + X + ", " + Z + "\nDamaged (";

        for (int i = 0; i < PartsCount; i++)
        {
            if (i == 0)
            {
                message += "part ";
            }
            else
            {
                message += ", part ";
            }
            message += i + ": " + partDamaged[i];
        }

        Debug.Log(message + ")");
    }

    public void Activate()
    {
        if (player.ActiveShip != this)
        {
            if (player.ActiveShip)
            {
                player.ActiveShip.Deactivate();
            }
            GetComponent<Renderer>().material.color = Color.black;
            Vector3 vectorUp = new(0f, 0.1f, 0f);
            GetComponent<Transform>().position += vectorUp;
            //ReleaseCell();
            player.ActiveShip = this;
        }
    }

    public void Deactivate()
    {
        if (player.number == 1)
        {
            GetComponent<Renderer>().material.color = new Color(0.8f, 0.5f, 0.5f, 1);
        }
        else
        {
            GetComponent<Renderer>().material.color = new Color(0.8f, 0.7f, 0.5f, 1);
        }

        Vector3 vectorDown = new(0f, -0.1f, 0f);
        GetComponent<Transform>().position += vectorDown;
        player.ActiveShip = null;
    }

    public void Move(int x, int y)
    {
        if (X + x < OverworldData.DimensionSize && Z + y < OverworldData.DimensionSize && X + x >= 0 && Z + y >= 0)
        {
            ReleaseCell();

            if (x == 0)
            {
                Z += y;
            }
            else
            {
                X += x;
            }

            position.position += new Vector3(x, 0, y);
            OccupyCell();
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

    public void Fire()
    {
        //Fire on selected cell
        Cell activeCell = player.ActiveCell;
        Material cellMaterial = activeCell.GetComponent<Renderer>().material;

        if (player.name == "Player1")
        {
            cellMaterial.color = Color.red;
        }
        else
        {
            cellMaterial.color = Color.yellow;
        }

        activeCell.Hitted = true;

        if (activeCell.Occupied)
        {
            GameObject opponentShipObj = Dimension.GetShipOnCell(activeCell.X, activeCell.Y);
            Ship opponentShip = opponentShipObj.GetComponent<Ship>();
            opponentShip.TakeHit(activeCell.X, activeCell.Y);

            Material shipMaterial = opponentShipObj.GetComponent<Renderer>().material;
            opponentShipObj.GetComponent<Renderer>().material = shipMaterial;

            if (player.number == 1)
            {
                shipMaterial.color = Color.red;
            }
            else
            {
                shipMaterial.color = Color.yellow;
            }
        }
    }

    public void TakeHit(int x, int y)
    {
        x += 1;
        y += 1;

        int part = ((x % (X + 1) + 1) % ((y % (Z + 1)) + 1));
        partDamaged[part] = true;
    }

    public void OccupyCell()
    {
        Dimension.GetCell(X, Z).GetComponent<Cell>().Occupied = true;
    }

    public void ReleaseCell()
    {
        Dimension.GetCell(X, Z).GetComponent<Cell>().Occupied = false;
    }

    public bool[] GetDamagedParts()
    {
        return partDamaged;
    }

    public void InitiateShip(Player player, int shipNr)
    {
        this.player = player;
        ShipName = "ship" + shipNr;
        position = GetComponent<Transform>();
        X = shipNr;
        Direction = Directions.North;
        PartsCount = shipNr + 1;
        partDamaged = new bool[PartsCount];

        for (int i = 0; i < shipNr; i++)
        {
            partDamaged[i] = false;
        }
    }
}
