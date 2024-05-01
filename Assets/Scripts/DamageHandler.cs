using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    [HideInInspector] public PlayerData player;
    [HideInInspector] public AudioPlayer audioPlayer;
                      public ShipStatus ShipStatus { get; set; }
    [HideInInspector] public ShipManager manager;

    public bool TakeHit(ShipPartManager part, int shipNo, ref DimensionManager dimension, Lifter lifter, CellOccupier occupier)
    {
        part.Damaged = true;
        part.PartMaterial.color += Color.red;

        int targetLayer = LayerMask.NameToLayer("VisibleShips" + player.number);
        gameObject.layer = targetLayer;
        part.gameObject.layer = targetLayer;

        if (!Sunk()) return false;

        if (dimension.No != 0)
        {
            player.HUD.underAttack.SetActive(false);
            DescendShip(lifter, ref dimension, shipNo);
            player.onboarding.ShowTip("OwnShipDown");
        }
        else
        {
            ShipOrFleetDestroyed(shipNo);
            player.onboarding.ShowTip("OwnShipDestroyed");
        }

        return true;
    }

    private bool Sunk()
    {
        foreach (ShipPartManager part in manager.parts)
        {
            if (!part.Damaged) return false;
        }

        return true;
    }

    private void DescendShip(Lifter lifter, ref DimensionManager dimension, int shipNo)
    {
        ShipStatus = ShipStatus.Sunk;
        transform.position -= new Vector3(0, 0.5f, 0);

        foreach (ShipPartManager shipPart in manager.parts)
        {
            shipPart.PartMaterial.color = Color.black;
        }

        lifter.SinkShip(ref dimension, shipNo, ShipStatus);
    }

    private void ShipOrFleetDestroyed(int shipNo)
    {
        if (!FleetDestroyed())
        {
            List<GameObject> fleet = player.fleet.GetFleet();
            player.HUD.RemoveShipButton(fleet.IndexOf(gameObject));
            
            Destroy(player.HUD.HUD_Fleet[shipNo]);
            Destroy(player.opponent.HUD.HUD_FleetOpponent[shipNo]);
            fleet.Remove(gameObject);
            StartCoroutine(DestroyShip());
        }
        else
        {
            audioPlayer.OnVictory();
            player.HUD.WriteText($"Capt'n {player.opponent.number} wins for destroying the opponent's fleet!");
            GameData.winner = player.opponent.name;
            SceneChanger.LoadResolveGame();
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

    private IEnumerator DestroyShip()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1f;

        player.dimensions.ReleaseCells(player.dimensions.GetCellGroup(manager.GetShipCoordinates(), manager.dimension.No));
        Destroy(gameObject);
    }
}
