using UnityEngine;

public class EnemyCharacter : Character
{
    [SerializeField] private Character targetCharacter;
    [SerializeField] private AiState aiState;
    [SerializeField] private float attackDistance = 1.5f;

    protected override void Start()
    {
        base.Start();
        LiveComponent = new EnemyLiveComponent();
        AttackComponent = new CharacterAttackComponent();

        // Передаём Transform вместо Character
        InputReader = new NpcInputReader(
            this.transform,
            targetCharacter != null ? targetCharacter.transform : null
        );

        aiState = AiState.MoveToTarget;
    }

    public override void Update()
    {
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
        if (targetCharacter == null)
            return;

        float distance = Vector3.Distance(
            targetCharacter.transform.position,
            characterData.CharacterTransform.position
        );

        // Если близко — переходим в атаку
        if (distance <= attackDistance)
        {
            aiState = AiState.Attack;
            return;
        }

        // Получаем направление через InputReader
        Vector3 direction = InputReader.GetMovementDirection();

        MovableComponent.Move(direction);
        MovableComponent.Rotation(direction);
    }

    private void AttackTarget()
    {
        if (targetCharacter == null)
            return;

        float distance = Vector3.Distance(
            targetCharacter.transform.position,
            characterData.CharacterTransform.position
        );

        // Если игрок убежал — возвращаемся в преследование
        if (distance > attackDistance)
        {
            aiState = AiState.MoveToTarget;
            return;
        }

        // Атакуем игрока
        AttackComponent.MakeDamage(targetCharacter);
    }
}