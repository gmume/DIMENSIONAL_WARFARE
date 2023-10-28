using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Scripts/Dimensions")]

public class Dimensions : ScriptableObject
{
    private readonly ArrayList dimensions = new();

    public void InitDimensions(Player player, GameObject prefabDimension, GameObject prefabCell)
    {
        InitFleet(player);
        CreateDimensions(player, prefabDimension, prefabCell);
    }

    public void CreateDimensions(Player player, GameObject dimensionPrefab, GameObject cellPrefab)
    {
        for (int dimensionNo = 0; dimensionNo < OverworldData.DimensionsCount; dimensionNo++)
        {
            float halfDimensionSize = OverworldData.DimensionSize / 2;
            GameObject dimension = Instantiate(dimensionPrefab, new Vector3(halfDimensionSize, OverworldData.DimensionSize * dimensionNo * 2, halfDimensionSize), Quaternion.identity);
            dimension.transform.parent = GameObject.Find("Dimensions" + player.number).transform;
            dimension.name = "dimension" + player.number + "." + dimensionNo;
            dimension.layer = Layer.SetLayerPlayer(player);
            dimension.transform.localScale = new Vector3(OverworldData.DimensionDiagonal, OverworldData.DimensionDiagonal, OverworldData.DimensionDiagonal);
            dimension.GetComponent<Dimension>().InitDimension(player, dimensionNo, cellPrefab, player.fleet.GetFleet());
            dimensions.Add(dimension);
        }
    }

    public ArrayList GetDimensions()
    {
        return dimensions;
    }

    public Dimension GetDimension(int no)
    {
        GameObject dimension = (GameObject)dimensions[no];
        return dimension.GetComponent<Dimension>();
    }

    public void InitFleet(Player player)
    {
        player.fleet = ScriptableObject.CreateInstance("Fleet") as Fleet;
        player.fleet.name = "fleet" + player.number;
        player.fleet.CreateFleet(player);
    }
}