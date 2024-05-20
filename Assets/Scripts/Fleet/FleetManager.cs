using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Fleet")]

public class FleetManager : ScriptableObject
{
    private PlayerData player;
    public readonly List<GameObject> ships = new();

    public void ActivateShip(int index, PlayerData player)
    {
        ships[index].GetComponent<ShipManager>().Activate();
        if (OverworldData.GamePhase == GamePhases.Battle) player.inputEnabler.SwitchActionMap("Player");
    }

    public int GetShipIndex(int ofShipNo)
    {
        for (int i = 0; i < ships.Count; i++)
        {
            if (ships[i].GetComponent<ShipManager>().No == ofShipNo) return i;
        }

        Debug.LogWarning("Ship index not found!");
        return -1;
    }

    public void InitializeFleet(PlayerData player)
    {
        this.player = player;

        Object[] loadedAttackPatterns = Resources.LoadAll("AttackPatterns");

        for (int i = 0; i < OverworldData.FleetSize; i++)
        {
            GameObject shipPrefab = Resources.Load<GameObject>($"Ships/Ship{(i + 1)}_Prefab");

            if (shipPrefab != null)
            {
                GameObject shipObj = Instantiate(shipPrefab, new Vector3(0, 1, 0), Quaternion.identity);
                shipObj.name = "Ship" + player.number + "." + i;
                ShipInitializer ship = shipObj.GetComponent<ShipInitializer>();
                shipObj.layer = LayerSetter.SetLayerFleet(player);
                ship.Initialize(player, i, (AttackPattern)loadedAttackPatterns[i]);
                ships.Add(shipObj);
            }
            else
            {
                Debug.LogError($"Missing ship prefab: Ship{(i + 1)}_Prefab");
            }

            if (i == 0) player.LastActiveShip = ships[0].GetComponent<ShipManager>();
        }
    }
}
