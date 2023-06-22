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

    public void UpdateCamera(GamePhases phase)
    {
        switch (phase)
        {
            case GamePhases.Armed:
                if(name == "Camera1")
                {
                    camera.cullingMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "Water", "UI", "Player2", "Fleet1", "VisibleShips", "FleetMenu1", "Armed");
                }
                else
                {
                    camera.cullingMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "Water", "UI", "Player1", "Fleet2", "VisibleShips", "FleetMenu2", "Armed");
                }

                if (!armed.activeSelf)
                {
                    armed.SetActive(true);
                }
                break;

            case GamePhases.Attacked:
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

            case GamePhases.End:
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
                Debug.Log("No game phase found!");
                break;
        }
    }
}
