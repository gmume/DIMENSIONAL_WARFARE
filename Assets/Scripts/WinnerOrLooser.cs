using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinnerOrLooser : MonoBehaviour
{
    private void Start()
    {
        
        if (GameData.winner == "Player1" && transform.parent.name == "HUD1" || GameData.winner == "Player2" && transform.parent.name == "HUD2")
        {
            GetComponent<RawImage>().texture = Resources.Load<Texture>("HUD/Header Footer/Win");
        }
        else
        {
            GetComponent<RawImage>().texture = Resources.Load<Texture>("HUD/Header Footer/Loose");
        }
    }
}
