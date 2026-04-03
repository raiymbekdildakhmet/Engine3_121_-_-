using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayWindow : Window
{
    [Header("Здоровье")]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Slider healthSlider;

    [Header("Опыт")]
    [SerializeField] private Slider experienceSlider;

    [Header("Таймер и монеты")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text coinsText;

    [Header("Кнопка прокачки")]
    [SerializeField] private Button upgradeButton;

    public override void Initialize()
    {
        if (upgradeButton != null)
            upgradeButton.onClick.AddListener(OnUpgradeClicked);
    }

    protected override void OpenStart()
    {
        base.OpenStart();

        GameEvents.OnHealthChanged += UpdateHealth;
        GameEvents.OnScoreChanged += UpdateScore;

        UpdateHealth(100);
        UpdateScore(0);
    }

    protected override void CloseStart()
    {
        base.CloseStart();

        GameEvents.OnHealthChanged -= UpdateHealth;
        GameEvents.OnScoreChanged -= UpdateScore;
    }

    private void UpdateHealth(int health)
    {
        healthText.text = health + " / 100";
        healthSlider.maxValue = 100;
        healthSlider.value = health;
    }

    private void UpdateScore(int score)
    {
        coinsText.text = score.ToString();
    }

    private void OnUpgradeClicked()
    {
        GameManager.Instance.WindowsService
            .ShowWindow<UpgradeWindow>(false);
        Debug.Log("Открыто окно прокачки!");
    }

    private void Update()
    {
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.gameTimer == null) return;
        if (!IsOpened) return;

        float gameSeconds = GameManager.Instance
            .gameTimer.ElapsedTime;

        int minutes = (int)(gameSeconds / 60);
        int seconds = (int)(gameSeconds % 60);

        timerText.text = minutes + ":"
            + (seconds < 10 ? "0" : "") + seconds;
    }
}