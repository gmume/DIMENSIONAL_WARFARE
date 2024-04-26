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

    public bool Fire() => artillerist.Fire(this);

    public bool ShipUp() => lifter.LiftShipUp(ref dimension, No, occupier);

    public bool TakeHit(ShipPartManager part) => damageHandler.TakeHit(part, No, ref dimension, lifter, occupier);

    public void SetDimension(DimensionManager newDimension)
    {
        dimension = newDimension;
        lifter.SetDimension(dimension, No);
    }

    public List<Vector2> GetShipCoodinates()
    {
        List<Vector2> coodinates = new();

        foreach (ShipPartManager part in parts)
        {
            coodinates.Add(new(part.X, part.Y));
        }

        return coodinates;
    }
}
