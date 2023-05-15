using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scripts/Dimensions")]

public class Dimensions : ScriptableObject
{
    private readonly ArrayList dimensions = new();

    public Fleet fleet;

    public void InitDimensions(PlayerScript playerScript, GameObject prefabDimension, GameObject prefabCell, GameObject prefabShip)
    {
        InitFleet(playerScript, prefabShip);
        CreateDimensions(playerScript, prefabDimension, prefabCell);
    }

    public void CreateDimensions(PlayerScript playerScript, GameObject dimensionPrefab, GameObject cellPrefab)
    {
        for (int dimensionNr = 0; dimensionNr < OverworldData.DimensionsCount; dimensionNr++)
        {
            float halfDimensionSize = OverworldData.DimensionSize / 2;
            GameObject dimension = Instantiate(dimensionPrefab, new Vector3(halfDimensionSize, OverworldData.DimensionSize * dimensionNr * 2, halfDimensionSize), Quaternion.identity);
            dimension.layer = Layer.SetLayerPlayer(playerScript);
            dimension.transform.localScale = new Vector3(OverworldData.DimensionDiagonal, OverworldData.DimensionDiagonal, OverworldData.DimensionDiagonal);
            dimension.GetComponent<Dimension>().InitDimension(playerScript, dimensionNr, cellPrefab, fleet.GetFleet());
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

    public void InitFleet(PlayerScript playerScript, GameObject prefabShip)
    {
        fleet = ScriptableObject.CreateInstance("Fleet") as Fleet;
        fleet.CreateFleet(playerScript, prefabShip);
    }

    public Fleet GetFleet()
    {
        return fleet;
    }
}