using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ShipPartManager : MonoBehaviour
{
    public int partNo;
    public int X { get; private set; }
    public int Y { get; private set; }
    public DimensionManager Dimension { get; set; }
    public Material PartMaterial { get; private set; }
    private Color colorIntact;
    private ExplosionTrigger explosion;
    public bool Damaged { get; set; }
    public bool ContinueGame = false;

    private GameObject HUD_ButtonPart;

    public void UpdateCoordinatesRelative(int x, int y)
    {
        X += x;
        Y += y;
    }

    public void UpdateCoordinatesAbsolute(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void Explode()
    {
        explosion.Explode();
        //PartMaterial = Materials.partHitMat;
        PartMaterial.color = Colors.damagedPart;
        Damaged = true;
    }

    void OnParticleSystemStopped()
    {
        Debug.Log("System has stopped!");
    }

    public void ResetPart()
    {
        Damaged = false;
        SetColorIntact();
    }

    private void SetColorIntact() => PartMaterial.color = colorIntact;

    public void Initialize(PlayerData player, int partNo, ShipManager ship)
    {
        this.partNo = partNo;
        X = ship.No;
        Y = partNo;
        Damaged = false;
        
        PartMaterial = GetComponent<Renderer>().material;
        colorIntact = player.fleetColor;
        explosion = transform.Find("Explosion").GetComponent<ExplosionTrigger>();
        gameObject.layer = LayerSetter.SetLayerFleet(player);

        SetColorIntact();
        explosion.Initialize();

        GameObject HUD_button = player.HUD.hudButtonHandler.shipButtons[ship.No];
        //Debug.Log("buttonParts: " + HUD_button.GetComponent<HUD_ButtonPartsHandler>().buttonParts.Length);

        //HUD_ButtonPart = HUD_button.GetComponent<HUD_ButtonPartsHandler>().buttonParts[partNo];

        //Debug.Log("HUD_ButtonPart: " + HUD_ButtonPart);
    }
}
