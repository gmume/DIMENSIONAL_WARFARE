using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CellOccupier : MonoBehaviour
{
    public void OccupyCells(List<(GameObject cell, GameObject part)> cellsAndObjs)
    {
        for (int i = 0; i < cellsAndObjs.Count; i++)
        {
            CellData cellData = cellsAndObjs[i].Item1.GetComponent<CellData>();
            cellData.Occupied = true;
            cellData.OccupyingObj = cellsAndObjs[i].Item2;
        }
        
        foreach ((GameObject cell, GameObject part) in cellsAndObjs)
        {
            CellData cellData = cell.GetComponent<CellData>();
            cellData.Occupied = true;
            cellData.OccupyingObj = part;
        }
    }

    public void ReleaseCells(List<GameObject> cells)
    {
        foreach (GameObject cell in cells)
        {
            CellData cellData= cell.GetComponent<CellData>();
            cellData.Occupied = false;
            cellData.OccupyingObj = null;
        }
    }
}
