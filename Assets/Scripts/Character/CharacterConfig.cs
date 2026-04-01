using UnityEngine;

[CreateAssetMenu(fileName = "CharacterConfig", menuName = "Game/CharacterConfig")]
public class CharacterConfig : ScriptableObject
{
    [Header("Здоровье")]
    public int health = 50;

    [Header("Атака")]
    public int damage = 5;
    public float attackCooldown = 1f;
    public float attackRadius = 3f;

    [Header("Движение")]
    public float speed = 3f;
    public float knockbackForce = 2f;
}