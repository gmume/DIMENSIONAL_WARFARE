using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class ShipManager : MonoBehaviour
{
    [HideInInspector] public PlayerData player;
    [HideInInspector] public AudioPlayer audioPlayer;
    [HideInInspector] public Navigator navigator;
    [HideInInspector] public Lifter lifter;
    [HideInInspector] public CellOccupier occupier;
    [HideInInspector] public Activator activator;
    [HideInInspector] public Artillerist artillerist;
    [HideInInspector] public DamageHandler damageHandler;

                      public string ShipName { get; set; }
                      public int No { get; set; }
    [HideInInspector] public ShipPartManager[] parts;
                      public List<GameObject> partsList = new();
                      public DimensionManager dimension;

    public void Activate() => activator.Activate(occupier, this);

    public void Deactivate() => activator.Deactivate(occupier);

    public void Move(Vector3 vector) => navigator.Move(vector, dimension, occupier);

    public void QuaterTurn(bool clockwise) => navigator.QuaterTurn(clockwise, dimension, occupier);

    public List<Vector2> GetFocusedCoordinates(CellData focusedCell) => artillerist.GetCellCoordinates(focusedCell);

    public bool Fire() => artillerist.Fire(this);

    public bool ShipUp() => lifter.LiftShipUp(ref dimension, No, occupier);

    public bool TakeHit(ShipPartManager part) => damageHandler.TakeHit(part, No, ref dimension, lifter);

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
