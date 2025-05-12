using UnityEngine;
using System.Collections;
using MyGame.Objects;

[CreateAssetMenu(fileName = "StunDebuff", menuName = "Scriptable Objects/Debuff/Stun")]
public class StunDebuff : debuffBase
{
    public override void Apply(GameObject target)
    {
        var monster = target.GetComponent<Monster>();
        if (monster == null || monster.isDead) return;

        // 디버프 구분에 사용하는 key 값은 debuffName
        string debuffKey = debuffName;

        IEnumerator co = StunCoroutine(monster, debuffKey);

        System.Action onEnd = () => {
            if (monster != null && !monster.isDead)
            {
                monster.SetStun(false);
                Debug.Log($"[StunDebuff] 기절 해제 (갱신)");
            }
        };

        monster.ApplyDebuff(debuffKey, co, onEnd);
    }

    private IEnumerator StunCoroutine(Monster monster, string debuffKey)
    {
        if (monster==null || monster.isDead) yield break;

        monster.SetStun(true);
        Debug.Log($"[StunDebuff] 기절 적용 : {duration} 동안 멈춤");

        yield return new WaitForSeconds(duration);

        if (monster != null && !monster.isDead)
        {
            monster.SetStun(false);
            monster.RemoveDebuff(debuffKey);

            Debug.Log($"[StunDebuff] 기절 해제");
        }
    }
}