using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryAnnouncer : MonoBehaviour
{
    private void Start()
    {
        GetComponent<RawImage>().texture = (IsWinner()) ? Resources.Load<Texture>("HUD_Sprites/Win") : Resources.Load<Texture>("HUD_Sprites/Loose");
    }

    private bool IsWinner()
    {
        return GameData.winner == "Player1" && transform.parent.name == "HUD1" || GameData.winner == "Player2" && transform.parent.name == "HUD2";
    }
}
