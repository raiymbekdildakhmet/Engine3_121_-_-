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

    [Space(10)]
    [Tooltip("Ставит ли это окно игру на паузу (Time.timeScale = 0)")]
    [SerializeField] private bool pausesGame = true;

    [Tooltip("Фоновое окно (например GameplayCanvas / MainMenu) — не вытесняется попапами и не уходит в стек")]
    [SerializeField] private bool isBackgroundWindow = false;

    public string WindowName => windowName;
    public bool PausesGame => pausesGame;
    public bool IsBackgroundWindow => isBackgroundWindow;
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

    public virtual void Initialize()
    {
        // Чтобы анимации работали при Time.timeScale = 0
        if (WindowAnimator != null)
            WindowAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    public void Show(bool isImmediately)
    {
        OpenStart();

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
        if (!IsOpened && !gameObject.activeSelf) return;

        CloseStart();

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

    // Вызывается через Animation Event на последнем кадре Open-анимации
    protected virtual void OpenEnd() { }

    protected virtual void CloseStart()
    {
        IsOpened = false;
    }

    // Вызывается через Animation Event на последнем кадре Close-анимации
    protected virtual void CloseEnd()
    {
        this.gameObject.SetActive(false);
        IsOpened = false;
    }
}