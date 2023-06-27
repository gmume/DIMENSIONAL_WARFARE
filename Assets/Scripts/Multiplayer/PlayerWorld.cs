using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWorld : MonoBehaviour
{
    public string ShipName;
    public GameObject dimensionPrefab, cellPrefab;

    private Player player;
    private int currentX = 0, currentY = 0;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    public void Start()
    {
        player.dimensions = ScriptableObject.CreateInstance("Dimensions") as Dimensions;
        player.dimensions.name = "Dimensions" + player.number;
        player.dimensions.InitDimensions(player, dimensionPrefab, cellPrefab);
        SetNewDimension(0);
    }

    public void SetNewDimension(int nr)
    {
        player.ActiveDimension = player.dimensions.GetDimension(nr);
    }

    public void SetNewCellRelative(int x, int y)
    {
        if (x < OverworldData.DimensionSize && y < OverworldData.DimensionSize)
        {
            if (player.ActiveCell != null)
            {
                DeactivateCell();
            }

            currentX += x;
            currentY += y;
            player.ActiveCell = player.dimensions.GetDimension(player.ActiveDimension.DimensionNr).GetCell(currentX, currentY).GetComponent<Cell>();
            ActivateCell();
        }
        else
        {
            Debug.Log("Cell outside of dimension!");
        }
    }

    public void SetNewCellAbsolute(int x, int y)
    {
        currentX = 0;
        currentY = 0;

        SetNewCellRelative(x, y);
    }

    public void ActivateCell()
    {
        player.ActiveCell.gameObject.transform.position += new Vector3(0, 0.2f, 0);
    }

    public void DeactivateCell()
    {
        player.ActiveCell.gameObject.transform.position -= new Vector3(0, 0.2f, 0);
    }
}
