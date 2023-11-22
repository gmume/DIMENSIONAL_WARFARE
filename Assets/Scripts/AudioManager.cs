using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using static UnityEngine.InputSystem.InputAction;

public class AudioManager : MonoBehaviour
{
    [Header("Audio sources")]
    public AudioSource backgroundSound;
    public AudioSource SFXSource1;
    public AudioSource SFXSource2;
    public AudioSource text;

    [Header("Audio clips")]
    public AudioClip background;

    public AudioClip attack;
    public AudioClip explosion;
    public AudioClip shipUp;
    public AudioClip shipDown;
    public AudioClip chooseShip;
    public AudioClip dimensionUp;
    public AudioClip dimensionDown;
    public AudioClip moveShip;
    public AudioClip victorySound;

    public AudioClip text_01;

    public void Start()
    {
        backgroundSound.clip = background;
        backgroundSound.Play();
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
            SFXSource1.clip = attack;
            SFXSource1.Play();

            SFXSource2.clip = explosion;
            SFXSource2.PlayDelayed(1.2f);
        }
    }

    public void OnDimensionUp(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            SFXSource1.clip = dimensionUp;
            SFXSource1.Play();
        }
    }

    public void OnDimensionDown(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            SFXSource1.clip = dimensionDown;
            SFXSource1.Play();
        }
    }

    private void ChooseShip()
    {
        SFXSource1.clip = chooseShip;
        SFXSource1.Play();
    }

    public void OnShipUp()
    {
        SFXSource1.clip = shipUp;
        SFXSource1.Play();
    }

    public void OnVictory()
    {
        SFXSource1.clip = victorySound;
        SFXSource1.Play();
    }

    public void OnFireWithSunkenShip()
    {
        text.clip = text_01;
        text.Play();
    }
}
