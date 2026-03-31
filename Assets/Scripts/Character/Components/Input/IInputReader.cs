using UnityEngine;

public interface IInputReader
{
    // Возвращает направление движения
    Vector3 GetMovementDirection();
}