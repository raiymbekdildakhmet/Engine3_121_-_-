using UnityEngine;
using UnityEngine.UI;

public class PauseWindow : Window
{
    [Header("Кнопки")]
    [SerializeField] private Button continueButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button mainMenuButton;

    public override void Initialize()
    {
        base.Initialize();

        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinueClicked);
        if (optionsButton != null)
            optionsButton.onClick.AddListener(OnOptionsClicked);
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(OnMainMenuClicked);
    }

    private void OnContinueClicked()
    {
        GameManager.Instance.ResumeGame();
    }

    private void OnOptionsClicked()
    {
        GameManager.Instance.OpenOptions();
    }

    private void OnMainMenuClicked()
    {
        GameManager.Instance.GoToMainMenu();
    }

    private void OnDestroy()
    {
        if (continueButton != null)
            continueButton.onClick.RemoveListener(OnContinueClicked);
        if (optionsButton != null)
            optionsButton.onClick.RemoveListener(OnOptionsClicked);
        if (mainMenuButton != null)
            mainMenuButton.onClick.RemoveListener(OnMainMenuClicked);
    }
}