using UnityEngine;

public class EnemyCharacter : Character
{
    [SerializeField] private Character targetCharacter;
    [SerializeField] private AiState aiState;
    [SerializeField] private float attackDistance = 1.5f;
    [SerializeField] private CharacterConfig config;

    private CharacterFactory characterFactory;
    private bool isDead = false;

    protected override void Start()
    {
        base.Start();

        int health = config != null ? config.health : 50;
        int damage = config != null ? config.damage : 5;
        float cooldown = config != null ? config.attackCooldown : 1f;

        LiveComponent = new EnemyLiveComponent(health);
        AttackComponent = new CharacterAttackComponent(damage, cooldown);

        InputReader = new NpcInputReader(
            this.transform,
            targetCharacter != null ? targetCharacter.transform : null
        );

        aiState = AiState.MoveToTarget;
        characterFactory = FindFirstObjectByType<CharacterFactory>();
    }

    public void Initialize(Character target)
    {
        targetCharacter = target;
        isDead = false;

        InputReader = new NpcInputReader(
            this.transform,
            target != null ? target.transform : null
        );

        aiState = AiState.MoveToTarget;

        int health = config != null ? config.health : 50;
        LiveComponent = new EnemyLiveComponent(health);
    }

    public override void Update()
    {
        if (isDead) return;

        if (!LiveComponent.IsAlive)
        {
            Die();
            return;
        }

        switch (aiState)
        {
            case AiState.None:
                return;
            case AiState.MoveToTarget:
                MoveToTarget();
                return;
            case AiState.Attack:
                AttackTarget();
                return;
        }
    }

    private void MoveToTarget()
    {
        if (targetCharacter == null) return;

        float distance = Vector3.Distance(
            targetCharacter.transform.position,
            characterData.CharacterTransform.position
        );

        if (distance <= attackDistance)
        {
            aiState = AiState.Attack;
            return;
        }

        Vector3 direction = InputReader.GetMovementDirection();
        MovableComponent.Move(direction);
        MovableComponent.Rotation(direction);
    }

    private void AttackTarget()
    {
        if (targetCharacter == null) return;

        float distance = Vector3.Distance(
            targetCharacter.transform.position,
            characterData.CharacterTransform.position
        );

        if (distance > attackDistance)
        {
            aiState = AiState.MoveToTarget;
            return;
        }

        AttackComponent.MakeDamage(targetCharacter);
    }

    // Остановить врага (когда игра закончилась)
    public void StopEnemy()
    {
        aiState = AiState.None;
    }

    public void Die()
    {
        isDead = true;
        GameManager.Instance.AddScore(10);
        Debug.Log("Враг убит! +10 очков");

        if (characterFactory != null)
            characterFactory.ReturnEnemy(gameObject);
    }
}