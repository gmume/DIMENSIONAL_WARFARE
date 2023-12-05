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
    public AudioSource SFXSource1, SFXSource2, textSource;

    [Header("Path to file")]
    public string backgroundSoundFileName;

    [Header("Audio collection")]
    public AudioCollection audioCollection;

    public void Start()
    {
        audioCollection.Init();
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

    private void PlayClip(AudioSource source, AudioClip clip)
    {
        PlayClip(0, source, clip);
    }

    //StartGame actionMap
    public void OnShipLeft(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            ChooseShip();
        }
    }

    public void OnShipRight(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            ChooseShip();
        }
    }

    private void ChooseShip()
    {
        PlayClip(SFXSource1, audioCollection.SFXSounds["SFX_ChooseShip"]);
    }

    public void OnMoveShip(CallbackContext ctx)
    {
        if (ctx.performed)
        {

        }
    }

    public void OnTurnLeft(CallbackContext ctx)
    {
        if (ctx.performed)
        {

        }
    }

    public void OnTurnRight(CallbackContext ctx)
    {
        if (ctx.performed)
        {

        }
    }

    public void OnSubmitFleet(CallbackContext ctx)
    {
        if (ctx.performed)
        {

        }
    }

    //Player actionMap
    public void OnMoveSelection(CallbackContext ctx)
    {
        if (ctx.performed)
        {

        }
    }

    public void OnFire(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            PlayClip(SFXSource1, audioCollection.SFXSounds["SFX_NavalGun"]);
            PlayClip(1.2f, SFXSource2, audioCollection.SFXSounds["SFX_ImpactExplosion"]);
        }
    }

    public void OnDimensionUp(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            ChooseDimension();
        }
    }

    public void OnDimensionDown(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            ChooseDimension();
        }
    }

    private void ChooseDimension()
    {
        PlayClip(SFXSource1, audioCollection.SFXSounds["SFX_ChooseDimension"]);
    }

    public void OnShipUp()
    {
        PlayClip(SFXSource1, audioCollection.SFXSounds["SFX_ShipUp"]);
    }

    public void OnVictory()
    {
        PlayClip(SFXSource1, audioCollection.SFXSounds["SFX_VictorySound"]);
    }

    public void OnFireWithSunkenShip()
    {
        PlayClip(textSource, audioCollection.SFXSounds["TXT_01"]);
    }
}
