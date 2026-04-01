using System.Collections;
using UnityEngine;

public class CharacterSpawnController : MonoBehaviour
{
    [Header("Настройки спавна")]
    [SerializeField] private float spawnRadius = 10f;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private int baseMaxEnemies = 5;

    [Header("Настройки сложности")]
    [SerializeField] private int enemiesPerMinute = 3;
    [SerializeField] private float intervalReduction = 0.3f;
    [SerializeField] private float minSpawnInterval = 0.5f;

    [Header("Ссылки")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Character playerCharacter;
    [SerializeField] private CharacterFactory characterFactory;
    [SerializeField] private GameTimer gameTimer;

    private Coroutine spawnCoroutine;
    private float currentSpawnInterval;
    private int currentMaxEnemies;
    private int currentMinute = 0;

    public void StartSpawn()
    {
        currentSpawnInterval = spawnInterval;
        currentMaxEnemies = baseMaxEnemies;
        currentMinute = 0;
        spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    public void StopSpawn()
    {
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);

        StopAllEnemies();
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            UpdateDifficulty();

            if (characterFactory.GetActiveEnemies().Count < currentMaxEnemies)
            {
                SpawnEnemy();
            }

            yield return new WaitForSeconds(currentSpawnInterval);
        }
    }

    private void UpdateDifficulty()
    {
        if (gameTimer == null) return;

        int minutes = gameTimer.ElapsedMinutes;

        // Сначала обновляем значения
        currentMaxEnemies = baseMaxEnemies + (minutes * enemiesPerMinute);

        currentSpawnInterval = Mathf.Max(
            minSpawnInterval,
            spawnInterval - (minutes * intervalReduction)
        );

        // Потом логируем после обновления
        if (minutes > currentMinute)
        {
            currentMinute = minutes;
            Debug.Log($"Минута {currentMinute}! " +
                      $"Макс врагов: {currentMaxEnemies}, " +
                      $"Интервал: {currentSpawnInterval}");
        }
    }

    private void SpawnEnemy()
    {
        Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPosition = new Vector3(
            playerTransform.position.x + randomCircle.x,
            0,
            playerTransform.position.z + randomCircle.y
        );

        GameObject enemyObj = characterFactory.GetEnemy();
        enemyObj.transform.position = spawnPosition;

        EnemyCharacter enemy = enemyObj.GetComponent<EnemyCharacter>();
        if (enemy != null)
            enemy.Initialize(playerCharacter);
    }

    private void StopAllEnemies()
    {
        foreach (GameObject enemyObj in characterFactory.GetActiveEnemies())
        {
            EnemyCharacter enemy = enemyObj.GetComponent<EnemyCharacter>();
            if (enemy != null)
                enemy.StopEnemy();
        }
    }

    public string GetDifficultyInfo()
    {
        return $"Минута: {currentMinute} | " +
               $"Макс врагов: {currentMaxEnemies} | " +
               $"Интервал: {currentSpawnInterval:F1}с";
    }
}