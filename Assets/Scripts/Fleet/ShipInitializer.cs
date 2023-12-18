using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInitializer : MonoBehaviour
{
    public PlayerData player;

    private AudioPlayer audioPlayer;

    public ShipManager shipManager;
    public ShipNavigator shipNavigator;
    public ShipLifter shipLifter;
    public CellOccupier occupier;

    public void Initialize(PlayerData player, int shipNo)
    {
        this.player = player;
        this.audioPlayer = GameObject.Find("AudioManager").GetComponent<AudioPlayer>();
        InitializeShipManager(shipNo);
        InitializeShipNavigator();
        InitializeShipLifter();
    }

    private void InitializeShipManager(int shipNo)
    {
        shipManager.player = player;
        shipManager.audioPlayer = audioPlayer;

        shipManager.navigator = shipNavigator;
        shipManager.lifter = shipLifter;
        shipManager.occupier = occupier;

        shipManager.ShipNo = shipNo;
        shipManager.parts = new ShipPart[shipNo + 1];
        shipManager.ShipName = $"ship{player.number}.{shipNo}";
        shipManager.ShipStatus = ShipStatus.Intact;
        shipManager.navigator.PivotX = shipNo;
        shipManager.PartsCount = shipNo + 1;

        for (int i = 0; i <= shipNo; i++)
        {
            GameObject partObj = transform.GetChild(i).gameObject;
            partObj.layer = Layer.SetLayerFleet(player);
            partObj.AddComponent<ShipPart>().Initialize(player, i, shipManager);
            shipManager.parts[i] = partObj.GetComponent<ShipPart>();
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
        shipLifter.parts = shipManager.parts;
    }
}
