using TMPro;
using UnityEngine;

public class HUD_DimensionsInitializer : MonoBehaviour
{
    private TextMeshProUGUI HUD_DimensionNo;

    public void InitializeHUDDimensions(HUD_FleetInitializer hudFleetInitializer, GameObject HUD_DimensionsObj, GameObject HUD_DimensionPrefab, GameObject[] HUD_Dimensions, GameObject[] HUD_Fleet, Color HUD_ShipColor)
    {
        Vector2[] pivots = { new Vector2(0.5f, 0f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 1f) };
        Vector2[] anchorsMin = { new Vector2(0f, 0f), new Vector2(0f, 0.3f), new Vector2(0f, 0.6f) };
        Vector2[] anchorsMax = { new Vector2(1f, 0.36f), new Vector2(1f, 0.66f), new Vector2(1f, 1f) };

        for (int i = 0; i < OverworldData.DimensionsCount; i++)
        {
            GameObject HUD_Dimension = Instantiate(HUD_DimensionPrefab) as GameObject;
            HUD_Dimension.name = "HUDDimension0" + (i + 1);
            HUD_Dimension.transform.SetParent(HUD_DimensionsObj.transform, false);
            HUD_Dimension.layer = HUD_DimensionsObj.layer;

            RectTransform rectTransform = HUD_Dimension.GetComponent<RectTransform>();
            rectTransform.pivot = pivots[i];
            rectTransform.anchorMin = anchorsMin[i];
            rectTransform.anchorMax = anchorsMax[i];
            rectTransform.sizeDelta = new Vector2(0,0);

            HUD_DimensionNo = HUD_Dimension.GetComponentInChildren<TextMeshProUGUI>();
            HUD_DimensionNo.text = "0" + (i + 1);
            HUD_Dimensions[i] = HUD_Dimension;

            if (i == 0)
            {
                GetComponent<HUD_Manager>().SetHUDDimension(0);
                hudFleetInitializer.InitializeHUDFleet(HUD_Dimension, HUD_Fleet, HUD_ShipColor);
            }
        }
    }
}
