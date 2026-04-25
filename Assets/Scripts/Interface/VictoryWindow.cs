using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VictoryWindow : Window
{
    [Header("Кнопки")]
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    [Header("Текст")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI killsText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    public override void Initialize()
    {
        base.Initialize();

        if (restartButton != null)
            restartButton.onClick.AddListener(OnRestartClicked);
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(OnMainMenuClicked);
    }

    protected override void OpenStart()
    {
        base.OpenStart();

        if (GameManager.Instance != null)
        {
            if (scoreText != null)
                scoreText.text = "Score: " + GameManager.Instance.Score;
            if (killsText != null)
                killsText.text = "Kills: " + GameManager.Instance.EnemiesKilled;
            if (highScoreText != null)
                highScoreText.text = "Best: " + GameManager.Instance.HighScore;
        }
    }

    private void OnRestartClicked()
    {
        GameManager.Instance.RestartGame();
    }

    private void OnMainMenuClicked()
    {
        GameManager.Instance.GoToMainMenu();
    }

    private void OnDestroy()
    {
        if (restartButton != null)
            restartButton.onClick.RemoveListener(OnRestartClicked);
        if (mainMenuButton != null)
            mainMenuButton.onClick.RemoveListener(OnMainMenuClicked);
    }
}