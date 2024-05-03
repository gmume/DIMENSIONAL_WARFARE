using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class LayerFilter : MonoBehaviour
{
    private Camera playerCamera;
    private string[] layers;
    private string /*dimensionsLayer,*/ fleetLayer, ownDamagedPartsLayer, opponentsDamagedPartsLayer;
    //private string /*dimensionsLayer,*/ fleetLayer, visibleShipsLayer;
    bool isCamera1;

    public void Start()
    {
        playerCamera = GetComponent<Camera>();
        isCamera1 = (name == "Camera1");
        //dimensionsLayer = $"Dimensions{(isCamera1 ? 1 : 2)}";
        fleetLayer = $"Fleet{(isCamera1 ? 1 : 2)}";
        //visibleShipsLayer = $"VisibleShips{(isCamera1 ? 1 : 2)}";
        ownDamagedPartsLayer = $"DamagedParts{(isCamera1 ? 1 : 2)}";
        opponentsDamagedPartsLayer = $"DamagedParts{(isCamera1 ? 2 : 1)}";
        layers = new string[] { "Default", "Water", $"Player{(isCamera1 ? 1 : 2)}", $"HUD{(isCamera1 ? 1 : 2)}", $"Dimensions{(isCamera1 ? 1 : 2)}", null, null, null };
        //layers = new string[] { "Default", "Water", $"Player{(isCamera1 ? 1 : 2)}", $"HUD{(isCamera1 ? 1 : 2)}", $"Dimensions{(isCamera1 ? 1 : 2)}", null, null };
        layers[^2] = fleetLayer;
        playerCamera.cullingMask = LayerMask.GetMask(layers);
    }

    public void ShowLayers(bool ownDimensions, bool ownFleet, bool ownDamagedParts, bool opponentsDamagedParts)
    {
        layers[^4] = ownDimensions ? $"Dimensions{(isCamera1 ? 1 : 2)}" : $"Dimensions{(isCamera1 ? 2 : 1)}";
        layers[^3] = ownFleet ? fleetLayer : null;
        layers[^2] = ownDamagedParts ? ownDamagedPartsLayer : null;
        layers[^1] = opponentsDamagedParts ? opponentsDamagedPartsLayer : null;


        //layers[^3] = ownDimensions ? $"Dimensions{(isCamera1 ? 1 : 2)}" : $"Dimensions{(isCamera1 ? 2 : 1)}";
        //layers[^2] = ownFleet ? fleetLayer : null;
        //layers[^1] = opponentsFleet ? visibleShipsLayer : null;

        playerCamera.cullingMask = LayerMask.GetMask(layers);
    }
}
