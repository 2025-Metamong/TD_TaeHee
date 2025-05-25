using UnityEngine;
using System.Collections;
using MyGame.Objects;

[CreateAssetMenu(fileName = "PoisonDebuff", menuName = "Scriptable Objects/Debuff/Poison")]
public class PoisonDebuff : debuffBase
{
    [Tooltip("독이 입힐 총 데미지")]
    public float totalPoisonDamage = 10f;

    public override void Apply(GameObject target)
    {
        var monster = target.GetComponent<Monster>();
        if (monster == null || monster.isDead) return;

        // 디버프 구분에 사용하는 key 값은 debuffName
        string debuffKey = debuffName;

        IEnumerator co = PoisonCoroutine(monster, debuffKey);

        System.Action onEnd = () =>
        {
            if (monster != null && !monster.isDead)
            {
                Debug.Log($"[PoisonDebuff] 독 해제 (갱신)");
            }
        };

        monster.ApplyDebuff(debuffKey, co, onEnd);
    }

    private IEnumerator PoisonCoroutine(Monster monster, string debuffKey)
    {
        if (monster==null || monster.isDead) yield break;

        float tick = 0f;
        while (tick < duration)
        {
            yield return new WaitForSeconds(0.5f);
            tick += 0.5f;
            monster.TakeDamage(totalPoisonDamage / duration);
        }

        Debug.Log($"[PoisonDebuff] 독 적용: {duration}초 동안 {totalPoisonDamage}데미지");

        if (monster != null && !monster.isDead)
        {
            monster.RemoveDebuff(debuffKey);

            Debug.Log($"[PoisonDebuff] 독 해제");
        }
    }
}