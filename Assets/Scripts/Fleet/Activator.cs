using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{
    [HideInInspector] public PlayerData player;
    [HideInInspector] public ShipPartManager[] parts;

    public void Activate(CellOccupier occupier, ShipManager shipManager)
    {
        if (player.ActiveShip != this)
        {
            if (player.ActiveShip) player.ActiveShip.activator.Deactivate(occupier);

            foreach (ShipPartManager part in parts)
            {
                part.PartMaterial.color += new Color(0.3f, 0.3f, 0.3f);
            }

            Vector3 vectorUp = new(0f, 0.1f, 0f);
            transform.position += vectorUp;
            player.ActiveShip = shipManager;
        }
    }

    public void Deactivate(CellOccupier occupier)
    {
        foreach (ShipPartManager part in parts)
        {
            part.PartMaterial.color -= new Color(0.3f, 0.3f, 0.3f);
        }

        Vector3 vectorDown = new(0f, -0.1f, 0f);
        transform.position += vectorDown;
        player.ActiveShip = null;
    }
}
