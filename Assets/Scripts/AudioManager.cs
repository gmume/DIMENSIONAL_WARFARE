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

    [Header("Audio clips")]
    public AudioClip background;
    public AudioClip attack;
    public AudioClip explosion;
    public AudioClip shipUp;
    public AudioClip shipDown;
    public AudioClip chooseShip;
    public AudioClip moveShip;

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
           
        }
    }

    public void OnShipRight(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            
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
}
