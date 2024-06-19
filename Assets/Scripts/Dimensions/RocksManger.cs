using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class RocksManger : MonoBehaviour
{
    [SerializeField] private GameObject rockPrefab;
    [SerializeField] private RocksPattern rockPattern;

    public List<Vector2> GetRocksCoordinates(int onDimensionNo)
    {
        List<Vector2> rocksCoordinates = GetFixedCoordinates(onDimensionNo);
        //To do: Add list of random distributet rocks.
        return rocksCoordinates;
    }

    private List<Vector2> GetFixedCoordinates(int onDimensionNo) => rockPattern.GetPatternLevel(onDimensionNo);

    public GameObject GetRock() => Instantiate(rockPrefab, new Vector3(0, 0, 0), Quaternion.identity);
}