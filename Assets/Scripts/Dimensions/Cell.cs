using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int X { get; set; }
    public int Y { get; set; }
    public bool Activated { get; set; }
    public bool Occupied { get; set; }
    public ShipPart part { get; set; }
    public bool Hitted { get; set; }
}
