using System;
using UnityEngine;

public static class GameEvents
{
    // Событие изменения очков
    public static event Action<int> OnScoreChanged;

    // Событие изменения здоровья
    public static event Action<int> OnHealthChanged;

    // Событие конца игры
    public static event Action<int> OnGameOver;

    // Событие старта игры
    public static event Action OnGameStarted;

    // Вызов событий
    public static void ScoreChanged(int score)
    {
        OnScoreChanged?.Invoke(score);
    }

    public static void HealthChanged(int health)
    {
        OnHealthChanged?.Invoke(health);
    }

    public static void GameOver(int highScore)
    {
        OnGameOver?.Invoke(highScore);
    }

    public static void GameStarted()
    {
        OnGameStarted?.Invoke();
    }
}