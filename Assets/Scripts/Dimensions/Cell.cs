using UnityEngine;

public class Cell : MonoBehaviour
{
    public int x;
    public int y;
    public bool activated;
    public bool occupied;
    public ShipPart part;
    public bool hitted;

    public int X { get => x; set => x = value; }
    public int Y { get => y; set => y = value; }
    public bool Activated { get => activated; set => activated = value; }
    public bool Occupied { get => occupied; set => occupied = value; }
    public ShipPart Part { get => part; set => part = value; }
    public bool Hitted { get => hitted; set => hitted = value; }
}
