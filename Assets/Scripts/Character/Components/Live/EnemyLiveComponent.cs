using UnityEngine;

public class EnemyLiveComponent : ILiveComponent
{
    private int health;

    public EnemyLiveComponent(int health)
    {
        this.health = health;
    }

    public bool IsAlive => health > 0;

    public void GetDamage(int damage)
    {
        health -= damage;
        Debug.Log($"Враг получил урон {damage}. Осталось жизней {health}");
    }
}