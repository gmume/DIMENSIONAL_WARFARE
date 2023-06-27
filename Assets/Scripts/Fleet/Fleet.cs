using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Scripts/Fleet")]

public class Fleet : ScriptableObject
{
    private readonly ArrayList fleet = new();

    public void CreateFleet(Player player)
    {
        for (int i = 0; i < OverworldData.FleetSize; i++)
        {
            GameObject shipObj = Instantiate(Resources.Load<GameObject>("Prefabs/ShipPrefab" + (i + 1)), new Vector3(i, 1, 0), Quaternion.identity);

            shipObj.name = "Ship" + player.number + "." + i;
            Ship ship = shipObj.GetComponent<Ship>();
            shipObj.layer = Layer.SetLayerFleet(player);
            ship.InitiateShip(player, i);
            fleet.Add(shipObj);
        }
    }

    public void ActivateShip(int shipNr, Player player)
    {
        GameObject shipObj = (GameObject)fleet[shipNr];
        shipObj.GetComponent<Ship>().Activate();

        if (OverworldData.GamePhase == GamePhases.Battle)
        {
            player.inputHandling.SwitchActionMap("Player");
        }
    }

    public ArrayList GetFleet()
    {
        return fleet;
    }
}
