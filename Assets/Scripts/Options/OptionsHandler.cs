using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsHandler : MonoBehaviour
{
    public PlayerData player;
    private string lastActionMap;
    private GameObject optionsList;

    public void Start()
    {
        optionsList = transform.Find("OptionsList").gameObject;
        optionsList.SetActive(false);
    }

    public void ShowOptions()
    {
        Debug.Log("currentActionMap: " + player.input);

        lastActionMap = player.input.currentActionMap.name;
        player.input.SwitchCurrentActionMap("Options");
        player.onboarding.gameObject.SetActive(false);
        optionsList.SetActive(true);
    }
    
    public void CloseOptions()
    {
        player.input.SwitchCurrentActionMap(lastActionMap);
        player.onboarding.gameObject.SetActive(true);
        optionsList.SetActive(false);
    }
}
