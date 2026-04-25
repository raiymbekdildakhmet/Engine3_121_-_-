using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuWindow : Window
{
    [Header("Кнопки")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button optionsButton;

    [Header("Опционально")]
    [SerializeField] private TextMeshProUGUI highScoreText;

    public override void Initialize()
    {
        base.Initialize();

        if (startButton != null)
            startButton.onClick.AddListener(OnStartClicked);
        if (optionsButton != null)
            optionsButton.onClick.AddListener(OnOptionsClicked);
    }

    protected override void OpenStart()
    {
        base.OpenStart();

        if (highScoreText != null && GameManager.Instance != null)
            highScoreText.text = "Best: " + GameManager.Instance.HighScore;
    }

    private void OnStartClicked()
    {
        GameManager.Instance.StartGame();
    }

    private void OnOptionsClicked()
    {
        GameManager.Instance.OpenOptions();
    }

    private void OnDestroy()
    {
        if (startButton != null)
            startButton.onClick.RemoveListener(OnStartClicked);
        if (optionsButton != null)
            optionsButton.onClick.RemoveListener(OnOptionsClicked);
    }
}