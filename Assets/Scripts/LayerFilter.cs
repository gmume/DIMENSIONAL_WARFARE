using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerFilter : MonoBehaviour
{
    private Camera playerCamera;
    private string[] layers;
    private string fleetLayer, visibleShipsLayer;

    public void Start()
    {
        playerCamera = GetComponent<Camera>();

        if (name == "Camera1")
        {
            fleetLayer = "Fleet1";
            visibleShipsLayer = "VisibleShips2";
            layers = new string[] { "Default", "Water", "Player1", "HUD1", null, null };
        }
        else
        {
            fleetLayer = "Fleet2";
            visibleShipsLayer = "VisibleShips1";
            layers = new string[] { "Default", "Water", "Player2", "HUD2", null, null };
        }

        layers[4] = fleetLayer;
        playerCamera.cullingMask = LayerMask.GetMask(layers);
    }

    public void ShowLayers(bool ownFleet, bool opponentsFleet)
    {
        layers[4] = null;
        layers[5] = null;

        if (ownFleet && !opponentsFleet)
        {
            layers[4] = fleetLayer;
        }
        else if (!ownFleet && opponentsFleet)
        {
            layers[4] = visibleShipsLayer;
        }
        else if (ownFleet && opponentsFleet)
        {
            layers[4] = fleetLayer;
            layers[5] = visibleShipsLayer;
        }

        playerCamera.cullingMask = LayerMask.GetMask(layers);
    }
}
