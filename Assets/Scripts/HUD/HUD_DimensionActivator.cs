using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD_DimensionActivator : MonoBehaviour
{
    [HideInInspector] public int currentHUD_Dimension, currentHUD_DimensionOpponent;
                      public Texture2D HUD_DimensionInactive, HUD_DimensionActive;

    public void ActivateDimensionAtNo(HUD_Manager hudManager, int no)
    {
        GameObject HUD_DimensionToSetInactive = hudManager.HUD_Dimensions[currentHUD_Dimension];
        HUD_DimensionToSetInactive.GetComponent<RawImage>().texture = HUD_DimensionInactive;
        HUD_DimensionToSetInactive.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
        HUD_DimensionToSetInactive.GetComponentInChildren<TextMeshProUGUI>().fontSize = 22;

        GameObject HUD_DimensionToSetActive = hudManager.HUD_Dimensions[no];
        HUD_DimensionToSetActive.GetComponent<RawImage>().texture = HUD_DimensionActive;
        HUD_DimensionToSetActive.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Bold | FontStyles.Underline;
        HUD_DimensionToSetActive.GetComponentInChildren<TextMeshProUGUI>().fontSize = 28;

        currentHUD_Dimension = no;
    }
}
