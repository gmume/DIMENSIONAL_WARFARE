using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Fleet")]

public class FleetManager : ScriptableObject
{
    private readonly List<GameObject> fleet = new();

    public void CreateFleet(PlayerData player)
    {
        for (int i = 0; i < OverworldData.FleetSize; i++)
        {
            GameObject shipPrefab = Resources.Load<GameObject>($"Ships/Ship{(i + 1)}_Prefab");

            if (shipPrefab != null)
            {
                GameObject shipObj = Instantiate(shipPrefab, new Vector3(i, 1, 0), Quaternion.identity);

                shipObj.name = "Ship" + player.number + "." + i;
                ShipInitializer ship = shipObj.GetComponent<ShipInitializer>();
                shipObj.layer = LayerSetter.SetLayerFleet(player);
                ship.Initialize(player, i);
                fleet.Add(shipObj);
            }
            else
            {
                Debug.LogError($"Missing ship prefab: Ship{(i + 1)}_Prefab");
            }
        }
    }

    public void ActivateShip(int shipNr, PlayerData player)
    {
        GameObject shipObj = fleet[shipNr];
        shipObj.GetComponent<ShipManager>().Activate();

        if (OverworldData.GamePhase == GamePhases.Battle) player.switcher.SwitchActionMap("Player");
    }

    public List<GameObject> GetFleet() => fleet;
}
