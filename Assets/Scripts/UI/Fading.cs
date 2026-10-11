using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fading : MonoBehaviour
{
    public Coroutine fadeCoroutine { get; private set; }

    [SerializeField] private float changeSceneInSecs;

    [Header("Panel Details")]
    [SerializeField] private Text gameOverTitle;
    [SerializeField] private Color winTextColor;
    [SerializeField] private Color loseTextColor;
    [Space]
    [SerializeField] private Image background;
    [SerializeField] private Color winBgColor;
    [SerializeField] private Color loseBgColor;

    private CanvasGroup changeScreenPanel;

    protected virtual void Awake()
    {
        changeScreenPanel = GetComponent<CanvasGroup>();
    }

    public void SetPanelOnGameEnd(bool playerWin)
    {
        gameOverTitle.text = playerWin ? "You WIN!!" : "You LOSE!!";
        gameOverTitle.color = playerWin ? winTextColor : loseTextColor;
        background.color = playerWin ? winBgColor : loseBgColor;
    }

    public void FadeIn()
    {
        DoFade(1);
    }

    public void FadeOut()
    {
        DoFade(0);
    }

    protected void DoFade(float targetAlpha)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadingCoroutine(targetAlpha));
    }

    IEnumerator FadingCoroutine(float targetAlpha)
    {
        float timePassed = 0;
        float startAlpha = changeScreenPanel.alpha;

        while (timePassed < changeSceneInSecs)
        {
            timePassed += Time.deltaTime;
            changeScreenPanel.alpha = Mathf.Lerp(startAlpha, targetAlpha, timePassed / changeSceneInSecs);

            yield return null;
        }

        changeScreenPanel.alpha = targetAlpha;
    }
}
