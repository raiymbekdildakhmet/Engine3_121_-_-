using UnityEngine;

public class CharacterData : MonoBehaviour
{
    [SerializeField] private float defaultSpeed = 3f;
    [SerializeField] private Transform characterTransform;
    [SerializeField] private CharacterController characterController;

    // —войства только дл€ чтени€ (get есть, set Ч нет!)
    public float DefaultSpeed
    {
        get { return defaultSpeed; }
    }

    public Transform CharacterTransform
    {
        get { return characterTransform; }
    }

    public CharacterController CharacterController
    {
        get { return characterController; }
    }
}