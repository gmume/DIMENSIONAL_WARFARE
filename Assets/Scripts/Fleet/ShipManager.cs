using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShipManager : MonoBehaviour
{
    [HideInInspector] public PlayerData player;

    [HideInInspector] public AudioPlayer audioPlayer;
    [HideInInspector] public ShipNavigator navigator;
    [HideInInspector] public ShipLifter lifter;
    [HideInInspector] public CellOccupier occupier;

    public string ShipName { get; set; }
                      public int ShipNo { get; set; } // 1 based
                      public ShipStatus ShipStatus { get; set; }
    [HideInInspector] public ShipPart[] parts;
                      private Color fireColor;
                      public int PartsCount { get; set; }

    public Dimension dimension;

    public void Activate()
    {
        if (player.ActiveShip != this)
        {
            if (player.ActiveShip) player.ActiveShip.Deactivate();

            foreach (ShipPart part in parts)
            {
                part.PartMaterial.color += new Color(0.3f, 0.3f, 0.3f);
            }

            ReleaseCells();
            Vector3 vectorUp = new(0f, 0.1f, 0f);
            transform.position += vectorUp;
            player.ActiveShip = this;
        }
    }

    public void Deactivate()
    {
        foreach (ShipPart part in parts)
        {
            part.PartMaterial.color -= new Color(0.3f, 0.3f, 0.3f);
        }

        OccupyCells();
        Vector3 vectorDown = new(0f, -0.1f, 0f);
        transform.position += vectorDown;
        player.ActiveShip = null;
    }

    public void Move(int deltaX, int deltaY)
    {
        navigator.Move(deltaX, deltaY, parts, dimension);
    }

    public void QuaterTurn(bool clockwise)
    {
        navigator.QuaterTurn(clockwise, parts, dimension);
    }

    public bool Fire()
    {
        Cell activeCell = player.ActiveCell;
        Material cellMaterial = activeCell.GetComponent<Renderer>().material;
        cellMaterial.color = fireColor;
        activeCell.Hitted = true;

        Dimension opponentDimension = player.opponent.dimensions.GetDimension(dimension.DimensionNo);
        Cell opponentCell = opponentDimension.GetCell(activeCell.X, activeCell.Y).GetComponent<Cell>();

        if (opponentCell.Occupied)
        {
            bool opponentSunk;
            ShipPart part = opponentCell.Part;

            ShipManager opponentShip = part.GetComponentInParent<ShipManager>();
            opponentSunk = opponentShip.TakeHit(part);

            if (opponentSunk)
            {
                ShipUp();
                return true;
            }
        }

        return false;
    }

    private void ShipUp()
    {
        lifter.LiftShipUp(ref dimension, ShipNo, occupier);
    }

    public bool TakeHit(ShipPart part)
    {
        part.Damaged = true;
        part.PartMaterial.color += Color.red;

        int targetLayer = LayerMask.NameToLayer("VisibleShips" + player.number);
        gameObject.layer = targetLayer;
        part.gameObject.layer = targetLayer;

        if (Sunk())
        {
            ShipStatus = ShipStatus.Sunk;
            transform.position -= new Vector3(0, 0.5f, 0);

            foreach (ShipPart shipPart in parts)
            {
                shipPart.PartMaterial.color = Color.black;
            }

            if (dimension.DimensionNo != 0)
            {
                lifter.SinkShip(ref dimension, ShipNo, ShipStatus, occupier);
            }
            else
            {
                ShipOrFleetDestroyed();
            }

            return true;
        }
        
        return false;
    }

    private void ShipOrFleetDestroyed()
    {
        if (!FleetDestroyed())
        {
            List<GameObject> fleet = player.fleet.GetFleet();

            player.HUD.RemoveButton(fleet.IndexOf(gameObject));
            Destroy(player.HUD.HUD_Fleet[ShipNo]);
            Destroy(player.opponent.HUD.HUD_FleetOpponent[ShipNo]);
            fleet.Remove(gameObject);
            StartCoroutine(DestroyShip());
        }
        else
        {
            audioPlayer.OnVictory();
            player.HUD.WriteText($"Capt'n {player.opponent.number} wins for destroying the opponent's fleet!");
            GameData.winner = player.opponent.name;
            player.GetComponent<SceneChanger>().LoadResolveGame();
        }
    }

    private IEnumerator DestroyShip()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1f;

        ReleaseCells();
        Destroy(gameObject);
    }

    private bool Sunk()
    {
        foreach (ShipPart part in parts)
        {
            if (!part.Damaged) return false;
        }

        return true;
    }

    private bool FleetDestroyed()
    {
        foreach (GameObject shipObj in player.fleet.GetFleet())
        {
            if (shipObj.GetComponent<ShipManager>().ShipStatus == ShipStatus.Intact) return false;
        }

        return true;
    }

    public void OccupyCells()
    {
        occupier.OccupyCells();
    }

    public void ReleaseCells()
    {
        occupier.ReleaseCells();
    }

    public void SetDimension(Dimension newDimension)
    {
        dimension = newDimension;
        lifter.SetDimension(dimension, ShipNo);
    }
}
