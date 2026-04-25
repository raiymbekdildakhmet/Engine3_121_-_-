using UnityEngine;
using UnityEngine.UI;

public class OptionsWindow : Window
{
    [Header("Переключатели")]
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle soundToggle;

    [Header("Кнопки")]
    [SerializeField] private Button exitButton;

    private const string MusicKey = "Settings_Music";
    private const string SoundKey = "Settings_Sound";

    public override void Initialize()
    {
        base.Initialize();

        if (musicToggle != null)
        {
            musicToggle.isOn = PlayerPrefs.GetInt(MusicKey, 1) == 1;
            musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
        }

        if (soundToggle != null)
        {
            soundToggle.isOn = PlayerPrefs.GetInt(SoundKey, 1) == 1;
            soundToggle.onValueChanged.AddListener(OnSoundToggleChanged);
        }

        if (exitButton != null)
            exitButton.onClick.AddListener(OnExitClicked);
    }

    private void OnMusicToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt(MusicKey, isOn ? 1 : 0);
        PlayerPrefs.Save();
        AudioListener.pause = !isOn;
    }

    private void OnSoundToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt(SoundKey, isOn ? 1 : 0);
        PlayerPrefs.Save();
        AudioListener.volume = isOn ? 1f : 0f;
    }

    private void OnExitClicked()
    {
        GameManager.Instance.WindowsService.CloseCurrentWindow(false);
    }

    private void OnDestroy()
    {
        if (musicToggle != null)
            musicToggle.onValueChanged.RemoveListener(OnMusicToggleChanged);
        if (soundToggle != null)
            soundToggle.onValueChanged.RemoveListener(OnSoundToggleChanged);
        if (exitButton != null)
            exitButton.onClick.RemoveListener(OnExitClicked);
    }
}