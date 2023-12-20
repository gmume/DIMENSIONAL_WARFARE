using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD_ButtonInitializer : MonoBehaviour
{
    [HideInInspector] public Transform shipButtonsTransform;

    public void InitializeShipButtons(HUD_Manager hudManager)
    {
        GameObject buttonObj;
        Button button;

        for (int i = 0; i < OverworldData.FleetSize; i++)
        {
            buttonObj = TMP_DefaultControls.CreateButton(new TMP_DefaultControls.Resources());
            Transform textObject = buttonObj.transform.Find("Text (TMP)");
            UnityEngine.Object.Destroy(textObject.gameObject);
            button = buttonObj.GetComponent<Button>();

            InitializeButton(hudManager, buttonObj, button, i);
            DesignButton(button);

            for (int j = 0; j <= i; j++)
            {
                InitializeHUDButtonShipPart(buttonObj);
            }
        }
    }

    private void InitializeButton(HUD_Manager hudManager, GameObject buttonObj, Button button, int i)
    {
        buttonObj.name = "ShipButton" + (i + 1);
        buttonObj.layer = (name == "HUD1") ? 11 : 12;

        Transform transformParent = shipButtonsTransform;
        button.transform.SetParent(transformParent, false);
        Navigation buttonNavigation = button.navigation;
        buttonNavigation.mode = Navigation.Mode.None;
        buttonObj.AddComponent<ShipButtonData>().ShipButtonNr = i;
        buttonObj.AddComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.MiddleCenter;

        if (i == 0)
        {
            hudManager.hudButtonHandler.selectedButton = buttonObj;
            GetComponent<HUD_Manager>().SetSelecetedButton();
        }

        hudManager.hudButtonHandler.shipButtons.Add(buttonObj);
    }

    private void DesignButton(Button button)
    {
        button.image.type = Image.Type.Simple;
        button.image.sprite = Resources.Load<Sprite>("HUD_Sprites/Buttons/Button");
        button.image.SetNativeSize();
        button.transition = Selectable.Transition.SpriteSwap;
        Sprite buttonSelected = Resources.Load<Sprite>("HUD_Sprites/Buttons/Selection");
        Sprite buttonDisabled = Resources.Load<Sprite>("HUD_Sprites/Buttons/Disabled");

        SpriteState spriteState = new() { selectedSprite = buttonSelected, disabledSprite = buttonDisabled };
        button.spriteState = spriteState;
    }

    private void InitializeHUDButtonShipPart(GameObject buttonObj)
    {
        GameObject shipPart = new("HUD_ShipPart", typeof(CanvasRenderer), typeof(Image));
        shipPart.transform.SetParent(buttonObj.transform, false);
        Image shipPartImage = shipPart.GetComponent<Image>();
        shipPartImage.sprite = Resources.Load<Sprite>("HUD_Sprites/Buttons/HUD_ShipPart");
        shipPartImage.type = Image.Type.Simple;
        shipPartImage.preserveAspect = true;
        shipPart.layer = (name == "HUD1") ? 11 : 12;
    }

}
