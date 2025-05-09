using UnityEngine;

[CreateAssetMenu(fileName = "SlowDebuff", menuName = "Scriptable Objects/Debuff/Slow")]
public class SlowDebuff : debuffBase
{
    public float slowAmount = 0.3f;

    public override void Apply(GameObject target) {
        // 몬스터에 디버프를 줄건데 어떻게...?

        // 1) 
        var method = target.GetType().GetMethod("applyDebuff", new System.Type[] { typeof(string), typeof(float) });
        method?.Invoke(target, new object[] { "Slow", duration });

        Debug.Log($"[SlowDebuff] {target.name}에게 슬로우 {slowAmount} 적용 ({duration}초)");
    }
}