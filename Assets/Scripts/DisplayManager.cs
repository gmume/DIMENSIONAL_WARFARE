using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    private Camera camera1, camera2;

    public void Start()
    {
        camera1 = GameObject.Find("Camera1").GetComponent<Camera>();
        camera2 = GameObject.Find("Camera2").GetComponent<Camera>();
        camera1.enabled = true;
        camera2.enabled = true;

        Display.displays[1].Activate();

        Debug.LogError("Force console open.");
    }
}