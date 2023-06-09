using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Scripts/Fleet")]

public class Fleet : ScriptableObject
{
    private readonly ArrayList fleet = new();

    public void CreateFleet(Player player, GameObject shipPrefab)
    {
        for (int i = 0; i < OverworldData.FleetSize; i++)
        {
            GameObject ship = Instantiate(shipPrefab, new Vector3(i, 1, 0), Quaternion.identity);
            if(player.number == 1)
            {
                ship.GetComponent<Renderer>().material.color = new Color(0.8f, 0.5f, 0.5f, 1);
            }
            else
            {
                ship.GetComponent<Renderer>().material.color = new Color(0.8f, 0.7f, 0.5f, 1);
            }
           
            ship.layer = Layer.SetLayerFleet(player);
            ship.GetComponent<Ship>().InitiateShip(player, i);
            fleet.Add(ship);
        }
    }

    public void ActivateShip(int shipNr, Player player)
    {        
        GameObject shipObj = (GameObject)fleet[shipNr];
        shipObj.GetComponent<Ship>().Activate();

        if(OverworldData.GamePhase == GamePhases.Battle) {
            player.input.SwitchCurrentActionMap("Player");
            player.inputHandling.SwitchActionMap("Player");
        }
    }

    public ArrayList GetFleet()
    {
        return fleet;
    }
}
