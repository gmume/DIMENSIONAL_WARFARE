using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TupleListProvider
{
    public static List<(GameObject, GameObject)> GetTuplesList(List<GameObject> objList1, List<GameObject> objList2)
    {
        List<(GameObject, GameObject)> tuplesList = new();

        if(objList1.Count != objList2.Count)
        {
            Debug.LogWarning("Count of object's list 1 and 2 differ!");
            return null;
        }

        for (int i = 0; i < objList1.Count; i++)
        {
            tuplesList.Add((objList1[i], objList2[i]));
        }

        return tuplesList;
    }
    
    public static List<(GameObject, GameObject)> GetTuplesList(DimensionManager dimension, List<Vector2> vectorList, List<GameObject> objList)
    {
        List<GameObject> cells = new();

        foreach (Vector2 vector in vectorList)
        {
            cells.Add(dimension.GetCell((int)vector.x, (int)vector.y));
        }

        return GetTuplesList(cells, objList);
    }
}
