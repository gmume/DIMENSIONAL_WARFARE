using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CellOccupier : MonoBehaviour
{
    public void OccupyCells(List<(GameObject cell, GameObject obj)> cellsAndObjs)
    {
        foreach ((GameObject cell, GameObject obj) in cellsAndObjs)
        {
            CellData cellData = cell.GetComponent<CellData>();
            cellData.Occupied = true;
            cellData.OccupyingObj = obj;

            if (obj.CompareTag("ShipPart"))
            {
                obj.transform.position = cell.transform.position;
            }
            else
            {
                obj.transform.SetParent(cell.transform, false);
            }

            /* ↓↓↓ for debugging ↓↓↓ */
            //cell.GetComponent<Renderer>().material.color = obj.GetComponent<Renderer>().material.color;
            /* ↑↑↑ for debugging ↑↑↑ */

            obj.transform.position += new Vector3(0, 0.5f, 0);
        }
    }

    public void ReleaseCells(List<GameObject> cells)
    {
        foreach (GameObject cell in cells)
        {
            CellData cellData= cell.GetComponent<CellData>();
            cellData.Occupied = false;
            cellData.OccupyingObj = null;

            /* ↓↓↓ for debugging ↓↓↓ */
           // cell.GetComponent<Renderer>().material.color = Colors.cell;
            /* ↑↑↑ for debugging ↑↑↑ */
        }
    }
}
