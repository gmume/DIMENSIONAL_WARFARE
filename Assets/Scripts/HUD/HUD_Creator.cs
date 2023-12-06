using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Creator : MonoBehaviour
{
    public PlayerData player;

    public GameObject HUD_DimensionsObj;
    public GameObject HUD_DimensionObj;
    private TextMeshProUGUI HUD_DimensionNo;
    public GameObject HUD_ShipPrefab;
    public Texture2D[] HUD_ShipTextures;
    
    private Transform shipButtonsTransform;

    public void CreateHUD(ref List<GameObject> shipButtons,
                          ref TextMeshProUGUI xCoord,
                          ref TextMeshProUGUI yCoord,
                          ref int currentHUD_Dimension,
                          ref GameObject[] HUD_Fleet,
                          ref GameObject armed,
                          ref TextMeshProUGUI crewText,
                          ref ShipButton currentButton,
                          ref GameObject selectedButton,
                          ref GameObject[] HUD_Dimensions)
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
            else if (HUD_Part.name == "HUD_Dimensions")
            {
                HUD_DimensionsObj = HUD_Part;
            }
            else if (HUD_Part.name == "Armed")
            {
                armed = HUD_Part;
                HUD_Part.SetActive(false);
            }
            else if (HUD_Part.name == "CrewText")
            {
                crewText = HUD_Part.GetComponent<TextMeshProUGUI>();
            }
            else if (HUD_Part.name == "ShipButtons")
            {
                shipButtonsTransform = HUD_Part.GetComponent<Transform>();
            }
        }

        CreateHUDDimensions(ref HUD_Dimensions, ref HUD_Fleet);
        currentHUD_Dimension = 0;

        

        CreateShipButtons(ref selectedButton, ref shipButtons);
        player.fleet.ActivateShip(currentButton.ShipButtonNr, player);
    }

    private void CreateHUDDimensions(ref GameObject[] HUD_Dimensions, ref GameObject[] HUD_Fleet)
    {
        HUD_Dimensions = new GameObject[OverworldData.DimensionsCount];

        for (int i = 0; i < OverworldData.DimensionsCount; i++)
        {
            GameObject HUD_Dimension = Instantiate(HUD_DimensionObj) as GameObject;
            HUD_Dimension.name = "HUDDimension0" + (i + 1);
            HUD_Dimension.transform.SetParent(HUD_DimensionsObj.transform, false);

            //Debug.Log(name + " parent layer: " + transform.parent);
            HUD_Dimension.layer = HUD_DimensionsObj.layer;
            HUD_Dimension.transform.position += new Vector3(0, i * 40, 0);
            HUD_DimensionNo = HUD_Dimension.GetComponentInChildren<TextMeshProUGUI>();
            HUD_DimensionNo.text = "0" + (i + 1);
            HUD_Dimensions[i] = HUD_Dimension;

            if (i == 0)
            {
                GetComponent<HUD_Manager>().SetHUDDimension(0);
                CreateHUDFleet(ref HUD_Fleet, HUD_Dimension);
            }
        }
    }

    private void CreateHUDFleet(ref GameObject[] HUD_Fleet, GameObject HUD_Dimension)
    {
        HUD_Fleet = new GameObject[OverworldData.FleetSize];
        
        for (int i = 0; i < OverworldData.FleetSize; i++)
        {
            GameObject HUD_Ship = Instantiate(HUD_ShipPrefab, HUD_Dimension.transform.position, Quaternion.identity);
            HUD_Ship.name = "HUD_Ship" + (i + 1);
            HUD_Ship.transform.SetParent(HUD_Dimension.transform);
            HUD_Ship.layer = HUD_Dimension.layer;
            HUD_Ship.GetComponent<RawImage>().texture = HUD_ShipTextures[i];

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

        if (player.number == 1)
        {
            shipPart.layer = 11;
        }
        else
        {
            shipPart.layer = 12;
        }
    }

    
}
