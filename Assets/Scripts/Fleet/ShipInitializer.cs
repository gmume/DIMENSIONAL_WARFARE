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
    public Activator activator;
    public Artillerist artillerist;
    public DamageHandler damageHandler;

    public void Initialize(PlayerData player, int shipNo, AttackPattern attackPattern)
    {
        this.player = player;
        this.audioPlayer = GameObject.Find("AudioManager").GetComponent<AudioPlayer>();
        InitializeShipManager(shipNo);
        InitializeShipNavigator();
        InitializeShipLifter();
        InitializeActivator();
        InitializeArtillerist(attackPattern);
        InitializeDamageHandler();
    }

    private void InitializeShipManager(int shipNo)
    {
        shipManager.player = player;
        shipManager.audioPlayer = audioPlayer;

        shipManager.navigator = shipNavigator;
        shipManager.lifter = shipLifter;
        shipManager.activator = activator;
        shipManager.artillerist = artillerist;
        shipManager.damageHandler = damageHandler;

        shipManager.No = shipNo;
        shipManager.parts = new ShipPartManager[shipNo + 1];
        shipManager.ShipName = $"ship{player.number}.{shipNo}";
        
        shipManager.navigator.PivotX = shipNo;

        for (int i = 0; i <= shipNo; i++)
        {
            GameObject partObj = transform.GetChild(i).gameObject;
            partObj.AddComponent<ShipPartManager>().Initialize(player, i, shipManager);
            shipManager.parts[i] = partObj.GetComponent<ShipPartManager>();
        }

        shipManager.partsList = GetPartsList();
    }

    private List<GameObject> GetPartsList()
    {
        List<GameObject> partsAsList = new();

        foreach (ShipPartManager part in shipManager.parts)
        {
            partsAsList.Add(part.gameObject);
        }

        return partsAsList;
    }

    private void InitializeShipNavigator()
    {
        shipNavigator.player = player;
        shipNavigator.audioPlayer = audioPlayer;
        shipNavigator.manager = shipManager;
        shipNavigator.Orientation = Directions.North;
    }

    private void InitializeShipLifter()
    {
        shipLifter.player = player;
        shipLifter.audioPlayer = audioPlayer;
        shipLifter.manager = shipManager;
        shipLifter.parts =  shipManager.parts;
        shipLifter.layerFilter = player.playerCamera.GetComponent<LayerFilter>();
    }

    private void InitializeActivator()
    {
        activator.player = player;
        activator.parts = shipManager.parts;
    }

    private void InitializeArtillerist(AttackPattern attackPattern)
    {
        artillerist.player = player;
        artillerist.attackPattern = attackPattern;
    }

    private void InitializeDamageHandler()
    {
        damageHandler.player = player;
        damageHandler.audioPlayer = audioPlayer;
        damageHandler.ShipStatus = ShipStatus.Intact;
        damageHandler.manager = shipManager;
        damageHandler.layerFilter = player.playerCamera.GetComponent<LayerFilter>();
        damageHandler.opponentLayerFilter = player.opponent.playerCamera.GetComponent<LayerFilter>();
    }
}
