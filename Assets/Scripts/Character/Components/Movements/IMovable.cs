using UnityEngine;

public interface IMovable
{
    // Метод движения
    void Move(Vector3 direction);

    // Метод поворота
    void Rotation(Vector3 direction);
}