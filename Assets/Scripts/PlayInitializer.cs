using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayInitializer : MonoBehaviour
{
    public PlayerData player;
    public FadeEffect fadeEffect;
    public HUD_Initializer HUD_Initializer;

    public void Start()
    {
        HUD_Initializer.Initialize();
        player.fleet.Initialize(player);
        
        Invoke("InitializeDimensions", 0.3f);
        Invoke("StartFade", 0.3f);
    }

    private void InitializeDimensions()
    {
        player.dimensions.Initialize();
        player.dimensions.CreateDimensions();
        player.world.SetNewDimension(0);
    }

    private void StartFade() => fadeEffect.StartEffect();
}
