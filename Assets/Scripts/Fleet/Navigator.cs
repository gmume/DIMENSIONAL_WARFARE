using System;
using UnityEngine;

public class Navigator : MonoBehaviour
{
    [HideInInspector] public PlayerData player;
    [HideInInspector] public AudioPlayer audioPlayer;
    [HideInInspector] public ShipManager manager;
                      public Directions Orientation { get; set; }
                      public int PivotX { get; set; }
                      public int PivotZ { get; set; }

    public void Move(Vector3 vector, DimensionManager dimension, CellOccupier occupier)
    {
        int[] deltaXY = GetAxis(vector.x, vector.y);
        int deltaX = deltaXY[0], deltaY = deltaXY[1];
        int newX = PivotX + deltaX, newY = PivotZ + deltaY;

        player.dimensions.ReleaseCells(player.dimensions.GetCellGroup(manager.GetShipCoordinates(), manager.dimension.No));

        if (!IsValidPosition(newX, newY))
        {
            Warn("Don't let your ship run aground, Capt'n!", "TXT_RunAground");
            return;
        }
        else if(CollisionCourseMove(deltaX, deltaY, dimension))
        {
            Warn("Capt'n, we are on collision course! Let the ship heave to!", "TXT_CollisionCourse");
            player.dimensions.OccupyCells(TupleListProvider.GetTuplesList(manager.dimension, manager.GetShipCoordinates(), manager.partsList));
            return;
        }
        
        UpdateShipPosition(deltaX, deltaY);
        player.dimensions.OccupyCells(TupleListProvider.GetTuplesList(manager.dimension, manager.GetShipCoordinates(), manager.partsList));
    }

    private int[] GetAxis(float deltaX, float deltaY)
    {
        // Returns the sign (Vorzeichen) of x. The Mathf.Sign function returns -1 for negative values, 1 for positive values, and 0 for zero.
        return (Math.Abs(deltaX) > Math.Abs(deltaY)) ? new int[] { (int)Mathf.Sign(deltaX), 0 } : new int[] { 0, (int)Mathf.Sign(deltaY) };
    }

    private bool IsValidPosition(int newX, int newY) => newX >= 0 && newX < OverworldData.DimensionSize && newY >= 0 && newY < OverworldData.DimensionSize;

    private void UpdateShipPosition(int x, int y)
    {
        PivotX += x;
        PivotZ += y;

        foreach (ShipPartManager part in manager.parts)
        {
            part.UpdateCoordinatesRelative(x, y);
        }

        transform.position += new Vector3(x, 0, y);
    }

    private bool CollisionCourseMove(int x, int y, DimensionManager dimension)
    {
        ShipPartManager part;

        for (int i = 0; i < manager.parts.Length; i++)
        {
            part = manager.parts[i];
            if (dimension.GetCell(part.X + x, part.Y + y).GetComponent<CellData>().Occupied) return true;
        }

        return false;
    }

    public void QuaterTurn(bool clockwise, DimensionManager dimension, CellOccupier occupier)
    {
        CellData[] cells = new CellData[manager.parts.Length];
        int enumIndex = (int)Orientation;

        for (int i = 0; i < manager.parts.Length; i++)
        {
            cells[i] = GetCellForPart(manager.parts[i], clockwise, dimension);
        }

        player.dimensions.ReleaseCells(player.dimensions.GetCellGroup(manager.GetShipCoordinates(), manager.dimension.No));

        if (CollisionCourseTurn(cells))
        {
            Warn("Capt'n, we are on collision course! Let the ship heave to!", "TXT_CollisionCourse");
            player.dimensions.OccupyCells(TupleListProvider.GetTuplesList(manager.dimension, manager.GetShipCoordinates(), manager.partsList));
            return;
        }

        int rotation = clockwise ? 90 : -90;
        GetComponent<Transform>().Rotate(0, rotation, 0);
        enumIndex = (enumIndex + (clockwise ? 1 : 3)) % 4;
        Orientation = (Directions)enumIndex;

        for (int i = 0; i < cells.Length; i++)
        {
            CellData cell = cells[i];
            manager.parts[i].UpdateCoordinatesAbsolute(cell.X, cell.Y);
        }

        player.dimensions.OccupyCells(TupleListProvider.GetTuplesList(manager.dimension, manager.GetShipCoordinates(), manager.partsList));
    }

    private bool CollisionCourseTurn(CellData[] cells)
    {
        foreach (CellData cellObj in cells)
        {
            if (cellObj.GetComponent<CellData>().Occupied) return true;
        }

        return false;
    }

    private void Warn(string text, string audioFileName)
    {
        player.HUD.WriteText(text);
        audioPlayer.PlayClip(audioPlayer.textSource, audioPlayer.audioCollection.texts[audioFileName]);
    }

    private CellData GetCellForPart(ShipPartManager part, bool clockwise, DimensionManager Dimension)
    {
        int deltaX = clockwise ? part.X - PivotX : PivotX - part.X;
        int deltaY = clockwise ? part.Y - PivotZ : PivotZ - part.Y;

        return (Orientation == Directions.North || Orientation == Directions.South)
            ? Dimension.GetCell(PivotX + deltaY, PivotZ).GetComponent<CellData>()
            : Dimension.GetCell(PivotX, PivotZ - deltaX).GetComponent<CellData>();
    }
}
