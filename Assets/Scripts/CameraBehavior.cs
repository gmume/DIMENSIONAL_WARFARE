using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    private Camera PlayerCamera;
    private GameObject armed;

    private void Start()
    {
        PlayerCamera = GetComponent<Camera>();

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
                    PlayerCamera.cullingMask = LayerMask.GetMask("Default", "Water", "Player2", "Fleet1", "VisibleShips", "FleetMenu1", "Armed");
                }
                else
                {
                    PlayerCamera.cullingMask = LayerMask.GetMask("Default", "Water", "Player1", "Fleet2", "VisibleShips", "FleetMenu2", "Armed");
                }

                if (!armed.activeSelf)
                {
                    armed.SetActive(true);
                }
                break;

            case GamePhases.Attacked:
                if (name == "Camera1")
                {
                    PlayerCamera.cullingMask = LayerMask.GetMask("Default", "Water", "Player1", "Fleet1", "VisibleShips", "FleetMenu1");
                }
                else
                {
                    PlayerCamera.cullingMask = LayerMask.GetMask("Default", "Water", "Player2", "Fleet2", "VisibleShips", "FleetMenu2");
                }

                if (armed.activeSelf)
                {
                    armed.SetActive(false);
                }
                break;

            case GamePhases.End:
                if (name == "Camera1")
                {
                    PlayerCamera.cullingMask = LayerMask.GetMask("Default", "Water", "Player1", "Fleet1", "VisibleShips", "FleetMenu1");
                }                                                   
                else                                                
                {                                                   
                    PlayerCamera.cullingMask = LayerMask.GetMask("Default", "Water", "Player2", "Fleet2", "VisibleShips", "FleetMenu2");
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
