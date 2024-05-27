using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_ButtonPartsHandler : MonoBehaviour
{
    public GameObject[] buttonParts;

    public void ButtonPartTakeHit(int partIndex) => buttonParts[partIndex].GetComponent<Image>().color = Colors.hitCell;
    
    public void RepairButtonPart(int partIndex) => buttonParts[partIndex].GetComponent<Image>().color = Color.white;

    public void Initialize()
    {
        buttonParts = new GameObject[OverworldData.FleetSize];
    }
}
