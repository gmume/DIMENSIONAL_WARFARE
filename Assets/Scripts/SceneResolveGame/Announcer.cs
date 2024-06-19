using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Announcer : MonoBehaviour
{
    public AudioPlayer audioPlayer;
    private bool isWinner;
    public Sprite BackgroundLoose;
    public Sprite BackgroungWin;
    private readonly string win = "You win!";
    private readonly string loose = "You loose!";

    private void Start()
    {
        isWinner = IsWinner();

        GetComponent<Image>().sprite = isWinner ? BackgroungWin : BackgroundLoose;
        GetComponentInChildren< TextMeshProUGUI>().text = isWinner ? win : loose;
        Invoke("PlayVictorySound", 0.5f);
    }

    private void PlayVictorySound()
    {
        //audioPlayer.OnVictory();
    }

    private bool IsWinner() => GameData.winner == "Player1" && transform.parent.name == "HUD1" || GameData.winner == "Player2" && transform.parent.name == "HUD2";
}
