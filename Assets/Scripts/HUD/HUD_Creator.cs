using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Creator : MonoBehaviour
{
    public PlayerData player;

    public GameObject HUD_DimensionsObj, HUD_DimensionsOpponentObj, HUD_DimensionObj, HUD_ShipPrefab;
    private TextMeshProUGUI HUD_DimensionNo;
    public Texture2D[] HUD_ShipTextures;

    private Transform shipButtonsTransform;

    public void CreateHUD(ref List<GameObject> shipButtons,
                          ref TextMeshProUGUI xCoord,
                          ref TextMeshProUGUI yCoord,
                          ref GameObject[] HUD_Dimensions,
                          ref int currentHUD_Dimension,
                          ref GameObject[] HUD_Fleet,
                          ref GameObject[] HUD_DimensionsOpponent,
                          ref int currentHUD_DimensionOpponent,
                          ref GameObject[] HUD_FleetOpponent,
                          ref GameObject armed,
                          ref TextMeshProUGUI crewText,
                          ref ShipButton currentButton,
                          ref GameObject selectedButton)
    {
        GameObject[] HUD_Parts = player.name == "Player1" ? GameObject.FindGameObjectsWithTag("HUD1") : GameObject.FindGameObjectsWithTag("HUD2");

        foreach (GameObject HUD_Part in HUD_Parts)
        {
            switch (HUD_Part.name)
            {
                case "X-Koordinate":
                    xCoord = HUD_Part.GetComponent<TextMeshProUGUI>();
                    break;
                case "Y-Koordinate":
                    yCoord = HUD_Part.GetComponent<TextMeshProUGUI>();
                    break;
                case "HUD_Dimensions":
                    HUD_DimensionsObj = HUD_Part;
                    break;
                case "HUD_DimensionsOpponent":
                    HUD_DimensionsOpponentObj = HUD_Part;
                    break;
                case "Armed":
                    armed = HUD_Part;
                    HUD_Part.SetActive(false);
                    break;
                case "CrewText":
                    crewText = HUD_Part.GetComponent<TextMeshProUGUI>();
                    break;
                case "ShipButtons":
                    shipButtonsTransform = HUD_Part.GetComponent<Transform>();
                    break;
            }
        }

        string color1 = (name == "HUD1") ? "brown" : "olive";
        string color2 = (name == "HUD1") ? "olive" : "brown";

        CreateHUDDimensions(ref HUD_Dimensions, HUD_DimensionsObj, ref HUD_Fleet, color1);
        CreateHUDDimensions(ref HUD_DimensionsOpponent, HUD_DimensionsOpponentObj, ref HUD_FleetOpponent, color2);

        currentHUD_Dimension = currentHUD_DimensionOpponent = 0;

        CreateShipButtons(ref selectedButton, ref shipButtons);
        player.fleet.ActivateShip(currentButton.ShipButtonNr, player);
    }

    private void CreateHUDDimensions(ref GameObject[] HUD_Dimensions, GameObject HUD_DimensionsObj, ref GameObject[] HUD_Fleet, string shipColor)
    {
        HUD_Dimensions = new GameObject[OverworldData.DimensionsCount];

        for (int i = 0; i < OverworldData.DimensionsCount; i++)
        {
            GameObject HUD_Dimension = Instantiate(HUD_DimensionObj) as GameObject;
            HUD_Dimension.name = "HUDDimension0" + (i + 1);
            HUD_Dimension.transform.SetParent(HUD_DimensionsObj.transform, false);

            HUD_Dimension.layer = HUD_DimensionsObj.layer;
            HUD_Dimension.transform.position += new Vector3(0, i * 40, 0);
            HUD_DimensionNo = HUD_Dimension.GetComponentInChildren<TextMeshProUGUI>();
            HUD_DimensionNo.text = "0" + (i + 1);
            HUD_Dimensions[i] = HUD_Dimension;

            if (i == 0)
            {
                GetComponent<HUD_Manager>().SetHUDDimension(0);
                CreateHUDFleet(ref HUD_Fleet, HUD_Dimension, shipColor);
            }
        }
    }

    private void CreateHUDFleet(ref GameObject[] HUD_Fleet, GameObject HUD_Dimension, string shipColor)
    {
        HUD_Fleet = new GameObject[OverworldData.FleetSize];

        for (int i = 0; i < OverworldData.FleetSize; i++)
        {
            GameObject HUD_Ship = Instantiate(HUD_ShipPrefab, HUD_Dimension.transform.position, Quaternion.identity);
            HUD_Ship.name = "HUD_Ship" + (i + 1);
            HUD_Ship.transform.SetParent(HUD_Dimension.transform);
            HUD_Ship.layer = HUD_Dimension.layer;
            HUD_Ship.GetComponent<RawImage>().texture = HUD_ShipTextures[i];
            HUD_Ship.GetComponent<RawImage>().color = (shipColor == "brown") ? new Color(0.3f, 0.12f, 0, 1) : new Color(0.3f, 0.3f, 0, 1); // brown or olive

            Vector3 xPosition = new(i * 30, 0, -1);
            HUD_Ship.transform.position += xPosition;
            HUD_Fleet[i] = HUD_Ship;
        }
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
        buttonObj.name = "ShipButton" + (i + 1);
        buttonObj.layer = (name == "HUD1") ? 11 : 12;

        Transform transformParent = shipButtonsTransform;
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
        shipPart.layer = (player.number == 1) ? 11 : 12;
    }
}
