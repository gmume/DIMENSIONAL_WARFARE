using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class VehicleManager : MonoBehaviour
{
    private PlayerData player;
    private Vector3 vector, zoomOut, currentPosition;
    private bool zoomedOut = false;
    private int currentDimension;
    public AnimationCurve curve;

    public void Start()
    {
        player = (name == "CameraVehicle1") ? OverworldData.Player1 : OverworldData.Player2;

        transform.position += new Vector3(OverworldData.DimensionSize / 2, OverworldData.DimensionSize * 2 / 3, -OverworldData.DimensionSize);
        vector = new Vector3(0, OverworldData.DimensionSize * 2, 0);
        zoomOut = new Vector3(OverworldData.DimensionSize / 2, OverworldData.DimensionSize * OverworldData.DimensionsCount, -OverworldData.DimensionSize * OverworldData.DimensionsCount * 2);
        currentDimension = 0;
    }

    public void SetViewOnDimension(int toNo)
    {
        int vectorFactor = toNo - currentDimension;
        transform.position += vector * vectorFactor;
        currentDimension = toNo;
        player.world.SetNewDimension(currentDimension);
    }

    public void OnZoom(CallbackContext ctx)
    {
        if (!ctx.performed) return;
        
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
