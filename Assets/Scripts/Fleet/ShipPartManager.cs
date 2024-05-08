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
    private GameObject explosion;
    public bool Damaged { get; set; }

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
        PartMaterial = Materials.partHitMat;
        explosion.GetComponent<ParticleSystem>().Play();
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
        explosion = transform.Find("Explosion").gameObject;

        gameObject.layer = LayerSetter.SetLayerFleet(player);
        explosion.layer = LayerSetter.SetLayerFleet(player);

        foreach (Transform t in explosion.GetComponentsInChildren<Transform>())
        {
            t.gameObject.layer = LayerSetter.SetLayerFleet(player);
        }

        SetColorIntact();
    }
}
