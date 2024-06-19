using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class GameResetter : MonoBehaviour
{
    public PlayerInput input1, input2;
    float timestamp = 600.0f;

    public void Start()
    {
        input1.onActionTriggered += OnResetTimer;
        if (input2) input2.onActionTriggered += OnResetTimer;
    }

    public void Update()
    {
        if (Time.time >= timestamp) SceneChanger.LoadIntro();
    }

    public void OnResetTimer(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) timestamp = Time.time + 600;
    }
}
