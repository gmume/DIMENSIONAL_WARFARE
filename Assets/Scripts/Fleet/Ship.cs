using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ship : MonoBehaviour
{
    private Player player;

    private AudioPlayer audioPlayer;
    
    public string ShipName { get; private set; }
    public int ShipNo { get; private set; } // 1 based
    public ShipStatus ShipStatus { get; private set; }
    private ShipPart[] parts;
    private Color fireColor;
    private Transform position;
    public Directions Orientation { get; set; }
    public int PartsCount { get; private set; }
    private string visibleShip;

    public Dimension Dimension { get; set; }
    public int PivotX { get; private set; }
    public int PivotZ { get; private set; }

    public void GetStatus()
    {
        string message = ShipName + " on dimension " + Dimension.DimensionNo + "\nCoordiantes: " + PivotX + ", " + PivotZ + "\nDamaged (";

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
                player.HUD.WriteText("Capt'n, we are on collision course! Let the ship heave to!");
                print("Capt'n, we are on collision course! Let the ship heave to!");
            }
        }
        else
        {
            player.HUD.WriteText("Don't let your ship run aground, capt'n!");
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
                enumIndex = (enumIndex + 4 - 1) % 4;
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
            player.HUD.WriteText("Capt'n, we are on collision course! Let the ship heave to!");
            print("Capt'n, we are on collision course! Let the ship heave to!");
        }
    }

    public bool Fire()
    {
        Cell activeCell = player.ActiveCell;
        Material cellMaterial = activeCell.GetComponent<Renderer>().material;
        cellMaterial.color = fireColor;
        activeCell.Hitted = true;

        Dimension opponentDimension = player.opponent.dimensions.GetDimension(Dimension.DimensionNo);
        Cell opponentCell = opponentDimension.GetCell(activeCell.X, activeCell.Y).GetComponent<Cell>();

        if (opponentCell.Occupied)
        {
            bool opponentSunk;
            ShipPart part = opponentCell.Part;
            
            Ship opponentShip = part.GetComponentInParent<Ship>();
            opponentSunk = opponentShip.TakeHit(part);

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
        if (Dimension.DimensionNo < OverworldData.DimensionsCount - 2)
        {
            audioPlayer.OnShipUp();
            player.world.SetNewDimension(Dimension.DimensionNo + 1);
            player.HUD.SetHUDDimension(player.ActiveDimension.DimensionNo);
            player.vehicle.SetViewOnDimension(player.ActiveDimension.DimensionNo);
            SwitchDimension(player.ActiveDimension.DimensionNo);
            SetDimension(Dimension);
            gameObject.transform.position += new Vector3(0, OverworldData.DimensionSize * 2, 0);
            player.input.SwitchCurrentActionMap("GameStart");

            player.HUD.WriteText("Capt'n " + player.number + " hide your ship!");
            print(player.name + " hide your ship!");
        }
        else
        {
            audioPlayer.OnVictory();
            player.HUD.WriteText("Capt'n " + player.number + " wins for reaching the top dimension!");
            print(player.name + " wins for reaching the top dimension!");
            GameData.winner = player.name;
            player.GetComponent<SceneChanger>().LoadResolveGame();
        }
    }

    private void SwitchDimension(int dimensionNr)
    {
        ReleaseCells();
        Dimension.RemoveShip(gameObject);
        Dimension = player.dimensions.GetDimension(dimensionNr);
        OccupyCells();
    }

    public bool TakeHit(ShipPart part)
    {
        part.Damaged = true;
        part.PartMaterial.color += new Color(0.3f, 0, 0);

        if (!gameObject.layer.Equals(visibleShip))
        {
            gameObject.layer = LayerMask.NameToLayer(visibleShip);
        }

        if (!part.gameObject.layer.Equals(visibleShip))
        {
            part.gameObject.layer = LayerMask.NameToLayer(visibleShip);
        }

        if (Sunk())
        {
            ShipStatus = ShipStatus.Sunk;
            gameObject.transform.position -= new Vector3(0, 0.5f, 0);

            foreach (ShipPart shipPart in parts)
            {
                shipPart.PartMaterial.color = Color.black;
            }

            if (Dimension.DimensionNo != 0)
            {
                ShipDown();
            }
            else
            {
                if (!FleetDestroyed())
                {
                    List<GameObject> fleet = player.fleet.GetFleet();

                    player.HUD.RemoveButton(fleet.IndexOf(gameObject));
                    fleet.Remove(gameObject);
                    StartCoroutine(DestroyShip());
                }
                else
                {
                    audioPlayer.OnVictory();
                    player.HUD.WriteText("Capt'n " + player.opponent.number + " wins for destroying the opponent's fleet!");
                    print(player.opponent.name + " wins for destroying the opponent's fleet!");
                    GameData.winner = player.opponent.name;
                    player.GetComponent<SceneChanger>().LoadResolveGame();
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
        
        ReleaseCells();
        Destroy(gameObject);
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

    private void ShipDown()
    {
            player.world.SetNewDimension(Dimension.DimensionNo - 1);
            player.HUD.SetHUDDimension(player.ActiveDimension.DimensionNo);
            player.vehicle.SetViewOnDimension(player.ActiveDimension.DimensionNo);
            ShipStatus = ShipStatus.Sunk;
            gameObject.transform.position += new Vector3(0, 0.5f, 0);
            gameObject.layer = LayerMask.NameToLayer("Fleet" + player.number);
            gameObject.transform.position -= new Vector3(0, OverworldData.DimensionSize * 2, 0);
            SwitchDimension(Dimension.DimensionNo - 1);
            SetDimension(Dimension);
            player.inputHandling.continueGame = false;
            StartCoroutine(ResetShip());
    }

    private IEnumerator ResetShip()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1f;

        foreach (ShipPart part in parts)
        {
            part.ResetPart();
        }

        ShipStatus = ShipStatus.Intact;
        player.input.SwitchCurrentActionMap("GameStart");
        player.HUD.WriteText("Capt'n " + player.number + " hide your ship!");
        print(player.name + " hide your ship!");
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
            parts[i].ReleaseCell();
        }
    }

    public void SetDimension(Dimension dimension)
    {
        Dimension = dimension;
        gameObject.transform.parent = Dimension.transform;

        for (int i = 0; i <= ShipNo; i++)
        {
            ShipPart part = parts[i];
            part.Dimension = Dimension;
            part.transform.parent = gameObject.transform;
        }
    }

    public void InitiateShip(Player player, int shipNo)
    {
        this.player = player;
        audioPlayer = player.audioManager.GetComponent<AudioPlayer>();
        ShipNo = shipNo;
        parts = new ShipPart[ShipNo + 1];
        ShipName = "ship" + player.number + "." + ShipNo;
        ShipStatus = ShipStatus.Intact;
        position = GetComponent<Transform>();
        PivotX = ShipNo;
        Orientation = Directions.North;
        PartsCount = ShipNo + 1;

        for (int i = 0; i <= ShipNo; i++)
        {
            GameObject partObj;
            partObj = gameObject.transform.GetChild(i).gameObject;
            partObj.layer = Layer.SetLayerFleet(player);
            partObj.AddComponent<ShipPart>().InitiateShipPart(player, i, this);
            parts[i] = partObj.GetComponent<ShipPart>();
        }

        if (player.number == 1)
        {
            fireColor = Color.red;
            visibleShip = "VisibleShips1";
        }
        else
        {
            fireColor = Color.yellow;
            visibleShip = "VisibleShips2";
        }
    }
}
