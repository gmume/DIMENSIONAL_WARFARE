using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class LayerFilter : MonoBehaviour
{
    private Camera playerCamera;
    private string[] layers;
    private string fleetLayer, ownVisiblePartsLayer, opponentsVisiblePartsLayer;
    bool isCamera1;

    public void Start()
    {
        playerCamera = GetComponent<Camera>();
        isCamera1 = (name == "Camera1");
        fleetLayer = $"Fleet{(isCamera1 ? 1 : 2)}";
        ownVisiblePartsLayer = $"VisibleParts{(isCamera1 ? 1 : 2)}";
        opponentsVisiblePartsLayer = $"VisibleParts{(isCamera1 ? 2 : 1)}";
        layers = new string[] { "Default", "Water", $"Player{(isCamera1 ? 1 : 2)}", $"HUD{(isCamera1 ? 1 : 2)}", $"Dimensions{(isCamera1 ? 1 : 2)}", null, null, null };
        layers[^2] = fleetLayer;
        playerCamera.cullingMask = LayerMask.GetMask(layers);
    }

    public void ShowLayers(bool opponentDimensions, bool ownFleet, bool ownVisibleParts, bool opponentsVisibleParts)
    {
        layers[^4] = opponentDimensions ? $"Dimensions{(isCamera1 ? 1 : 2)}" : $"Dimensions{(isCamera1 ? 2 : 1)}";
        layers[^3] = ownFleet ? fleetLayer : null;
        layers[^2] = ownVisibleParts ? ownVisiblePartsLayer : null;
        layers[^1] = opponentsVisibleParts ? opponentsVisiblePartsLayer : null;

        playerCamera.cullingMask = LayerMask.GetMask(layers);
    }
}
