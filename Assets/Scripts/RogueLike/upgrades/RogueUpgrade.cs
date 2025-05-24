using UnityEngine;
//using System;

[CreateAssetMenu(menuName = "Game/RogueUpgrade")]
public class RogueUpgrade : ScriptableObject
{
    [Tooltip("업그레이드 이름")]
    public string upgradeName;
    public int rogueID = -1;

    [Tooltip("옵션 설명 (수치를 알파벳 N으로 표현)")]
    [TextArea]
    public string description;

    [Tooltip("값 범위 (랜덤)")]
    [SerializeField] private float minValue = 0.1f;
    [SerializeField] private float maxValue = 0.3f;
    [SerializeField] private bool isPercent = false;

    [Header("업그레이드 아이콘")]
    [SerializeField] public Sprite Icon;

    [HideInInspector]
    public float value;


    public void RandomizeValue()
    {
        value = Random.Range(minValue, maxValue);
        if (isPercent) value = Mathf.Round(value*100)/100;
        else value = Mathf.Round(value);
    }

    public string getDescription()
    {
        // string TXT = this.description;
        if (this.isPercent) return this.description.Replace("N", $"{value * 100}%");
        return this.description.Replace("N", $"{value}");
    }
}
