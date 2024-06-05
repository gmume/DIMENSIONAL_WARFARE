using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Pointer : MonoBehaviour
{
    public PlayerData player;
    private List<GameObject> shipButtonsObj = new();

    [SerializeField] private AnimationCurve curveY = new();
    private readonly List<GameObject> pointerObjs = new();
    private readonly int pointerObjCount = 30;
    [SerializeField] private GameObject spherePrefab;
    [SerializeField] private GameObject conePrefab;
    private readonly float cellYOffset = 0.5f + 0.125f; // 0.5f = half cell hight, 0.125f = estimated half sphere hight

    public void OnEnable()
    {
        shipButtonsObj = player.HUD.GetShipButtons();

        for (int i = 1; i < pointerObjCount; i++)
        {
            AddPointerObj(spherePrefab);
        }

        //AddPonterObj(conePrefab);
    }

    private void AddPointerObj(GameObject prefab)
    {
        GameObject sphere = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        sphere.transform.SetParent(transform, true);
        sphere.layer = gameObject.layer;
        pointerObjs.Add(sphere);
    }

    public void ShipPointAtFocussedCell()
    {
        Vector3 shipButtonPos = shipButtonsObj[player.fleet.GetShipIndex(player.ActiveShip.No)].transform.position;
        shipButtonPos.z += 0.2f;

        Vector3 focussedCellPos = player.FocusedCell.transform.position;
        focussedCellPos = new(focussedCellPos.x, focussedCellPos.y + cellYOffset, focussedCellPos.z);

        SetCurve(shipButtonPos, focussedCellPos);

        for (int i = 0; i < pointerObjs.Count; i ++)
        {
            Vector3 position = GetPostionOnVector(0, pointerObjs.Count - 1, shipButtonPos, focussedCellPos, i);
            float time = Vector3.Distance(shipButtonPos, position);
            float deltaY = curveY.Evaluate(time);
            position.y += deltaY;
            pointerObjs[i].transform.position = position;

            //pointerObj[i].transform.LookAt(focussedCellWorldPos, Vector3.up);

            //Vector3 directionToDest = focussedCellWorldPos - transform.position;
            //pointerObj[i].transform.rotation = Quaternion.LookRotation(directionToDest);
            pointerObjs[i].SetActive(true);
        }
    }

    private void SetCurve(Vector3 distanceStart, Vector3 distanceEnd)
    {
        // The distance between the shipButton and the focused cell positions represent the x-axis of the AnimationCurve for the y-coordinate.
        float time = Vector3.Distance(distanceStart, distanceEnd);
        Keyframe keyframe = new(time, 0, curveY[1].inTangent, curveY[1].outTangent); // time is x-axis of an AnimationCurve
        curveY.MoveKey(1, keyframe);
    }

    private Vector3 GetPostionOnVector(int startPositon, int endPosition, Vector3 startCoordinates, Vector3 endCooordinates, int positionToRemap) => new()
    {
        x = math.remap(startPositon, endPosition, startCoordinates.x, endCooordinates.x, positionToRemap),
        y = math.remap(startPositon, endPosition, startCoordinates.y, endCooordinates.y, positionToRemap),
        z = math.remap(startPositon, endPosition, startCoordinates.z, endCooordinates.z, positionToRemap)
    };

    public void Deactivate() => ActivateDeactivate(false);

    public void Activate() => ActivateDeactivate(true);

    private void ActivateDeactivate(bool activate)
    {
        foreach (GameObject pointerObj in pointerObjs)
        {
            pointerObj.SetActive(activate);
        }

        if (activate) ShipPointAtFocussedCell();
    }
}
