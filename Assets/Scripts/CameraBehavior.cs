using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    private new Camera camera;
    private GameObject armed;

    private void Start()
    {
        camera = GetComponent<Camera>();

        if(name == "Camera1")
        {
            armed = GameObject.Find("Armed1");
        }
        else
        {
            armed = GameObject.Find("Armed2");
        }
        armed.SetActive(false);
    }

    public void UpdateCamera(GamePhaces phace)
    {
        switch (phace)
        {
            case GamePhaces.Armed:
                if(name == "Camera1")
                {
                    camera.cullingMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "Water", "UI", "Player2", "VisibleShips", "FleetMenu1", "Armed");
                }
                else
                {
                    camera.cullingMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "Water", "UI", "Player1", "VisibleShips", "FleetMenu2", "Armed");
                }

                if (!armed.activeSelf)
                {
                    armed.SetActive(true);
                }
                break;

            case GamePhaces.Attacked:
                if (name == "Camera1")
                {
                    camera.cullingMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "Water", "UI", "Player1", "Fleet1", "FleetMenu1");
                }
                else
                {
                    camera.cullingMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "Water", "UI", "Player2", "Fleet2", "FleetMenu2");
                }

                if (armed.activeSelf)
                {
                    armed.SetActive(false);
                }
                break;

            case GamePhaces.End:
                if (name == "Camera1")
                {
                    camera.cullingMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "Water", "UI", "Player1", "Fleet1", "VisibleShips", "FleetMenu1");
                }
                else
                {
                    camera.cullingMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "Water", "UI", "Player2", "Fleet2", "VisibleShips", "FleetMenu2");
                }
                if (armed.activeSelf)
                {
                    armed.SetActive(false);
                }
                break;
            default:
                Debug.Log("No game phace found!");
                break;
        }
    }
}
