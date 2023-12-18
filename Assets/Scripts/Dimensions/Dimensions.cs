using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dimensions : MonoBehaviour
{
    public PlayerData player;
    public GameObject dimensionPrefab, cellPrefab;
    private readonly List<GameObject> dimensions = new();

    public void Initialize()
    {
        Initialize(player);
        CreateDimensions();
    }

    public void CreateDimensions()
    {
        for (int dimensionNo = 0; dimensionNo < OverworldData.DimensionsCount; dimensionNo++)
        {
            float halfDimensionSize = OverworldData.DimensionSize / 2;
            GameObject dimension = Instantiate(dimensionPrefab, new Vector3(halfDimensionSize, OverworldData.DimensionSize * dimensionNo * 2, halfDimensionSize), Quaternion.identity);
            dimension.transform.parent = transform;
            dimension.name = $"dimension{player.number}.{dimensionNo}";
            dimension.layer = Layer.SetLayerDimensions(player);
            dimension.transform.localScale = new Vector3(OverworldData.DimensionDiagonal, OverworldData.DimensionDiagonal, OverworldData.DimensionDiagonal);
            dimension.GetComponent<Dimension>().Initialize(player, dimensionNo, cellPrefab, player.fleet.GetFleet());
            dimensions.Add(dimension);
        }
    }

    public List<GameObject> GetDimensions()
    {
        return dimensions;
    }

    public Dimension GetDimension(int no)
    {
        GameObject dimension = dimensions[no];
        return dimension.GetComponent<Dimension>();
    }

    public void Initialize(PlayerData player)
    {
        player.fleet = ScriptableObject.CreateInstance<Fleet>();
        player.fleet.name = $"fleet{player.number}";
        player.fleet.CreateFleet(player);
    }
}