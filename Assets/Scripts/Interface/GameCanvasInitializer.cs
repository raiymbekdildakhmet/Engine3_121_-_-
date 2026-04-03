using UnityEngine;

public class GameCanvasInitializer : MonoBehaviour
{
    [Header("Сервисы")]
    [SerializeField] private WindowsService windowsService;

    private void Start()
    {
        if (windowsService == null)
        {
            Debug.LogError("WindowsService не назначен!");
            return;
        }

        windowsService.Initialize();
        Debug.Log("UI система инициализирована!");
    }
}