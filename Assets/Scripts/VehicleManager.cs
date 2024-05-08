using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class VehicleManager : MonoBehaviour
{
    PlayerData player;
    private Vector3 changeDimensionVector, endPosition, startPosition, zoomedInPosition, zoomedOutPosition;
    private bool zoomedOut = false;
    private int currentDimension;
    public AnimationCurve curve;
    private float panDuration, journeyFraction;
    private DateTime startTime;
    private readonly string[] actionNames = { "DimensionDown", "DimensionUp", "Zoom" };
    private readonly List<InputAction> actions = new();

    public void Start()
    {
        player = name[^1].ToString() == "1" ? OverworldData.Player1 : OverworldData.Player2;
        InitializePosition();
        InitializePanning();
    }

    private void InitializePosition()
    {
        transform.position += new Vector3(OverworldData.DimensionSize / 2, OverworldData.DimensionSize / 1.2f, -OverworldData.DimensionSize * 0.4f);
        currentDimension = 0;
    }

    private void InitializePanning()
    {
        changeDimensionVector = new Vector3(0, OverworldData.DimensionSize * 2, 0);
        zoomedOutPosition = new Vector3(OverworldData.DimensionSize / 2, OverworldData.DimensionSize * OverworldData.DimensionsCount, -OverworldData.DimensionSize * OverworldData.DimensionsCount * 2);

        for (int i = 0; i < actions.Count; i++)
        {
            actions[i] = player.input.actions.FindAction(actionNames[i]);
        }
    }

    public void SetViewOnDimension(int toNo)
    {
        InitiatePanning(1f, transform.position + changeDimensionVector * (toNo - currentDimension));
        currentDimension = toNo;
    }

    public void OnZoom(CallbackContext ctx)
    {
        if (!ctx.performed) return;
        if (!zoomedOut) zoomedInPosition = transform.position;
        InitiatePanning(1.5f, (zoomedOut) ? zoomedInPosition : zoomedOutPosition);
        zoomedOut = !zoomedOut;
    }

    private void InitiatePanning(float panDuration, Vector3 endPosition)
    {
        player.input.currentActionMap.Disable();
        this.panDuration = panDuration;
        this.endPosition = endPosition;
        startPosition = transform.position;
        journeyFraction = 0f;
        startTime = DateTime.Now;

        StartCoroutine("PanCamera");
    }

    private IEnumerator PanCamera()
    {
        while (journeyFraction < panDuration)
        {
            DateTime currentTime = DateTime.Now;
            float timeRange = (float)(currentTime.Subtract(startTime).TotalMilliseconds / 1000);
            journeyFraction = timeRange / panDuration;
            float curveSample = curve.Evaluate(journeyFraction);
            Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, curveSample);
            transform.position = newPosition;

            yield return new WaitForSecondsRealtime(0.07f);
        }

        transform.position = endPosition;
        player.input.currentActionMap.Enable();
    }
}