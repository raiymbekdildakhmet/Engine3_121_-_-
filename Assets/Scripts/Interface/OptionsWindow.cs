using UnityEngine;
using UnityEngine.UI;

public class OptionsWindow : Window
{
    [Header("Переключатели")]
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle soundsToggle;

    [Header("Кнопки")]
    [SerializeField] private Button exitButton;

    public override void Initialize()
    {
        musicToggle.isOn = PlayerPrefs.GetInt("Music", 1) == 1;
        soundsToggle.isOn = PlayerPrefs.GetInt("Sounds", 1) == 1;

        musicToggle.onValueChanged.AddListener(OnMusicChanged);
        soundsToggle.onValueChanged.AddListener(OnSoundsChanged);
        exitButton.onClick.AddListener(OnExitClicked);
    }

    protected override void OpenEnd()
    {
        base.OpenEnd();
        exitButton.interactable = true;
    }

    protected override void CloseStart()
    {
        base.CloseStart();
        exitButton.interactable = false;
    }

    private void OnMusicChanged(bool value)
    {
        PlayerPrefs.SetInt("Music", value ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log("Музыка: " + (value ? "ВКЛ" : "ВЫКЛ"));
    }

    private void OnSoundsChanged(bool value)
    {
        PlayerPrefs.SetInt("Sounds", value ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log("Звуки: " + (value ? "ВКЛ" : "ВЫКЛ"));
    }

    private void OnExitClicked()
    {
        // false = с анимацией
        Hide(false);
    }
}