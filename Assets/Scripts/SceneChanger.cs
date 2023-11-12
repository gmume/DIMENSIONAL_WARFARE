using UnityEngine;
using UnityEngine.SceneManagement;

//[CreateAssetMenu(menuName = "SceneChanger")]

public class SceneChanger : MonoBehaviour
{
    public void LoadLobby() => SceneManager.LoadSceneAsync("Assets/Scenes/Lobby.unity", LoadSceneMode.Single);
    public void LoadPlay() => SceneManager.LoadSceneAsync("Assets/Scenes/Play.unity", LoadSceneMode.Single);
    public void LoadResolveGame() => SceneManager.LoadSceneAsync("Assets/Scenes/ResolveGame.unity", LoadSceneMode.Single);
}
