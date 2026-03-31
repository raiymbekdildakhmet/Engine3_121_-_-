using UnityEngine;

public class PlayerCharacter : Character
{
    protected override void Start()
    {
        base.Start();
        LiveComponent = new PlayerLiveComponent();

        // Инициализируем ввод игрока
        InputReader = new PlayerInputReader();
    }

    public override void Update()
    {
        if (!LiveComponent.IsAlive)
            return;

        // Получаем направление через InputReader
        Vector3 movementVector = InputReader.GetMovementDirection();

        MovableComponent.Move(movementVector);
        MovableComponent.Rotation(movementVector);
    }
}