using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD_DimensionsInitializer : MonoBehaviour
{
    public GameObject HUD_DimensionObj;
    private TextMeshProUGUI HUD_DimensionNo;

    public void InitializeHUDDimensions(HUD_Manager hudManager, HUD_FleetInitializer hudFleetInitializer, GameObject HUD_DimensionsObj, GameObject[] HUD_Dimensions, GameObject[] HUD_Fleet, Color HUD_ShipColor)
    {
        for (int i = 0; i < OverworldData.DimensionsCount; i++)
        {
            GameObject HUD_Dimension = Instantiate(HUD_DimensionObj) as GameObject;
            HUD_Dimension.name = "HUDDimension0" + (i + 1);
            HUD_Dimension.transform.SetParent(HUD_DimensionsObj.transform, false);

            HUD_Dimension.layer = HUD_DimensionsObj.layer;
            HUD_Dimension.transform.position += new Vector3(0, i * 40, 0);
            HUD_DimensionNo = HUD_Dimension.GetComponentInChildren<TextMeshProUGUI>();
            HUD_DimensionNo.text = "0" + (i + 1);

            Debug.Log("HUD_Dimensions: " + HUD_Dimensions.Length);
            Debug.Log("HUD_Dimension: " + HUD_Dimension);
            HUD_Dimensions[i] = HUD_Dimension;

            if (i == 0)
            {
                GetComponent<HUD_Manager>().SetHUDDimension(0);
                hudFleetInitializer.InitializeHUDFleet(hudManager, HUD_Dimension, HUD_Fleet, HUD_ShipColor);
            }
        }
    }
}
