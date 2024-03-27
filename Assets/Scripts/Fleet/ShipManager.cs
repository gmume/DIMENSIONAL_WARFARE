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
                      public int ShipNo { get; set; }
    [HideInInspector] public ShipPartManager[] parts;
                      public DimensionManager dimension;

    public void Activate() => activator.Activate(occupier, this);

    public void Deactivate() => activator.Deactivate(occupier);

    public void Move(Vector3 vector) => navigator.Move(vector, dimension, occupier);

    public void QuaterTurn(bool clockwise) => navigator.QuaterTurn(clockwise, dimension, occupier);

    public bool Fire() => artillerist.Fire(this, player.ActiveDimension);

    public bool ShipUp() => lifter.LiftShipUp(ref dimension, ShipNo, occupier);

    public bool TakeHit(ShipPartManager part) => damageHandler.TakeHit(part, ShipNo, ref dimension, lifter, occupier);

    public void OccupyCells() => occupier.OccupyCells();

    public void ReleaseCells() => occupier.ReleaseCells();

    public void SetDimension(DimensionManager newDimension)
    {
        dimension = newDimension;
        lifter.SetDimension(dimension, ShipNo);
    }
}
