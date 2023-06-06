using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWorld : MonoBehaviour
{
    public PlayerData playerData;
    public string ShipName;
    public GameObject dimensionPrefab, cellPrefab, shipPrefab;
    public Dimensions dimensions;

    private GameObject cameraVehicle;
    private VehicleBehavior vehicleBehavior;
    private int currentX = 0, currentY = 0;

    public void Start()
    {
        dimensions = ScriptableObject.CreateInstance("Dimensions") as Dimensions;
        dimensions.InitDimensions(this.GetComponent<PlayerWorld>(), dimensionPrefab, cellPrefab, shipPrefab);
        SetNewDimension(0);

        if (name == "Player1")
        {
            cameraVehicle = GameObject.Find("CameraVehicle1");
            vehicleBehavior = cameraVehicle.GetComponent<VehicleBehavior>();
            playerData.VehicleBehavior = vehicleBehavior;
        }
        else
        {
            cameraVehicle = GameObject.Find("CameraVehicle2");
            vehicleBehavior = cameraVehicle.GetComponent<VehicleBehavior>();
            playerData.VehicleBehavior = vehicleBehavior;
        }
    }

    private void Update()
    {
        if(playerData.ActiveShip == null)
        {
            ShipName = "no ship";
        }
        else
        {
            ShipName = playerData.ActiveShip.ShipName;
        }
    }

    public void SetNewDimension(int nr)
    {
        playerData.ActiveDimension = dimensions.GetDimension(nr);
    }

    public void SetNewCellRelative(int x, int y)
    {
        if (playerData.ActiveCell != null)
        {
            DeactivateCell();
        }
        currentX += x;
        currentY += y;
        playerData.ActiveCell = dimensions.GetDimension(playerData.ActiveDimension.DimensionNr).GetCell(currentX, currentY).GetComponent<Cell>();
        ActivateCell();
    }

    public void SetNewCellAbsolute(int x, int y)
    {
        currentX = 0;
        currentY = 0;

        SetNewCellRelative(x, y);
    }

    public void ActivateCell()
    {
        playerData.ActiveCell.gameObject.transform.position += new Vector3(0, 0.2f, 0);
    }

    public void DeactivateCell()
    {
        playerData.ActiveCell.gameObject.transform.position -= new Vector3(0, 0.2f, 0);
    }

    public Dimensions GetDimensions()
    {
        return dimensions;
    }

    public Fleet GetFleet()
    {
        return GetDimensions().GetFleet();
    }
}
