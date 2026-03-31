using UnityEngine;

public class PlayerLiveComponent : ILiveComponent
{
    private int health = 100;

    public bool IsAlive => health > 0;

    public void GetDamage(int damage)
    {
        // Не получаем урон если уже мёртвы
        if (!IsAlive)
            return;

        health -= damage;

        // Не даём уйти ниже 0
        if (health < 0)
            health = 0;

        Debug.Log($"Игрок получил урон {damage}. Осталось жизней {health}");

        if (!IsAlive)
            Debug.Log("Игрок погиб!");
    }
}