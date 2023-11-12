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
    public AnimationCurve curve;

    public void Start()
    {
        if (name == "CameraVehicle1")
        {
            player = OverworldData.Player1;
        }
        else
        {
            player = OverworldData.Player2;
        }

        transform.position += new Vector3(OverworldData.DimensionSize / 2, OverworldData.DimensionSize * 2 / 3, -OverworldData.DimensionSize);
        vector = new Vector3(0, OverworldData.DimensionSize * 2, 0);
        zoomOut = new Vector3(OverworldData.DimensionSize / 2, OverworldData.DimensionSize * OverworldData.DimensionsCount, -OverworldData.DimensionSize * OverworldData.DimensionsCount * 2);
        currentDimension = 0;
    }

    public void SetViewOnDimension(int toNo)
    {
        //if (currentDimension == player.GetActiveDimension().DimensionNo)
        //{
            // Calculate vector and change position
            int vectorFactor = toNo - currentDimension;
            transform.position += vector * vectorFactor;
            currentDimension = toNo;
            player.world.SetNewDimension(currentDimension);
            //player.fleetMenu.SetHUDDimension(currentDimension);
        //}
        //else
        //{
        //    Debug.LogWarning(name + ": Current camera dimension " + currentDimension + " and active dimension " + player.GetActiveDimension().DimensionNo + " differ!");
        //}
    }

    public void OnDimensionUp(CallbackContext ctx)
    {
        if (ctx.performed && currentDimension < OverworldData.DimensionsCount - 1)
        {
            SetViewOnDimension(currentDimension + 1);
        }
    }

    public void DimensionUp()
    {
        SetViewOnDimension(currentDimension + 1);
    }

    public void OnDimensionDown(CallbackContext ctx)
    {
        if (ctx.performed && currentDimension > 0)
        {
            SetViewOnDimension(currentDimension - 1);
        }
    }

    public void DimensionDown()
    {
        SetViewOnDimension(currentDimension - 1);
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
