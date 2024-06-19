using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroHandler : MonoBehaviour
{
    public FadeEffect effect1;
    public FadeEffect effect2;

    public void Start()
    {
        effect1.StartEffect();
        effect2.StartEffect();

        StartCoroutine(DoEffectAgain());
    }

    private IEnumerator DoEffectAgain()
    {
        yield return new WaitUntil(() => effect1.finished && effect2.finished);
        yield return new WaitForSeconds(2);
        effect1.StartEffect();
        effect2.StartEffect();

        StartCoroutine (LoadLobby());
    }

    private IEnumerator LoadLobby()
    {
        yield return new WaitUntil(() => effect1.finished && effect2.finished);
        SceneChanger.LoadLobby();
    }
}
