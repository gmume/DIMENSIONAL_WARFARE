using UnityEngine;

public class MultiDisplay : MonoBehaviour
{
    private Camera camera1, camera2;

    public void Start()
    {
        //Debug.LogError("Force the build console open...");

        camera1 = GameObject.Find("Camera1").GetComponent<Camera>();
        camera2 = GameObject.Find("Camera2").GetComponent<Camera>();

        //GUI is rendered with last camera.
        //Make sure main camera is the last one drawn.
        camera1.enabled = true;
        camera2.enabled = true;

        Display.displays[1].Activate();
        camera1.SetTargetBuffers(Display.main.colorBuffer, Display.main.depthBuffer);
        camera2.SetTargetBuffers(Display.displays[1].colorBuffer, Display.displays[1].depthBuffer);
    }
}