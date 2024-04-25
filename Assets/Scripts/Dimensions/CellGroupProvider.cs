using System.Collections.Generic;
using UnityEngine;

public class CellGroupProvider : MonoBehaviour
{
    private DimensionsManager dimensions;
    private readonly List<AttackPattern> attackPatterns = new();

    public List<GameObject> GetCells(int shipNo, int shipLevel, int attackedDimensionNo, Vector2 attackedCellCoordinates)
    {
        List<GameObject> cells = new();

        for (int level = 0; level <= shipLevel; level++)
        {
            cells.AddRange(GetCellsForLevel(shipNo, level, attackedDimensionNo, attackedCellCoordinates));
        }

        return cells;
    }

    private List<GameObject> GetCellsForLevel(int shipNo, int shipLevel, int attackedDimensionNo, Vector2 attackedCellCoordinates)
    {
        AttackPattern attackPattern = attackPatterns[shipNo];
        List<Vector2> patternLevel = attackPattern.GetPatternLevel(shipLevel);
        List<GameObject> cells = new();

        for (int i = 0; i < patternLevel.Count; i++)
        {
            GameObject cell = GetCell(attackedDimensionNo, attackedCellCoordinates, patternLevel[i]);
            if(cell != null) cells.Add(cell);
        }

        return cells;
    }

    private GameObject GetCell(int attackedDimensionNo, Vector2 attackedCellCoordinates, Vector2 deltaCoordinates)
    {
        int x = (int)(attackedCellCoordinates[0] + deltaCoordinates[0]);
        int y = (int)(attackedCellCoordinates[1] + deltaCoordinates[1]);
        return dimensions.GetDimension(attackedDimensionNo).GetCell(x, y);
    }

    public void Initialize()
    {
        dimensions = GetComponent<DimensionsManager>();
        Object[] loadedAttackPatterns = Resources.LoadAll("AttackPatterns");
        
        foreach (AttackPattern attackPattern in loadedAttackPatterns)
        {
            attackPatterns.Add((AttackPattern)attackPattern);
        }
    }
}
