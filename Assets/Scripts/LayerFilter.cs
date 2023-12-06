using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
            layers = new string[] { "Default", "Water", "Player1", "HUD1", "VisibleHUDShips2", null, null };
        }
        else
        {
            fleetLayer = "Fleet2";
            visibleShipsLayer = "VisibleShips1";
            layers = new string[] { "Default", "Water", "Player2", "HUD2", "VisibleHUDShips1", null, null };
        }

        layers[^2] = fleetLayer;
        playerCamera.cullingMask = LayerMask.GetMask(layers);
    }

    public void ShowLayers(bool ownFleet, bool opponentsFleet)
    {
        layers[^2] = null;
        layers[^1] = null;

        if (ownFleet && !opponentsFleet)
        {
            layers[^2] = fleetLayer;
        }
        else if (!ownFleet && opponentsFleet)
        {
            layers[^2] = visibleShipsLayer;
        }
        else if (ownFleet && opponentsFleet)
        {
            layers[^2] = fleetLayer;
            layers[^1] = visibleShipsLayer;
        }

        playerCamera.cullingMask = LayerMask.GetMask(layers);
    }
}
