using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInitializer : MonoBehaviour
{
    public PlayerData player;
    private AudioPlayer audioPlayer;

    public ShipManager shipManager;

    [Header("Manager helpers")]
    public Navigator shipNavigator;
    public Lifter shipLifter;
    public CellOccupier occupier;
    public Activator activator;
    public Artillerist artillerist;
    public DamageHandler damageHandler;

    public void Initialize(PlayerData player, int shipNo)
    {
        this.player = player;
        this.audioPlayer = GameObject.Find("AudioManager").GetComponent<AudioPlayer>();
        InitializeShipManager(shipNo);
        InitializeShipNavigator();
        InitializeShipLifter();
        InitializeCellOccupier();
        InitializeActivator();
        InitializeArtillerist();
        InitializeDamageHandler();
    }

    private void InitializeShipManager(int shipNo)
    {
        shipManager.player = player;
        shipManager.audioPlayer = audioPlayer;

        shipManager.navigator = shipNavigator;
        shipManager.lifter = shipLifter;
        shipManager.occupier = occupier;
        shipManager.activator = activator;
        shipManager.artillerist = artillerist;
        shipManager.damageHandler = damageHandler;

        shipManager.ShipNo = shipNo;
        shipManager.parts = new ShipPartManager[shipNo + 1];
        shipManager.ShipName = $"ship{player.number}.{shipNo}";
        
        shipManager.navigator.PivotX = shipNo;

        for (int i = 0; i <= shipNo; i++)
        {
            GameObject partObj = transform.GetChild(i).gameObject;
            partObj.layer = LayerSetter.SetLayerFleet(player);
            partObj.AddComponent<ShipPartManager>().Initialize(player, i, shipManager);
            shipManager.parts[i] = partObj.GetComponent<ShipPartManager>();
        }
    }

    private void InitializeShipNavigator()
    {
        shipNavigator.player = player;
        shipNavigator.audioPlayer = audioPlayer;
        shipNavigator.parts = shipManager.parts;
        shipNavigator.Orientation = Directions.North;
    }

    private void InitializeShipLifter()
    {
        shipLifter.player = player;
        shipLifter.audioPlayer = audioPlayer;
        shipLifter.parts =  shipManager.parts;
    }

    private void InitializeCellOccupier()
    {
        occupier.parts = shipManager.parts;
    }

    private void InitializeActivator()
    {
        activator.player = player;
        activator.parts = shipManager.parts;
    }

    private void InitializeArtillerist()
    {
        artillerist.player = player;
    }

    private void InitializeDamageHandler()
    {
        damageHandler.player = player;
        damageHandler.audioPlayer = audioPlayer;
        damageHandler.ShipStatus = ShipStatus.Intact;
        damageHandler.parts = shipManager.parts;
    }
}
