using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.Users;

public class InputHandling : MonoBehaviour
{
    private PlayerScript playerScript, opponent;
    private PlayerInput playerInput, opponentInput;
    private InputActionMap gameStartMap, playerMap, fleetMenuMap;
    //private InputAction moveShipAction;
    private CameraBehavior cameraBehavior1, cameraBehavior2;
    private FleetMenuScript fleetMenuScript1, fleetMenuScript2;

    private void Awake()
    {
        playerScript = GetComponent<PlayerScript>();
        playerInput = GetComponent<PlayerInput>();
        gameStartMap = playerInput.actions.FindActionMap("GameStart");
        playerMap = playerInput.actions.FindActionMap("Player");
        fleetMenuMap = playerInput.actions.FindActionMap("FleetMenu");
       // moveShipAction = gameStartMap.FindAction("MoveShip", true);
       //// moveShipAction.performed += OnMoveShip;
       // Debug.Log("moveShipAction: " + moveShipAction);
    }

    private void Start()
    {
        
        cameraBehavior1 = GameObject.Find("Camera1").GetComponent<CameraBehavior>();
        cameraBehavior2 = GameObject.Find("Camera2").GetComponent<CameraBehavior>();
        fleetMenuScript1 = GameObject.Find("FleetMenu1").GetComponent<FleetMenuScript>();
        fleetMenuScript2 = GameObject.Find("FleetMenu2").GetComponent<FleetMenuScript>();

        ArrayList devices = new();

        foreach (var device in InputSystem.devices)
        {
            if (device.ToString().Contains("Gamepad")){
                devices.Add(device);
            }
        }

        if(devices.Count >= 2)
        {
            playerInput.user.UnpairDevices();

            if (name == "Player1")
            {
                InputUser.PerformPairingWithDevice((InputDevice)devices[0], playerInput.user);
                opponent = GameObject.Find("Player2").GetComponent<PlayerScript>();
            }
            else
            {
                InputUser.PerformPairingWithDevice((InputDevice)devices[1], playerInput.user);
                opponent = GameObject.Find("Player1").GetComponent<PlayerScript>();
            }

            opponentInput = opponent.GetComponent<PlayerInput>();
        }
        else
        {
            Debug.Log("Gamepad missing!");
        }

        playerInput.SwitchCurrentActionMap("FleetMenu");
        SwitchActionMap("FleetMenu");
    }

    //private void OnEnable()
    //{
    //    if(OverworldData.GamePhase == GamePhases.Start)
    //    {
    //        Debug.Log("entred if GamePhases.Start");
    //        moveShipAction.Disable();
    //        Debug.Log("moveShipAction: " + moveShipAction+", enabled? " +moveShipAction.enabled);
            
    //    }
        
    //}

    //StartGame actionMap
    public void OnMoveShip(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Vector2 vector = ctx.ReadValue<Vector2>();
            float x = vector.x;
            float y = vector.y;

            //Get right axis
            if (Math.Abs(x) > Math.Abs(y))
            {
                if (x > 0)
                {
                    playerScript.playerData.ActiveShip.Move(1, 0);
                }
                else
                {
                    playerScript.playerData.ActiveShip.Move(-1, 0);
                }
            }
            else
            {
                if (y > 0)
                {
                    playerScript.playerData.ActiveShip.Move(0, 1);
                }
                else
                {
                    playerScript.playerData.ActiveShip.Move(0, -1);
                }
            }

            fleetMenuScript1.UpdateFleetMenuCoords(playerScript.playerData.ActiveShip.X, playerScript.playerData.ActiveShip.Z);
            fleetMenuScript2.UpdateFleetMenuCoords(playerScript.playerData.ActiveShip.X, playerScript.playerData.ActiveShip.Z);
        }
    }

    public void OnTurnLeft(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            //Turn ship left
            playerScript.playerData.ActiveShip.GetComponent<Transform>().Rotate(0, -90, 0);
        }
    }

    public void OnTurnRight(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            //Turn ship right
            playerScript.playerData.ActiveShip.GetComponent<Transform>().Rotate(0, 90, 0);
        }
    }

    public void OnSubmitFleet(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            MultiplayerEventSystem eventSystem;

            if (name == "Player1")
            {
                eventSystem = GameObject.Find("EventSystem1").GetComponent<MultiplayerEventSystem>();
            }
            else
            {
                eventSystem = GameObject.Find("EventSystem2").GetComponent<MultiplayerEventSystem>();
            }

            eventSystem.SetSelectedGameObject(null);

            if (playerScript.playerData.ActiveShip != null)
            {
                playerScript.playerData.ActiveShip.Deactivate(playerScript);
            }

            if (name == "Player1")
            {
                OverworldData.Player1SubmittedFleet = true;
            }
            else
            {
                OverworldData.Player2SubmittedFleet = true;
            }

            if (!OverworldData.Player1SubmittedFleet || !OverworldData.Player2SubmittedFleet)
            {
                Debug.Log("Please, wait until your opponent is ready.");
                playerInput.enabled = false;
                StartCoroutine(WaitForOpponent());
            }

            fleetMenuScript1.UpdateFleetMenuCoords();
            fleetMenuScript2.UpdateFleetMenuCoords();
        }
    }

    private IEnumerator WaitForOpponent()
    {
        yield return new WaitUntil(() => (OverworldData.Player1SubmittedFleet && OverworldData.Player2SubmittedFleet));
        Debug.Log("Your opponent is ready. Let's go!");

        CameraBehavior behavior1 = GameObject.Find("Camera1").GetComponent<CameraBehavior>();
        behavior1.UpdateCamera(GamePhases.Armed);
        CameraBehavior behavior2 = GameObject.Find("Camera2").GetComponent<CameraBehavior>();
        behavior2.UpdateCamera(GamePhases.Attacked);

        OverworldData.GamePhase = GamePhases.Battle;
        playerInput.enabled = true;

        if(playerScript.name == "Player1")
        {
            playerInput.SwitchCurrentActionMap("FleetMenu");
            opponentInput.SwitchCurrentActionMap("Player");
        }
        else
        {
            playerInput.SwitchCurrentActionMap("Player");
            opponentInput.SwitchCurrentActionMap("FleetMenu");
        }
        
        //SwitchActionMap("Player");
    }

    //StartGame and Player actionMap
    public void OnReturnToFleetMenu(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            playerScript.playerData.ActiveShip.Deactivate(playerScript);
            playerInput.SwitchCurrentActionMap("FleetMenu");

            if (name == "Player1")
            {
                GameObject.Find("FleetMenu1").GetComponent<FleetMenuScript>().SetFirstSelecetedButton();
            }
            else
            {
                GameObject.Find("FleetMenu2").GetComponent<FleetMenuScript>().SetFirstSelecetedButton();
            }
        }
    }

    //Player actionMap
    //public void OnSubmitAttackingShip(CallbackContext ctx)
    //{
    //    if (ctx.performed)
    //    {
    //        Debug.Log("OnSubmit!");
    //    }
    //}

    public void OnMoveSelection(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (name == "Player1" && OverworldData.PlayerTurn == 1 || name == "Player2" && OverworldData.PlayerTurn == 2)
            {
                //Change activeted cell
                Vector2 vector = ctx.ReadValue<Vector2>();
                float x = vector.x;
                float y = vector.y;

                //Get right axis
                if (Math.Abs(x) > Math.Abs(y))
                {
                    //negative or positive?
                    if (x > 0)
                    {
                        playerScript.SetNewCell(1, 0);
                    }
                    else
                    {
                        playerScript.SetNewCell(-1, 0);
                    }
                }
                else
                {
                    if (y > 0)
                    {
                        playerScript.SetNewCell(0, 1);
                    }
                    else
                    {
                        playerScript.SetNewCell(0, -1);
                    }
                }

                fleetMenuScript1.UpdateFleetMenuCoords(playerScript.playerData.ActiveCell.X, playerScript.playerData.ActiveCell.Y);
                fleetMenuScript2.UpdateFleetMenuCoords(playerScript.playerData.ActiveCell.X, playerScript.playerData.ActiveCell.Y);
            }
            else
            {
                print("It's not your turn!");
            }
        }
    }

    public void OnFire(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (name == "Player1" && OverworldData.PlayerTurn == 1 || name == "Player2" && OverworldData.PlayerTurn == 2)
            {
                ////Fire on selected cell
                //Cell activeCell = playerScript.playerData.ActiveCell;
                //Material cellMaterial = activeCell.GetComponent<Renderer>().material;

                //if (name == "Player1")
                //{
                //    cellMaterial.color = Color.red;

                //}
                //else
                //{
                //    cellMaterial.color = Color.yellow;

                //}

                //playerScript.playerData.ActiveCell.Hitted = true;

                playerScript.playerData.ActiveShip.Fire(playerScript);

                //Pause(3);
                if (name == "Player1")
                {
                    OverworldData.PlayerTurn = 2;
                    cameraBehavior1.UpdateCamera(GamePhases.Attacked);
                    cameraBehavior2.UpdateCamera(GamePhases.Armed);
                }
                else
                {
                    OverworldData.PlayerTurn = 1;
                    cameraBehavior2.UpdateCamera(GamePhases.Attacked);
                    cameraBehavior1.UpdateCamera(GamePhases.Armed);
                }
                playerInput.SwitchCurrentActionMap("Player");
                opponentInput.SwitchCurrentActionMap("FleetMenu");
            }
            else
            {
                print("It's not your turn, yet!");
            }
        }
    }

    //public void Pause(float pauseTime)
    //{
    //    playerInput.enabled = false;
    //    opponentInput.enabled = false;
    //    float pauseEndTime = Time.realtimeSinceStartup + pauseTime;
    //    Time.timeScale = 0f;
    //    while (Time.realtimeSinceStartup < pauseEndTime)
    //    {
    //        //paused
    //    }
    //    Time.timeScale = 1f;

    //    if (name == "Player1")
    //    {
    //        OverworldData.PlayerTurn = 2;
    //        cameraBehavior1.UpdateCamera(GamePhases.Attacked);
    //        cameraBehavior2.UpdateCamera(GamePhases.Armed);
    //    }
    //    else
    //    {
    //        OverworldData.PlayerTurn = 1;
    //        cameraBehavior2.UpdateCamera(GamePhases.Attacked);
    //        cameraBehavior1.UpdateCamera(GamePhases.Armed);
    //    }

    //    playerInput.enabled = true;
    //    opponentInput.enabled = true;
    //}

    //public IEnumerator PauseGame(float pauseTime)
    //{
    //    Time.timeScale = 0f;
    //    float pauseEndTime = Time.realtimeSinceStartup + pauseTime;
    //    while (Time.realtimeSinceStartup < pauseEndTime)
    //    {
    //        yield return 0;
    //    }
    //    Time.timeScale = 1f;
    //}

    public void SwitchActionMap(string actionMapName)
    {
        switch (actionMapName)
        {
            case "GameStart":
                if (!gameStartMap.enabled)
                {
                    gameStartMap.Enable();
                }
                if (playerMap.enabled)
                {
                    playerMap.Disable();
                }
                if (fleetMenuMap.enabled)
                {
                    fleetMenuMap.Disable();
                }
                break;
            case "Player":
                if (gameStartMap.enabled)
                {
                    gameStartMap.Disable();
                }
                if (!playerMap.enabled)
                {
                    playerMap.Enable();
                }
                if (fleetMenuMap.enabled)
                {
                    fleetMenuMap.Disable();
                }
                break;
            case "FleetMenu":
                if (gameStartMap.enabled)
                {
                    gameStartMap.Disable();
                }
                if (playerMap.enabled)
                {
                    playerMap.Disable();
                }
                if (!fleetMenuMap.enabled)
                {
                    fleetMenuMap.Enable();
                }
                break;
            default:
                Debug.Log("No such action map!");
                break;
        }
    }
}
