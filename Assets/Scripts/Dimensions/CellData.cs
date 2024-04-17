using UnityEngine;

public class CellData : MonoBehaviour
{
    public DimensionManager Dimension { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public bool Activated { get; set; }
    public bool Occupied { get; set; }
    public ShipPartManager Part { get; set; }
    public bool Hitted { get; set; }
}
