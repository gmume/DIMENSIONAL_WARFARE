using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class VehicleManager : MonoBehaviour
{
    private Vector3 vector, endPosition, startPosition, zoomedInPosition, zoomedOutPosition;
    private bool zoomedOut = false;
    private int currentDimension;

    public AnimationCurve zoomCurve;

    public float panDuration = 1f;
    private System.DateTime startTime;
    private float journeyFraction;

    public void Start()
    {
        transform.position += new Vector3(OverworldData.DimensionSize / 2, OverworldData.DimensionSize / 1.2f, -OverworldData.DimensionSize * 1.3f);
        vector = new Vector3(0, OverworldData.DimensionSize * 2, 0);

        zoomedOutPosition = new Vector3(OverworldData.DimensionSize / 2, OverworldData.DimensionSize * OverworldData.DimensionsCount, -OverworldData.DimensionSize * OverworldData.DimensionsCount * 2);
        currentDimension = 0;
    }

    //public void SetViewOnDimension(int toNo)
    //{
    //    int vectorFactor = toNo - currentDimension;
    //    transform.position += vector * vectorFactor;
    //    currentDimension = toNo;
    //}

    public void SetViewOnDimension(int toNo)
    {
        panDuration = 1f;
        int vectorFactor = toNo - currentDimension;

        endPosition = transform.position + vector * vectorFactor;
        StartCoroutine("PanCamera");

        currentDimension = toNo;
    }

    public void OnZoom(CallbackContext ctx)
    {
        if (!ctx.performed) return;

        if (zoomedOut)
        {
            endPosition = zoomedInPosition;
        }
        else
        {
            zoomedInPosition = transform.position;
            endPosition = zoomedOutPosition;
        }

        panDuration = 1.5f;
        StartCoroutine("PanCamera");
        zoomedOut = !zoomedOut;
    }

    private IEnumerator PanCamera()
    {
        startPosition = transform.position;
        startTime = System.DateTime.Now;
        journeyFraction = 0f;

        while (journeyFraction < panDuration)
        {
            System.DateTime currentTime = System.DateTime.Now;
            float timeRange = (float)(currentTime.Subtract(startTime).TotalMilliseconds / 1000);
            journeyFraction = timeRange / panDuration;

            float curveSample = zoomCurve.Evaluate(journeyFraction);

            Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, curveSample);
            transform.position = newPosition;

            yield return new WaitForSecondsRealtime(0.07f);
        }
    }
}