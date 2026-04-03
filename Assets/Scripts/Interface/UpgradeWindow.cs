using UnityEngine;
using UnityEngine.UI;

public class UpgradeWindow : Window
{
    [Header("Кнопки")]
    [SerializeField] private Button closeButton;

    public override void Initialize()
    {
        closeButton.onClick.AddListener(OnCloseClicked);
    }

    protected override void OpenEnd()
    {
        base.OpenEnd();
        closeButton.interactable = true;
    }

    protected override void CloseStart()
    {
        base.CloseStart();
        closeButton.interactable = false;
    }

    private void OnCloseClicked()
    {
        Hide(true);
    }
}