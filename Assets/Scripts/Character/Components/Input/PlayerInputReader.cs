using UnityEngine;

public class PlayerInputReader : IInputReader
{
    // Считываем ввод с клавиатуры (WASD)
    public Vector3 GetMovementDirection()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        return new Vector3(
            moveHorizontal, 0, moveVertical
        ).normalized;
    }
}