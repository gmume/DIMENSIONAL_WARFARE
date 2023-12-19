using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Navigator : MonoBehaviour
{
    [HideInInspector] public PlayerData player;
    [HideInInspector] public AudioPlayer audioPlayer;
    [HideInInspector] public ShipPartManager[] parts;
                      public Directions Orientation { get; set; }
                      public int PivotX { get; set; }
                      public int PivotZ { get; set; }

    public void Move(int deltaX, int deltaY, DimensionManager dimension)
    {
        int newX = PivotX + deltaX;
        int newY = PivotZ + deltaY;

        if (newX < OverworldData.DimensionSize && newY < OverworldData.DimensionSize && newX >= 0 && newY >= 0)
        {
            if (!CollisionCourseMove(deltaX, deltaY, dimension))
            {
                UpdateShipPosition(deltaX, deltaY);
            }
            else
            {
                Warn("Capt'n, we are on collision course! Let the ship heave to!", "TXT_CollisionCourse");
            }
        }
        else
        {
            Warn("Don't let your ship run aground, Capt'n!", "TXT_RunAground");
        }
    }

    private void UpdateShipPosition(int x, int y)
    {
        PivotX += x;
        PivotZ += y;

        foreach (ShipPartManager part in parts)
        {
            part.UpdateCoordinatesRelative(x, y);
        }

        transform.position += new Vector3(x, 0, y);
    }

    private bool CollisionCourseMove(int x, int y, DimensionManager dimension)
    {
        for (int i = 0; i < parts.Length; i++)
        {
            ShipPartManager part = parts[i];
            if (dimension.GetCell(part.X + x, part.Y + y).GetComponent<CellData>().Occupied) return true;
        }

        return false;
    }

    private bool CollisionCourseTurn(CellData[] cells)
    {
        foreach (CellData cellObj in cells)
        {
            if (cellObj.GetComponent<CellData>().Occupied) return true;
        }

        return false;
    }

    public void QuaterTurn(bool clockwise, DimensionManager dimension)
    {
        CellData[] cells = new CellData[parts.Length];
        int enumIndex = (int)Orientation;

        for (int i = 0; i < parts.Length; i++)
        {
            cells[i] = GetCellForPart(parts[i], clockwise, dimension);
        }

        if (!CollisionCourseTurn(cells))
        {
            int rotation = clockwise ? 90 : -90;
            GetComponent<Transform>().Rotate(0, rotation, 0);
            enumIndex = (enumIndex + (clockwise ? 1 : 3)) % 4;
            Orientation = (Directions)enumIndex;

            for (int i = 0; i < cells.Length; i++)
            {
                CellData cell = cells[i];
                parts[i].UpdateCoordinatesAbsolute(cell.X, cell.Y);
            }
        }
        else
        {
            Warn("Capt'n, we are on collision course! Let the ship heave to!", "TXT_CollisionCourse");
        }
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
