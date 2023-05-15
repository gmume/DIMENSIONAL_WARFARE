using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    //Debugging ↓
    //public string vehicleBehavior;
    public string activeDimension;
    public string activeCell;
    public string activeShip;
    //Debugging ↑


    public PlayerData playerData;
    public string ShipName;
    public GameObject dimensionPrefab, cellPrefab, shipPrefab;

    private GameObject cameraVehicle;
    private VehicleBehavior vehicleBehavior;
    private Dimensions dimensions;
    private int currentX = 0, currentY = 0;

    public void Start()
    {
        dimensions = ScriptableObject.CreateInstance("Dimensions") as Dimensions;
        dimensions.InitDimensions(this.GetComponent<PlayerScript>(), dimensionPrefab, cellPrefab, shipPrefab);
        SetNewDimension(0);
        SetNewCell(0, 0);

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
        //Debugging ↓
        //vehicleBehavior = playerData.VehicleBehavior.name;
        activeDimension = "Dimension "+playerData.ActiveDimension.DimensionNr.ToString();
        activeCell = "Cell "+playerData.ActiveCell.X.ToString()+", "+playerData.ActiveCell.Y.ToString();

        if (playerData.ActiveShip)
        {
            activeShip = playerData.ActiveShip.name;
        }
        //Debugging ↑


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

    public void SetNewCell(int x, int y)
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
