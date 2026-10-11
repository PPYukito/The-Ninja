using NUnit.Framework.Internal;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneController : SceneControllerEntities
{
    public enum BackgroundMusic
    {
        Normal,
        Battle
    }

    public static GameSceneController instance { get; private set; }

    [Header("General Details")]
    [SerializeField] private EquipWeapon currentWeapon;

    [Header("Score Details")]
    [SerializeField] private TextMeshProUGUI targetGoalText;

    #region Events
    [Header("Events")]
    [SerializeField] private float waitSecsUntilPlayerDiedToShowGameOverPanel;
    #endregion

    [SerializeField] private Fading GameOverPanel;

    private Player player;
    private GameObject[] enemies;
    private int enemyKillAmount;
    private int enemyAmount;

    private Coroutine waitForPlayerCoroutine;
    private Coroutine OnGameOverCoroutine;

    protected override void Awake()
    {
        base.Awake();

        player = FindFirstObjectByType<Player>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        if (instance != null || instance != this)
            instance = this;

        player.SetEquipWeapon(currentWeapon);

        GetEnemy();
        UpdateTargetGoalText();
    }

    private void GetEnemy()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemyAmount = enemies.Length;

        foreach (GameObject enemy in enemies)
        {
            EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
            enemyAI.InitializeEnemy(EnemyDied);
        }
    }

    private void UpdateTargetGoalText()
    {
        targetGoalText.text = $"{enemyKillAmount}/{enemyAmount}";
    }

    private void EnemyDied()
    {
        enemyKillAmount++;
        UpdateTargetGoalText();

        if (enemyKillAmount >= enemyAmount)
        {
            Debug.LogWarning("You won!!");
            player.StopPlayer();
            OnGameOver(true);
        }
    }

    public void OnGameOver(bool playerWin)
    {
        GameOverPanel.SetPanelOnGameEnd(playerWin);

        if (waitForPlayerCoroutine != null)
            StopCoroutine(waitForPlayerCoroutine);

        waitForPlayerCoroutine = StartCoroutine(WaitForPlayerCo());
    }

    IEnumerator WaitForPlayerCo()
    {
        yield return new WaitForSeconds(waitSecsUntilPlayerDiedToShowGameOverPanel);

        if (OnGameOverCoroutine != null)
            StopCoroutine(OnGameOverCoroutine);

        OnGameOverCoroutine = StartCoroutine(OnGameOverCo());
    }

    IEnumerator OnGameOverCo()
    {
        StopBGMSound();
        GameOverPanel.FadeIn();
        yield return GameOverPanel.fadeCoroutine;

        Cursor.lockState = CursorLockMode.None;
    }

    public void ChangeBackgroundMusic()
    {

    }

    public void RestartGame()
    {
        PlayClickSound();
        NinjaSceneManager.instance.ChangeScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        PlayClickSound();
        NinjaSceneManager.instance.ChangeScene("TitleScene");
    }
}
