using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShipNavigator : MonoBehaviour
{
    [HideInInspector] public PlayerData player;
    [HideInInspector] public AudioPlayer audioPlayer;
    [HideInInspector] public ShipPart[] parts;
                      public Directions Orientation { get; set; }
                      public int PivotX { get; set; }
                      public int PivotZ { get; set; }

    public void Move(int deltaX, int deltaY, ShipPart[] parts, Dimension dimension)
    {
        int newX = PivotX + deltaX;
        int newY = PivotZ + deltaY;

        if (newX < OverworldData.DimensionSize && newY < OverworldData.DimensionSize && newX >= 0 && newY >= 0)
        {
            if (!CollisionCourseMove(deltaX, deltaY, parts, dimension))
            {
                UpdateShipPosition(deltaX, deltaY, parts);
            }
            else
            {
                player.HUD.WriteText("Capt'n, we are on collision course! Let the ship heave to!");
                audioPlayer.PlayClip(audioPlayer.textSource, audioPlayer.audioCollection.texts["TXT_CollisionCourse"]);
            }
        }
        else
        {
            player.HUD.WriteText("Don't let your ship run aground, Capt'n!");
            audioPlayer.PlayClip(audioPlayer.textSource, audioPlayer.audioCollection.texts["TXT_RunAground"]);
        }
    }

    private void UpdateShipPosition(int x, int y, ShipPart[] parts)
    {
        PivotX += x;
        PivotZ += y;

        foreach (ShipPart part in parts)
        {
            part.UpdateCoordinatesRelative(x, y);
        }

        transform.position += new Vector3(x, 0, y);
    }

    private bool CollisionCourseMove(int x, int y, ShipPart[] parts, Dimension dimension)
    {
        Cell cell;

        for (int i = 0; i < parts.Length; i++)
        {
            ShipPart part = parts[i];
            cell = dimension.GetCell(part.X + x, part.Y + y).GetComponent<Cell>();

            if (cell.Occupied) return true;
        }

        return false;
    }

    private bool CollisionCourseTurn(Cell[] cells)
    {
        foreach (Cell cellObj in cells)
        {
            Cell cell = cellObj.GetComponent<Cell>();

            if (cell.Occupied) return true;
        }

        return false;
    }

    public void QuaterTurn(bool clockwise, ShipPart[] parts, Dimension dimension)
    {
        Cell[] cells = new Cell[parts.Length];

        for (int i = 0; i < parts.Length; i++)
        {
            cells[i] = GetCellForPart(parts[i], clockwise, dimension);
        }

        int enumIndex = (int)Orientation;

        if (!CollisionCourseTurn(cells))
        {
            int rotation = clockwise ? 90 : -90;
            GetComponent<Transform>().Rotate(0, rotation, 0);
            enumIndex = (enumIndex + (clockwise ? 1 : 3)) % 4;
            Orientation = (Directions)enumIndex;

            for (int i = 0; i < cells.Length; i++)
            {
                Cell cell = cells[i];
                parts[i].UpdateCoordinatesAbsolute(cell.X, cell.Y);
            }
        }
        else
        {
            player.HUD.WriteText("Capt'n, we are on collision course! Let the ship heave to!");
            audioPlayer.PlayClip(audioPlayer.textSource, audioPlayer.audioCollection.texts["TXT_CollisionCourse"]);
        }
    }

    private Cell GetCellForPart(ShipPart part, bool clockwise, Dimension Dimension)
    {
        int deltaX = clockwise ? part.X - PivotX : PivotX - part.X;
        int deltaY = clockwise ? part.Y - PivotZ : PivotZ - part.Y;

        return (Orientation == Directions.North || Orientation == Directions.South)
            ? Dimension.GetCell(PivotX + deltaY, PivotZ).GetComponent<Cell>()
            : Dimension.GetCell(PivotX, PivotZ - deltaX).GetComponent<Cell>();
    }
}
