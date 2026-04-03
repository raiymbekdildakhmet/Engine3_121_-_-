using UnityEngine;

public class PlayerCharacter : Character
{
    [SerializeField] private CharacterConfig config;

    // Сохраняем начальную позицию при старте
    private Vector3 startPosition;
    private Quaternion startRotation;

    protected override void Start()
    {
        base.Start();

        // Запоминаем начальную позицию
        startPosition = transform.position;
        startRotation = transform.rotation;

        int damage = config != null ? config.damage : 5;
        float cooldown = config != null ? config.attackCooldown : 1f;

        LiveComponent = new PlayerLiveComponent();
        AttackComponent = new CharacterAttackComponent(damage, cooldown);

        // Пока используем клавиатуру
        // Joystick добавим позже отдельно
        InputReader = new PlayerInputReader();
    }

    public void ResetHealth()
    {
        // Восстанавливаем здоровье
        LiveComponent = new PlayerLiveComponent();
        GameEvents.HealthChanged(100);

        // Возвращаем на начальную позицию
        transform.position = startPosition;
        transform.rotation = startRotation;

        Debug.Log("Игрок возрождён на стартовой позиции!");
    }

    public override void Update()
    {
        if (!LiveComponent.IsAlive)
            return;

        Vector3 movementVector = InputReader.GetMovementDirection();
        MovableComponent.Move(movementVector);
        MovableComponent.Rotation(movementVector);

        AttackNearestEnemy();
    }

    private void AttackNearestEnemy()
    {
        float radius = config != null ? config.attackRadius : 3f;

        Collider[] colliders = Physics.OverlapSphere(
            transform.position,
            radius
        );

        EnemyCharacter nearestEnemy = null;
        float nearestDistance = float.MaxValue;

        foreach (Collider col in colliders)
        {
            EnemyCharacter enemy = col.GetComponent<EnemyCharacter>();
            if (enemy != null && enemy.LiveComponent.IsAlive)
            {
                float distance = Vector3.Distance(
                    transform.position,
                    enemy.transform.position
                );

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = enemy;
                }
            }
        }

        if (nearestEnemy != null)
            AttackComponent.MakeDamage(nearestEnemy);
    }

    private void OnDrawGizmosSelected()
    {
        float radius = config != null ? config.attackRadius : 3f;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}