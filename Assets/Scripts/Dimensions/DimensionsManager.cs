using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionsManager : MonoBehaviour
{
    public PlayerData player;
    public GameObject dimensionPrefab, cellPrefab;
    private readonly List<GameObject> dimensions = new();
    private CellOccupier occupier;
    private CellGroupProvider cellGroupProvider;

    public List<GameObject> GetDimensions() => dimensions;

    public DimensionManager GetDimension(int no) => dimensions[no].GetComponent<DimensionManager>();

    public List<GameObject> GetCellGroup(List<Vector2> cellCoordinates, int dimensionNo) => cellGroupProvider.GetCells(cellCoordinates, dimensionNo);

    public GameObject GetCell(Vector2 cellCoordinates, int dimensionNo) => GetCellGroup(new() { cellCoordinates }, dimensionNo)[0];

    public void OccupyCells(List<(GameObject cell, GameObject part)> cellsAndObjs) => occupier.OccupyCells(cellsAndObjs);

    public void ReleaseCells(List<GameObject> cells) => occupier.ReleaseCells(cells);

    public void Initialize()
    {
        occupier = GetComponent<CellOccupier>();
        cellGroupProvider = GetComponent<CellGroupProvider>();

        cellGroupProvider.Initialize();
        player.fleet.InitializeFleet(player);
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
            dimension.layer = LayerSetter.SetLayerDimensions(player);
            dimension.transform.localScale = new Vector3(OverworldData.DimensionDiagonal, OverworldData.DimensionDiagonal, OverworldData.DimensionDiagonal);

            dimension.GetComponent<DimensionManager>().Initialize(player, dimensionNo, cellPrefab, player.fleet.GetFleet());
            dimensions.Add(dimension);
        }
    }
}