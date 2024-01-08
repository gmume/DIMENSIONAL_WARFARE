using System;
using UnityEngine;

public class PlayerWorldManager : MonoBehaviour
{
    public string ShipName;
    public GameObject dimensionPrefab, cellPrefab;

    private PlayerData player;
    private int currentX = 0, currentY = 0;

    public void SetNewDimension(int no) => player.ActiveDimension = player.dimensions.GetDimension(no);

    public void MoveSelection(float x, float y)
    {
        // Get right axis and move in correct direction
        SetNewCellRelative((int)((Math.Abs(x) > Math.Abs(y)) ? Mathf.Sign(x) : 0), (int)((Math.Abs(x) > Math.Abs(y)) ? 0 : Mathf.Sign(y)));
    }

    public void SetNewCellRelative(int deltaX, int deltaY)
    {
        int newX = currentX + deltaX;
        int newY = currentY + deltaY;

        if (IsWithinDimensionBounds(newX, newY))
        {
            currentX = newX;
            currentY = newY;

            DeactivateCell();
            player.ActiveCell = player.dimensions.GetDimension(player.ActiveDimension.DimensionNo).GetCell(currentX, currentY).GetComponent<CellData>();
            ActivateCell();
        }
        else
        {
            player.HUD.WriteText($"Capt'n {player.number}, target coordinates are out of range!");
        }
    }

    private bool IsWithinDimensionBounds(int x, int y) => x >= 0 && x < OverworldData.DimensionSize && y >= 0 && y < OverworldData.DimensionSize;

    public void SetNewCellAbsolute(int newX, int newY)
    {
        currentX = 0;
        currentY = 0;

        SetNewCellRelative(newX, newY);
    }

    public void ActivateCell() => player.ActiveCell.transform.position += new Vector3(0, 0.2f, 0);

    public void DeactivateCell()
    {
        if (player.ActiveCell != null)
        {
            player.ActiveCell.transform.position -= new Vector3(0, 0.2f, 0);
        }
    }

    public void Initialize(PlayerData player)
    {
        this.player = player;
        
        player.dimensions.name = $"Dimensions{player.number}";
        player.dimensions.Initialize();
        SetNewDimension(0);
    }
}
