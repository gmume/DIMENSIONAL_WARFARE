using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OnboardingManager : MonoBehaviour
{
    public GameObject onboarding;
    public TextMeshProUGUI onboardingText;
    public Dictionary<string, string> tips;

    public void Start() => tips = BuildDictionary();

    public void ActivateTip() => onboarding.SetActive(true);

    public void DeactivateTip() => onboarding.SetActive(false);

    // Tip is showing permanently for the Werkschau.
    public void ShowTip(string forFirstTime)
    {
        //if (!tips.ContainsKey(forFirstTime))
        //{
            //onboarding.SetActive(false);
            //return;
        //}

        //ActivateTip();
        onboardingText.text = tips[forFirstTime];
        //tips.Remove(forFirstTime);
    }

    private Dictionary<string, string> BuildDictionary() =>
    new()
    {
        {"PlaceShips", "Switch ships\t◄ ►\r\nMove ship\tLeft stick\r\nTurn ship\tL1  R1\r\nPlace ship\tX"},
        {"SubmitFleet", "Start battle:\r\npress and hold\tX\r\nReturn\t\tO"},
        {"Wait", "Wait for your opponent\nto get ready!"},
        {"Attack", "Switch ships\t◄ ►\nMove aim\tLeft stick\nFire\t\tX\nSwitch dimensions\t▲ ▼"},
        {"UnderAttack", "Await your\nopponent's attack!\nSwitch dimensions\t▲ ▼"},
        {"OwnShipUp", "Your ship ascended\nto the upper dimension!\nMove ship\tLeft stick\r\nTurn ship\tL1  R1\r\nReady to play\tX\nSwitch dimensions\t▲ ▼"},
        {"OwnShipDown", "Caution!\nYour opponent's\nship leveled up.\nIt attacks harder now!\nYour ship descended\nto the lower dimension!\nMove ship\tLeft stick\r\nTurn ship\tL1  R1\r\nReady to play\tX\nSwitch dimensions\t▲ ▼"},
        {"OwnShipDestroyed", "Your ship is destroyed!\nCaution!\nYour opponent's\nship leveled up.\nIt attacks harder now!"},
    };
}
