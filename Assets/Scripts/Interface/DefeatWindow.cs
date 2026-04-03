using UnityEngine;
using UnityEngine.UI;

public class DefeatWindow : Window
{
    [Header("Кнопки")]
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    public override void Initialize()
    {
        restartButton.onClick.AddListener(OnRestartClicked);
        mainMenuButton.onClick.AddListener(OnMainMenuClicked);
    }

    protected override void OpenEnd()
    {
        base.OpenEnd();
        // Включаем кнопки после анимации
        restartButton.interactable = true;
        mainMenuButton.interactable = true;
    }

    protected override void CloseStart()
    {
        base.CloseStart();
        // Выключаем кнопки во время анимации
        restartButton.interactable = false;
        mainMenuButton.interactable = false;
    }

    private void OnRestartClicked()
    {
        // false = с анимацией закрытия
        Hide(false);
        GameManager.Instance.WindowsService
            .ShowWindow<GameplayWindow>(false);
        GameManager.Instance.StartGame();
    }

    private void OnMainMenuClicked()
    {
        Hide(false);
        GameManager.Instance.WindowsService
            .ShowWindow<MainMenuWindow>(false);
    }
}