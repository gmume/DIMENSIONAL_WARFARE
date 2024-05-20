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
    private PositionFinder finder;

    public List<GameObject> GetDimensions() => dimensions;

    public DimensionManager GetDimension(int no) => dimensions[no].GetComponent<DimensionManager>();

    public List<GameObject> GetCellGroup(List<Vector2> cellCoordinates, int dimensionNo) => cellGroupProvider.GetCells(cellCoordinates, GetDimension(dimensionNo));

    public List<GameObject> GetCellGroup(List<Vector2> cellCoordinates, DimensionManager dimension) => cellGroupProvider.GetCells(cellCoordinates, dimension);

    public GameObject GetCell(Vector2 cellCoordinates, int dimensionNo) => GetCellGroup(new() { cellCoordinates }, dimensionNo)[0];

    public void ResetCellPositions(int onDimensionNo) => dimensions[onDimensionNo].GetComponent<DimensionManager>().ResetCellPositions();

    public void OccupyCells(List<(GameObject cell, GameObject part)> cellsAndObjs) => occupier.OccupyCells(cellsAndObjs);

    public void ReleaseCells(List<GameObject> cells) => occupier.ReleaseCells(cells);

    public int CountVacantCells(List<GameObject> cells) => finder.CountVacantCells(cells);

    public List<Vector2> FindVacantCoordinates(int onDimensionNo, List<Vector2> position) => finder.FindVacantCoordinates(onDimensionNo, position);

    public void Initialize()
    {
        occupier = GetComponent<CellOccupier>();
        cellGroupProvider = GetComponent<CellGroupProvider>();
        finder = GetComponent<PositionFinder>();

        cellGroupProvider.Initialize();
        finder.Initialize(player.dimensions);
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