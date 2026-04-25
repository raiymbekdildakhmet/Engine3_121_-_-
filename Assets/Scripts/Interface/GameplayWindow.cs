using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayWindow : Window
{
    [Header("Опыт / Уровень")]
    [SerializeField] private Slider experienceSlider;

    [Header("Таймер, монеты, киллы")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private TMP_Text enemiesKilledText;

    [Header("Кнопки")]
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button pauseButton;

    public override void Initialize()
    {
        base.Initialize();

        if (upgradeButton != null)
            upgradeButton.onClick.AddListener(OnUpgradeClicked);
        if (pauseButton != null)
            pauseButton.onClick.AddListener(OnPauseClicked);
    }

    protected override void OpenStart()
    {
        base.OpenStart();
        UpdateScore(0);
    }

    private void OnEnable()
    {
        GameEvents.OnScoreChanged += UpdateScore;
    }

    private void OnDisable()
    {
        GameEvents.OnScoreChanged -= UpdateScore;
    }

    private void UpdateScore(int score)
    {
        if (coinsText != null)
            coinsText.text = score.ToString();
        if (enemiesKilledText != null && GameManager.Instance != null)
            enemiesKilledText.text = GameManager.Instance.EnemiesKilled.ToString();
    }

    private void OnUpgradeClicked()
    {
        GameManager.Instance.OpenUpgrade();
    }

    private void OnPauseClicked()
    {
        GameManager.Instance.PauseGame();
    }

    private void Update()
    {
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.gameTimer == null) return;
        if (!IsOpened) return;

        float gameSeconds = GameManager.Instance.gameTimer.ElapsedTime;
        int minutes = (int)(gameSeconds / 60);
        int seconds = (int)(gameSeconds % 60);
        if (timerText != null)
            timerText.text = minutes + ":" + (seconds < 10 ? "0" : "") + seconds;
    }

    private void OnDestroy()
    {
        if (upgradeButton != null)
            upgradeButton.onClick.RemoveListener(OnUpgradeClicked);
        if (pauseButton != null)
            pauseButton.onClick.RemoveListener(OnPauseClicked);
    }
}