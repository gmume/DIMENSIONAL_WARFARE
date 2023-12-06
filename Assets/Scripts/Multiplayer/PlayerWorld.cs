using UnityEngine;

public class PlayerWorld : MonoBehaviour
{
    public string ShipName;
    public GameObject dimensionPrefab, cellPrefab;

    private PlayerData player;
    private int currentX = 0, currentY = 0;

    public void SetNewDimension(int no)
    {
        player.ActiveDimension = player.dimensions.GetDimension(no);
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
            player.ActiveCell = player.dimensions.GetDimension(player.ActiveDimension.DimensionNo).GetCell(currentX, currentY).GetComponent<Cell>();
            ActivateCell();
        }
        else
        {
            Debug.LogWarning(name + ": Cell outside of dimension!");
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

    public void InitPlayerWorld()
    {
        player = GetComponent<PlayerData>();
        
        player.dimensions = ScriptableObject.CreateInstance("Dimensions") as Dimensions;
        player.dimensions.name = "Dimensions" + player.number;
        player.dimensions.InitDimensions(player, dimensionPrefab, cellPrefab);
        SetNewDimension(0);
    }
}
