using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{
    public PlayerData player;
    public ShipPartManager[] parts;

    public void Activate(ShipManager shipManager)
    {
        if (player.ActiveShip != this)
        {
            if (player.ActiveShip) player.ActiveShip.activator.Deactivate();

            foreach (ShipPartManager part in parts)
            {
                part.PartMaterial.color += Colors.deltaActivShip;
            }

            Vector3 vectorUp = new(0f, 0.1f, 0f);
            transform.position += vectorUp;
            player.ActiveShip = shipManager;
        }
    }

    public void Deactivate()
    {
        foreach (ShipPartManager part in parts)
        {
            part.PartMaterial.color -= Colors.deltaActivShip;
        }

        Vector3 vectorDown = new(0f, -0.1f, 0f);
        transform.position += vectorDown;
        player.LastActiveShip = player.ActiveShip;
        player.ActiveShip = null;
    }
}
