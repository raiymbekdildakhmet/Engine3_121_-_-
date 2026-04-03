using System;
using System.Collections.Generic;
using UnityEngine;

public class WindowsService : MonoBehaviour
{
    // Массив всех окон — заполняется в Inspector
    // Перетащи: MainMenuWindow, GameplayCanvas,
    // OptionsWindow, DefeatWindow, VictoryWindow
    [SerializeField] private Window[] windows;

    private Dictionary<Type, Window> windowsDictionary;

    /// <summary>
    /// Инициализация всех окон.
    /// Вызывается из GameCanvasInitializer при старте.
    /// </summary>
    public void Initialize()
    {
        windowsDictionary = new Dictionary<Type, Window>();

        foreach (Window window in windows)
        {
            // Добавляем в словарь по типу
            windowsDictionary.Add(window.GetType(), window);

            // Скрываем сразу — деактивируем GameObject
            window.gameObject.SetActive(false);

            // Инициализируем каждое окно
            // (подписка на кнопки внутри каждого окна)
            window.Initialize();

            Debug.Log("Инициализировано: " + window.GetType().Name);
        }

        // Показываем главное меню при старте
        ShowWindow<MainMenuWindow>(true);
        Debug.Log("MainMenuWindow показан!");
    }

    /// <summary>
    /// Получить окно по типу.
    /// Пример: GetWindow&lt;MainMenuWindow&gt;()
    /// </summary>
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
    /// Показать окно по типу.
    /// isImmediately = true  → мгновенно без анимации
    /// isImmediately = false → с анимацией открытия
    /// </summary>
    public void ShowWindow<T>(bool isImmediately) where T : Window
    {
        var window = GetWindow<T>();
        if (window == null) return;

        // Активируем GameObject перед показом
        window.gameObject.SetActive(true);

        // Запускаем Show (с анимацией или без)
        window.Show(isImmediately);

        Debug.Log("Показано окно: " + typeof(T).Name);
    }

    /// <summary>
    /// Скрыть окно по типу.
    /// isImmediately = true  → мгновенно без анимации
    /// isImmediately = false → с анимацией закрытия
    /// </summary>
    public void HideWindow<T>(bool isImmediately) where T : Window
    {
        var window = GetWindow<T>();
        if (window == null) return;

        // Запускаем Hide (с анимацией или без)
        window.Hide(isImmediately);

        // Если мгновенно — сразу деактивируем GameObject
        // Если с анимацией — деактивация происходит
        // в CloseEnd() через Animation Event
        if (isImmediately)
            window.gameObject.SetActive(false);

        Debug.Log("Скрыто окно: " + typeof(T).Name);
    }
}