using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_FleetInitializer : MonoBehaviour
{
    public GameObject HUD_ShipPrefab;
    public Texture2D[] HUD_ShipTextures;

    public void InitializeHUDFleet(HUD_Manager hudManager, GameObject HUD_Dimension, Color HUD_ShipColor)
    {
        hudManager.HUD_Fleet = new GameObject[OverworldData.FleetSize];

        for (int i = 0; i < OverworldData.FleetSize; i++)
        {
            GameObject HUD_Ship = Instantiate(HUD_ShipPrefab, HUD_Dimension.transform.position, Quaternion.identity);
            HUD_Ship.name = "HUD_Ship" + (i + 1);
            HUD_Ship.transform.SetParent(HUD_Dimension.transform);
            HUD_Ship.layer = HUD_Dimension.layer;
            HUD_Ship.GetComponent<RawImage>().texture = HUD_ShipTextures[i];
            HUD_Ship.GetComponent<RawImage>().color = HUD_ShipColor;

            Vector3 xPosition = new(i * 30, 0, -1);
            HUD_Ship.transform.position += xPosition;
            hudManager.HUD_Fleet[i] = HUD_Ship;
        }
    }
}
