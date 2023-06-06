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

    private Player player;
    private int currentX = 0, currentY = 0;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    public void Start()
    {
        dimensions = ScriptableObject.CreateInstance("Dimensions") as Dimensions;
        dimensions.InitDimensions(player, dimensionPrefab, cellPrefab, shipPrefab);
        SetNewDimension(0);
    }

    private void Update()
    {
        if(player.data.ActiveShip == null)
        {
            ShipName = "no ship";
        }
        else
        {
            ShipName = player.data.ActiveShip.ShipName;
        }
    }

    public void SetNewDimension(int nr)
    {
        player.data.ActiveDimension = dimensions.GetDimension(nr);
    }

    public void SetNewCellRelative(int x, int y)
    {
        if (player.data.ActiveCell != null)
        {
            DeactivateCell();
        }
        currentX += x;
        currentY += y;
        player.data.ActiveCell = dimensions.GetDimension(player.data.ActiveDimension.DimensionNr).GetCell(currentX, currentY).GetComponent<Cell>();
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
        player.data.ActiveCell.gameObject.transform.position += new Vector3(0, 0.2f, 0);
    }

    public void DeactivateCell()
    {
        player.data.ActiveCell.gameObject.transform.position -= new Vector3(0, 0.2f, 0);
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
