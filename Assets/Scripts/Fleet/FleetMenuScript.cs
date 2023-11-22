using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class FleetMenuScript : MonoBehaviour
{
    private Player player;
    private GameObject shipButtonsObj;
    private List<GameObject> shipButtons;
    private string x = "--";
    private string y = "--";
    //private string HUDDimensionNo = "01";
    private TextMeshProUGUI xCoord;
    private TextMeshProUGUI yCoord;
    //private TextMeshProUGUI HUDDimension;
    //private GameObject HUDDimensionsHeader;
    //private GameObject[] HUDDimensions;
    private int currentHUDDimension;

    public GameObject playerObj;
    public ShipButton currentButton;
    public GameObject selectedButton;
    public GameObject selectedElement;

    private void Update()
    {
        xCoord.text = x;
        yCoord.text = y;
        //HUDDimension.text = HUDDimensionNo;
    }

    public void SetHUDDimension(int toNo)
    {
        int difference = toNo - currentHUDDimension;
        currentHUDDimension += difference;
        //HUDDimensions[currentHUDDimension].SetActive(true);
        UpdateFleetMenuDimension(currentHUDDimension);
    }

    public void OnDimensionUp(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (currentHUDDimension + 1 < OverworldData.DimensionsCount)
            {
                SetHUDDimension(currentHUDDimension + 1);
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
            if (currentHUDDimension - 1 >= 0)
            {
                SetHUDDimension(currentHUDDimension - 1);
            }
            else
            {
                Debug.LogWarning(name + ": Desired dimension out of scope!");
            }
        }
    }

    public void UpdateFleetMenuCoords(int xCoord, int yCoord)
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

    public void UpdateFleetMenuCoords()
    {
        x = "--";
        y = "--";
    }

    private void UpdateFleetMenuDimension(int dimension)
    {
        //HUDDimensionNo = "0" + (dimension + 1).ToString();
    }

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
        if (name == "FleetMenu1")
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
        button.image.sprite = Resources.Load<Sprite>("HUD_Elemente/ButtonElements/Button") as Sprite;
        button.image.SetNativeSize();
        button.transition = Selectable.Transition.SpriteSwap;
        Sprite buttonSelected = Resources.Load<Sprite>("HUD_Elemente/ButtonElements/Selection") as Sprite;
        Sprite buttonDisabled = Resources.Load<Sprite>("HUD_Elemente/ButtonElements/Disabled") as Sprite;

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
        shipPartImage.sprite = Resources.Load<Sprite>("HUD_Elemente/ButtonElements/ShipPart") as Sprite;
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
        //HUDDimensions = new GameObject[OverworldData.DimensionsCount];

        //for (int i = 0; i < OverworldData.DimensionsCount; i++)
        //{
        //    GameObject HUDDimension = new("HUDDimension0" + (i + 1), typeof(CanvasRenderer), typeof(Image));
        //    HUDDimension.transform.SetParent(HUDDimensionsHeader.transform, false);

        //    Image HUDDimensionImage = HUDDimension.GetComponent<Image>();
        //    HUDDimensionImage.sprite = Resources.Load<Sprite>("HUD_Elemente/Levels/Dimension0" + (i + 1)) as Sprite;
        //    HUDDimensionImage.type = Image.Type.Simple;
        //    HUDDimensionImage.SetNativeSize();
        //    HUDDimensions[i] = HUDDimension;
        //    HUDDimensions[i] = HUDDimension;

        //    if (i != 0)
        //    {
        //        HUDDimension.SetActive(false);
        //    }
        //}
    }

    public void InitFleetMenu()
    {
        player = playerObj.GetComponent<Player>();

        GameObject[] fleetMenuParts;

        if (player.number == 1)
        {
            fleetMenuParts = GameObject.FindGameObjectsWithTag("FleetMenu1");
        }
        else
        {
            fleetMenuParts = GameObject.FindGameObjectsWithTag("FleetMenu2");
        }

        shipButtons = new List<GameObject>();

        foreach (GameObject fleetMenuPart in fleetMenuParts)
        {
            if (fleetMenuPart.name == "X-Koordinate")
            {
                xCoord = fleetMenuPart.GetComponent<TextMeshProUGUI>();
            }
            else if (fleetMenuPart.name == "Y-Koordinate")
            {
                yCoord = fleetMenuPart.GetComponent<TextMeshProUGUI>();
            }
            else if (fleetMenuPart.name == "ShipButtons")
            {
                shipButtonsObj = fleetMenuPart;
            }
        }

        //if (player.number == 1)
        //{
        //    HUDDimension = GameObject.Find("Dimension1").GetComponent<TextMeshProUGUI>();
        //    HUDDimensionsHeader = GameObject.Find("DimensionsHeader1");
        //}
        //else
        //{
        //    HUDDimension = GameObject.Find("Dimension2").GetComponent<TextMeshProUGUI>();
        //    HUDDimensionsHeader = GameObject.Find("DimensionsHeader2");
        //}

        CreateShipButtons();
        CreateHUDDimensions();
        currentHUDDimension = 0;
        player.fleet.ActivateShip(currentButton.ShipButtonNr, player);
    }
}
