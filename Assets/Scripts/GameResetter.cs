using UnityEngine;
using UnityEngine.InputSystem;

public class GameResetter : MonoBehaviour
{
    public PlayerData player1, player2;
    float timestamp = 600.0f;

    public void Start()
    {
        player1.input.onActionTriggered += OnResetTimer;
        player2.input.onActionTriggered += OnResetTimer;
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
