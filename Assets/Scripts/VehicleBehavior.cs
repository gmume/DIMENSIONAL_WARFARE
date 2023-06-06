using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class VehicleBehavior : MonoBehaviour
{
    public int CurrentDimension { get; private set; }
    private Vector3 vector;
    private Player player;

    public void Start()
    {
        if(this.name == "CameraVehicle1")
        {
            player = OverworldData.player1;
        }
        else
        {
            player = OverworldData.player2;
        }

        transform.position += new Vector3(OverworldData.DimensionSize / 2, OverworldData.DimensionSize * 2 / 3, -OverworldData.DimensionSize);
        vector = new Vector3(0, OverworldData.DimensionSize * 2, 0);
        CurrentDimension = 0;
    }

    public void OnDimensionUp(CallbackContext ctx)
    {
        if (OverworldData.DimensionsCount - 1 > player.data.ActiveDimension.DimensionNr && ctx.performed == true)
        {
            CurrentDimension += 1;
            player.world.SetNewDimension(player.data.ActiveDimension.DimensionNr + 1);
            transform.position += vector;
            player.fleetMenu.UpdateFleetMenuDimension(CurrentDimension);
        }
    }

    public void OnDimensionDown(CallbackContext ctx)
    {
        if (player.data.ActiveDimension.DimensionNr > 0 && ctx.performed == true)
        {
            CurrentDimension -= 1;
            player.world.SetNewDimension(player.data.ActiveDimension.DimensionNr - 1);
            transform.position -= vector;
            player.fleetMenu.UpdateFleetMenuDimension(CurrentDimension);
        }
    }
}
