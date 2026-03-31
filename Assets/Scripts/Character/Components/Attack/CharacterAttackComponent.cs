using UnityEngine;

public class CharacterAttackComponent : IAttackComponent
{
    // Урон за одну атаку
    private int damage = 5;

    // Время между атаками (секунды)
    private float attackCooldown = 1f;

    // Время последней атаки
    private float lastAttackTime;

    // Реализация атаки
    public void MakeDamage(Character target)
    {
        // Проверяем cooldown — не атакуем слишком часто
        if (Time.time - lastAttackTime < attackCooldown)
            return;

        // Запоминаем время атаки
        lastAttackTime = Time.time;

        // Наносим урон цели
        target.LiveComponent.GetDamage(damage);
    }
}