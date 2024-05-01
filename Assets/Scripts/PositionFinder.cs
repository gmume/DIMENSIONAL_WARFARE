using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PositionFinder : MonoBehaviour
{
    DimensionsManager dimensions;

    private readonly List<Vector2> searchDirections = new() { new(1, 0), new(0, 1), new(-1, 0), new(0, -1) };
    List<Vector2> deltaCoords;
    bool isValidPos;
    private List<Vector2> newCoords;

    public List<Vector2> FindVacantCoordinates(int onDimensionNo, List<Vector2> currentCoords)
    {
        isValidPos = false;
        deltaCoords = new() { new(0, 0), new(0, 0), new(0, 0), new(0, 0) };
        newCoords = currentCoords;

        while (!isValidPos)
        {
            for (int i = 0; i < deltaCoords.Count; i++)
            {
                isValidPos = VacantPosFound(currentCoords, deltaCoords[i], onDimensionNo);
                if (isValidPos) break;
            }

            UpdateDeltaCoords();
        }

        return newCoords;
    }

    private bool VacantPosFound(List<Vector2> currentCoords, Vector2 delta, int onDimensionNo)
    {
        for (int i = 0; i < currentCoords.Count; i++)
        {
            newCoords[i] += delta;
        }

        return CountVacantCells(dimensions.GetCellGroup(newCoords, onDimensionNo)) == currentCoords.Count;
    }

    public int CountVacantCells(List<GameObject> cells)
    {
        int vacantCells = new();

        for (int i = 0; i < cells.Count; i++)
        {
            if (!cells[i].GetComponent<CellData>().Occupied) vacantCells++;
        }

        return vacantCells;
    }

    private void UpdateDeltaCoords()
    {
        for (int i = 1; i < deltaCoords.Count; i++)
        {
            deltaCoords[i] += searchDirections[i];
        }
    }

    public void Initialize(DimensionsManager dimensions) => this.dimensions = dimensions;
}
