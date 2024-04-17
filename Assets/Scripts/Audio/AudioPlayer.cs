using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using static UnityEngine.InputSystem.InputAction;

public class AudioPlayer : MonoBehaviour
{
    [Header("Audio sources")]
    public AudioSource backgroundSource;
    public AudioSource sfxSource1, sfxSource2, textSource;

    [Header("Path to file")]
    public string backgroundSoundFileName;

    [Header("Audio collection")]
    public AudioLoader audioCollection;

    public void Start()
    {
        audioCollection.Initialize();
        PlayClip(backgroundSource, audioCollection.backgroundSounds[backgroundSoundFileName]);
    }

    private void PlayClip(float deleyTime, AudioSource source, AudioClip clip)
    {
        if (clip)
        {
            source.clip = clip;

            if (deleyTime == 0)
            {
                source.Play();
            }
            else
            {
                source.PlayDelayed(deleyTime);
            }
        }
        else
        {
            Debug.Log("Clip not found!");
        }
    }

    public void PlayClip(AudioSource source, AudioClip clip) => PlayClip(0, source, clip);

    public void ChooseShip() => PlayClip(sfxSource1, audioCollection.sfxSounds["SFX_ChooseShip"]);

    public void OnMoveShip(CallbackContext ctx)
    {
        if (!ctx.performed) return;
    }

    public void OnTurnLeft(CallbackContext ctx)
    {
        if (!ctx.performed) return;
    }

    public void OnTurnRight(CallbackContext ctx)
    {
        if (!ctx.performed) return;
    }

    public void OnSubmitFleet(CallbackContext ctx)
    {
        if (!ctx.performed) return;
    }

    //Player actionMap
    public void OnMoveSelection(CallbackContext ctx)
    {
        if (!ctx.performed) return;
    }

    public void OnFire(CallbackContext ctx)
    {
        if (!ctx.performed) return;
        PlayClip(sfxSource1, audioCollection.sfxSounds["SFX_NavalGun"]);
        PlayClip(1.2f, sfxSource2, audioCollection.sfxSounds["SFX_ImpactExplosion"]);
    }

    public void ChooseDimension()
    {
        PlayClip(sfxSource1, audioCollection.sfxSounds["SFX_ChooseDimension"]);
    }

    public void OnShipUp() => PlayClip(sfxSource1, audioCollection.sfxSounds["SFX_ShipUp"]);

    public void OnVictory() => PlayClip(sfxSource1, audioCollection.sfxSounds["SFX_VictorySound"]);
}
