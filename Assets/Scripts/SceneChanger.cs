using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneChanger
{
    public static void LoadIntro() => SceneManager.LoadSceneAsync("Assets/Scenes/Intro.unity", LoadSceneMode.Single);
    public static void LoadLobby() => SceneManager.LoadSceneAsync("Assets/Scenes/Lobby.unity", LoadSceneMode.Single);
    public static void LoadPlay() => SceneManager.LoadSceneAsync("Assets/Scenes/Play.unity", LoadSceneMode.Single);
    public static void LoadResolveGame() => SceneManager.LoadSceneAsync("Assets/Scenes/ResolveGame.unity", LoadSceneMode.Single);
}
