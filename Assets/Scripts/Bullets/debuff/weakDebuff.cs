using UnityEngine;
using System.Collections;
using MyGame.Objects;

[CreateAssetMenu(fileName = "SlowDebuff", menuName = "Scriptable Objects/Debuff/Weak")]
public class WeakDebuff : debuffBase
{
    public float damageAmount = 0.2f;

    public override void Apply(GameObject target)
    {
        var monster = target.GetComponent<Monster>();
        if (monster == null || monster.isDead) return;

        // 디버프 구분에 사용하는 key 값은 debuffName
        string debuffKey = debuffName;

        IEnumerator co = WeakCoroutine(monster, debuffKey);

        System.Action onEnd = () => {
            if (monster != null && !monster.isDead)
            {
                monster.SetDamageAmplify(monster.GetDamageAmplify() / (1f + damageAmount));
                Debug.Log($"[WeakDebuff] 나약함 해제 (갱신)");
            }
        };

        monster.ApplyDebuff(debuffKey, co, onEnd);
    }

    private IEnumerator WeakCoroutine(Monster monster, string debuffKey)
    {
        if (monster==null || monster.isDead) yield break;

        // 현재 속도 감소 (중첩 없이)
        monster.SetDamageAmplify(monster.GetDamageAmplify() * (1f + damageAmount));
        Debug.Log($"[WeakDebuff] 나약함 적용: 받는 데미지 {damageAmount * 100}% 증가");

        yield return new WaitForSeconds(duration);

        if (monster != null && !monster.isDead)
        {
            // 디버프 종료: 감소분 만큼 복구
            monster.SetDamageAmplify(monster.GetDamageAmplify() / (1f + damageAmount));
            monster.RemoveDebuff(debuffKey);

            Debug.Log($"[WeakDebuff] 나약함 해제");
        }
    }
}