using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NinjaSceneManager : MonoBehaviour
{
    public static NinjaSceneManager instance { get; private set; }

    private Coroutine changeSceneCoroutine;
    private FadeUI fadeUI;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        fadeUI = FindAnyObjectByType<FadeUI>();
    }

    private FadeUI GetFadeUIComponent()
    {
        if (fadeUI == null)
            fadeUI = FindFirstObjectByType<FadeUI>();

        return fadeUI;
    }

    public void ChangeScene(string sceneName)
    {
        if (changeSceneCoroutine != null)
            StopCoroutine(changeSceneCoroutine);

        changeSceneCoroutine = StartCoroutine(ChangeSceneCo(sceneName));
    }

    IEnumerator ChangeSceneCo(string sceneName)
    {
        GetFadeUIComponent().FadeIn();

        yield return GetFadeUIComponent().fadeCoroutine;

        SceneManager.LoadScene(sceneName);
    }
}
