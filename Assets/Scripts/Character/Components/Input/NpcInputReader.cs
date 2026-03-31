using UnityEngine;

public class NpcInputReader : IInputReader
{
    private Transform ownerTransform;
    private Transform targetTransform;

    public NpcInputReader(Transform owner, Transform target)
    {
        ownerTransform = owner;
        targetTransform = target;
    }

    public Vector3 GetMovementDirection()
    {
        // Проверяем что цель существует
        if (targetTransform == null)
            return Vector3.zero;

        Vector3 direction = targetTransform.position
                          - ownerTransform.position;

        return direction.normalized;
    }
}