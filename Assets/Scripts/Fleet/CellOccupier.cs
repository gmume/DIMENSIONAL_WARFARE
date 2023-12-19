using UnityEngine;

public class CellOccupier : MonoBehaviour
{
    [HideInInspector] public ShipPartManager[] parts;

    public void OccupyCells()
    {
        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].OccupyCell();
        }
    }

    public void ReleaseCells()
    {
        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].ReleaseCell();
        }
    }
}
