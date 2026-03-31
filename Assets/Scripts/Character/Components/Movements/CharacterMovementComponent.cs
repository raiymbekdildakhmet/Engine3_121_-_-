using UnityEngine;

public class CharacterMovementComponent : IMovable
{
    private CharacterData characterData;
    private float speed;

    // Теперь принимаем Character а не CharacterData!
    public void Initialize(Character character)
    {
        // Берём данные через свойство CharacterData
        characterData = character.CharacterData;
        speed = characterData.DefaultSpeed;
    }

    public void Move(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return;

        characterData.CharacterController.Move(
            direction * speed * Time.deltaTime
        );
    }

    public void Rotation(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return;

        float targetAngle = Mathf.Atan2(direction.x, direction.z)
                            * Mathf.Rad2Deg;

        characterData.CharacterTransform.rotation =
            Quaternion.Euler(0, targetAngle, 0);
    }
}