using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/RocksPattern")]

public class RocksPattern : PatternCoordinates
{
    public override List<Vector2> GetPatternLevel(int level) => level switch
    {
        0 => patternLevel0,
        1 => patternLevel1,
        _ => patternLevel2,
    };
}
