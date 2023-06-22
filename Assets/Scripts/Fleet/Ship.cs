using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public Material ShipMaterial { get; set; }

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

        //Debug.Log(message + ")");
    }

    public void Activate()
    {
        if (player.ActiveShip != this)
        {
            if (player.ActiveShip)
            {
                player.ActiveShip.Deactivate();
            }

            ShipMaterial.color += new Color(0.3f, 0.3f, 0.3f);
            Vector3 vectorUp = new(0f, 0.1f, 0f);
            GetComponent<Transform>().position += vectorUp;
            player.ActiveShip = this;
        }
    }

    public void Deactivate()
    {
        ShipMaterial.color -= new Color(0.3f, 0.3f, 0.3f);
        Vector3 vectorDown = new(0f, -0.1f, 0f);
        GetComponent<Transform>().position += vectorDown;
        player.ActiveShip = null;
    }

    public void Move(int x, int y)
    {
        if (X + x < OverworldData.DimensionSize && Z + y < OverworldData.DimensionSize && X + x >= 0 && Z + y >= 0)
        {
            if (!CollisionCourse(x, y))
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
                print("Capt'n, we are on collision course! Let the ship heave to!");
            }
        }
        else
        {
            print("Don't let your ship run aground, capt'n!");
        }
    }

    private bool CollisionCourse(int x, int y)
    {
        int xPos = X + x;
        int yPos = Z + y;
        Cell cell = player.opponent.dimensions.GetDimension(Dimension.DimensionNr).GetCell(xPos, yPos).GetComponent<Cell>();
        
        if(cell.Occupied)
        {
            return true;
        }

        return false;
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

        Dimension opponentDimension = player.opponent.dimensions.GetDimension(Dimension.DimensionNr);
        Debug.Log("opponentDimension: " + opponentDimension);
        Cell opponentCell = opponentDimension.GetCell(activeCell.X, activeCell.Y).GetComponent<Cell>();
        Debug.Log("opponentCell: " + opponentCell.Occupied);

        if (opponentCell.Occupied)
        {
            Debug.Log("opponentCell.Occupied");
            GameObject opponentShipObj = opponentDimension.GetShipOnCell(activeCell.X, activeCell.Y);
            Ship opponentShip = opponentShipObj.GetComponent<Ship>();
            opponentShip.TakeHit(activeCell.X, activeCell.Y);
        }
    }

    public void TakeHit(int x, int y)
    {
        Debug.Log("entred TakeHit");
        x += 1;
        y += 1;

        int part = ((x % (X + 1) + 1) % ((y % (Z + 1)) + 1));
        partDamaged[part] = true;
        ShipMaterial.color += new Color(0.3f, 0, 0);

        if (this.gameObject.layer != LayerMask.NameToLayer("VisibleShips"))
        {
            this.gameObject.layer = LayerMask.NameToLayer("VisibleShips");
        }

        //SunkCheck coming
    }

    private bool SunkCheck()
    {
        int damagedParts = 0;

        foreach (var part in partDamaged)
        {
            if (part == true)
            {
                damagedParts++;
            }
        }

        if (damagedParts == partDamaged.Length)
        {
            return true;
        }

        return false;
    }

    public void OccupyCell()
    {
        Debug.Log("cell: " + Dimension.GetCell(X, Z).GetComponent<Cell>().X + ", " + Dimension.GetCell(X, Z).GetComponent<Cell>().Y);
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
        X = shipNr - 1;
        Direction = Directions.North;
        PartsCount = shipNr + 1;
        partDamaged = new bool[PartsCount];

        for (int i = 0; i < shipNr; i++)
        {
            partDamaged[i] = false;
        }
    }
}
