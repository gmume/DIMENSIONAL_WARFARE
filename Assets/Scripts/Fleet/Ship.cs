using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Ship : MonoBehaviour
{
    private Player player;
    private ShipPart[] parts;

    public string ShipName { get; private set; }
    public int ShipNr { get; private set; } // 1 based
    public ShipStatus ShipStatus { get; private set; }
    public Dimension Dimension { get; set; }
    private Transform position;
    public int PivotX { get; private set; }
    public int PivotZ { get; private set; }
    public Directions Orientation { get; set; }
    public int PartsCount { get; private set; }

    public void GetStatus()
    {
        string message = ShipName + " on dimension " + Dimension.DimensionNr + "\nCoordiantes: " + PivotX + ", " + PivotZ + "\nDamaged (";

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
            message += i + ": " + parts[i].Damaged;
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

            foreach (ShipPart part in parts)
            {
                part.PartMaterial.color += new Color(0.3f, 0.3f, 0.3f);
            }

            ReleaseCells();
            Vector3 vectorUp = new(0f, 0.1f, 0f);
            GetComponent<Transform>().position += vectorUp;
            player.ActiveShip = this;
        }
    }

    public void Deactivate()
    {
        foreach (ShipPart part in parts)
        {
            part.PartMaterial.color -= new Color(0.3f, 0.3f, 0.3f);
        }

        OccupyCells();
        Vector3 vectorDown = new(0f, -0.1f, 0f);
        GetComponent<Transform>().position += vectorDown;
        player.ActiveShip = null;
    }

    public void Move(int x, int y)
    {
        if (PivotX + x < OverworldData.DimensionSize && PivotZ + y < OverworldData.DimensionSize && PivotX + x >= 0 && PivotZ + y >= 0)
        {
            if (!CollisionCourseMove(x, y))
            {
                PivotX += x;
                PivotZ += y;

                foreach (ShipPart part in parts)
                {
                    part.UpdateCoordinatesRelative(x, y);
                }

                position.position += new Vector3(x, 0, y);
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

    private bool CollisionCourseMove(int x, int y)
    {
        Cell cell;

        for (int i = 0; i < parts.Length; i++)
        {
            ShipPart part = parts[i];
            cell = Dimension.GetCell(part.X + x, part.Y + y).GetComponent<Cell>();

            if (cell.Occupied)
            {
                return true;
            }
        }

        return false;
    }

    private bool CollisionCourseTurn(Cell[] cells)
    {
        foreach (Cell cellObj in cells)
        {
            Cell cell = cellObj.GetComponent<Cell>();

            if (cell.Occupied)
            {
                return true;
            }
        }

        return false;
    }

    public void QuaterTurn(bool clockwise)
    {
        Cell[] cells = new Cell[parts.Length];
        Cell cell;
        int x, y;

        for (int i = 0; i < parts.Length; i++)
        {
            ShipPart part = parts[i];

            if (clockwise)
            {
                x = part.X - PivotX;
                y = part.Y - PivotZ;
            }
            else
            {
                x = PivotX - part.X;
                y = PivotZ - part.Y;
            }

            if (Orientation == Directions.North || Orientation == Directions.South)
            {
                cell = Dimension.GetCell(PivotX + y, PivotZ).GetComponent<Cell>();
            }
            else
            {
                cell = Dimension.GetCell(PivotX, PivotZ - x).GetComponent<Cell>();
            }

            cells[i] = cell;
        }

        int enumIndex = (int)Orientation;

        if (!CollisionCourseTurn(cells))
        {
            if (clockwise)
            {
                GetComponent<Transform>().Rotate(0, 90, 0);
                enumIndex = ++enumIndex % 4;
            }
            else
            {
                GetComponent<Transform>().Rotate(0, -90, 0);
                enumIndex = (enumIndex + 4 - 1) % 4 ;
            }

            Orientation = (Directions)enumIndex;

            for (int i = 0; i < cells.Length; i++)
            {
                cell = cells[i];
                parts[i].UpdateCoordinatesAbsolute(cell.X, cell.Y);
            }
        }
        else
        {
            print("Capt'n, we are on collision course! Let the ship heave to!");
        }

        for (int i = 0;i < Dimension.cells.Length; i++)
        {
            for (int j = 0; j < Dimension.cells.Length; j++)
            {
                cell = Dimension.cells[i][j].GetComponent<Cell>();
                if (cell.Occupied)
                {
                    Debug.Log("cell "+cell.X+", "+cell.Y);
                }
            }
        }
    }

    public bool Fire()
    {
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
                return true;
            }
        }

        return false;
    }

    private void ShipUp()
    {
        int dimensionNr = Dimension.DimensionNr + 1;

        if (dimensionNr < OverworldData.DimensionsCount)
        {
            SwitchDimension(dimensionNr, "up");
            this.gameObject.transform.position += new Vector3(0, OverworldData.DimensionSize * dimensionNr * 2, 0);
            player.vehicle.OnDimensionUp();
            player.input.SwitchCurrentActionMap("GameStart");
            print("Hide your ship!");
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
        ReleaseCells();
        Dimension.RemoveShip(this.gameObject);
        Dimension = player.dimensions.GetDimension(dimensionNr);
        OccupyCells();
    }

    public bool TakeHit(int x, int y)
    {
        x += 1;
        y += 1;

        if (this.gameObject.layer != LayerMask.NameToLayer("VisibleShips"))
        {
            this.gameObject.layer = LayerMask.NameToLayer("VisibleShips");
        }

        ShipPart part = parts[((x % (PivotX + 1) + 1) % ((y % (PivotZ + 1)) + 1))];
        part.Damaged = true;
        part.PartMaterial.color += new Color(0.3f, 0, 0);

        if (part.gameObject.layer != LayerMask.NameToLayer("VisibleShips"))
        {
            part.gameObject.layer = LayerMask.NameToLayer("VisibleShips");
        }

        if (Sunk())
        {
            ShipStatus = ShipStatus.Sunk;

            foreach (ShipPart shipPart in parts)
            {
                shipPart.PartMaterial.color = Color.black;
            }

            gameObject.transform.position += new Vector3(0, -0.5f, 0);
            player.fleet.GetFleet().Remove(this);
            GameObject[] shipButtons = player.fleetMenu.GetShipButtons();
            shipButtons[PartsCount - 1].GetComponent<Button>().interactable = false;

            if (!FleetDestroyed())
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
            else
            {
                print(player.opponent.name + "won!");
                // resolve game
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

        foreach (ShipPart part in parts)
        {
            if (part.Damaged)
            {
                damagedParts++;
            }
        }

        if (damagedParts == parts.Length)
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

    public void OccupyCells()
    {
        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].OccupyCell();
        }
    }

    public void ReleaseCells()
    {
        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].ReleaseCell(player);
        }
    }

    public void SetDimension(Dimension dimension)
    {
        Dimension = dimension;
        gameObject.transform.parent = dimension.transform;

        for (int i = 0; i <= ShipNr; i++)
        {
            ShipPart part = parts[i];
            part.Dimension = Dimension;
            part.transform.parent = gameObject.transform;
        }
    }

    public void InitiateShip(Player player, int shipNr)
    {
        this.player = player;
        ShipNr = shipNr;
        parts = new ShipPart[ShipNr + 1];
        ShipName = "ship" + player.number + "." + ShipNr;
        ShipStatus = ShipStatus.Intact;
        position = GetComponent<Transform>();
        PivotX = ShipNr;
        Orientation = Directions.North;
        PartsCount = ShipNr + 1;

        for (int i = 0; i <= ShipNr; i++)
        {
            GameObject partObj;
            partObj = gameObject.transform.GetChild(i).gameObject;
            partObj.layer = Layer.SetLayerFleet(player);
            partObj.AddComponent<ShipPart>().InitiateShipPart(player, i, this);
            parts[i] = partObj.GetComponent<ShipPart>();
        }
    }
}
