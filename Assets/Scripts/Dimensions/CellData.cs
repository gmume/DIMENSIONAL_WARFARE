using UnityEngine;

public class CellData : MonoBehaviour
{
    public DimensionManager Dimension { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public bool Focused { get; set; }
    public bool Active { get; set; }
    public bool Occupied { get; set; }
    public GameObject OccupyingObj { get; set; }
    public bool Hit { get; set; }
}
