using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private CharacterSpawnController spawnController;
    [SerializeField] private GameTimer gameTimer;

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
        StartGame();
    }

    private void Update()
    {
        if (gameTimer != null)
            UIManager.Instance?.UpdateTimer(gameTimer.ElapsedTime);
    }

    public void StartGame()
    {
        score = 0;
        gameTimer.StartTimer();
        spawnController.StartSpawn();
        GameEvents.GameStarted();
        GameEvents.ScoreChanged(score);
        Debug.Log("Игра началась!");
    }

    public void StopGame()
    {
        gameTimer.StopTimer();
        spawnController.StopSpawn();

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(Constants.HIGH_SCORE_KEY, highScore);
            PlayerPrefs.Save();
            Debug.Log("Новый рекорд: " + highScore);
        }

        GameEvents.GameOver(highScore);
        Debug.Log("Игра остановлена! Счёт: " + score);
    }

    public void AddScore(int points)
    {
        score += points;
        GameEvents.ScoreChanged(score);
        Debug.Log("Очки: " + score);
    }

    public void RestartGame()
    {
        CharacterFactory factory = FindFirstObjectByType<CharacterFactory>();
        if (factory != null)
        {
            var enemies = new System.Collections.Generic.List<GameObject>(
                factory.GetActiveEnemies()
            );
            foreach (GameObject enemy in enemies)
                factory.ReturnEnemy(enemy);
        }

        PlayerCharacter player = FindFirstObjectByType<PlayerCharacter>();
        if (player != null)
            player.ResetHealth();

        StartGame();
        Debug.Log("Игра перезапущена!");
    }
}