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

    public void ShowTip(string forFirstTime)
    {
        if (!tips.ContainsKey(forFirstTime))
        {
            onboarding.SetActive(false);
            return;
        }

        ActivateTip();
        onboardingText.text = tips[forFirstTime];
        tips.Remove(forFirstTime);
    }

    private Dictionary<string, string> BuildDictionary() =>
    new()
    {
        {"DistributeShips", "Switch ships\t← →\r\nMove ship\tLeft stick\r\nTurn ship\tL1  R1\r\nReady to play\tX"},
        {"Attack", "Switch ships\t← →\nMove aim\tLeft stick\nFire\t\tR2"},
        {"UnderAttack", "Await your opponent's attack!"},
        {"OwnShipUp", "Your ship ascended to the upper dimension!\nMove ship\tLeft stick\r\nTurn ship\tL1  R1\r\nReady to play\tX\nSwitch dimensions\t↑ ↓"},
        {"OpponentShipUp", "Your opponent's ship ascended to the upper dimension!\nSwitch dimensions\t↑ ↓"},
        {"OwnShipDown", "Your ship descended to the lower dimension!\nMove ship\tLeft stick\r\nTurn ship\tL1  R1\r\nReady to play\tX\nSwitch dimensions\t↑ ↓"},
        {"OwnShipDestroyed", "Your ship is destroyed!"},
    };
}
