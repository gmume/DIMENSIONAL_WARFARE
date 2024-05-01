using System.Collections.Generic;
using UnityEngine;

public class CellGroupProvider : MonoBehaviour
{
    private DimensionsManager dimensions;
    
    public List<GameObject> GetCells(List<Vector2> cellCoordinates, DimensionManager dimension)
    {
        List<GameObject> cells = new();

        for (int i = 0; i < cellCoordinates.Count; i++)
        {
            GameObject cell = dimension.GetCell((int)cellCoordinates[i].x, (int)cellCoordinates[i].y);
            if (cell != null) cells.Add(cell);
        }

        return cells;
    }

    //public List<GameObject> GetCells(List<Vector2> cellCoordinates, int dimensionNo) => GetCells(cellCoordinates, dimensions.GetDimension(dimensionNo));

    public void Initialize() => dimensions = GetComponent<DimensionsManager>();
}
