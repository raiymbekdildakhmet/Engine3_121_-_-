using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Игровой UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Экран смерти")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private Button restartButton;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        GameEvents.OnScoreChanged += UpdateScore;
        GameEvents.OnHealthChanged += UpdateHealth;
        GameEvents.OnGameOver += ShowGameOver;
        GameEvents.OnGameStarted += HideGameOver;
    }

    private void OnDisable()
    {
        GameEvents.OnScoreChanged -= UpdateScore;
        GameEvents.OnHealthChanged -= UpdateHealth;
        GameEvents.OnGameOver -= ShowGameOver;
        GameEvents.OnGameStarted -= HideGameOver;
    }

    private void Start()
    {
        // Подключаем кнопку рестарта
        if (restartButton != null)
            restartButton.onClick.AddListener(OnRestartClicked);
    }

    private void OnRestartClicked()
    {
        GameManager.Instance.RestartGame();
    }

    public void UpdateTimer(float elapsedTime)
    {
        if (timerText == null) return;
        int minutes = (int)(elapsedTime / 60f);
        int seconds = (int)(elapsedTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void UpdateScore(int score)
    {
        if (scoreText != null)
            scoreText.text = "Очки: " + score;
    }

    private void UpdateHealth(int health)
    {
        if (healthText != null)
            healthText.text = "HP: " + health;
    }

    private void ShowGameOver(int highScore)
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        if (highScoreText != null)
            highScoreText.text = "Рекорд: " + highScore;
    }

    private void HideGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }
}