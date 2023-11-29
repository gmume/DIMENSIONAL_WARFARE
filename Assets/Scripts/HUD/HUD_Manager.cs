using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class HUD_Manager : MonoBehaviour
{
    private Player player;
    
    private string x = "--", y = "--";
    private TextMeshProUGUI xCoord, yCoord;
    
    private GameObject[] HUD_Dimensions;
    private int currentHUD_Dimension;
    private TextMeshProUGUI HUD_DimensionNo;
    
    private GameObject shipButtonsObj;
    private List<GameObject> shipButtons;

    public GameObject HUD_DimensionsObj;
    public GameObject HUD_DimensionObj;
    public Texture2D HUD_DimensionInactive;
    public Texture2D HUD_DimensionActive;

    public GameObject playerObj;
    public ShipButton currentButton;
    public GameObject selectedButton, selectedElement;

    private void Update()
    {
        xCoord.text = x;
        yCoord.text = y;
    }

    public void SetHUDDimension(int toNo)
    {
        GameObject HUD_DimensionToSetInactive = HUD_Dimensions[currentHUD_Dimension];
        HUD_DimensionToSetInactive.GetComponent<RawImage>().texture = HUD_DimensionInactive;
        HUD_DimensionToSetInactive.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
        HUD_DimensionToSetInactive.GetComponentInChildren<TextMeshProUGUI>().fontSize = 22;

        GameObject HUD_DimensionToSetActive = HUD_Dimensions[toNo];
        HUD_DimensionToSetActive.GetComponent<RawImage>().texture = HUD_DimensionActive;
        HUD_DimensionToSetActive.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
        HUD_DimensionToSetActive.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Underline;
        HUD_DimensionToSetActive.GetComponentInChildren<TextMeshProUGUI>().fontSize = 28;

        currentHUD_Dimension = toNo;
    }

    public void OnDimensionUp(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (currentHUD_Dimension + 1 < OverworldData.DimensionsCount)
            {
                SetHUDDimension(currentHUD_Dimension + 1);
            }
            else
            {
                Debug.LogWarning(name + ": Desired dimension out of scope!");
            }
        }
    }

    public void OnDimensionDown(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (currentHUD_Dimension - 1 >= 0)
            {
                SetHUDDimension(currentHUD_Dimension - 1);
            }
            else
            {
                Debug.LogWarning(name + ": Desired dimension out of scope!");
            }
        }
    }

    public void UpdateHUDCoords(int xCoord, int yCoord)
    {
        if (xCoord.ToString().Length < 2)
        {
            x = "0" + xCoord.ToString();
        }
        else
        {
            x = xCoord.ToString();
        }

        if (yCoord.ToString().Length < 2)
        {
            y = "0" + yCoord.ToString();
        }
        else
        {
            y = yCoord.ToString();
        }
    }

    public void UpdateHUDCoords()
    {
        x = "--";
        y = "--";
    }

    //private void UpdateHUDDimension(int dimension)
    //{
    //    HUD_Dimensions[currentHUD_Dimension].GetComponent<RawImage>().texture = HUD_DimensionInactive;
    //    HUD_Dimensions[dimension].GetComponent<RawImage>().texture = HUD_DimensionActive;
    //}

    public List<GameObject> GetShipButtons()
    {
        return shipButtons;
    }

    private void CreateShipButtons()
    {
        GameObject buttonObj;
        Button button;

        for (int i = 0; i < OverworldData.FleetSize; i++)
        {
            buttonObj = TMP_DefaultControls.CreateButton(new TMP_DefaultControls.Resources());
            Transform textObject = buttonObj.transform.Find("Text (TMP)");
            UnityEngine.Object.Destroy(textObject.gameObject);
            button = buttonObj.GetComponent<Button>();

            CreateButton(buttonObj, button, i);
            DesignButton(button);

            for (int j = 0; j <= i; j++)
            {
                CreateShipPart(buttonObj);
            }
        }
    }

    private void CreateButton(GameObject buttonObj, Button button, int i)
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
            SetSelecetedButton();
        }

        shipButtons.Add(buttonObj);
    }

    public void SetSelecetedButton()
    {
        if (!selectedButton)
        {
            selectedButton = shipButtons[0];
        }

        player.eventSystem.firstSelectedGameObject = selectedButton;
        player.eventSystem.SetSelectedGameObject(selectedButton);
        currentButton = selectedButton.GetComponent<ShipButton>();
        player.CurrentShipButton = currentButton;
    }

    public void RemoveButton(int index)
    {
        Destroy(shipButtons[index]);
        shipButtons.RemoveAt(index);
    }

    private void DesignButton(Button button)
    {
        button.image.type = Image.Type.Simple;
        button.image.sprite = Resources.Load<Sprite>("HUD/Buttons/Button");
        button.image.SetNativeSize();
        button.transition = Selectable.Transition.SpriteSwap;
        Sprite buttonSelected = Resources.Load<Sprite>("HUD/Buttons/Selection");
        Sprite buttonDisabled = Resources.Load<Sprite>("HUD/Buttons/Disabled");

        SpriteState spriteState = new()
        {
            selectedSprite = buttonSelected,
            disabledSprite = buttonDisabled
        };
        button.spriteState = spriteState;
    }

    private void CreateShipPart(GameObject buttonObj)
    {
        GameObject shipPart = new("ShipPart", typeof(CanvasRenderer), typeof(Image));
        shipPart.transform.SetParent(buttonObj.transform, false);

        Image shipPartImage = shipPart.GetComponent<Image>();
        shipPartImage.sprite = Resources.Load<Sprite>("HUD/Buttons/ShipPart");
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

    private void CreateHUDDimensions()
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
                SetHUDDimension(0);
            }
        }
    }

    public void InitHUD()
    {
        player = playerObj.GetComponent<Player>();
        shipButtons = new List<GameObject>();
        GameObject[] HUD_Parts;

        if (player.number == 1)
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
            } else if (name == "HUD2" && HUD_Part.name == "Armed")
            {
                HUD_Part.SetActive(false);
            }
        }

        CreateHUDDimensions();
        currentHUD_Dimension = 0;

        CreateShipButtons();
        player.fleet.ActivateShip(currentButton.ShipButtonNr, player);
    }
}
