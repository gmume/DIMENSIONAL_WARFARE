using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
                      public int ShipNo { get; set; } // 1 based
    [HideInInspector] public ShipPartManager[] parts;
                      public DimensionManager dimension;

    public void Activate()
    {
        activator.Activate(occupier, this);
    }

    public void Deactivate()
    {
        activator.Deactivate(occupier);
    }

    public void Move(int deltaX, int deltaY)
    {
        navigator.Move(deltaX, deltaY, dimension);
    }

    public void QuaterTurn(bool clockwise)
    {
        navigator.QuaterTurn(clockwise, dimension);
    }

    public bool Fire()
    {
        return artillerist.Fire(this, dimension);
    }

    public void ShipUp()
    {
        lifter.LiftShipUp(ref dimension, ShipNo, occupier);
    }

    public bool TakeHit(ShipPartManager part)
    {
        return damageHandler.TakeHit(part, ShipNo, dimension, lifter, occupier);
    }

    public void OccupyCells()
    {
        occupier.OccupyCells();
    }

    public void ReleaseCells()
    {
        occupier.ReleaseCells();
    }

    public void SetDimension(DimensionManager newDimension)
    {
        dimension = newDimension;
        lifter.SetDimension(dimension, ShipNo);
    }
}
