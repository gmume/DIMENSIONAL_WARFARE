using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD_DimensionActivator : MonoBehaviour
{
    [HideInInspector] public int currentHUD_Dimension, currentHUD_DimensionOpponent;

    public void ActivateDimensionAtNo(HUD_Manager hudManager, int no)
    {
        DeactivateDimension(hudManager);
        ActivateDimension(hudManager, no);
    }

    private void DeactivateDimension(HUD_Manager hudManager)
    {
        GameObject HUD_DimensionToSetInactive = hudManager.HUD_Dimensions[currentHUD_Dimension];
        HUD_DimensionToSetInactive.GetComponent<RawImage>().color -= Colors.deltaActiveHUDDimension;
        HUD_DimensionToSetInactive.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
        HUD_DimensionToSetInactive.GetComponentInChildren<TextMeshProUGUI>().fontSize = 22;
    }

    public void ActivateDimension(HUD_Manager hudManager, int no)
    {
        GameObject HUD_DimensionToSetActive = hudManager.HUD_Dimensions[no];
        HUD_DimensionToSetActive.GetComponent<RawImage>().color += Colors.deltaActiveHUDDimension;
        HUD_DimensionToSetActive.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Bold | FontStyles.Underline;
        HUD_DimensionToSetActive.GetComponentInChildren<TextMeshProUGUI>().fontSize = 28;
        currentHUD_Dimension = no;
    }
}