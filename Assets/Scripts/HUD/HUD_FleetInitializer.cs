using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HUD_FleetInitializer : MonoBehaviour
{
    public GameObject HUD_ShipPrefab;
    public Texture2D[] HUD_ShipTextures;

    public void InitializeHUDFleet(GameObject HUD_Dimension, GameObject[] HUD_Fleet, Color HUD_ShipColor)
    {
        Vector2[] pivots =  { new Vector2(0f, 0.5f), new Vector2(1f, 0.5f), new Vector2(1f, 0.5f) };
        Vector2[] anchors = { new Vector2(0.05f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.95f, 0.5f) };
        Rect rect = HUD_Dimension.GetComponent<RectTransform>().rect;

        for (int i = 0; i < OverworldData.FleetSize; i++)
        {
            GameObject HUD_Ship = Instantiate(HUD_ShipPrefab) as GameObject;
            HUD_Ship.name = "HUD_Ship" + (i + 1);
            HUD_Ship.transform.SetParent(HUD_Dimension.transform, false);
            HUD_Ship.layer = HUD_Dimension.layer;
            HUD_Ship.GetComponent<RawImage>().texture = HUD_ShipTextures[i];

            Texture shipTexture = HUD_Ship.GetComponent<RawImage>().texture;
            float aspectRatio = shipTexture.width / shipTexture.height;
            float x = rect.width / 7 * (i + 1);
            float y = x / aspectRatio;
            Vector2 sizeDelta = new(x, y);

            RectTransform rectTransform = HUD_Ship.GetComponent<RectTransform>();
            rectTransform.pivot = pivots[i];
            rectTransform.anchorMin = rectTransform.anchorMax = anchors[i];
            rectTransform.sizeDelta = sizeDelta;
            //rectTransform.SetLocalPositionAndRotation(rectTransform.localPosition, rectTransform.localRotation);

            HUD_Fleet[i] = HUD_Ship;
        }
    }
}
