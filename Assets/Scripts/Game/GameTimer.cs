using UnityEngine;

public class GameTimer : MonoBehaviour
{
    // Время игры в секундах
    private float elapsedTime = 0f;
    private bool isRunning = false;

    // Прошедшее время в секундах
    public float ElapsedTime => elapsedTime;

    // Прошедшее время в минутах
    public int ElapsedMinutes => (int)(elapsedTime / 60f);

    public void StartTimer()
    {
        elapsedTime = 0f;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    private void Update()
    {
        if (!isRunning) return;
        elapsedTime += Time.deltaTime;
    }
}