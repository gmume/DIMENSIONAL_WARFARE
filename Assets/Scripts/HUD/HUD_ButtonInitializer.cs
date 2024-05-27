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
            DesignButton(button, Resources.Load<Sprite>("HUD_Sprites/HUD_ShipSprites/ShipSprite" + i), Resources.Load<Sprite>("HUD_Sprites/HUD_ShipSprites/ShipSpriteActive" + i));

            GameObject[] buttonParts = buttonObj.GetComponent<HUD_ButtonPartsHandler>().buttonParts;

            for (int j = 0; j <= i; j++)
            {
                buttonParts[i] = InitializeHUDButtonShipPart(buttonObj);
            }
        }
    }

    private void InitializeButton(HUD_Manager hudManager, GameObject buttonObj, Button button, int i)
    {
        buttonObj.name = "ShipButton" + (i);
        buttonObj.layer = (name == "HUD1") ? 11 : 12;

        button.transform.SetParent(shipButtonsTransform, false);
        button.image.SetNativeSize();
        Navigation buttonNavigation = button.navigation;
        buttonNavigation.mode = Navigation.Mode.None;
        buttonObj.AddComponent<ShipButtonData>().No = i;
        buttonObj.AddComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.MiddleCenter;
        HUD_ButtonPartsHandler partsHandler = buttonObj.AddComponent<HUD_ButtonPartsHandler>();
        partsHandler.Initialize();

        if (i == 0)
        {
            hudManager.hudButtonHandler.selectedButton = buttonObj;
            GetComponent<HUD_Manager>().SetSelecetedButton();
        }

        hudManager.hudButtonHandler.shipButtons.Add(buttonObj);
    }

    private void DesignButton(Button button, Sprite shipSprite, Sprite shipSpriteActive)
    {
        button.image.type = Image.Type.Simple;
        button.image.sprite = shipSprite;
        button.image.SetNativeSize();
        button.transition = Selectable.Transition.SpriteSwap;
        Sprite buttonSelected = shipSpriteActive;

        SpriteState spriteState = new() { selectedSprite = buttonSelected};
        button.spriteState = spriteState;
    }

    private GameObject InitializeHUDButtonShipPart(GameObject buttonObj)
    {
        GameObject shipPart = new("HUD_ShipPart", typeof(CanvasRenderer), typeof(Image));
        shipPart.transform.SetParent(buttonObj.transform, false);
        Image shipPartImage = shipPart.GetComponent<Image>();
        shipPartImage.sprite = Resources.Load<Sprite>("HUD_Sprites/HUD_ShipSprites/HUD_ShipPart");
        shipPartImage.preserveAspect = true;
        shipPart.layer = (name == "HUD1") ? 11 : 12;
        return shipPart;
    }
}
