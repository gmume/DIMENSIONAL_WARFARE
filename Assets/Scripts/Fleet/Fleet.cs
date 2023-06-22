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
            GameObject shipObj = Instantiate(shipPrefab, new Vector3(i, 1, 0), Quaternion.identity);

            Ship ship = shipObj.GetComponent<Ship>();
            ship.ShipMaterial = shipObj.GetComponent<Renderer>().material;

            if (player.number == 1)
            {
                ship.ShipMaterial.color = new Color(0.3f, 0.12f, 0, 1); // brown
            }
            else
            {
                ship.ShipMaterial.color = new Color(0.3f, 0.3f, 0, 1); // olive
            }

            shipObj.layer = Layer.SetLayerFleet(player);
            ship.InitiateShip(player, i + 1);
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
