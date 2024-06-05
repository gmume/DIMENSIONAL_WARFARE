using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Fleet")]

public class FleetManager : ScriptableObject
{
    PlayerData player;
    public  List<GameObject> ships = new();

    public void ActivateShip(int index)
    {
        ships[index].GetComponent<ShipManager>().Activate();
        if (OverworldData.GamePhase == GamePhases.Battle) player.inputEnabler.SwitchActionMap("Battle");
    }

    public bool SunkShips(List<GameObject> hitCells)
    {
        HashSet<ShipManager> hitShips = ShipsTakeHits(hitCells);

        foreach (ShipManager ship in hitShips)
        {
            if (ship.Sunk())
            {
                return true;
            }
        }

        return false;
    }

    private HashSet<ShipManager> ShipsTakeHits(List<GameObject> hitCells)
    {
        HashSet<ShipManager> hitShips = new();

        foreach (GameObject hitCell in hitCells)
        {
            CellData cell = hitCell.GetComponent<CellData>();

            if (cell.Occupied && cell.OccupyingObj.CompareTag("ShipPart"))
            {
                ShipPartManager shipPart = cell.OccupyingObj.GetComponent<ShipPartManager>();
                ShipManager ship = shipPart.transform.parent.GetComponent<ShipManager>();
                ship.HUD_buttonPartsHandler[ship.No].ButtonPartTakeHit(shipPart.partNo);
                shipPart.Explode();
                hitShips.Add(shipPart.transform.parent.GetComponent<ShipManager>());
            }
        }

        return hitShips;
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

    public void RemoveShip(int atIndex) => ships.Remove(ships[atIndex]);

    public void Initialize(PlayerData player)
    {
        this.player = player;
        Object[] loadedAttackPatterns = Resources.LoadAll("AttackPatterns");

        if(ships.Count > 0) ships.Clear();

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
