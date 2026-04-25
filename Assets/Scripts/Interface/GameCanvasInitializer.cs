using UnityEngine;

public class GameCanvasInitializer : MonoBehaviour
{
    [Header("Сервисы")]
    [SerializeField] private WindowsService windowsService;

    private void Awake()
    {
        if (windowsService == null)
        {
            Debug.LogError("[GameCanvasInitializer] WindowsService не назначен!");
            return;
        }

        windowsService.Initialize();
        Debug.Log("[GameCanvasInitializer] UI система инициализирована!");

        // Стартовое окно
        windowsService.ShowWindow<MainMenuWindow>(true);
    }
}