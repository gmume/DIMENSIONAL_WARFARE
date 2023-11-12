using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinnerOrLooser : MonoBehaviour
{
    private void Start()
    {
        
        if (GameData.winner == "Player1" && transform.parent.name == "FleetMenu1" || GameData.winner == "Player2" && transform.parent.name == "FleetMenu2")
        {
            GetComponent<RawImage>().texture = Resources.Load<Texture>("HUD_Elemente/Header Footer/Win") as Texture;
        }
        else
        {
            GetComponent<RawImage>().texture = Resources.Load<Texture>("HUD_Elemente/Header Footer/Loose") as Texture;
        }
    }
}
