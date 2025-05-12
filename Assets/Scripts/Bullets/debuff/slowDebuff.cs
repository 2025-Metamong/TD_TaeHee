using UnityEngine;
using System.Collections;
using MyGame.Objects;

[CreateAssetMenu(fileName = "SlowDebuff", menuName = "Scriptable Objects/Debuff/Slow")]
public class SlowDebuff : debuffBase
{
    public float slowAmount = 0.2f;

    public override void Apply(GameObject target)
    {
        var monster = target.GetComponent<Monster>();
        if (monster == null || monster.isDead) return;

        // 디버프 구분에 사용하는 key 값은 debuffName
        string debuffKey = debuffName;

        IEnumerator co = SlowCoroutine(monster, debuffKey);

        System.Action onEnd = () => {
            if (monster != null && !monster.isDead)
            {
                monster.SetSpeed(monster.GetSpeed() / (1f - slowAmount));
                Debug.Log($"[SlowDebuff] 디버프 해제 (갱신)");
            }
        };

        monster.ApplyDebuff(debuffKey, co, onEnd);
    }

    private IEnumerator SlowCoroutine(Monster monster, string debuffKey)
    {
        if (monster==null || monster.isDead) yield break;

        // 현재 속도 감소 (중첩 없이)
        monster.SetSpeed(monster.GetSpeed() * (1f - slowAmount));
        Debug.Log($"[SlowDebuff] 슬로우 적용: {slowAmount * 100}% 감소");

        yield return new WaitForSeconds(duration);

        if (monster != null && !monster.isDead)
        {
            // 디버프 종료: 감소분 만큼 복구
            monster.SetSpeed(monster.GetSpeed() / (1f - slowAmount));
            monster.RemoveDebuff(debuffKey);

            Debug.Log($"[SlowDebuff] 슬로우 해제");
        }
    }
}