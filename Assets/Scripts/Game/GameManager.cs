using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Сервисы")]
    [SerializeField] private WindowsService windowsService;
    [SerializeField] private CharacterSpawnController spawnController;
    [SerializeField] public GameTimer gameTimer;

    [Header("Игровые объекты сцены")]
    [Tooltip("Объект игрока — будет деактивирован на старте сцены и активирован при StartGame()")]
    [SerializeField] private GameObject playerObject;

    [Tooltip("Объект спавн-контроллера")]
    [SerializeField] private GameObject spawnControllerObject;

    [Tooltip("Объект таймера")]
    [SerializeField] private GameObject gameTimerObject;

    [Header("Условие победы")]
    [SerializeField] private int killsToWin = 10;

    public WindowsService WindowsService => windowsService;

    private int score = 0;
    public int Score => score;

    private int highScore = 0;
    public int HighScore => highScore;

    private int enemiesKilled = 0;
    public int EnemiesKilled => enemiesKilled;

    private int coinsCollected = 0;
    public int CoinsCollected => coinsCollected;

    private bool gameRunning = false;
    public bool IsGameRunning => gameRunning;

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
            return;
        }

        // КРИТИЧНО: выключаем игровые объекты на старте сцены,
        // чтобы спавн врагов и движение игрока не запустились
        // до того как игрок нажмёт Start.
        SetGameplayObjectsActive(false);
    }

    private void Start()
    {
        highScore = PlayerPrefs.GetInt(Constants.HIGH_SCORE_KEY, 0);
        Debug.Log("Рекорд загружен: " + highScore);
        // Инициализацию WindowsService делает GameCanvasInitializer
    }

    /// <summary>
    /// Включает/выключает все игровые объекты сцены (игрок, спавн, таймер).
    /// </summary>
    private void SetGameplayObjectsActive(bool isActive)
    {
        if (playerObject != null) playerObject.SetActive(isActive);
        if (spawnControllerObject != null) spawnControllerObject.SetActive(isActive);
        if (gameTimerObject != null) gameTimerObject.SetActive(isActive);
    }

    /// <summary>
    /// Старт новой игры. Вызывается из MainMenuWindow → Start.
    /// </summary>
    public void StartGame()
    {
        // Включаем игровые объекты
        SetGameplayObjectsActive(true);

        score = 0;
        enemiesKilled = 0;
        coinsCollected = 0;
        gameRunning = true;

        ClearAllEnemies();
        ResetPlayer();

        if (gameTimer != null) gameTimer.StartTimer();
        if (spawnController != null)
        {
            spawnController.StopSpawn();
            spawnController.StartSpawn();
        }

        // Закрываем все попапы и переключаем фон с MainMenu на GameplayWindow
        windowsService.CloseAllPopups(true);
        windowsService.ShowWindow<GameplayWindow>(false);

        Time.timeScale = 1f;

        GameEvents.GameStarted();
        GameEvents.ScoreChanged(score);
        GameEvents.HealthChanged(100);

        Debug.Log("Игра началась! Убей " + killsToWin + " врагов для победы.");
    }

    public void StopGame()
    {
        gameRunning = false;
        if (gameTimer != null) gameTimer.StopTimer();
        if (spawnController != null) spawnController.StopSpawn();
        StopAllEnemies();
        SaveHighScore();

        windowsService.ShowWindow<DefeatWindow>(false);

        GameEvents.GameOver(highScore);
        Debug.Log("Поражение! Счёт: " + score);
    }

    public void WinGame()
    {
        gameRunning = false;
        if (gameTimer != null) gameTimer.StopTimer();
        if (spawnController != null) spawnController.StopSpawn();
        StopAllEnemies();
        SaveHighScore();

        windowsService.ShowWindow<VictoryWindow>(false);

        Debug.Log("Победа! Убито врагов: " + enemiesKilled);
    }

    public void PauseGame()
    {
        if (!gameRunning) return;
        windowsService.ShowWindow<PauseWindow>(false);
    }

    public void ResumeGame()
    {
        windowsService.CloseCurrentWindow(false);
    }

    public void OpenOptions()
    {
        windowsService.ShowWindow<OptionsWindow>(false);
    }

    public void OpenUpgrade()
    {
        windowsService.ShowWindow<UpgradeWindow>(false);
    }

    public void RestartGame()
    {
        windowsService.CloseAllPopups(true);
        StartGame();
    }

    public void GoToMainMenu()
    {
        gameRunning = false;
        if (gameTimer != null) gameTimer.StopTimer();
        if (spawnController != null) spawnController.StopSpawn();
        StopAllEnemies();
        ClearAllEnemies();
        SaveHighScore();

        // Выключаем игровые объекты
        SetGameplayObjectsActive(false);

        windowsService.CloseAllPopups(true);
        windowsService.ShowWindow<MainMenuWindow>(false);

        Time.timeScale = 1f;
        Debug.Log("Возврат в главное меню");
    }

    private void SaveHighScore()
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(Constants.HIGH_SCORE_KEY, highScore);
            PlayerPrefs.Save();
        }
    }

    public void AddScore(int points)
    {
        if (!gameRunning) return;

        score += points;
        enemiesKilled++;
        coinsCollected += points;
        GameEvents.ScoreChanged(score);

        if (enemiesKilled >= killsToWin)
        {
            WinGame();
        }
    }

    private void ResetPlayer()
    {
        PlayerCharacter player = FindFirstObjectByType<PlayerCharacter>();
        if (player != null) player.ResetHealth();
    }

    private void ClearAllEnemies()
    {
        CharacterFactory factory = FindFirstObjectByType<CharacterFactory>();
        if (factory == null) return;

        var enemies = new System.Collections.Generic.List<GameObject>(
            factory.GetActiveEnemies()
        );
        foreach (GameObject enemy in enemies)
            factory.ReturnEnemy(enemy);
    }

    private void StopAllEnemies()
    {
        CharacterFactory factory = FindFirstObjectByType<CharacterFactory>();
        if (factory == null) return;

        foreach (GameObject enemyObj in factory.GetActiveEnemies())
        {
            EnemyCharacter enemy = enemyObj.GetComponent<EnemyCharacter>();
            if (enemy != null) enemy.StopEnemy();
        }
    }
}