using UnityEngine;

[CreateAssetMenu(menuName = "Game/RogueUpgrade")]
public class RogueUpgrade : ScriptableObject
{
    [Tooltip("업그레이드 이름")]
    public string upgradeName;

    [Tooltip("값 범위 (랜덤)")]
    public float minValue = 0.1f;
    public float maxValue = 0.3f;

    [HideInInspector]
    public float value;

    [Tooltip("옵션 설명")]
    [TextArea]
    public string description;

    public void RandomizeValue()
    {
        value = Random.Range(minValue, maxValue);
    }
}
