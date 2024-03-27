using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    [HideInInspector] public PlayerData player;
    [HideInInspector] public AudioPlayer audioPlayer;
                      public ShipStatus ShipStatus { get; set; }
    [HideInInspector] public ShipPartManager[] parts;

    public bool TakeHit(ShipPartManager part, int shipNo, ref DimensionManager dimension, Lifter lifter, CellOccupier occupier)
    {
        part.Damaged = true;
        part.PartMaterial.color += Color.red;

        int targetLayer = LayerMask.NameToLayer("VisibleShips" + player.number);
        gameObject.layer = targetLayer;
        part.gameObject.layer = targetLayer;

        if (!Sunk()) return false;

        if (dimension.DimensionNo != 0)
        {
            player.HUD.underAttack.SetActive(false);
            DescendShip(lifter, ref dimension, shipNo, occupier);
        }
        else
        {
            ShipOrFleetDestroyed(shipNo, occupier);
        }

        return true;
    }

    private bool Sunk()
    {
        foreach (ShipPartManager part in parts)
        {
            if (!part.Damaged) return false;
        }

        return true;
    }

    private void DescendShip(Lifter lifter, ref DimensionManager dimension, int shipNo, CellOccupier occupier)
    {
        ShipStatus = ShipStatus.Sunk;
        transform.position -= new Vector3(0, 0.5f, 0);

        foreach (ShipPartManager shipPart in parts)
        {
            shipPart.PartMaterial.color = Color.black;
        }

        lifter.SinkShip(ref dimension, shipNo, ShipStatus, occupier);
    }

    private void ShipOrFleetDestroyed(int shipNo, CellOccupier occupier)
    {
        if (!FleetDestroyed())
        {
            List<GameObject> fleet = player.fleet.GetFleet();
            player.HUD.RemoveShipButton(fleet.IndexOf(gameObject));
            
            Destroy(player.HUD.HUD_Fleet[shipNo]);
            Destroy(player.opponent.HUD.HUD_FleetOpponent[shipNo]);
            fleet.Remove(gameObject);
            StartCoroutine(DestroyShip(occupier));
        }
        else
        {
            audioPlayer.OnVictory();
            player.HUD.WriteText($"Capt'n {player.opponent.number} wins for destroying the opponent's fleet!");
            GameData.winner = player.opponent.name;
            player.GetComponent<SceneChanger>().LoadResolveGame();
        }
    }

    private bool FleetDestroyed()
    {
        foreach (GameObject shipObj in player.fleet.GetFleet())
        {
            if (shipObj.GetComponent<DamageHandler>().ShipStatus == ShipStatus.Intact) return false;
        }

        return true;
    }

    private IEnumerator DestroyShip(CellOccupier occupier)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1f;

        occupier.ReleaseCells();
        Destroy(gameObject);
    }
}
