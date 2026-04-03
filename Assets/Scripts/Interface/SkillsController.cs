using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Управляет списком скиллов в ScrollView.
/// Создаёт SkillItem для каждого скилла.
/// </summary>
public class SkillsController : MonoBehaviour
{
    [Header("ScrollView")]
    [SerializeField] private Transform content;
    [SerializeField] private GameObject skillItemPrefab;

    [Header("Скиллы")]
    [SerializeField] private List<SkillData> skills;

    private void Start()
    {
        CreateSkillItems();
    }

    private void CreateSkillItems()
    {
        foreach (SkillData skill in skills)
        {
            // Создаём элемент из префаба
            GameObject item = Instantiate(
                skillItemPrefab,
                content
            );

            // Инициализируем данными
            SkillItem skillItem = item.GetComponent<SkillItem>();
            skillItem.Setup(skill);
        }
    }
}