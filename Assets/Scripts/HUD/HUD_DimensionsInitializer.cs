using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD_DimensionsInitializer : MonoBehaviour
{
    public PlayerData player;
    private TextMeshProUGUI HUD_DimensionNo;

    private readonly Vector2[] pivots = { new Vector2(0.5f, 0f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 1f) };
    private readonly Vector2[] anchorsMin = { new Vector2(0f, 0f), new Vector2(0f, 0.3f), new Vector2(0f, 0.6f) };
    private readonly Vector2[] anchorsMax = { new Vector2(1f, 0.36f), new Vector2(1f, 0.66f), new Vector2(1f, 1f) };

    public void InitializeHUDDimensions(HUD_FleetInitializer hudFleetInitializer, GameObject HUD_DimensionsObj, GameObject HUD_DimensionPrefab, GameObject[] HUD_Dimensions, GameObject[] HUD_Fleet, Color HUD_ShipColor)
    {
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

            if (hudFleetInitializer)
            {
                HUD_Dimension.GetComponent<RawImage>().color = player.cellColor;
            }
            else
            {
                HUD_Dimension.GetComponent<RawImage>().color = player.opponent.cellColor;
            }

            HUD_Dimensions[i] = HUD_Dimension;

            if (hudFleetInitializer && i == 0)
            {
                GetComponent<HUD_DimensionActivator>().ActivateDimension(GetComponent<HUD_Manager>(), 0);
                hudFleetInitializer.InitializeHUDFleet(HUD_Dimension, HUD_Fleet, HUD_ShipColor);
            }
        }
    }

    public void InitializeOpponentHUDDimensions(GameObject HUD_DimensionsObj, GameObject HUD_DimensionPrefab, GameObject[] HUD_Dimensions, GameObject[] HUD_Fleet, Color HUD_ShipColor)
    {
        InitializeHUDDimensions(null, HUD_DimensionsObj, HUD_DimensionPrefab, HUD_Dimensions, HUD_Fleet, HUD_ShipColor);
    }
}
