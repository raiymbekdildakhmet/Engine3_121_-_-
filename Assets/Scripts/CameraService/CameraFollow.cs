using UnityEngine;

/// <summary>
/// Камера следует за игроком.
/// Вешается на Main Camera.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("Цель")]
    [SerializeField] private Transform target;

    [Header("Настройки")]
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private Vector3 offset = new Vector3(0, 15, 0);

    private void LateUpdate()
    {
        if (target == null)
        {
            // Ищем игрока если не назначен
            PlayerCharacter player =
                FindFirstObjectByType<PlayerCharacter>();
            if (player != null)
                target = player.transform;
            return;
        }

        // Целевая позиция камеры
        Vector3 targetPosition = new Vector3(
            target.position.x,
            offset.y,
            target.position.z
        );

        // Плавно двигаем камеру к цели
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );
    }
}