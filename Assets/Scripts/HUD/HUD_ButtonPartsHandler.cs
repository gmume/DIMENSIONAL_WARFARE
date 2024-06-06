using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_ButtonPartsHandler : MonoBehaviour
{
    public List<GameObject> buttonParts = new();

    public void ButtonPartTakeHit(int partIndex) => buttonParts[partIndex].GetComponent<Image>().color = Colors.hitCell;
    
    public void RepairButtonPart(int partIndex) => buttonParts[partIndex].GetComponent<Image>().color = Colors.intactPart;
}
