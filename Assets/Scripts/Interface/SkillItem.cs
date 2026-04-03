using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Один контейнер скилла в ScrollView.
/// Показывает название, описание, стоимость, кнопку.
/// </summary>
public class SkillItem : MonoBehaviour
{
    [Header("Тексты")]
    [SerializeField] private TMP_Text skillNameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text costText;

    [Header("Кнопка")]
    [SerializeField] private Button buyButton;
    [SerializeField] private TMP_Text buyButtonText;

    [Header("Иконка")]
    [SerializeField] private Image skillIcon;

    private SkillData skillData;

    /// <summary>
    /// Инициализация элемента данными скилла
    /// </summary>
    public void Setup(SkillData data)
    {
        skillData = data;

        // Заполняем текстовые поля
        skillNameText.text = data.skillName;
        descriptionText.text = data.description;
        costText.text = data.cost.ToString() + " 💰";

        // Иконка если есть
        if (data.icon != null)
            skillIcon.sprite = data.icon;

        // Подписываемся на кнопку
        buyButton.onClick.AddListener(OnBuyClicked);

        // Обновляем состояние кнопки
        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        if (skillData.isPurchased)
        {
            // Скилл куплен
            buyButton.interactable = false;
            buyButtonText.text = "Куплено ✓";
        }
        else
        {
            // Скилл не куплен
            buyButton.interactable = true;
            buyButtonText.text = "Купить";
        }
    }

    private void OnBuyClicked()
    {
        // Проверяем хватает ли денег
        int playerScore = GameManager.Instance.Score;

        if (playerScore >= skillData.cost)
        {
            skillData.isPurchased = true;
            UpdateButtonState();
            Debug.Log("Куплен скилл: " + skillData.skillName);
        }
        else
        {
            Debug.Log("Не хватает денег! Нужно: "
                + skillData.cost
                + " Есть: " + playerScore);
        }
    }
}