using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeWindow : Window
{
    [Header("Кнопки")]
    [SerializeField] private Button closeButton;

    [Header("Заголовок")]
    [SerializeField] private TextMeshProUGUI titleText;

    [Header("Скилы")]
    [SerializeField] private Transform skillsContent;
    [SerializeField] private GameObject skillItemPrefab;

    public override void Initialize()
    {
        base.Initialize();

        if (closeButton != null)
            closeButton.onClick.AddListener(OnCloseClicked);
    }

    protected override void OpenStart()
    {
        base.OpenStart();
        RefreshSkills();
    }

    private void RefreshSkills()
    {
        if (skillsContent == null) return;

        foreach (Transform child in skillsContent)
            Destroy(child.gameObject);

        // TODO: подгрузить актуальный список скилов из сервиса/конфига
        // foreach (var skill in SkillService.Instance.GetAvailableSkills())
        // {
        //     var item = Instantiate(skillItemPrefab, skillsContent);
        //     item.GetComponent<SkillItemView>().Bind(skill);
        // }
    }

    private void OnCloseClicked()
    {
        GameManager.Instance.WindowsService.CloseCurrentWindow(false);
    }

    private void OnDestroy()
    {
        if (closeButton != null)
            closeButton.onClick.RemoveListener(OnCloseClicked);
    }
}