using UnityEngine;

[CreateAssetMenu(fileName = "debuff", menuName = "Scriptable Objects/debuff")]
public abstract class debuffBase : ScriptableObject
{
    public string debuffName;
    public float duration;

    public abstract void Apply(GameObject target);
}
