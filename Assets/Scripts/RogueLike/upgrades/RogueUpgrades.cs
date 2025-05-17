using UnityEngine;

[CreateAssetMenu(menuName = "Game/RogueUpgrade")]
public class RogueUpgrade : ScriptableObject
{
    [Tooltip("업그레이드 이름")]
    public string upgradeName;

    public float value;

    [Tooltip("옵션 설명")]
    [TextArea]
    public string description;
}
