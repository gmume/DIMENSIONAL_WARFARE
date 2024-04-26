using System.Collections.Generic;
using UnityEngine;

public class CellGroupProvider : MonoBehaviour
{
    private DimensionsManager dimensions;

    public List<GameObject> GetCells(List<Vector2> cellCoordinates, int dimensionNo)
    {
        List<GameObject> cells = new();

        for (int i = 0; i < cellCoordinates.Count; i++)
        {
            GameObject cell = dimensions.GetDimension(dimensionNo).GetCell((int)cellCoordinates[i].x, (int)cellCoordinates[i].y);
            if (cell != null) cells.Add(cell);
        }

        return cells;
    }
    
    public void Initialize() => dimensions = GetComponent<DimensionsManager>();
}
