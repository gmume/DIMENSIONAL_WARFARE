using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Creator : MonoBehaviour
{
    private GameObject shipButtonsObj;
    private TextMeshProUGUI HUD_DimensionNo;

    public Player player;
    public GameObject HUD_DimensionsObj;
    public GameObject HUD_DimensionObj;

    public void CreateHUD(ref List<GameObject> shipButtons, ref TextMeshProUGUI xCoord, ref TextMeshProUGUI yCoord,  ref GameObject armed, ref int currentHUD_Dimension, ref ShipButton currentButton, ref GameObject selectedButton,  ref GameObject[] HUD_Dimensions)
    {
        GameObject[] HUD_Parts;

        if (player.name == "Player1")
        {
            HUD_Parts = GameObject.FindGameObjectsWithTag("HUD1");
        }
        else
        {
            HUD_Parts = GameObject.FindGameObjectsWithTag("HUD2");
        }

        foreach (GameObject HUD_Part in HUD_Parts)
        {
            if (HUD_Part.name == "X-Koordinate")
            {
                xCoord = HUD_Part.GetComponent<TextMeshProUGUI>();
            }
            else if (HUD_Part.name == "Y-Koordinate")
            {
                yCoord = HUD_Part.GetComponent<TextMeshProUGUI>();
            }
            else if (HUD_Part.name == "ShipButtons")
            {
                shipButtonsObj = HUD_Part;
            }
            else if (HUD_Part.name == "HUD_Dimensions")
            {
                HUD_DimensionsObj = HUD_Part;
            }
            else if (HUD_Part.name == "Armed")
            {
                armed = HUD_Part;
                HUD_Part.SetActive(false);
            }
        }

        CreateHUDDimensions(ref HUD_Dimensions);
        currentHUD_Dimension = 0;

        CreateShipButtons(ref selectedButton, ref shipButtons);
        player.fleet.ActivateShip(currentButton.ShipButtonNr, player);
    }

    private void CreateShipButtons(ref GameObject selectedButton, ref List<GameObject> shipButtons)
    {
        GameObject buttonObj;
        Button button;

        for (int i = 0; i < OverworldData.FleetSize; i++)
        {
            buttonObj = TMP_DefaultControls.CreateButton(new TMP_DefaultControls.Resources());
            Transform textObject = buttonObj.transform.Find("Text (TMP)");
            UnityEngine.Object.Destroy(textObject.gameObject);
            button = buttonObj.GetComponent<Button>();

            CreateButton(ref selectedButton, ref shipButtons, buttonObj, button, i);
            DesignButton(button);

            for (int j = 0; j <= i; j++)
            {
                CreateHUDShipPart(buttonObj);
            }
        }
    }

    private void CreateButton(ref GameObject selectedButton, ref List<GameObject> shipButtons, GameObject buttonObj, Button button, int i)
    {
        if (name == "HUD1")
        {
            buttonObj.name = "ShipButton" + (i + 1);
            buttonObj.layer = 11;
        }
        else
        {
            buttonObj.name = "ShipButton" + (i + 1);
            buttonObj.layer = 12;
        }

        Transform transformParent = shipButtonsObj.GetComponent<Transform>();
        button.transform.SetParent(transformParent, false);
        Navigation buttonNavigation = button.navigation;
        buttonNavigation.mode = Navigation.Mode.None;
        buttonObj.AddComponent<ShipButton>().ShipButtonNr = i;
        buttonObj.AddComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.MiddleCenter;

        if (i == 0)
        {
            selectedButton = buttonObj;
            GetComponent<HUD_Manager>().SetSelecetedButton();
        }

        shipButtons.Add(buttonObj);
    }

    private void DesignButton(Button button)
    {
        button.image.type = Image.Type.Simple;
        button.image.sprite = Resources.Load<Sprite>("HUD_Sprites/Buttons/Button");
        button.image.SetNativeSize();
        button.transition = Selectable.Transition.SpriteSwap;
        Sprite buttonSelected = Resources.Load<Sprite>("HUD_Sprites/Buttons/Selection");
        Sprite buttonDisabled = Resources.Load<Sprite>("HUD_Sprites/Buttons/Disabled");

        SpriteState spriteState = new()
        {
            selectedSprite = buttonSelected,
            disabledSprite = buttonDisabled
        };
        button.spriteState = spriteState;
    }

    private void CreateHUDShipPart(GameObject buttonObj)
    {
        GameObject shipPart = new("HUD_ShipPart", typeof(CanvasRenderer), typeof(Image));
        shipPart.transform.SetParent(buttonObj.transform, false);
        Image shipPartImage = shipPart.GetComponent<Image>();
        shipPartImage.sprite = Resources.Load<Sprite>("HUD_Sprites/Buttons/HUD_ShipPart");
        shipPartImage.type = Image.Type.Simple;
        shipPartImage.preserveAspect = true;

        if (player.number == 1)
        {
            shipPart.layer = 11;
        }
        else
        {
            shipPart.layer = 12;
        }
    }

    private void CreateHUDDimensions(ref GameObject[] HUD_Dimensions)
    {
        HUD_Dimensions = new GameObject[OverworldData.DimensionsCount];

        for (int i = 0; i < OverworldData.DimensionsCount; i++)
        {
            GameObject HUD_Dimension = Instantiate(HUD_DimensionObj) as GameObject;
            HUD_Dimension.name = "HUDDimension0" + (i + 1);
            HUD_Dimension.transform.SetParent(HUD_DimensionsObj.transform, false);
            HUD_Dimension.transform.position += new Vector3(0, i * 40, 0);
            HUD_DimensionNo = HUD_Dimension.GetComponentInChildren<TextMeshProUGUI>();
            HUD_DimensionNo.text = "0" + (i + 1);
            HUD_Dimensions[i] = HUD_Dimension;

            if (i == 0)
            {
                GetComponent<HUD_Manager>().SetHUDDimension(0);
            }
        }
    }
}
