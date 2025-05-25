using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "debuff", menuName = "Scriptable Objects/debuff")]
public abstract class debuffBase : ScriptableObject
{
    public string debuffName;
    public float duration;
    // 디버프 아이콘 저장용 변수 추가
    public Sprite Icon;

    public abstract void Apply(GameObject target);
}
