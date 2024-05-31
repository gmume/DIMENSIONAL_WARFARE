using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/AttackPattern")]

public abstract class PatternCoordinates : ScriptableObject
{
    [Header("Delta coordinates of elements")]
    [Header("Level 1")]
    public List<Vector2> patternLevel0 = new() { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };

    [Header("Level 2")]
    public List<Vector2> patternLevel1 = new() { new Vector2(0, 0), new Vector2(0, 0) };

    [Header("Level 3")]
    public List<Vector2> patternLevel2 = new() { new Vector2(0, 0), new Vector2(0, 0) };

    public abstract List<Vector2> GetPatternLevel(int level);
}
