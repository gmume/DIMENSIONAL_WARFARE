using System.Collections;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    public PlayerData player;
    public AudioPlayer audioPlayer;
    public ShipStatus ShipStatus { get; set; }
    public ShipManager manager;
    public LayerFilter layerFilter;
    public LayerFilter opponentLayerFilter;

    public bool Sunk()
    {
        foreach (ShipPartManager part in manager.parts)
        {
            if (!part.Damaged) return false;
        }

        if (manager.dimension.No != 0)
        {
            player.HUD.Instruct("None");
            DescendShip(manager.lifter, ref manager.dimension, manager.No);
            player.onboarding.ShowTip("OwnShipDown");
        }
        else
        {
            ShipOrFleetDestroyed(manager.No);
            player.onboarding.ShowTip("OwnShipDestroyed");
        }

        return true;
    }

    private void DescendShip(Lifter lifter, ref DimensionManager dimension, int shipNo)
    {
        ShipStatus = ShipStatus.Sunk;
        transform.position -= new Vector3(0, 0.5f, 0);

        foreach (ShipPartManager shipPart in manager.parts)
        {
            shipPart.PartMaterial.color = player.fleetColor + Colors.deltaActivShip;
        }

        lifter.SinkShip(ref dimension, shipNo, ShipStatus);
    }

    private void ShipOrFleetDestroyed(int shipNo)
    {
        if (!FleetDestroyed())
        {
            int index = player.fleet.GetShipIndex(shipNo);
            player.HUD.RemoveShipButton(index);

            if (player.LastActiveShip == GetComponent<ShipManager>()) player.world.DeactivateCells();
            player.LastActiveShip = null;

            Destroy(player.HUD.HUD_Fleet[shipNo]);
            Destroy(player.opponent.HUD.HUD_FleetOpponent[shipNo]);

            player.fleet.RemoveShip(index);
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
        foreach (GameObject shipObj in player.fleet.ships)
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
