using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private CharacterSpawnController spawnController;
    [SerializeField] public GameTimer gameTimer;
    [SerializeField] private WindowsService windowsService;
    public WindowsService WindowsService => windowsService;

    private int score = 0;
    public int Score => score;

    private int highScore = 0;
    public int HighScore => highScore;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        highScore = PlayerPrefs.GetInt(Constants.HIGH_SCORE_KEY, 0);
        Debug.Log("Рекорд загружен: " + highScore);
    }

    // Вызывается каждый раз при нажатии Start Game или Restart
    public void StartGame()
    {
        // Сбрасываем счёт
        score = 0;

        // Убираем ВСЕХ врагов со сцены
        ClearAllEnemies();

        // Возвращаем игрока на старт и восстанавливаем здоровье
        ResetPlayer();

        // Сбрасываем и запускаем таймер
        gameTimer.StartTimer();

        // Запускаем спавн врагов заново
        spawnController.StopSpawn();
        spawnController.StartSpawn();

        // Отправляем события
        GameEvents.GameStarted();
        GameEvents.ScoreChanged(score);
        GameEvents.HealthChanged(100);

        Debug.Log("Игра началась!");
    }

    public void StopGame()
    {
        // Останавливаем таймер
        gameTimer.StopTimer();

        // Останавливаем спавн
        spawnController.StopSpawn();

        // Останавливаем всех врагов
        StopAllEnemies();

        // Сохраняем рекорд
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(Constants.HIGH_SCORE_KEY, highScore);
            PlayerPrefs.Save();
            Debug.Log("Новый рекорд: " + highScore);
        }

        // Показываем окно поражения
        windowsService.HideWindow<GameplayWindow>(true);
        windowsService.ShowWindow<DefeatWindow>(true);

        GameEvents.GameOver(highScore);
        Debug.Log("Игра остановлена! Счёт: " + score);
    }

    public void AddScore(int points)
    {
        score += points;
        GameEvents.ScoreChanged(score);
        Debug.Log("Очки: " + score);
    }

    // Возвращаем игрока на стартовую позицию
    private void ResetPlayer()
    {
        PlayerCharacter player = FindFirstObjectByType<PlayerCharacter>();
        if (player != null)
        {
            player.ResetHealth();
            Debug.Log("Игрок сброшен на старт!");
        }
    }

    // Убираем всех врагов
    private void ClearAllEnemies()
    {
        CharacterFactory factory = FindFirstObjectByType<CharacterFactory>();
        if (factory == null) return;

        var enemies = new System.Collections.Generic.List<GameObject>(
            factory.GetActiveEnemies()
        );

        foreach (GameObject enemy in enemies)
            factory.ReturnEnemy(enemy);

        Debug.Log("Все враги убраны!");
    }

    // Останавливаем врагов (не убираем)
    private void StopAllEnemies()
    {
        CharacterFactory factory = FindFirstObjectByType<CharacterFactory>();
        if (factory == null) return;

        foreach (GameObject enemyObj in factory.GetActiveEnemies())
        {
            EnemyCharacter enemy = enemyObj.GetComponent<EnemyCharacter>();
            if (enemy != null)
                enemy.StopEnemy();
        }
    }

    // Старый метод RestartGame теперь просто вызывает StartGame
    public void RestartGame()
    {
        StartGame();
        Debug.Log("Игра перезапущена!");
    }
}