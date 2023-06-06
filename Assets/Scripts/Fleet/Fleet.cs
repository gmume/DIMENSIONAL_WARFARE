using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Scripts/Fleet")]

public class Fleet : ScriptableObject
{
    private readonly ArrayList fleet = new();

    public void CreateFleet(PlayerWorld playerWorld, GameObject shipPrefab)
    {
        for (int i = 0; i < OverworldData.FleetSize; i++)
        {
            GameObject ship = Instantiate(shipPrefab, new Vector3(i, 1, 0), Quaternion.identity);
            if(playerWorld.name == "Player1")
            {
                ship.GetComponent<Renderer>().material.color = new Color(0.8f, 0.5f, 0.5f, 1);
            }
            else
            {
                ship.GetComponent<Renderer>().material.color = new Color(0.8f, 0.7f, 0.5f, 1);
            }
           
            ship.layer = Layer.SetLayerFleet(playerWorld);
            ship.GetComponent<Ship>().InitiateShip(i);
            fleet.Add(ship);
        }
    }

    public void ActivateShip(int shipNr, GameObject player)
    {
        PlayerWorld playerWorld = player.GetComponent<PlayerWorld>();
        
        if (playerWorld.playerData.ActiveShip != null)
        {
            playerWorld.playerData.ActiveShip.Deactivate(playerWorld);
        }

        GameObject shipObj = (GameObject)fleet[shipNr];
        shipObj.GetComponent<Ship>().Activate(player.GetComponent<PlayerWorld>());

        if(OverworldData.GamePhase == GamePhases.Battle) {
            player.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
            player.GetComponent<InputHandling>().SwitchActionMap("Player");
        }
    }

    public ArrayList GetFleet()
    {
        return fleet;
    }
}
