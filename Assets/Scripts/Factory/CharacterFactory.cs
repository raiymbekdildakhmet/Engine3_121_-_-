using System.Collections.Generic;
using UnityEngine;

public class CharacterFactory : MonoBehaviour
{
    [Header("Префабы")]
    [SerializeField] private GameObject enemyPrefab;

    // Queue — пул неактивных врагов
    private Queue<GameObject> enemyPool = new Queue<GameObject>();

    // List — список активных врагов на сцене
    private List<GameObject> activeEnemies = new List<GameObject>();

    // Получить врага из пула
    public GameObject GetEnemy()
    {
        GameObject enemy;

        if (enemyPool.Count > 0)
        {
            enemy = enemyPool.Dequeue();
            enemy.SetActive(true);
        }
        else
        {
            enemy = Instantiate(enemyPrefab);
        }

        activeEnemies.Add(enemy);  // добавляем в активный список
        return enemy;
    }

    // Вернуть врага в пул
    public void ReturnEnemy(GameObject enemy)
    {
        activeEnemies.Remove(enemy);  // убираем из активного списка
        enemy.SetActive(false);
        enemyPool.Enqueue(enemy);
    }

    // Получить список всех активных врагов
    public List<GameObject> GetActiveEnemies()
    {
        return activeEnemies;
    }
}