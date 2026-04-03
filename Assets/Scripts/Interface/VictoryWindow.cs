using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VictoryWindow : Window
{
    [Header("Тексты")]
    [SerializeField] private TMP_Text scoreCountText;
    [SerializeField] private TMP_Text newRecordText;

    [Header("Кнопки")]
    [SerializeField] private Button continueButton;

    public override void Initialize()
    {
        continueButton.onClick.AddListener(OnContinueClicked);
    }

    protected override void OpenStart()
    {
        base.OpenStart();

        int score = GameManager.Instance.Score;
        int highScore = GameManager.Instance.HighScore;

        scoreCountText.text = score.ToString();

        bool isNewRecord = score >= highScore && score > 0;
        newRecordText.gameObject.SetActive(isNewRecord);
    }

    protected override void OpenEnd()
    {
        base.OpenEnd();
        continueButton.interactable = true;
    }

    protected override void CloseStart()
    {
        base.CloseStart();
        continueButton.interactable = false;
    }

    private void OnContinueClicked()
    {
        // false = с анимацией
        Hide(false);
        GameManager.Instance.WindowsService
            .ShowWindow<MainMenuWindow>(false);
    }
}