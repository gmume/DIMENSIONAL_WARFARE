using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class Ship : MonoBehaviour
{
    private Player player;
    private bool[] partDamaged;

    public string ShipName { get; private set; }
    public ShipStatus ShipStatus { get; private set; }
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

        if (cell.Occupied)
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
        Cell opponentCell = opponentDimension.GetCell(activeCell.X, activeCell.Y).GetComponent<Cell>();

        if (opponentCell.Occupied)
        {
            bool opponentSunk;

            GameObject opponentShipObj = opponentDimension.GetShipOnCell(activeCell.X, activeCell.Y);
            Ship opponentShip = opponentShipObj.GetComponent<Ship>();
            opponentSunk = opponentShip.TakeHit(activeCell.X, activeCell.Y);

            if (opponentSunk)
            {
                ShipUp();
            }
        }
    }

    private void ShipUp()
    {
        int dimensionNr = Dimension.DimensionNr + 1;

        if (dimensionNr < OverworldData.DimensionsCount)
        {
            SwitchDimension(dimensionNr, "up");
            this.gameObject.transform.position += new Vector3(0, OverworldData.DimensionSize * dimensionNr * 2, 0);
            player.vehicle.OnDimensionUp();


        }
        else
        {
            print(player.name + "won!");
            // resolve game
        }
    }

    private void ShipDown()
    {
        int dimensionNr = Dimension.DimensionNr - 1;

        SwitchDimension(dimensionNr, "down");
        this.gameObject.transform.position += new Vector3(0, -OverworldData.DimensionSize * dimensionNr * 2, 0);
        player.vehicle.OnDimensionDown();
    }

    private void SwitchDimension(int dimensionNr, string upOrDown)
    {
        ReleaseCell();
        Dimension.RemoveShip(this.gameObject);
        Dimension = player.dimensions.GetDimension(dimensionNr);
        OccupyCell();
    }

    public bool TakeHit(int x, int y)
    {
        x += 1;
        y += 1;

        int part = ((x % (X + 1) + 1) % ((y % (Z + 1)) + 1));
        partDamaged[part] = true;
        ShipMaterial.color += new Color(0.3f, 0, 0);

        if (this.gameObject.layer != LayerMask.NameToLayer("VisibleShips"))
        {
            this.gameObject.layer = LayerMask.NameToLayer("VisibleShips");
        }

        if (Sunk())
        {
            ShipStatus = ShipStatus.Sunk;
            ShipMaterial.color = Color.black;
            gameObject.transform.position += new Vector3(0, -0.5f, 0);
            player.fleet.GetFleet().Remove(this);
            GameObject[] shipButtons = player.fleetMenu.GetShipButtons();
            shipButtons[PartsCount - 1].GetComponent<Button>().interactable = false;

            if (FleetDestroyed())
            {
                print(player.opponent.name + "won!");
                // resolve game
            }
            else
            {
                if (Dimension.DimensionNr != 0)
                {
                    ShipDown();
                }
                else
                {
                    StartCoroutine(DestroyShip());
                }
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator DestroyShip()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1f;
        this.gameObject.SetActive(false);
    }

    private bool Sunk()
    {
        int damagedParts = 0;

        foreach (bool part in partDamaged)
        {
            if (part)
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

    private bool FleetDestroyed()
    {
        foreach (GameObject shipObj in player.fleet.GetFleet())
        {
            if (shipObj.GetComponent<Ship>().ShipStatus == ShipStatus.Intact)
            {
                return false;
            }
        }

        return true;
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
        ShipStatus = ShipStatus.Intact;
        position = GetComponent<Transform>();
        X = shipNr - 1;
        Direction = Directions.North;
        PartsCount = shipNr;
        partDamaged = new bool[PartsCount];

        for (int i = 0; i < shipNr; i++)
        {
            partDamaged[i] = false;
        }
    }
}
