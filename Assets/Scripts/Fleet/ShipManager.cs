using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour
{
    public PlayerData player;
    public AudioPlayer audioPlayer;
    public Navigator navigator;
    public Lifter lifter;
    public CellOccupier occupier;
    public Activator activator;
    public Artillerist artillerist;
    public DamageHandler damageHandler;

    public string ShipName { get; set; }
    public int No { get; set; }
    public ShipPartManager[] parts;
    public List<GameObject> partsList = new();
    public DimensionManager dimension;

    public HUD_ButtonPartsHandler[] HUD_buttonPartsHandler;

    public void Activate() => activator.Activate(this);

    public void Deactivate() => activator.Deactivate();

    public void Move(Vector3 vector) => navigator.Move(vector, dimension);

    public void QuaterTurn(bool clockwise) => navigator.QuaterTurn(clockwise, dimension, occupier);

    public List<Vector2> GetFocusedCoordinates(CellData focusedCell) => artillerist.GetCellCoordinates(focusedCell);

    public string Fire() => artillerist.Fire(this);

    public string ShipUp() => lifter.LiftShipUp(ref dimension, No);

    public bool Sunk() => damageHandler.Sunk();

    //public void TakeHit(ShipPartManager part)
    //{
    //    HUD_buttonPartsHandler[No].ButtonPartTakeHit(part.partNo);
    //    damageHandler.TakeHit(part, No, ref dimension, lifter);
    //}

    //public bool TakeHit(ShipPartManager part)
    //{
    //    HUD_buttonPartsHandler[No].ButtonPartTakeHit(part.partNo);
    //    return damageHandler.TakeHit(part, No, ref dimension, lifter);
    //}

    public void SetDimension(DimensionManager newDimension) => lifter.SetDimension(newDimension, No);

    public List<Vector2> GetShipCoordinates()
    {
        List<Vector2> coodinates = new();

        foreach (ShipPartManager part in parts)
        {
            coodinates.Add(new(part.X, part.Y));
        }

        return coodinates;
    }

    public void SetShipCoordinates(List<Vector2> coordinates)
    {
        for (int i = 0; i < coordinates.Count; i++)
        {
            parts[i].UpdateCoordinatesAbsolute((int)coordinates[i].x, (int)coordinates[i].y);

            if (i == 0)
            {
                navigator.PivotX = parts[i].X;
                navigator.PivotZ = parts[i].Y;
            }
        }
    }
}
