using UnityEngine;

public abstract class Window : MonoBehaviour
{
    [SerializeField] private string windowName;

    [Space(10)]
    [SerializeField] private Animator windowAnimator;

    [SerializeField] protected string openAnimationName = "Open";
    [SerializeField] protected string idleAnimationName = "Idle";
    [SerializeField] protected string closeAnimationName = "Close";
    [SerializeField] protected string hiddenAnimationName = "Hidden";

    public bool IsOpened { get; protected set; } = false;

    protected Animator WindowAnimator
    {
        get
        {
            if (windowAnimator == null)
                windowAnimator = GetComponent<Animator>();
            return windowAnimator;
        }
    }

    public virtual void Initialize() { }

    public void Show(bool isImmediately)
    {
        OpenStart();

        // Играем анимацию только если есть Controller
        if (WindowAnimator != null &&
            WindowAnimator.runtimeAnimatorController != null)
        {
            WindowAnimator.Play(isImmediately
                ? idleAnimationName
                : openAnimationName);
        }

        if (isImmediately)
            OpenEnd();
    }

    public void Hide(bool isImmediately)
    {
        CloseStart();

        // Играем анимацию только если есть Controller
        if (WindowAnimator != null &&
            WindowAnimator.runtimeAnimatorController != null)
        {
            WindowAnimator.Play(isImmediately
                ? hiddenAnimationName
                : closeAnimationName);
        }

        if (isImmediately)
            CloseEnd();
    }

    protected virtual void OpenStart()
    {
        this.gameObject.SetActive(true);
        IsOpened = true;
    }

    // Вызывается через Animation Event
    // на последнем кадре Open анимации
    protected virtual void OpenEnd() { }

    protected virtual void CloseStart() { }

    // Вызывается через Animation Event
    // на последнем кадре Close анимации
    protected virtual void CloseEnd()
    {
        this.gameObject.SetActive(false);
        IsOpened = false;
    }
}