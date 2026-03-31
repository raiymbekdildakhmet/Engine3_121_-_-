using UnityEngine;

public class EnemyLiveComponent : ILiveComponent
{
    // Текущее здоровье
    private int health = 50;

    // Живой ли персонаж
    public bool IsAlive => health > 0;

    // Получить урон
    public void GetDamage(int damage)
    {
        health -= damage;
        Debug.Log($"Враг получил урон {damage}. Осталось жизней {health}");
    }
}