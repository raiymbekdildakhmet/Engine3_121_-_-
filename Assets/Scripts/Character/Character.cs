using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected CharacterData characterData;

    // Публичное свойство только для чтения
    public CharacterData CharacterData
    {
        get { return characterData; }
    }

    protected CharacterMovementComponent MovableComponent;
    public ILiveComponent LiveComponent { get; protected set; }
    protected IAttackComponent AttackComponent;
    protected IInputReader InputReader;

    protected virtual void Start()
    {
        MovableComponent = new CharacterMovementComponent();

        // Передаём Character вместо CharacterData!
        MovableComponent.Initialize(this);
    }

    public abstract void Update();
}