using System;
using System.Collections.Generic;
using UnityEngine;

public class WindowsService : MonoBehaviour
{
    [SerializeField] private Window[] windows;

    private Dictionary<Type, Window> windowsDictionary;

    // Текущий попап (Pause, Options, Victory, Defeat, Upgrade)
    private Window currentWindow;

    // Текущее фоновое окно (MainMenuWindow / GameplayCanvas)
    private Window currentBackgroundWindow;

    // Стек попапов: Pause → Options → Pause
    private readonly Stack<Window> windowStack = new Stack<Window>();

    public Window CurrentWindow => currentWindow;
    public Window CurrentBackgroundWindow => currentBackgroundWindow;

    /// <summary>
    /// Инициализация всех окон. Вызывается из GameCanvasInitializer.
    /// Стартовое окно показывает GameCanvasInitializer.
    /// </summary>
    public void Initialize()
    {
        windowsDictionary = new Dictionary<Type, Window>();

        foreach (Window window in windows)
        {
            windowsDictionary.Add(window.GetType(), window);
            window.gameObject.SetActive(false);
            window.Initialize();
            Debug.Log("Инициализировано: " + window.GetType().Name);
        }
    }

    public T GetWindow<T>() where T : Window
    {
        if (!windowsDictionary.ContainsKey(typeof(T)))
        {
            Debug.LogError($"Окно {typeof(T).Name} не найдено!");
            return null;
        }
        return windowsDictionary[typeof(T)] as T;
    }

    /// <summary>
    /// Показать окно. Фоновое — заменит фон. Попап — вытеснит текущий попап в стек.
    /// </summary>
    public void ShowWindow<T>(bool isImmediately) where T : Window
    {
        var window = GetWindow<T>();
        if (window == null) return;

        if (window.IsBackgroundWindow)
            ShowBackgroundWindow(window, isImmediately);
        else
            ShowPopupWindow(window, isImmediately);
    }

    private void ShowBackgroundWindow(Window window, bool isImmediately)
    {
        if (currentBackgroundWindow == window && currentBackgroundWindow.IsOpened) return;

        if (currentBackgroundWindow != null && currentBackgroundWindow.IsOpened)
        {
            currentBackgroundWindow.Hide(isImmediately);
            if (isImmediately) currentBackgroundWindow.gameObject.SetActive(false);
        }

        window.gameObject.SetActive(true);
        window.Show(isImmediately);
        currentBackgroundWindow = window;

        UpdateTimeScale();
        Debug.Log("Показан фон: " + window.GetType().Name);
    }

    private void ShowPopupWindow(Window window, bool isImmediately)
    {
        if (currentWindow == window && currentWindow.IsOpened) return;

        if (currentWindow != null && currentWindow.IsOpened)
        {
            currentWindow.Hide(false);
            windowStack.Push(currentWindow);
        }

        window.gameObject.SetActive(true);
        window.Show(isImmediately);
        currentWindow = window;

        UpdateTimeScale();
        Debug.Log("Показан попап: " + window.GetType().Name);
    }

    /// <summary>
    /// Скрыть конкретное окно (без возврата к предыдущему).
    /// </summary>
    public void HideWindow<T>(bool isImmediately) where T : Window
    {
        var window = GetWindow<T>();
        if (window == null) return;

        window.Hide(isImmediately);
        if (isImmediately) window.gameObject.SetActive(false);

        if (currentWindow == window) currentWindow = null;
        if (currentBackgroundWindow == window) currentBackgroundWindow = null;

        UpdateTimeScale();
        Debug.Log("Скрыто: " + typeof(T).Name);
    }

    /// <summary>
    /// Закрыть текущий попап. Если в стеке есть предыдущий — вернуться к нему.
    /// </summary>
    public void CloseCurrentWindow(bool isImmediately = false)
    {
        if (currentWindow == null || !currentWindow.IsOpened) return;

        currentWindow.Hide(isImmediately);
        if (isImmediately) currentWindow.gameObject.SetActive(false);
        currentWindow = null;

        if (windowStack.Count > 0)
        {
            Window previous = windowStack.Pop();
            previous.gameObject.SetActive(true);
            previous.Show(isImmediately);
            currentWindow = previous;
        }

        UpdateTimeScale();
    }

    /// <summary>
    /// Закрыть все попапы. Фоновое окно остаётся.
    /// </summary>
    public void CloseAllPopups(bool isImmediately = true)
    {
        if (currentWindow != null && currentWindow.IsOpened)
        {
            currentWindow.Hide(isImmediately);
            if (isImmediately) currentWindow.gameObject.SetActive(false);
        }

        while (windowStack.Count > 0)
        {
            var w = windowStack.Pop();
            if (w.IsOpened)
            {
                w.Hide(true);
                w.gameObject.SetActive(false);
            }
        }

        currentWindow = null;
        windowStack.Clear();
        UpdateTimeScale();
    }

    /// <summary>
    /// Закрыть всё (включая фон) и снять паузу.
    /// </summary>
    public void CloseAllWindows(bool isImmediately = true)
    {
        CloseAllPopups(isImmediately);

        if (currentBackgroundWindow != null && currentBackgroundWindow.IsOpened)
        {
            currentBackgroundWindow.Hide(isImmediately);
            if (isImmediately) currentBackgroundWindow.gameObject.SetActive(false);
            currentBackgroundWindow = null;
        }

        Time.timeScale = 1f;
    }

    private void UpdateTimeScale()
    {
        if (currentWindow != null && currentWindow.IsOpened && currentWindow.PausesGame)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }
}