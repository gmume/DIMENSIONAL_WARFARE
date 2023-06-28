using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class VehicleBehavior : MonoBehaviour
{
    private Player player;
    private Vector3 vector;
    private Vector3 zoomOut;
    private bool zoomedOut = false;
    private Vector3 currentPosition;
    private int currentDimension;

    public void Start()
    {
        if (name == "CameraVehicle1")
        {
            player = OverworldData.player1;
        }
        else
        {
            player = OverworldData.player2;
        }

        transform.position += new Vector3(OverworldData.DimensionSize / 2, OverworldData.DimensionSize * 2 / 3, -OverworldData.DimensionSize);
        vector = new Vector3(0, OverworldData.DimensionSize * 2, 0);
        zoomOut = new Vector3(OverworldData.DimensionSize / 2, OverworldData.DimensionSize * OverworldData.DimensionsCount, -OverworldData.DimensionSize * OverworldData.DimensionsCount * 2);
        currentDimension = 0;
    }

    public void OnDimensionUp(CallbackContext ctx)
    {
        if (ctx.performed && currentDimension < OverworldData.DimensionsCount - 1)
        {
            DimensionUp();
            //player.opponent.vehicle.DimensionUp();
        }
    }

    public void DimensionUp()
    {
        if(currentDimension == player.ActiveDimension.DimensionNr)
        {
            currentDimension++;
            player.world.SetNewDimension(currentDimension);
            transform.position += vector;
            player.fleetMenu.UpdateFleetMenuDimension(currentDimension);
        }
        else
        {
            Debug.Log(this + ": Current dimension " + currentDimension + " and active dimension " + player.ActiveDimension.DimensionNr + " differ!");
        }
    }

    public void OnDimensionDown(CallbackContext ctx)
    {
        if (ctx.performed && currentDimension > 0)
        {
            DimensionDown();
            //player.opponent.vehicle.DimensionDown();
        }
    }

    public void DimensionDown()
    {
        if(currentDimension == player.ActiveDimension.DimensionNr)
        {
            currentDimension--;
            player.world.SetNewDimension(currentDimension);
            transform.position -= vector;
            player.fleetMenu.UpdateFleetMenuDimension(currentDimension);
        }
        else
        {
            Debug.Log(this + ": Current dimension " + currentDimension + " and active dimension " + player.ActiveDimension.DimensionNr + " differ!");
        }
    }

    public void OnZoom(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (zoomedOut)
            {
                transform.position = currentPosition;
                zoomedOut = !zoomedOut;
            }
            else
            {
                currentPosition = transform.position;
                transform.position = zoomOut;
                zoomedOut = !zoomedOut;
            }
        }
    }
}
