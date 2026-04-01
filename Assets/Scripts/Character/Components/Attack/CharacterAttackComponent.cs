using UnityEngine;

public class CharacterAttackComponent : IAttackComponent
{
    private int damage;
    private float attackCooldown;
    private float knockbackForce;
    private float lastAttackTime;

    public CharacterAttackComponent(int damage, float attackCooldown, float knockbackForce = 0f)
    {
        this.damage = damage;
        this.attackCooldown = attackCooldown;
        this.knockbackForce = knockbackForce;
    }

    public void MakeDamage(Character target)
    {
        if (Time.time - lastAttackTime < attackCooldown)
            return;

        lastAttackTime = Time.time;
        target.LiveComponent.GetDamage(damage);
    }
}