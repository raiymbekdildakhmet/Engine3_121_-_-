using UnityEngine;

/// <summary>
/// Данные одного скилла прокачки.
/// Не MonoBehaviour — просто данные!
/// </summary>
[System.Serializable]
public class SkillData
{
    [Header("Информация")]
    public string skillName;
    public string description;
    public int cost;
    public Sprite icon;
    public bool isPurchased;
}