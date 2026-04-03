using UnityEngine;
using UnityEngine.UI;

public class MainMenuWindow : Window
{
    [Header("Кнопки")]
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button optionsButton;

    public override void Initialize()
    {
        startGameButton.onClick.AddListener(OnStartGameClicked);
        optionsButton.onClick.AddListener(OnOptionsClicked);
    }

    protected override void OpenEnd()
    {
        base.OpenEnd();
        // Включаем кнопки после анимации открытия
        startGameButton.interactable = true;
        optionsButton.interactable = true;
    }

    protected override void CloseStart()
    {
        base.CloseStart();
        // Выключаем кнопки во время анимации закрытия
        startGameButton.interactable = false;
        optionsButton.interactable = false;
    }

    private void OnStartGameClicked()
    {
        // false = с анимацией закрытия
        Hide(false);
        GameManager.Instance.WindowsService
            .ShowWindow<GameplayWindow>(false);
        GameManager.Instance.StartGame();
    }

    private void OnOptionsClicked()
    {
        GameManager.Instance.WindowsService
            .ShowWindow<OptionsWindow>(false);
    }
}