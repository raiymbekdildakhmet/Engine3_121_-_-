using UnityEngine;

public class PlayerLiveComponent : ILiveComponent
{
    private int health = 100;

    public bool IsAlive => health > 0;

    public void GetDamage(int damage)
    {
        if (!IsAlive) return;

        health -= damage;
        if (health < 0) health = 0;

        Debug.Log($"Игрок получил урон {damage}. Осталось жизней {health}");

        // Вызываем событие изменения здоровья
        GameEvents.HealthChanged(health);

        if (!IsAlive)
        {
            Debug.Log("Игрок погиб!");
            GameManager.Instance.StopGame();
        }
    }
}