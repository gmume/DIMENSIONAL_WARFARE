using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scripts/Dimensions")]

public class Dimensions : ScriptableObject
{
    private readonly ArrayList dimensions = new();

    public Fleet fleet;

    public void InitDimensions(Player player, GameObject prefabDimension, GameObject prefabCell, GameObject prefabShip)
    {
        InitFleet(player, prefabShip);
        CreateDimensions(player, prefabDimension, prefabCell);
    }

    public void CreateDimensions(Player player, GameObject dimensionPrefab, GameObject cellPrefab)
    {
        for (int dimensionNr = 0; dimensionNr < OverworldData.DimensionsCount; dimensionNr++)
        {
            float halfDimensionSize = OverworldData.DimensionSize / 2;
            GameObject dimension = Instantiate(dimensionPrefab, new Vector3(halfDimensionSize, OverworldData.DimensionSize * dimensionNr * 2, halfDimensionSize), Quaternion.identity);
            dimension.layer = Layer.SetLayerPlayer(player);
            dimension.transform.localScale = new Vector3(OverworldData.DimensionDiagonal, OverworldData.DimensionDiagonal, OverworldData.DimensionDiagonal);
            dimension.GetComponent<Dimension>().InitDimension(player, dimensionNr, cellPrefab, fleet.GetFleet());
            dimensions.Add(dimension);
        }
    }

    public ArrayList GetDimensions()
    {
        return dimensions;
    }

    public Dimension GetDimension(int nr)
    {
        GameObject dimension = (GameObject)dimensions[nr];
        return dimension.GetComponent<Dimension>();
    }

    public void InitFleet(Player player, GameObject prefabShip)
    {
        fleet = ScriptableObject.CreateInstance("Fleet") as Fleet;
        fleet.CreateFleet(player, prefabShip);
    }

    public Fleet GetFleet()
    {
        return fleet;
    }
}